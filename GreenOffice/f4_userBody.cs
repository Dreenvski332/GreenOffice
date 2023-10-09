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
                        MessageBox.Show("Dzisiejszego dnia, praca została już rozpoczęta");
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
                            }
                        }
                        catch { MessageBox.Show("Nieoczekiwany błąd zaczynania pracy"); }
                    }
                }
                catch
                { MessageBox.Show("Nieoczewkiwany błąd sprawdzania rejestru pracy na dzień " + DateTime.Now.ToString("yyyy-MM-dd")); }
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                String addStartingTimeQuery = "UPDATE timer SET endtime=@endTime WHERE date=@date AND userEmail=@userEmail";
                try
                {
                    using (MySqlCommand addStartingTimeCommand = new MySqlCommand(addStartingTimeQuery, databaseConnection))
                    {
                        string time = DateTime.Now.ToString("hh:mm");
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        addStartingTimeCommand.Parameters.AddWithValue("@endTime", time);
                        addStartingTimeCommand.Parameters.AddWithValue("@date", date);
                        addStartingTimeCommand.Parameters.AddWithValue("@userEmail", viewUserTextbox.Text);

                        databaseConnection.Open();
                        int queryFeedback = addStartingTimeCommand.ExecuteNonQuery();
                        databaseConnection.Close();
                    }
                }
                catch { MessageBox.Show("Nieoczekiwany błąd zaczynania pracy"); }
            }
        }
    }
}
