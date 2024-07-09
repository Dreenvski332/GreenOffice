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

namespace GreenOffice
{
    public partial class AdminCalendarUserControl : UserControl
    {
        public int DayNumber { get; set; }
        public DateTime CurrentDate { get; set; }
        public static string leaveID = "";
        public static string passLeaveID = "";
        public int isApproved;
        public static string ApprovedStatus = "";
        private static string codeAdmin;
        public AdminCalendarUserControl()
        {
            InitializeComponent();
            codeViewUserLabel.Text = f1_login.email;
            codeAdmin = f3_adminBody.adminCode;
            isApproved = 2;
        }
        public void SetDay(int day, DateTime date)
        {
            DayNumber = day;
            CurrentDate = date;
            dayLabel.Text = day.ToString();
            displayLeave();
        }
        private string FormatTime(TimeSpan timeSpan)
        {
            DateTime dateTime = DateTime.Today.Add(timeSpan);
            return dateTime.ToString("HH:mm");
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
                    if (codeAdmin != null)
                    {
                        displayLeaveCommand.Parameters.AddWithValue("@email", codeAdmin);
                    }
                    else
                    {
                        displayLeaveCommand.Parameters.AddWithValue("@email", codeViewUserLabel.Text);
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
                            if (isApproved == 0)
                            {
                                this.BackColor = Color.MintCream; displayLeaveTextbox.BackColor = Color.MintCream; ApprovedStatus = "Niezatwierdzone";
                                TimeSpan specificStart = new TimeSpan(0, 0, 0);
                                TimeSpan specificFinish = new TimeSpan(23, 59, 59);
                                if (leaveStartTime == specificStart && leaveFinishTime == specificFinish)
                                {
                                    leaveDescriptionTooltip.SetToolTip(displayLeaveTextbox, ApprovedStatus + " \nCały dzień");
                                }
                                else
                                {
                                    leaveDescriptionTooltip.SetToolTip(displayLeaveTextbox, ApprovedStatus + " \nOd: " + formatLeaveStartTime + "\nDo: " + formatLeaveFinishTime);
                                }
                                displayLeaveTextbox.Text = leaveReason;
                                idLabel.Text = leaveID;
                            }

                        }

                    }
                }
                databaseConnection.Close();
            }
        }


        private void displayLeaveTextbox_MouseEnter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(displayLeaveTextbox.Text)) { displayLeaveTextbox.Cursor = Cursors.Hand; }
        }
        private void displayLeaveTextbox_MouseLeave(object sender, EventArgs e) { displayLeaveTextbox.Cursor = Cursors.Default; }
        private void displayLeaveTextbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(displayLeaveTextbox.Text))
            {
                passLeaveID = idLabel.Text;
                f7_approveLeave Open_f7_approveLeave = new f7_approveLeave();
                Open_f7_approveLeave.ShowDialog();
            }
        }
    }
}
