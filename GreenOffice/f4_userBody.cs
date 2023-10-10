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
    public partial class f4_userBody : Form
    {
        public f4_userBody()
        {
            InitializeComponent();
            timerGroupbox.Visible = false;
            viewUserTextbox.Text = f1_login.email;
        }

        private void timerButton_Click(object sender, EventArgs e)
        {
            timerGroupbox.Visible = true;
        }
        private void timerStartButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                try
                {
                    MySqlCommand workStartChecker = new MySqlCommand($"SELECT date, userEmail FROM timer WHERE date=@date AND userEmail=@userEmail");
                    workStartChecker.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                    workStartChecker.Parameters.AddWithValue("@userEmail", viewUserTextbox.Text);
                    workStartChecker.CommandType = CommandType.Text;
                    workStartChecker.Connection = databaseConnection;

                    databaseConnection.Open();
                    MySqlDataReader sqlDataReaderWorkStart = workStartChecker.ExecuteReader();
                    bool queryWorkCheckerSuccessful = sqlDataReaderWorkStart.HasRows;
                    if (queryWorkCheckerSuccessful == true)
                    {
                        databaseConnection.Close();
                        MessageBox.Show("Praca dnia " + DateTime.Now.ToString("yyyy-MM-dd") + " została już rozpoczęta");
                    }
                    else
                    {
                        databaseConnection.Close();
                        String addStartingTimeQuery = "INSERT INTO `timer`(`startTime`, `date`, `userEmail`) VALUES (@startTime,@date,@userEmail)";
                        try
                        {
                            using (MySqlCommand addStartingTimeCommand = new MySqlCommand(addStartingTimeQuery, databaseConnection))
                            {
                                addStartingTimeCommand.Parameters.AddWithValue("@startTime", DateTime.Now.ToString("hh:mm"));
                                addStartingTimeCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                                addStartingTimeCommand.Parameters.AddWithValue("@userEmail", viewUserTextbox.Text);

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
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                try
                {
                    MySqlCommand verifyWorkStartChecker = new MySqlCommand($"SELECT timer.startTime, timer.date, timer.userEmail FROM timer WHERE timer.startTime IS NOT NULL AND timer.date=@date AND timer.userEmail=@userEmail");
                    verifyWorkStartChecker.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                    verifyWorkStartChecker.Parameters.AddWithValue("@userEmail", viewUserTextbox.Text);
                    verifyWorkStartChecker.CommandType = CommandType.Text;
                    verifyWorkStartChecker.Connection = databaseConnection;

                    databaseConnection.Open();
                    MySqlDataReader sqlDataReaderVerifyWorkStart = verifyWorkStartChecker.ExecuteReader();
                    bool queryVerifyWorkStartSuccessful = sqlDataReaderVerifyWorkStart.HasRows;
                    if (queryVerifyWorkStartSuccessful == true) 
                    {
                        databaseConnection.Close();
                        try
                        {
                            MySqlCommand workEndChecker = new MySqlCommand($"SELECT timer.endTime, timer.date, timer.userEmail FROM timer WHERE timer.endTime IS NULL AND timer.date=@date AND timer.userEmail=@userEmail");
                            workEndChecker.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                            workEndChecker.Parameters.AddWithValue("@userEmail", viewUserTextbox.Text);
                            workEndChecker.CommandType = CommandType.Text;
                            workEndChecker.Connection = databaseConnection;

                            databaseConnection.Open();
                            MySqlDataReader sqlDataReaderWorkEnd = workEndChecker.ExecuteReader();
                            bool queryWorkEndCheckerSuccessful = sqlDataReaderVerifyWorkStart.HasRows;
                            if(queryWorkEndCheckerSuccessful == true)
                            {

                            }
                        }
                        catch { MessageBox.Show("Dnia " + DateTime.Now.ToString("yyyy-MM-dd") + " praca została zakończona"); }
                    }
                    else { databaseConnection.Close(); MessageBox.Show("Nieoczekiwany błąd kończenia pracy"); }
                }
                catch { MessageBox.Show("Dnia " + DateTime.Now.ToString("yyyy-MM-dd") + " nie rozpoczęto pracy"); }
            }
        }
    }
}
