using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GreenOffice.f4_userBody;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GreenOffice
{
    public partial class DayUserControl : UserControl
    {
        public int DayNumber { get; set; }
        public DateTime CurrentDate { get; set; }
        public static string eventID = "";
        public static string passEventID = "";
        public DayUserControl()
        {
            InitializeComponent();
            codeViewUserLabel.Text = f1_login.email;

        }
        public void SetDay(int day, DateTime date)
        {
            DayNumber = day;
            CurrentDate = date;
            dayLabel.Text = day.ToString();
            displayEvent();
        }
        private string FormatTime(TimeSpan timeSpan)
        {
            DateTime dateTime = DateTime.Today.Add(timeSpan);
            return dateTime.ToString("HH:mm");
        }
        private void displayEvent()
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"
                string displayEventQuery = "SELECT eventID, eventCategory, eventDescription, eventStartDate, eventFinishDate, eventStartTime, eventFinishTime FROM events WHERE @currentDate BETWEEN eventStartDate AND eventFinishDate";
                databaseConnection.Open();
                using (MySqlCommand displayEventCommand = new MySqlCommand(displayEventQuery, databaseConnection))
                {
                    displayEventCommand.Parameters.AddWithValue("@currentDate", CurrentDate);
                    using (MySqlDataReader reader = displayEventCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string eventCategory = reader["eventCategory"].ToString();
                            string eventDescription = reader["eventDescription"].ToString();
                            TimeSpan eventStartTime = (TimeSpan)reader["eventStartTime"];
                            TimeSpan eventFinishTime = (TimeSpan)reader["eventFinishTime"];
                            string formatEventStartTime = FormatTime(eventStartTime);
                            string formatEventFinishTime = FormatTime(eventFinishTime);
                            eventID = reader["eventID"].ToString();
                            eventDescriptionTooltip.SetToolTip(displayEventTextbox, "Opis: " + eventDescription + "\n" + "Godzina rozpoczęcia: " + formatEventStartTime + "\nGodzina zakończenia: " + formatEventFinishTime);
                            displayEventTextbox.Text = eventCategory;
                            idLabel.Text = eventID;
                        }
                    }
                }
                databaseConnection.Close();
            }
        }

        private void displayEventTextbox_MouseEnter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(displayEventTextbox.Text)) { displayEventTextbox.Cursor = Cursors.Hand; }
        }
        private void displayEventTextbox_MouseLeave(object sender, EventArgs e) { displayEventTextbox.Cursor = Cursors.Default; }
        private void displayEventTextbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(displayEventTextbox.Text))
            {
                passEventID = idLabel.Text;
                f6_deleteEvent Open_f6_deleteEvent = new f6_deleteEvent(); //opens up user form instead
                Open_f6_deleteEvent.ShowDialog();
            }
        }
    }
}
