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
        public static string leaveID = "";
        public static string passEventID = "";
        public int isApproved;
        public static string ApprovedStatus = "";
        private static string codeAdmin;
        private static string codeCalendar;
        public static string passCategory = "";
        public DayUserControl()
        {
            InitializeComponent();
            codeViewUserLabel.Text = f1_login.email;
            codeCalendar = calendarCode;
            codeAdmin = f3_adminBody.adminCode;

            isApproved = 2;
    }
        public void SetDay(int day, DateTime date)
        {
            DayNumber = day;
            CurrentDate = date;
            dayLabel.Text = day.ToString();
            displayEvent();
            displayLeave();
        }
        private string FormatTime(TimeSpan timeSpan)
        {
            DateTime dateTime = DateTime.Today.Add(timeSpan);
            return dateTime.ToString("HH:mm");
        }
        private void displayEvent()
        {
            PathFactory pathFactory = new PathFactory(); 
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) 
            {
                string connection = streamReader.ReadToEnd(); 
                string connectionString = connection; 
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); 
                string displayEventQuery = "SELECT eventID, eventCategory, eventDescription, eventStartDate, eventFinishDate, eventStartTime, eventFinishTime FROM events " + 
                    " WHERE email=@email AND @currentDate BETWEEN eventStartDate AND eventFinishDate";
                databaseConnection.Open();
                using (MySqlCommand displayEventCommand = new MySqlCommand(displayEventQuery, databaseConnection))
                {
                    if(codeAdmin != "")
                    {
                        displayEventCommand.Parameters.AddWithValue("@email", codeAdmin);
                    }
                    else
                    {
                        displayEventCommand.Parameters.AddWithValue("@email", codeCalendar);
                    }
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
                            TimeSpan specificStart = new TimeSpan(0, 0, 0);
                            TimeSpan specificFinish = new TimeSpan(23, 59, 59);
                            if (eventStartTime == specificStart && eventFinishTime == specificFinish)
                            {
                                eventDescriptionTooltip.SetToolTip(displayEventTextbox, "Opis: " + eventDescription + " \nCały dzień");
                            }
                            else
                            {
                                eventDescriptionTooltip.SetToolTip(displayEventTextbox, "Opis: " + eventDescription + "\n" + "Godzina rozpoczęcia: " 
                                    + formatEventStartTime + "\nGodzina zakończenia: " + formatEventFinishTime);
                            }
                            displayEventTextbox.Text = eventCategory;
                            idLabel.Text = eventID;
                        }
                    }
                }
                databaseConnection.Close();
            }
        }
        private void displayLeave()
        {
            PathFactory pathFactory = new PathFactory(); 
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) 
            {
                string connection = streamReader.ReadToEnd(); 
                string connectionString = connection; 
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); 
                string displayLeaveQuery = "SELECT leaveID, leaveStartDate, leaveFinishDate, leaveStartTime, leaveFinishTime, leaveApproved, leaveReason FROM leavetable WHERE email=@email AND @currentDate BETWEEN leaveStartDate AND leaveFinishDate";
                databaseConnection.Open();
                using (MySqlCommand displayLeaveCommand = new MySqlCommand(displayLeaveQuery, databaseConnection))
                {
                    if (codeAdmin != "")
                    {
                        displayLeaveCommand.Parameters.AddWithValue("@email", codeAdmin);
                    }
                    else
                    {
                        displayLeaveCommand.Parameters.AddWithValue("@email", codeCalendar);
                    }
                    displayLeaveCommand.Parameters.AddWithValue("@currentDate", CurrentDate);
                    using (MySqlDataReader reader = displayLeaveCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TimeSpan leaveStartTime = (TimeSpan)reader["leaveStartTime"];
                            TimeSpan leaveFinishTime = (TimeSpan)reader["leaveFinishTime"];
                            string formatLeaveStartTime = FormatTime(leaveStartTime);
                            string formatLeaveFinishTime = FormatTime(leaveFinishTime);
                            string leaveReason = reader["leaveReason"].ToString();
                            isApproved = reader.GetInt32("leaveApproved");
                            leaveID = reader["leaveID"].ToString();
                            TimeSpan specificStart = new TimeSpan(0, 0, 0);
                            TimeSpan specificFinish = new TimeSpan(23, 59, 59);
                            if (isApproved == 0)
                            { 
                                this.BackColor = Color.MintCream; displayEventTextbox.BackColor = Color.MintCream; ApprovedStatus = "Niezatwierdzone";
                                
                                if (leaveStartTime == specificStart && leaveFinishTime == specificFinish)
                                {
                                    eventDescriptionTooltip.SetToolTip(displayEventTextbox, ApprovedStatus + " \nCały dzień");
                                }
                                else
                                {
                                    eventDescriptionTooltip.SetToolTip(displayEventTextbox, ApprovedStatus + " \nOd: " + formatLeaveStartTime + "\nDo: " + formatLeaveFinishTime);
                                }
                                displayEventTextbox.Text = leaveReason;
                                idLabel.Text = leaveID;
                            }
                            else if (isApproved == 1)
                            { 
                                this.BackColor = Color.LightYellow; displayEventTextbox.BackColor = Color.LightYellow; ApprovedStatus = "Zatwierdzone";
                                if (leaveStartTime == specificStart && leaveFinishTime == specificFinish)
                                {
                                    eventDescriptionTooltip.SetToolTip(displayEventTextbox, ApprovedStatus + " \nCały dzień");
                                }
                                else
                                {
                                    eventDescriptionTooltip.SetToolTip(displayEventTextbox, ApprovedStatus + " \nOd: " + formatLeaveStartTime + "\nDo: " + formatLeaveFinishTime);
                                }
                                displayEventTextbox.Text = leaveReason;
                                idLabel.Text = leaveID;
                            }
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
                passCategory = displayEventTextbox.Text;
                passEventID = idLabel.Text;
                f6_deleteEvent Open_f6_deleteEvent = new f6_deleteEvent(); 
                Open_f6_deleteEvent.ShowDialog();
            }
        }
    }
}
