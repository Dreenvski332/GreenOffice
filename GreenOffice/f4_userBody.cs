using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
            frontPagePanel.Visible = true;
            timerPanel.Visible = false;


            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"

                MySqlCommand displayName = new MySqlCommand($"SELECT email, name FROM user WHERE email=@email");
                displayName.Parameters.AddWithValue("@email", viewUserTextbox.Text);
                displayName.CommandType = CommandType.Text;
                displayName.Connection = databaseConnection;

                databaseConnection.Open();
                using (MySqlDataReader readerDisplayName = displayName.ExecuteReader())
                {
                    readerDisplayName.Read();
                    displayNameTextbox.Text = readerDisplayName["name"].ToString() + "!";
                    databaseConnection.Close();
                }
            }
        }
        private void timerPanelButton_Click(object sender, EventArgs e) //TIMER PANEL BUTTON
        {
            timerPanel.Visible = true;
            string trashcan = "";
            displayDateTextbox.Text = trashcan;
            displayFinishTimeTextbox.Text = trashcan;
            displayStartTimeTextbox.Text = trashcan;
            displayTimeSpanTextbox.Text = trashcan;


            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                try
                {
                    string connection = streamReader.ReadToEnd();
                    string connectionString = connection;
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                    MySqlCommand displayDateQuery = new MySqlCommand($"SELECT startDate FROM timer WHERE username=@login AND MONTH(startDate)=@month");
                    displayDateQuery.Parameters.AddWithValue("@login", viewUserTextbox.Text);
                    displayDateQuery.Parameters.AddWithValue("@month", codeMonthLabel.Text);
                    displayDateQuery.CommandType = CommandType.Text;
                    displayDateQuery.Connection = databaseConnection;
                    databaseConnection.Open();

                    MySqlDataReader reader = displayDateQuery.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime date = reader.GetDateTime("startDate");
                        string formattedDate = date.ToString("yyyy-MM-dd"); // Format the date

                        // Append each date to the TextBox with a new line
                        displayDateTextbox.AppendText(formattedDate + Environment.NewLine);
                    }
                    reader.Close();
                }
                catch { MessageBox.Show("Błąd wczytywania daty z bazy danych"); }
            }
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand startingTimeQuery = new MySqlCommand($"SELECT startTime FROM timer WHERE username=@login AND MONTH(startDate)=@month");
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
                displayStartTimeTextbox.Text = stringBuilderStart.ToString();

                MySqlCommand finishTimeQuery = new MySqlCommand($"SELECT finishTime FROM timer WHERE username=@login AND MONTH(startDate)=@month");
                finishTimeQuery.Parameters.AddWithValue("@login", viewUserTextbox.Text);
                finishTimeQuery.Parameters.AddWithValue("@month", codeMonthLabel.Text);
                finishTimeQuery.CommandType = CommandType.Text;
                finishTimeQuery.Connection = databaseConnection;

                MySqlDataAdapter endDataAdapter = new MySqlDataAdapter(finishTimeQuery);
                DataTable timeTable = new DataTable();
                endDataAdapter.Fill(timeTable);
                StringBuilder stringBuilderEnd = new StringBuilder();
                foreach (DataRow row in timeTable.Rows)
                {
                    for (int i = 0; i < timeTable.Columns.Count; i++)
                    {
                        stringBuilderEnd.Append(row[i].ToString());
                        if (i < timeTable.Columns.Count - 1)
                        {
                            stringBuilderEnd.Append("\t");
                        }
                    }
                    stringBuilderEnd.AppendLine();
                }
                displayFinishTimeTextbox.Text = stringBuilderEnd.ToString();
            }
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
        private void displayFinishTimeTextbox_TextChanged(object sender, EventArgs e)
        {
            string[] startTimes = displayStartTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] finishTimes = displayFinishTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            TimeSpan totalDifference = TimeSpan.Zero;

            // Iterate through each pair of start and finish times
            for (int i = 0; i < startTimes.Length; i++)
            {
                // Parse the strings to DateTime
                if (DateTime.TryParse(startTimes[i], out DateTime startTime) &&
                    DateTime.TryParse(finishTimes[i], out DateTime finishTime))
                {
                    // Subtract finish time from start time and add to total difference
                    totalDifference += finishTime - startTime;
                    string formattedDifference = $"{(int)totalDifference.TotalHours}:{totalDifference.Minutes:D2}";
                    displayTimeSpanTextbox.Text = formattedDifference.ToString();
                }
                else
                {
                    // Handle parsing error if needed
                    MessageBox.Show($"Error parsing datetime at row {i + 1}");
                    return;
                }
            }
        }
        private void killTimerButton_Click(object sender, EventArgs e)
        {
            timerPanel.Visible = false;
            frontPagePanel.Visible = true;
        }
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
