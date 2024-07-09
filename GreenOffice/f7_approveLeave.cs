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
    public partial class f7_approveLeave : Form
    {
        public string passedLeaveID;
        public string passedAdminCode;
        private int dayCountPaid;
        private int dayCountOnDemand;
        private int dayCount;
        private int adminNum;
        public f7_approveLeave()
        {
            InitializeComponent();
            passedLeaveID = AdminCalendarUserControl.passLeaveID;
            passedAdminCode = f3_adminBody.adminCode;

            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand selectLeaveReasonQuery = new MySqlCommand($"SELECT leftLeaveDays, leftLeaveOnDemandDays FROM user WHERE email=@email");
                selectLeaveReasonQuery.Parameters.AddWithValue("@email", passedAdminCode);
                selectLeaveReasonQuery.CommandType = CommandType.Text;
                selectLeaveReasonQuery.Connection = databaseConnection;

                databaseConnection.Open();
                using (MySqlDataReader reader = selectLeaveReasonQuery.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dayCountPaid = reader.GetInt32("leftLeaveDays");
                        dayCountOnDemand = reader.GetInt32("leftLeaveOnDemandDays");
                    }
                }
            }
        }

        private void doNotApproveButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                String deleteUserQuery = "DELETE FROM leavetable WHERE leaveID=@leaveID";
                using (MySqlCommand deleteUserCommand = new MySqlCommand(deleteUserQuery, databaseConnection))
                {
                    deleteUserCommand.Parameters.AddWithValue("@leaveID", passedLeaveID);

                    databaseConnection.Open();
                    int queryFeedback = deleteUserCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
            }
            this.Close();
        }

        private void ApproveButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                String deleteUserQuery = "UPDATE leavetable SET leaveApproved=@leaveApproved WHERE leaveID=@leaveID";
                using (MySqlCommand deleteUserCommand = new MySqlCommand(deleteUserQuery, databaseConnection))
                {
                    int leaveApproved = 1;
                    deleteUserCommand.Parameters.AddWithValue("@leaveID", passedLeaveID);
                    deleteUserCommand.Parameters.AddWithValue("@leaveApproved", leaveApproved);

                    databaseConnection.Open();
                    int queryFeedback = deleteUserCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
            }
            calculateLeftLeaveDays();
            this.Close();
        }
        private void produceAdminNum()
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand selectLeaveReasonQuery = new MySqlCommand($"SELECT leaveID, email, leaveStartDate, leaveFinishDate, leaveReason FROM leavetable WHERE leaveID=@leaveID");
                selectLeaveReasonQuery.Parameters.AddWithValue("@leaveID", passedLeaveID);
                selectLeaveReasonQuery.CommandType = CommandType.Text;
                selectLeaveReasonQuery.Connection = databaseConnection;

                databaseConnection.Open();
                using (MySqlDataReader reader = selectLeaveReasonQuery.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string leaveReason = reader.GetString("leaveReason");
                        if(leaveReason == "Urlop (płatny)")
                        {
                            adminNum = 1;
                        }
                        if (leaveReason == "Na żądanie")
                        {
                            adminNum = 2;
                        }
                        DateTime leaveStartDate = reader.GetDateTime("leaveStartDate");
                        DateTime leaveFinishDate = reader.GetDateTime("leaveFinishDate");
                        dayCount = (leaveFinishDate - leaveStartDate).Days;
                    
                        dayCountPaid = dayCountPaid - dayCount;
                        dayCountOnDemand = dayCountOnDemand - dayCount;
                    }
                }
            }
        }
        private void calculateLeftLeaveDays()
        {
            produceAdminNum();
            PathFactory pathFactory = new PathFactory();
            if (adminNum == 1)
            {
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
                {
                    string connection = streamReader.ReadToEnd();
                    string connectionString = connection;
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    String updateLeaveQuery = "UPDATE `user` SET `leftLeaveDays`=@leftLeaveDays WHERE email=@email";
                    using (MySqlCommand updateLeaveCommand = new MySqlCommand(updateLeaveQuery, databaseConnection))
                    {
                        databaseConnection.Open();
                        updateLeaveCommand.Parameters.AddWithValue("@email", passedAdminCode);
                        updateLeaveCommand.Parameters.AddWithValue("@leftLeaveDays", dayCountPaid);
                        int result = updateLeaveCommand.ExecuteNonQuery();
                    }
                }

            }
            else if (adminNum == 2)
            {
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
                {
                    string connection = streamReader.ReadToEnd();
                    string connectionString = connection;
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    String updateLeaveQuery = "UPDATE `user` SET `leftLeaveOnDemandDays`=@leftLeaveOnDemandDays WHERE email=@email";
                    using (MySqlCommand updateLeaveCommand = new MySqlCommand(updateLeaveQuery, databaseConnection))
                    {
                        databaseConnection.Open();
                        updateLeaveCommand.Parameters.AddWithValue("@email", passedAdminCode);
                        updateLeaveCommand.Parameters.AddWithValue("@leftLeaveOnDemandDays", dayCountOnDemand);
                        int result = updateLeaveCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
