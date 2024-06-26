﻿using MySql.Data.MySqlClient;
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
    public partial class f6_deleteEvent : Form
    {
        public string passedEventID;
        public string passedCategory;

        public f6_deleteEvent()
        {
            InitializeComponent();
            passedEventID = DayUserControl.passEventID;
            passedCategory = DayUserControl.passCategory;
        }

        private void keepButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory();
            if (passedCategory == "Chorobowy (L4)" || passedCategory == "Z tytułu krwiodawstwa")
            {
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
                {
                    string connection = streamReader.ReadToEnd();
                    string connectionString = connection;
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    String deleteUserQuery = "DELETE FROM leavetable WHERE leaveID=@eventID";
                    using (MySqlCommand deleteUserCommand = new MySqlCommand(deleteUserQuery, databaseConnection))
                    {
                        deleteUserCommand.Parameters.AddWithValue("@eventID", passedEventID);

                        databaseConnection.Open();
                        int queryFeedback = deleteUserCommand.ExecuteNonQuery();
                        databaseConnection.Close();
                    }
                }
                this.Close();
            }
            else
            {
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
                {
                    string connection = streamReader.ReadToEnd();
                    string connectionString = connection;
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    String deleteUserQuery = "DELETE FROM events WHERE eventID=@eventID";
                    using (MySqlCommand deleteUserCommand = new MySqlCommand(deleteUserQuery, databaseConnection))
                    {
                        deleteUserCommand.Parameters.AddWithValue("@eventID", passedEventID);

                        databaseConnection.Open();
                        int queryFeedback = deleteUserCommand.ExecuteNonQuery();
                        databaseConnection.Close();
                    }
                }
                this.Close();
            }
        }
    }
}
