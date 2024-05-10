﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenOffice
{
    public partial class f4_userBody : Form
    {
        // ============================ OVERALL BODY START ==================================
        public f4_userBody()
        {
            InitializeComponent();
            viewUserTextbox.Text = f1_login.email; //sets user email, puts it into textbox - taken from login screen
        }
        private void timerPanelButton_Click(object sender, EventArgs e) //TIMER PANEL BUTTON
        {
            timerPanel.Visible = true;

            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand startingTimeQuery = new MySqlCommand($"SELECT startDate FROM timer WHERE username=@login AND MONTH(startDate)=@month");
                startingTimeQuery.Parameters.AddWithValue("@login", viewUserTextbox.Text);
                startingTimeQuery.Parameters.AddWithValue("@month", codeMonthLabel.Text);
                startingTimeQuery.CommandType = CommandType.Text;
                startingTimeQuery.Connection = databaseConnection;
                databaseConnection.Open();

                MySqlDataAdapter startDataAdapter = new MySqlDataAdapter(startingTimeQuery);
                DataTable startingTimeTable = new DataTable();
                startDataAdapter.Fill(startingTimeTable);
                StringBuilder stringBuilderStart = new StringBuilder();
                foreach (DataRow row in startingTimeTable.Rows)
                {
                    for (int i = 0; i < startingTimeTable.Columns.Count; i++)
                    {
                        stringBuilderStart.Append(row[i].ToString());
                        if (i < startingTimeTable.Columns.Count - 1)
                        {
                            stringBuilderStart.Append("\t");
                        }
                    }
                    stringBuilderStart.AppendLine();
                }
                displayDateTextbox.Text = stringBuilderStart.ToString();
            }

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            string invisibleMonth = new DateTime(currentYear, currentMonth, 1).ToString("MM");
            codeMonthLabel.Text = invisibleMonth;
        }
        private void timerStartButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"
                try
                { //checks if there is a ROW in "timer" that contains current date and currently logged user
                    MySqlCommand workStartChecker = new MySqlCommand($"SELECT startDate, username FROM timer WHERE startDate=@startDate AND username=@username");
                    workStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    workStartChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text);
                    workStartChecker.CommandType = CommandType.Text;
                    workStartChecker.Connection = databaseConnection;

                    databaseConnection.Open();
                    MySqlDataReader readerWorkStart = workStartChecker.ExecuteReader();
                    bool boolWorkStart = readerWorkStart.HasRows;
                    if (boolWorkStart == true)
                    { //if that row exists, that means the work has already started for the day
                        databaseConnection.Close();
                        MessageBox.Show("Praca dnia " + DateTime.Now.ToString("yyyy-MM-dd") + " została już rozpoczęta");
                    }
                    else
                    { //if it doesn't then starts work for the day
                        databaseConnection.Close();
                        String addStartingTimeQuery = "INSERT INTO `timer`(`username`, `startDate`, `startTime`) VALUES (@username,@startDate,@startTime)";
                        try
                        {
                            using (MySqlCommand addStartingTimeCommand = new MySqlCommand(addStartingTimeQuery, databaseConnection))
                            {
                                addStartingTimeCommand.Parameters.AddWithValue("@startTime", DateTime.Now.ToString("H:mm"));
                                addStartingTimeCommand.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                addStartingTimeCommand.Parameters.AddWithValue("@username", viewUserTextbox.Text);

                                databaseConnection.Open();
                                int queryFeedback = addStartingTimeCommand.ExecuteNonQuery();
                                databaseConnection.Close();
                                MessageBox.Show("Praca rozpoczęta");
                            }
                        }
                        catch { MessageBox.Show("Nieoczekiwany błąd zaczynania pracy"); }
                    }
                }
                catch { MessageBox.Show("Nieoczewkiwany błąd sprawdzania rejestru pracy na dzień " + DateTime.Now.ToString("yyyy-MM-dd")); }
            }
        }

        private void endTimerButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"
                try
                { //checks if there is a ROW in "timer" that contains current date and currently logged user
                    MySqlCommand rowExistenceChecker = new MySqlCommand($"SELECT * FROM timer WHERE startDate=@startDate AND username=@username");
                    rowExistenceChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    rowExistenceChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text);
                    rowExistenceChecker.CommandType = CommandType.Text;
                    rowExistenceChecker.Connection = databaseConnection;

                    databaseConnection.Open();
                    MySqlDataReader readerRowExistence = rowExistenceChecker.ExecuteReader();
                    bool boolRowExistence = readerRowExistence.HasRows;
                    if (boolRowExistence == true)
                    { //if row like that exists, then program checks whether the work has been ended for the day or not
                        databaseConnection.Close(); //QUERY BELLOW is looking for a ROW where startTime is set, but the endTime isn't \/
                        MySqlCommand verifyWorkStartChecker = new MySqlCommand($"SELECT * FROM timer WHERE timer.startTime IS NOT NULL AND timer.finishTime IS NULL AND timer.startDate=@startDate AND timer.username=@username");
                        verifyWorkStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        verifyWorkStartChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text);
                        verifyWorkStartChecker.CommandType = CommandType.Text;
                        verifyWorkStartChecker.Connection = databaseConnection;

                        databaseConnection.Open();
                        MySqlDataReader readerVerifyWorkStart = verifyWorkStartChecker.ExecuteReader();
                        bool boolVerifyWorkStart = readerVerifyWorkStart.HasRows;
                        if (boolVerifyWorkStart == true)
                        { //if a row like that exists then program updates said row with endTime - that is time when button was pressed
                            databaseConnection.Close();
                            String addEndTimeQuery = "UPDATE timer SET finishTime=@finishTime WHERE startDate=@startDate AND username=@username";
                            using (MySqlCommand addEndTimeCommand = new MySqlCommand(addEndTimeQuery, databaseConnection))
                            {
                                addEndTimeCommand.Parameters.AddWithValue("@finishTime", DateTime.Now.ToString("H:mm"));
                                addEndTimeCommand.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                addEndTimeCommand.Parameters.AddWithValue("@username", viewUserTextbox.Text);

                                databaseConnection.Open();
                                int queryFeedback = addEndTimeCommand.ExecuteNonQuery();
                                databaseConnection.Close();
                                MessageBox.Show("Praca zakończona"); //also a message not to leave user standing
                            }
                        } //if row where startTime is set, and endTime isn't doesn't exist, then the only other option is that startTime and endTime are both set
                        else { databaseConnection.Close(); MessageBox.Show("W dniu " + DateTime.Now.ToString("yyyy-MM-dd") + " zakończono już pracę"); }
                    }//this is because option where startTime isn't set was filtered earlier, and there is no way to set endTime without startTime
                    else { databaseConnection.Close(); MessageBox.Show("W dniu " + DateTime.Now.ToString("yyyy-MM-dd") + " nie rozpoczęto jeszcze pracy"); }
                }
                catch { MessageBox.Show("Nieoczekiwany błąd weryfikacji rozpoczęcia pracy"); }
            }
        }


        // ============================ OVERALL BODY END ====================================



        // ============================ TIMER PANEL START ===================================
        private void styczeńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "1";
        }

        private void lutyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "2";
        }

        private void marzecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "3";
        }

        private void kwiecieńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "4";
        }

        private void majToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "5";
        }

        private void czerwiecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "6";
        }

        private void lipiecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "7";
        }

        private void sierpieńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "8";
        }

        private void wrzesieńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "9";
        }

        private void październikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "10";
        }

        private void listopadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "11";
        }

        private void grudzieńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeMonthLabel.Text = "12";
        }
        // ============================ TIMER PANEL END =====================================
    }
}
