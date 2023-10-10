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
    public partial class f1_login : Form
    {
        public static string email = ""; //sets string "email" as empty, that's for passing it to another forms
        public f1_login()
        {
            InitializeComponent();
        }
        
        private void settingsButton_Click(object sender, EventArgs e)
        { //opens setting form
                f2_conSettings Open_f2_conSettings = new f2_conSettings();
                Open_f2_conSettings.ShowDialog();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"


                if (String.IsNullOrEmpty(emailTextbox.Text) || String.IsNullOrEmpty(passwordTextbox.Text))
                { // checks if BOTH of textboxes are filled out - you could check them both out separately, but this leaves less "thrash code"
                    MessageBox.Show("Wpisz email i/lub hasło");
                }
                else
                { //if both are filled out, then we get to the meat
                    try
                    { //checks 3 things: is this userEmail in our DB, does the password match the email and is this user an admin
                        MySqlCommand loginChecker = new MySqlCommand($"SELECT user.email, user.password, user.isAdmin FROM user WHERE user.email = @email AND user.password = @password");
                        loginChecker.Parameters.AddWithValue("@email", emailTextbox.Text);
                        loginChecker.Parameters.AddWithValue("@password", passwordTextbox.Text);
                        loginChecker.CommandType = CommandType.Text;
                        loginChecker.Connection = databaseConnection;

                        databaseConnection.Open();
                        MySqlDataReader readerLoginChecker = loginChecker.ExecuteReader();
                        bool boolLoginChecker = readerLoginChecker.HasRows;
                        if (boolLoginChecker == true)
                        { //if query was successful, and given user does exist, then we need to establish whether or not he's an admin
                            if (readerLoginChecker.Read())
                            { //we do that from a column in DB called "isAdmin" it's an int with either 1 or 0; 1 means user is in fact an admin
                                int isAdmin = readerLoginChecker.GetInt32("isAdmin");
                                if (isAdmin == 1)
                                { //if isAdmin is actually 1, then:
                                    using (StreamWriter usernameWriter = new StreamWriter(pathFactory.userString))
                                    { //program passes info of the user email/name to another form
                                        usernameWriter.Write(emailTextbox.Text);
                                    }
                                    email = emailTextbox.Text; //sets previous string "email" with what's in the login textbox
                                    // hides login form
                                    this.Hide();
                                    f3_adminBody Open_f3_adminBody = new f3_adminBody(); //opens up admin form
                                    Open_f3_adminBody.ShowDialog();
                                    databaseConnection.Close();
                                }
                                else
                                { //if the user is not an admin then:
                                    using (StreamWriter usernameWriter = new StreamWriter(pathFactory.userString))
                                    { //program passes info of the user email/name to another form
                                        usernameWriter.Write(emailTextbox.Text);
                                    }
                                    email = emailTextbox.Text; //sets previous string "email" with what's in the login textbox
                                    //hides login form
                                    this.Hide();
                                    f4_userBody Open_f4_userBody = new f4_userBody(); //opens up user form instead
                                    Open_f4_userBody.ShowDialog();
                                    databaseConnection.Close();
                                }
                            } //if something messed up, during differentiation of admin and user, this error will pop up - like is someone addad "isAdmin" as any other nuber then 1 or 0
                            else { MessageBox.Show("Nieoczekiwany błąd rozróżniania rodzaju użytkownika"); }   
                        } //if login and/or password are wrong, then this error will pop up
                        else { MessageBox.Show("Błędne dane logowania"); } 
                    }
                    catch { MessageBox.Show("Błędne dane wyszukania bazy danych"); }
                }
            }
        }
    }
}

