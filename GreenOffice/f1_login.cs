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
    public partial class f1_login : Form
    {
        public static string email = "";
        public f1_login()
        {
            InitializeComponent();
        }
        
        private void settingsButton_Click(object sender, EventArgs e)
        {
                f2_conSettings Open_f2_conSettings = new f2_conSettings();
                Open_f2_conSettings.ShowDialog();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);


                if (String.IsNullOrEmpty(emailTextbox.Text) || String.IsNullOrEmpty(passwordTextbox.Text))
                {
                    MessageBox.Show("Wpisz email i/lub hasło");
                }
                else
                {
                    try
                    {
                        MySqlCommand loginChecker = new MySqlCommand($"SELECT user.email, user.password, user.isAdmin FROM user WHERE user.email = @email AND user.password = @password");
                        loginChecker.Parameters.AddWithValue("@email", emailTextbox.Text);
                        loginChecker.Parameters.AddWithValue("@password", passwordTextbox.Text);
                        loginChecker.CommandType = CommandType.Text;
                        loginChecker.Connection = databaseConnection;

                        databaseConnection.Open();
                        MySqlDataReader readerLoginChecker = loginChecker.ExecuteReader();
                        bool boolLoginChecker = readerLoginChecker.HasRows;
                        if (boolLoginChecker == true)
                        {
                            if (readerLoginChecker.Read())
                            {
                                int isAdmin = readerLoginChecker.GetInt32("isAdmin");
                                if (isAdmin == 1)
                                {
                                    using (StreamWriter usernameWriter = new StreamWriter(pathFactory.userString))
                                    {
                                        usernameWriter.Write(emailTextbox.Text);
                                    }
                                    email = emailTextbox.Text;
                                    this.Hide();
                                    f3_adminBody Open_f3_adminBody = new f3_adminBody();
                                    Open_f3_adminBody.ShowDialog();
                                    databaseConnection.Close();
                                }
                                else
                                {
                                    using (StreamWriter usernameWriter = new StreamWriter(pathFactory.userString))
                                    {
                                        usernameWriter.Write(emailTextbox.Text);
                                    }
                                    email = emailTextbox.Text;
                                    this.Hide();
                                    f4_userBody Open_f4_userBody = new f4_userBody();
                                    Open_f4_userBody.ShowDialog();
                                    databaseConnection.Close();
                                }
                            }
                            else { MessageBox.Show("Nieoczekiwany błąd rozróżniania rodzaju użytkownika"); }   
                        }
                        else { MessageBox.Show("Błędne dane logowania"); } 
                    }
                    catch { MessageBox.Show("Błędne dane wyszukania bazy danych"); }
                }
            }
        }
    }
}

