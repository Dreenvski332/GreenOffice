using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using com.itextpdf.text.pdf;
using System.Windows.Forms.VisualStyles;

namespace GreenOffice
{
    public partial class f3_adminBody : Form
    {
        private int currentYear; //creates global year int
        private int currentMonth;
        private int currentDay;//same thing but with month
        private readonly CultureInfo polishCulture; //POLSKA GUROM - POLISH MOUNTAIN
        private const string PlaceholderText = "Opis wydarzenia (opcjonalne)";
        private Color PlaceholderColor = Color.Gray;
        private Color TextColor = Color.Black;
        private int rowCount = 6;
        private int columnCount = 7;
        private int cellWidth = 115;
        private int cellHeight = 71;
        private int isApproved;
        private byte[] imageBytes = null;
        public static string adminCode =  "";
        public static string adminUser = "";
        TimeSpan totalDifference = TimeSpan.Zero;
        public static int dayCount;
        public static int dayCountPaid;
        public static int dayCountOnDemand;
        private int adminNum;
        private string workerName;
        private string workerSurname;

        public f3_adminBody()
        {
            InitializeComponent(); //you know starts the whole ordeal
            viewUserTextbox.Text = f1_login.email;
            displayedViewUserTextbox.Text = f1_login.email;//sets user email, puts it into textbox - taken from login screen
            timerPanel.Visible = false; //makes scary Timer not appear at first
            mainCalendarPanel.Visible = false; //makes scary Calendar not appear at first
            approveLeavePanel.Visible = false;
            manageUsersPanel.Visible = false;
            currentYear = DateTime.Now.Year; //sets global int to currnt year
            currentMonth = DateTime.Now.Month;
            currentDay = DateTime.Now.Day;//sets global int to current month
            polishCulture = new CultureInfo("pl-PL"); //this bad girl is to translate month names into polish later
            calendarJuicePanel.RowCount = rowCount;
            calendarJuicePanel.ColumnCount = columnCount;
            startTimePicker.CustomFormat = "hh:mm tt";
            finishTimePicker.CustomFormat = "hh:mm tt";
            leaveStartTimePicker.CustomFormat = "hh:mm tt";
            leaveFinishTimePicker.CustomFormat = "hh:mm tt";
            isApproved = 0;
            managedAccount.DrawMode = DrawMode.OwnerDrawFixed;
            adminCode = f1_login.email;
            adminNum = 0;

            leaveStartTimePicker.Enabled = false;
            leaveFinishTimePicker.Enabled = false;
            leaveStartTimePicker.Value = new DateTime(currentYear, currentMonth, currentDay, 00, 00, 00);
            leaveFinishTimePicker.Value = new DateTime(currentYear, currentMonth, currentDay, 23, 59, 59);
            leaveStartTimePicker.ValueChanged += leaveStartTimePicker_ValueChanged;

            for (int i = 0; i < rowCount; i++)
            {
                calendarJuicePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, cellHeight));
            }
            for (int i = 0; i < columnCount; i++)
            {
                calendarJuicePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellWidth));
            }

            descriptionTextbox.Text = PlaceholderText;
            descriptionTextbox.ForeColor = PlaceholderColor;
            descriptionTextbox.Enter += descriptionTextbox_Enter;
            descriptionTextbox.Leave += descriptionTextbox_Leave;

            displayLoggedUser();
            selectManagedUsers();
            leaveNotification();
        }
        private void selectManagedUsers()
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                using(MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    string displayManagedAccountsQuery = $"SELECT email FROM user"; //query to find name based on email
                    databaseConnection.Open(); //opens connection
                    MySqlCommand displayManagedAccountsCommand = new MySqlCommand(displayManagedAccountsQuery, databaseConnection);
                    MySqlDataReader reader = displayManagedAccountsCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        managedAccount.Items.Add(reader["email"].ToString());
                    }
                }
            }
        }
        private void displayLoggedUser()
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"

                MySqlCommand displayName = new MySqlCommand($"SELECT email, name FROM user WHERE email=@email"); //query to find name based on email
                displayName.Parameters.AddWithValue("@email", viewUserTextbox.Text); //takes email from textbox
                displayName.CommandType = CommandType.Text; //makes command readable for the app
                displayName.Connection = databaseConnection; //does something?

                databaseConnection.Open(); //opens connection
                using (MySqlDataReader readerDisplayName = displayName.ExecuteReader()) //executes command
                {
                    readerDisplayName.Read(); //reads data recieved from query
                    nameWelcomeTextbox.Text = readerDisplayName["name"].ToString() + "!"; //shoots name into a textbox with "!" at the end
                    databaseConnection.Close(); //stops the connection
                }
            }
        }
        private void leaveNotification()
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"

                MySqlCommand displayName = new MySqlCommand($"SELECT leaveApproved FROM leaveTable WHERE leaveApproved = @leaveApproved"); //query to find name based on email
                displayName.Parameters.AddWithValue("@leaveApproved", 0);
                displayName.CommandType = CommandType.Text; //makes command readable for the app
                displayName.Connection = databaseConnection; //does something?
                databaseConnection.Open(); //opens connection
                
                using(MySqlDataReader reader = displayName.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        notificationPanel.Visible = true;
                    }
                }
            }
        }
        private void confirmNotificationButton_Click(object sender, EventArgs e)
        {
            notificationPanel.Visible = false;
        }
        private void moveToAdminPanelButton_Click(object sender, EventArgs e)
        {
            adminPanel.Visible = true;
            leaveApproval_Click(sender, e);
            notificationPanel.Visible = false;
        }
        private void f3_adminBody_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); //just makes sure, that entire app closes when the windows X button is pressed
        }
        private void calendarButton_Click(object sender, EventArgs e) //makes scary Calendar appear
        {
            timerPanel.Visible = false; //also makes sure that timer is, you know, not in the way
            welcomeGroupbox.Visible = false; //makes big bad welcome go away
            mainCalendarPanel.Visible = true; //calendar is so good it needs to appear three times
            subCalendarPanel.Visible = true; // it actually doesn't
            calendarJuicePanel.Visible = true; //just needed to be sure
            leavePanel.Visible = false;
            adminPanel.Visible = false;
            DisplayCurrentMonth(); //starts DisplayCurrentMonth function, the code is way down
        }
        private void timerPanelButton_Click(object sender, EventArgs e) //TIMER PANEL BUTTON
        {
            timerPanel.Visible = true; //I mean, it does what it says it does
            welcomeGroupbox.Visible = false; //makes big bad welcome go away
            mainCalendarPanel.Visible = false;
            subCalendarPanel.Visible = false;
            calendarJuicePanel.Visible = false;
            leavePanel.Visible = false;
            adminPanel.Visible = false;
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1);
            codeMonthLabel.Text = firstDayOfMonth.ToString("MM");
            timerMonthLabel.Text = firstDayOfMonth.ToString("MMMM", polishCulture);
            timerStats(); //calls timerStats function, that's named in camelCase although it shouldn't
        }
        private void timeoutButton_Click(object sender, EventArgs e)
        {
            timerPanel.Visible = false;
            mainCalendarPanel.Visible = false;
            subCalendarPanel.Visible = false;
            calendarJuicePanel.Visible = false;
            welcomeGroupbox.Visible = false;
            leavePanel.Visible = true;
            adminPanel.Visible = false;
        }
        private void adminPanelButton_Click(object sender, EventArgs e)
        {
            timerPanel.Visible = false;
            mainCalendarPanel.Visible = false;
            subCalendarPanel.Visible = false;
            calendarJuicePanel.Visible = false;
            welcomeGroupbox.Visible = false;
            leavePanel.Visible = false;
            adminPanel.Visible = true;
            adminPanelBG.Visible = true;
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
                    MySqlCommand workStartChecker = new MySqlCommand($"SELECT startDate, email FROM timer WHERE startDate=@startDate AND email=@email"); 
                    workStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); 
                    workStartChecker.Parameters.AddWithValue("@email", adminCode); 
                    workStartChecker.CommandType = CommandType.Text; 
                    workStartChecker.Connection = databaseConnection; 

                    databaseConnection.Open(); 
                    MySqlDataReader readerWorkStart = workStartChecker.ExecuteReader();
                    bool boolWorkStart = readerWorkStart.HasRows; 
                    if (boolWorkStart == true) 
                    {
                        databaseConnection.Close(); 
                        MessageBox.Show("Praca dnia " + DateTime.Now.ToString("yyyy-MM-dd") + " została już rozpoczęta"); 
                    }
                    else 
                    {
                        databaseConnection.Close(); 
                        String addStartingTimeQuery = "INSERT INTO `timer`(`email`, `startDate`, `startTime`) VALUES (@email,@startDate,@startTime)";
                        try
                        {
                            using (MySqlCommand addStartingTimeCommand = new MySqlCommand(addStartingTimeQuery, databaseConnection)) 
                            {
                                addStartingTimeCommand.Parameters.AddWithValue("@startTime", DateTime.Now.ToString("H:mm")); 
                                addStartingTimeCommand.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); 
                                addStartingTimeCommand.Parameters.AddWithValue("@email", adminCode); 
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

        private void endTimerButton_Click(object sender, EventArgs e) //stops the timer for the day
        {

            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                try
                { 
                    MySqlCommand rowExistenceChecker = new MySqlCommand($"SELECT * FROM timer WHERE startDate=@startDate AND email=@email");
                    rowExistenceChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); 
                    rowExistenceChecker.Parameters.AddWithValue("@email", adminCode); 
                    rowExistenceChecker.CommandType = CommandType.Text; 
                    rowExistenceChecker.Connection = databaseConnection; 
                    databaseConnection.Open(); 
                    MySqlDataReader readerRowExistence = rowExistenceChecker.ExecuteReader(); 
                    bool boolRowExistence = readerRowExistence.HasRows; 
                    if (boolRowExistence == true) 
                    {
                        databaseConnection.Close(); 
                        MySqlCommand verifyWorkStartChecker = new MySqlCommand($"SELECT * FROM timer WHERE timer.startTime IS NOT NULL" + 
                            " AND timer.finishTime IS NULL AND timer.startDate=@startDate AND timer.email=@email");
                        verifyWorkStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); 
                        verifyWorkStartChecker.Parameters.AddWithValue("@email", adminCode); 
                        verifyWorkStartChecker.CommandType = CommandType.Text; 
                        verifyWorkStartChecker.Connection = databaseConnection; 
                        databaseConnection.Open(); 
                        MySqlDataReader readerVerifyWorkStart = verifyWorkStartChecker.ExecuteReader(); 
                        bool boolVerifyWorkStart = readerVerifyWorkStart.HasRows; 
                        if (boolVerifyWorkStart == true) 
                        {
                            databaseConnection.Close(); 
                            String addEndTimeQuery = "UPDATE timer SET finishTime=@finishTime WHERE startDate=@startDate AND email=@email"; 
                            using (MySqlCommand addEndTimeCommand = new MySqlCommand(addEndTimeQuery, databaseConnection)) 
                            {
                                addEndTimeCommand.Parameters.AddWithValue("@finishTime", DateTime.Now.ToString("H:mm")); 
                                addEndTimeCommand.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); 
                                addEndTimeCommand.Parameters.AddWithValue("@email", adminCode); 

                                databaseConnection.Open(); 
                                int queryFeedback = addEndTimeCommand.ExecuteNonQuery(); 
                                databaseConnection.Close(); 
                                MessageBox.Show("Praca zakończona"); 
                            }
                        } 
                        else { databaseConnection.Close(); MessageBox.Show("W dniu " + DateTime.Now.ToString("yyyy-MM-dd") + " zakończono już pracę"); }
                    }
                    else { databaseConnection.Close(); MessageBox.Show("W dniu " + DateTime.Now.ToString("yyyy-MM-dd") + " nie rozpoczęto jeszcze pracy"); }
                }
                catch { MessageBox.Show("Nieoczekiwany błąd weryfikacji rozpoczęcia pracy"); } 
            }
        }
        private void managedAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(managedAccount.SelectedItem != null)
            {
                string selectedItem = managedAccount.SelectedItem.ToString();
                adminCode = selectedItem;
                PathFactory pathFactory = new PathFactory();
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
                {
                    string connection = streamReader.ReadToEnd();
                    string connectionString = connection;
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                    MySqlCommand selectNameForTimerStatsQuery = new MySqlCommand($"SELECT name, surname FROM user WHERE email=@email");
                    selectNameForTimerStatsQuery.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    selectNameForTimerStatsQuery.Parameters.AddWithValue("@email", adminCode);
                    selectNameForTimerStatsQuery.CommandType = CommandType.Text;
                    selectNameForTimerStatsQuery.Connection = databaseConnection;
                    databaseConnection.Open();
                    using (MySqlDataReader readerName = selectNameForTimerStatsQuery.ExecuteReader())
                    {
                        readerName.Read();
                        workerName = readerName["name"].ToString();
                        workerSurname = readerName["surname"].ToString();
                        databaseConnection.Close();
                    }
                }
                if (timerPanel.Visible)       
                {
                    timerStats();
                }
                if (calendarJuicePanel.Visible)
                {
                    DisplayCurrentMonth();
                }
                if (adminApproveLeaveCalendarPanel.Visible)
                {
                    displayAdminCalendar();
                }
            }
        }
        private void managedAccount_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds); }
            else { e.Graphics.FillRectangle(Brushes.White, e.Bounds); }
            e.Graphics.DrawString(managedAccount.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }


        // ============================ OVERALL BODY END ====================================



        // ============================ TIMER PANEL START ===================================


        private void killTimerButton_Click(object sender, EventArgs e) //makes scary Timer go away
        {
            timerPanel.Visible = false;
            welcomeGroupbox.Visible = true;
        }
        private void styczeńToolStripMenuItem_Click(object sender, EventArgs e) //next twelve actions are my proud baby of lazines
        { //all this thing do is sets invisible label depending on which month was chosen from a list
            codeMonthLabel.Text = "1"; timerMonthLabel.Text = "styczeń"; timerStats();
        }
        private void lutyToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "2"; timerMonthLabel.Text = "luty"; timerStats(); }
        private void marzecToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "3"; timerMonthLabel.Text = "marzec"; timerStats(); }
        private void kwiecieńToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "4"; timerMonthLabel.Text = "kwiecień"; timerStats(); }
        private void majToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "5"; timerMonthLabel.Text = "maj"; timerStats(); }
        private void czerwiecToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "6"; timerMonthLabel.Text = "czerwiec"; timerStats(); }
        private void lipiecToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "7"; timerMonthLabel.Text = "lipiec"; timerStats(); }
        private void sierpieńToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "8"; timerMonthLabel.Text = "sierpień"; timerStats(); }
        private void wrzesieńToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "9"; timerMonthLabel.Text = "wrzesień"; timerStats(); }
        private void październikToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "10"; timerMonthLabel.Text = "październik"; timerStats(); }
        private void listopadToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "11"; timerMonthLabel.Text = "listopad"; timerStats(); }
        private void grudzieńToolStripMenuItem_Click(object sender, EventArgs e) { codeMonthLabel.Text = "12"; timerMonthLabel.Text = "grudzień"; timerStats(); }

        private void timerStats()
        {
            displayDateTextbox.Text = ""; //all of those are just to clear textboxes, so the data doesn't repeat itself
            displayFinishTimeTextbox.Text = "";
            displayStartTimeTextbox.Text = "";
            displayTimeSpanTextbox.Text = "";
            suggestedPayTextbox.Text = "";
            try //debug try, kinda afraid to delete it :/
            {
                PathFactory pathFactory = new PathFactory(); //path to use pathFactory
                using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
                {
                    try //real try this time
                    {
                        string connection = streamReader.ReadToEnd(); //let's read the connection first. Literally that
                        string connectionString = connection; //create connection string
                        MySqlConnection databaseConnection = new MySqlConnection(connectionString); //that's used here, it's to create connection with DB

                        MySqlCommand displayDateQuery = new MySqlCommand($"SELECT startDate FROM timer WHERE email=@email AND MONTH(startDate)=@month"); //an SQL query, kinda self explanatory
                        displayDateQuery.Parameters.AddWithValue("@email", adminCode); //takes login from viewUserTextbox
                        displayDateQuery.Parameters.AddWithValue("@month", codeMonthLabel.Text); //takes month from invisible label :)
                        displayDateQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                        displayDateQuery.Connection = databaseConnection; //idk connects with DB or something
                        databaseConnection.Open(); //starts the actual connection

                        MySqlDataReader reader = displayDateQuery.ExecuteReader(); //this boy executes my commands!
                        while (reader.Read()) //basically, as long as the command is still being read
                        {
                            DateTime date = reader.GetDateTime("startDate"); //sets date to startDate from DB
                            string formattedDate = date.ToString("yyyy-MM-dd"); // formats the date into MySQL approved system
                            displayDateTextbox.AppendText(formattedDate + Environment.NewLine);// append each date to the TextBox with a new line
                        }
                        databaseConnection.Close(); //ends the connection
                        reader.Close(); //and stops the reader
                    }
                    catch { MessageBox.Show("Błąd wczytywania daty z bazy danych"); } //if s*&t hits the fan, there's a failsafe - this is the failsafe
                }
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
                {
                    string connection = streamReader.ReadToEnd(); //let's read the connection first. Literally that
                    string connectionString = connection; //create connection string
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString); //that's used here, it's to create connection with DB

                    MySqlCommand startingTimeQuery = new MySqlCommand($"SELECT startTime FROM timer WHERE email=@email AND MONTH(startDate)=@month"); //an SQL query, kinda self explanatory
                    startingTimeQuery.Parameters.AddWithValue("@email", adminCode); //takes login from viewUserTextbox
                    startingTimeQuery.Parameters.AddWithValue("@month", codeMonthLabel.Text); //takes month from invisible label :)
                    startingTimeQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                    startingTimeQuery.Connection = databaseConnection; //idk connects with DB or something
                    databaseConnection.Open(); //starts the actual connection

                    MySqlDataAdapter startDataAdapter = new MySqlDataAdapter(startingTimeQuery); //it gets real, when you need an adapter
                    DataTable startingTimeTable = new DataTable(); //creates DataTable
                    startDataAdapter.Fill(startingTimeTable); //populates said table with data from adapter
                    StringBuilder stringBuilderStart = new StringBuilder(); //build string with data from DataTable
                    foreach (DataRow row in startingTimeTable.Rows) //now as long as there are rows in DataTable
                    {
                        for (int i = 0; i < startingTimeTable.Columns.Count; i++) //counts columns in DataTable and increses "i" every repeat
                        {
                            stringBuilderStart.Append(row[i].ToString()); //takes data from DataTable and makes it into string
                            if (i < startingTimeTable.Columns.Count - 1) //decreses imaginary "i" by one, you know to count how many more repeats are needed
                            {
                                stringBuilderStart.Append("\t"); //makes it so text in a string starts with new line every time
                            }
                        }
                        stringBuilderStart.AppendLine(); //I mean, it must do something, cause it doesn't work without it
                    }
                    databaseConnection.Close(); //shuts down the connection with DB
                    displayStartTimeTextbox.Text = stringBuilderStart.ToString(); //and puts all of those dates in correct textbox

                    //does exactly same thing as the previous one, but with finish time, so imma just copy the comments, I like this green text :)
                    MySqlCommand finishTimeQuery = new MySqlCommand($"SELECT finishTime FROM timer WHERE email=@email AND MONTH(startDate)=@month"); //an SQL query, kinda self explanatory
                    finishTimeQuery.Parameters.AddWithValue("@email", adminCode); //takes login from viewUserTextbox
                    finishTimeQuery.Parameters.AddWithValue("@month", codeMonthLabel.Text); //takes month from invisible label :)
                    finishTimeQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                    finishTimeQuery.Connection = databaseConnection; //idk connects with DB or something
                    databaseConnection.Open(); //starts the actual connection

                    MySqlDataAdapter endDataAdapter = new MySqlDataAdapter(finishTimeQuery); //it gets real, when you need an adapter
                    DataTable timeTable = new DataTable(); //creates DataTable
                    endDataAdapter.Fill(timeTable); //populates said table with data from adapter
                    StringBuilder stringBuilderEnd = new StringBuilder();//build string with data from DataTable
                    foreach (DataRow row in timeTable.Rows) //now as long as there are rows in DataTable
                    {
                        for (int i = 0; i < timeTable.Columns.Count; i++) //counts columns in DataTable and increses "i" every repeat
                        {
                            stringBuilderEnd.Append(row[i].ToString()); //takes data from DataTable and makes it into string
                            if (i < timeTable.Columns.Count - 1) //decreses imaginary "i" by one, you know to count how many more repeats are needed
                            {
                                stringBuilderEnd.Append("\t"); //makes it so text in a string starts with new line every time
                            }
                        }
                        stringBuilderEnd.AppendLine(); //It must do something!
                    }
                    databaseConnection.Close();//shuts down the connection with DB
                    displayFinishTimeTextbox.Text = stringBuilderEnd.ToString();//and puts all of those dates in correct textbox

                    
                    string[] startTimes = displayStartTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); 
                    string[] finishTimes = displayFinishTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); 
                    for (int i = 0; i < startTimes.Length; i++) 
                    {
                        if (DateTime.TryParse(startTimes[i], out DateTime startTime) && 
                            DateTime.TryParse(finishTimes[i], out DateTime finishTime))
                        {
                            totalDifference += finishTime - startTime;
                            string formattedDifference = $"{(int)totalDifference.TotalHours}:{totalDifference.Minutes:D2}"; 
                            displayTimeSpanTextbox.Text = formattedDifference.ToString(); 
                        }
                        else { MessageBox.Show($"Błąd w przekazywaniu danych w rzędzie: {i + 1}"); return; } 
                    }
                }
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
                {
                    string connection = streamReader.ReadToEnd(); //let's read the connection first. Literally that
                    string connectionString = connection; //create connection string
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString); //that's used here, it's to create connection with DB

                    MySqlCommand calculateWageQuery = new MySqlCommand($"SELECT wage FROM user WHERE email=@email"); //an SQL query, kinda self explanatory
                    calculateWageQuery.Parameters.AddWithValue("@email", adminCode); //takes login from viewUserTextbox
                    calculateWageQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                    calculateWageQuery.Connection = databaseConnection; //idk connects with DB or something
                    databaseConnection.Open(); //starts the actual connection
                    using (MySqlDataReader reader = calculateWageQuery.ExecuteReader()) //executes command
                    {
                        reader.Read(); //reads data recieved from query
                        int wage = reader.GetInt32("wage");
                        databaseConnection.Close(); //stops the connection
                    
                        int calculatedWage = CalculateWage(totalDifference, wage);
                        suggestedPayTextbox.Text = calculatedWage.ToString() + " PLN";
                    }
                }
                int CalculateWage(TimeSpan timeWorked, int hourlyWage)
                {
                    // Convert TimeSpan to total hours
                    double totalHours = timeWorked.TotalHours;

                    // Calculate total wage
                    int totalWage = (int)(totalHours * hourlyWage);

                    return totalWage;
                }
            }
            catch //like I said before, this is debug try-catch, I'm just afraid to delete it
            {
                timerPanel.Visible = true;
                mainCalendarPanel.Visible = false;
            }
        }
        private void generateTimerDataset_Click(object sender, EventArgs e)
        {
            try
            {
                string startTime = displayStartTimeTextbox.Text;
                string finishTime = displayFinishTimeTextbox.Text;
                string date = displayDateTextbox.Text;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string pdfFile = Path.Combine(desktopPath, "Godziny_pracy.pdf");
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFile, FileMode.Append));
                document.Open();
                
                document.Add(new Paragraph("Godziny pracy dla pracownika: " + workerName + " " + workerSurname));
                document.Add(new Paragraph("   "));
                
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;

                table.AddCell("Data");
                table.AddCell("Godzina rozpoczęcia:");
                table.AddCell("Godzina zakończenia:");

                table.AddCell(date);
                table.AddCell(startTime);
                table.AddCell(finishTime);
                document.Add(table);

                document.Close();
                MessageBox.Show("Raport wygenerowany");
            }
            catch { MessageBox.Show("Błąd generowania raportu"); }
        }


        // ============================ TIMER PANEL END =====================================



        // ============================ CALENDAR PANEL START ================================


        public void DisplayCurrentMonth() 
        {
            calendarJuicePanel.Controls.Clear(); 
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1); 
            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth); 
            int dayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; 
            monthLabel.Text = firstDayOfMonth.ToString("MMMM", polishCulture); 
            yearLabel.Text = firstDayOfMonth.ToString("yyyy" + ","); 
            for (int i = 0; i < dayOfWeek; i++) 
            { 
                EmptyUserControl emptyUserControl = new EmptyUserControl(); 
                calendarJuicePanel.Controls.Add(emptyUserControl); 
            } 
            for (int day = 1; day <= daysInMonth; day++)
            {
                DayUserControl dayControl = new DayUserControl();
                dayControl.SetDay(day, new DateTime(currentYear, currentMonth, day));
                calendarJuicePanel.Controls.Add(dayControl);
            }
        }
        private void killCalendarButton_Click(object sender, EventArgs e) //makes scary Calendar go away
        {
            mainCalendarPanel.Visible = false;
            welcomeGroupbox.Visible = true;
        }
        private void previousButton_Click(object sender, EventArgs e) //changes month to previous one
        {
            if (currentMonth == 1) //simple maths really, if currentMonth is equal to one(january)
            {
                currentMonth = 12; //then sets currentMonth to twelve(december)
                currentYear--; //and decreses year
            }
            else
            {
                currentMonth--; //else just decreses month by one
            }
            DisplayCurrentMonth(); //also calls a function
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            if (currentMonth == 12) //same thing here, but in reverse, if month is 12(december)
            {
                currentMonth = 1; //then sets currenMonth to one(January)
                currentYear++; //and increses year by one
            }
            else
            {
                currentMonth++; //else just increses month by one
            }
            DisplayCurrentMonth(); //and still calls a function, that's like the main thing
        }

        private void categoryChecklist_ItemCheck(object sender, ItemCheckEventArgs e) //makes sure only one category can be picked
        {
            if (e.NewValue == CheckState.Checked) //when new value is picked
            {
                for (int i = 0; i < categoryChecklist.Items.Count; i++) //checks how many boxes are checked
                {
                    if (i != e.Index) { categoryChecklist.SetItemChecked(i, false); } //literally just wipes the previous check
                }
            }
        }
        private void descriptionTextbox_Enter(object sender, EventArgs e)
        {
            if (descriptionTextbox.Text == PlaceholderText)
            {
                descriptionTextbox.Text = "";
                descriptionTextbox.ForeColor = TextColor;
            }
        }
        private void descriptionTextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(descriptionTextbox.Text))
            {
                descriptionTextbox.Text = PlaceholderText;
                descriptionTextbox.ForeColor = PlaceholderColor;
            }
        }
        private void oneDayEventCheckbox_CheckedChanged(object sender, EventArgs e) //handles sytuation when it's a one day only event
        {
            if (oneDayEventCheckbox.Checked) //if textbox is checked
            {
                finishEventDatePicker.Enabled = false; //makes finishEventDatePicker read-only
                finishEventDatePicker.Value = startEventDatePicker.Value; //sets the same value in finishEventDatePicker as in startEventDatePicker
                startEventDatePicker.ValueChanged += startEventDatePicker_ValueChanged; //handle the ValueChanged event of startEventDatePicker to keep finishEventDatePicker updated
            }
            else //when unchecked
            {
                finishEventDatePicker.Enabled = true; //enables finishEventDatePicker
                startEventDatePicker.ValueChanged -= startEventDatePicker_ValueChanged; // stop handling the ValueChanged event of startEventDatePicker
            }
        }

        private void startEventDatePicker_ValueChanged(object sender, EventArgs e)
        {
            finishEventDatePicker.Value = startEventDatePicker.Value; //sync the date of both DatePickers
        }
        private void allDayEventCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (allDayEventCheckbox.Checked)
            {
                startTimePicker.Enabled = false;
                finishTimePicker.Enabled = false;
                startTimePicker.Value = new DateTime(currentYear, currentMonth, currentDay, 00, 00, 00);
                finishTimePicker.Value = new DateTime(currentYear, currentMonth, currentDay, 23, 59, 59);
                startTimePicker.ValueChanged += StartTimePicker_ValueChanged;
            }
            else
            {
                startTimePicker.Enabled = true;
                finishTimePicker.Enabled = true;
                startTimePicker.ValueChanged += StartTimePicker_ValueChanged;
            }
        }

        private void StartTimePicker_ValueChanged(object sender, EventArgs e)
        {
            finishTimePicker.Value = startTimePicker.Value;
        }

        private void addEventButton_Click(object sender, EventArgs e)
        {
            var checkedItems = categoryChecklist.CheckedItems;
            string checkboxListValue = string.Empty;
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                String addEventQuery = "INSERT INTO `events`(`email`, `eventCategory`, `eventDescription`, `eventStartDate`, `eventFinishDate`, `eventStartTime`, `eventFinishTime`) VALUES (@email,@eventCategory,@eventDescription,@eventStartDate,@eventFinishDate,@eventStartTime,@eventFinishTime)";

                try
                {
                    bool checkChecker = false;
                    foreach (int index in categoryChecklist.CheckedIndices) { checkChecker = true; break; }
                    if (checkChecker)
                    {
                        try
                        {
                            foreach (var categoryItem in checkedItems)
                            {
                                checkboxListValue += categoryItem.ToString();
                            }
                            using (MySqlCommand addEventCommand = new MySqlCommand(addEventQuery, databaseConnection))
                            {
                                addEventCommand.Parameters.AddWithValue("@email", adminCode);
                                addEventCommand.Parameters.AddWithValue("@eventCategory", checkboxListValue);
                                addEventCommand.Parameters.AddWithValue("@eventDescription", descriptionTextbox.Text);
                                addEventCommand.Parameters.AddWithValue("@eventStartDate", startEventDatePicker.Value);
                                addEventCommand.Parameters.AddWithValue("@eventFinishDate", finishEventDatePicker.Value);
                                addEventCommand.Parameters.AddWithValue("@eventStartTime", startTimePicker.Value);
                                addEventCommand.Parameters.AddWithValue("@eventFinishTime", finishTimePicker.Value);

                                databaseConnection.Open();
                                int queryFeedback = addEventCommand.ExecuteNonQuery();
                                databaseConnection.Close();
                            }
                            MessageBox.Show("Wydarzenie dodane");
                        }
                        catch { MessageBox.Show("Błąd dodawania wydarzenia do bazy danych"); }
                    }
                    else { MessageBox.Show("Należy zaznaczyć kategorię wydarzenia!"); }
                }
                catch { MessageBox.Show("Nieoczekiwany błąd dodawania wydarzenia do bazy danych"); }
            }
            DisplayCurrentMonth();
        }


        // ============================ CALENDAR PANEL END ================================


        // ============================ LEAVE PANEL START =================================


        private void killLeavePanel_Click(object sender, EventArgs e)
        {
            leavePanel.Visible = false;
            welcomeGroupbox.Visible = true;
        }

        private void reasonChecklist_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox reasonChecklist = sender as CheckedListBox;
            for (int i = 0; i < reasonChecklist.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    reasonChecklist.SetItemChecked(i, false);
                }
            }
            switch (e.Index)
            {
                case 0:
                    if (e.NewValue == CheckState.Checked) //Paid leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = false;
                        doctorsNoticePictureBox.Enabled = false;
                        addBLOB.Enabled = false;
                        addBLOB.BackColor = Color.Gainsboro;
                        reasonDescriptionLabel.ForeColor = Color.Gray;
                        availableDaysLabel.Visible = true;
                        availableDaysTextBox.Visible = true;
                        collectAvailableDaysPaid();
                        adminNum = 1;
                    }
                    break;
                case 1:
                    if (e.NewValue == CheckState.Checked) //Unpaid leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = false;
                        doctorsNoticePictureBox.Enabled = false;
                        addBLOB.Enabled = false;
                        addBLOB.BackColor = Color.Gainsboro;
                        reasonDescriptionLabel.ForeColor = Color.Gray;
                        availableDaysLabel.Visible = false;
                        availableDaysTextBox.Visible = false;
                        adminNum = 0;
                    }
                    break;
                case 2:
                    if (e.NewValue == CheckState.Checked) //sick leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = false;
                        doctorsNoticePictureBox.Enabled = true;
                        addBLOB.Enabled = true;
                        addBLOB.BackColor = Color.Honeydew;
                        reasonDescriptionLabel.ForeColor = Color.Gray;
                        availableDaysLabel.Visible = false;
                        availableDaysTextBox.Visible = false;
                        adminNum = 0;
                    }
                    break;
                case 3:
                    if (e.NewValue == CheckState.Checked) //occasional leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = true;
                        doctorsNoticePictureBox.Enabled = false;
                        addBLOB.Enabled = false;
                        addBLOB.BackColor = Color.Gainsboro;
                        reasonDescriptionLabel.ForeColor = Color.Black;
                        availableDaysLabel.Visible = false;
                        availableDaysTextBox.Visible = false;
                        adminNum = 0;
                    }
                    break;
                case 4:
                    if (e.NewValue == CheckState.Checked) //maternity leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = false;
                        doctorsNoticePictureBox.Enabled = false;
                        addBLOB.Enabled = false;
                        addBLOB.BackColor = Color.Gainsboro;
                        reasonDescriptionLabel.ForeColor = Color.Gray;
                        availableDaysLabel.Visible = false;
                        availableDaysTextBox.Visible = false;
                        adminNum = 0;
                    }
                    break;
                case 5:
                    if (e.NewValue == CheckState.Checked) //taternity leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = false;
                        doctorsNoticePictureBox.Enabled = false;
                        addBLOB.Enabled = false;
                        addBLOB.BackColor = Color.Gainsboro;
                        reasonDescriptionLabel.ForeColor = Color.Gray;
                        availableDaysLabel.Visible = false;
                        availableDaysTextBox.Visible = false;
                        adminNum = 0;
                    }
                    break;
                case 6:
                    if (e.NewValue == CheckState.Checked) //blood donor leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = false;
                        doctorsNoticePictureBox.Enabled = true;
                        addBLOB.Enabled = true;
                        addBLOB.BackColor = Color.Honeydew;
                        reasonDescriptionLabel.ForeColor = Color.Gray;
                        availableDaysLabel.Visible = false;
                        availableDaysTextBox.Visible = false;
                        adminNum = 0;
                    }
                    break;
                case 7:
                    if (e.NewValue == CheckState.Checked) //on demand leave
                    {
                        reasonDescriptionTextbox.Text = "";
                        doctorsNoticePictureBox.Image = null;
                        reasonDescriptionTextbox.Enabled = false;
                        doctorsNoticePictureBox.Enabled = false;
                        addBLOB.Enabled = false;
                        addBLOB.BackColor = Color.Gainsboro;
                        reasonDescriptionLabel.ForeColor = Color.Gray;
                        availableDaysLabel.Visible = true;
                        availableDaysTextBox.Visible = true;
                        collectAvailableDaysOnDemand();
                        adminNum = 2;
                    }
                    break;
            }
        }
        private void collectAvailableDaysPaid()
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd(); //let's read the connection first. Literally that
                string connectionString = connection; //create connection string
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //that's used here, it's to create connection with DB

                MySqlCommand calculateWageQuery = new MySqlCommand($"SELECT leftLeaveDays FROM user WHERE email=@email"); //an SQL query, kinda self explanatory
                calculateWageQuery.Parameters.AddWithValue("@email", adminCode); //takes login from viewUserTextbox
                calculateWageQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                calculateWageQuery.Connection = databaseConnection; //idk connects with DB or something
                databaseConnection.Open(); //starts the actual connection
                using (MySqlDataReader reader = calculateWageQuery.ExecuteReader()) //executes command
                {
                    reader.Read(); //reads data recieved from query
                    int leftLeaveDays = reader.GetInt32("leftLeaveDays");
                    databaseConnection.Close(); //stops the connection
                    availableDaysTextBox.Text = leftLeaveDays.ToString();
                    dayCountPaid = leftLeaveDays;
                }
            }
        }
        private void collectAvailableDaysOnDemand() 
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd(); //let's read the connection first. Literally that
                string connectionString = connection; //create connection string
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //that's used here, it's to create connection with DB

                MySqlCommand calculateWageQuery = new MySqlCommand($"SELECT leftLeaveOnDemandDays FROM user WHERE email=@email"); //an SQL query, kinda self explanatory
                calculateWageQuery.Parameters.AddWithValue("@email", adminCode); //takes login from viewUserTextbox
                calculateWageQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                calculateWageQuery.Connection = databaseConnection; //idk connects with DB or something
                databaseConnection.Open(); //starts the actual connection
                using (MySqlDataReader reader = calculateWageQuery.ExecuteReader()) //executes command
                {
                    reader.Read(); //reads data recieved from query
                    int leftLeaveDays = reader.GetInt32("leftLeaveOnDemandDays");
                    databaseConnection.Close(); //stops the connection
                    availableDaysTextBox.Text = leftLeaveDays.ToString();
                    dayCountOnDemand = leftLeaveDays;
                }
            }
        }
        private void oneDayLeave_CheckedChanged(object sender, EventArgs e)
        {
            if (oneDayLeave.Checked) //if textbox is checked
            {
                leaveFinishDatePicker.Enabled = false; //makes finishEventDatePicker read-only
                leaveFinishDatePicker.Value = startEventDatePicker.Value; //sets the same value in finishEventDatePicker as in startEventDatePicker
                leaveStartDatePicker.ValueChanged += leaveStartDatePicker_ValueChanged; //handle the ValueChanged event of startEventDatePicker to keep finishEventDatePicker updated
            }
            else //when unchecked
            {
                leaveFinishDatePicker.Enabled = true; //enables finishEventDatePicker
                leaveStartDatePicker.ValueChanged -= leaveStartDatePicker_ValueChanged; // stop handling the ValueChanged event of startEventDatePicker
            }
        }

        private void leaveStartDatePicker_ValueChanged(object sender, EventArgs e)
        {
            leaveFinishDatePicker.Value = leaveStartDatePicker.Value;
        }

        private void fullDayLeave_CheckedChanged(object sender, EventArgs e)
        {
            if (fullDayLeave.Checked)
            {
                leaveStartTimePicker.Enabled = false;
                leaveFinishTimePicker.Enabled = false;
                leaveStartTimePicker.Value = new DateTime(currentYear, currentMonth, currentDay, 00, 00, 00);
                leaveFinishTimePicker.Value = new DateTime(currentYear, currentMonth, currentDay, 23, 59, 59);
                leaveStartTimePicker.ValueChanged += leaveStartTimePicker_ValueChanged;
            }
            else
            {
                leaveStartTimePicker.Enabled = true;
                leaveFinishTimePicker.Enabled = true;
                leaveStartTimePicker.ValueChanged += leaveStartTimePicker_ValueChanged;
            }
        }

        private void leaveStartTimePicker_ValueChanged(object sender, EventArgs e)
        {
            leaveFinishTimePicker.Value = leaveStartTimePicker.Value;
        }
        private void addBLOB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPictureDialog = new OpenFileDialog();

            // image filters  
            openPictureDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.jfif)|*.jpg; *.jpeg; *.gif; *.bmp; *.jfif";
            openPictureDialog.Title = "Wybierz zdjęcie adnotacji od lekarza";
            if (openPictureDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openPictureDialog.FileName);
                if (fileInfo.Length > 1 * 1024 * 1024) // Check if file size exceeds 1MB
                {
                    MessageBox.Show("Plik przekracza maksymalną wielkość (1MB) proszę dodać mniejszy plik!");
                    return;
                }
                // Read the entire file into a byte array
                imageBytes = File.ReadAllBytes(openPictureDialog.FileName);

                // Load the image into the PictureBox
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    doctorsNoticePictureBox.Load(openPictureDialog.FileName);
                    doctorsNoticePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    deleteLabel.Visible = true;
                }
            }
        }
        private void doctorsNoticePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            doctorsNoticePictureBox.Image = null;
            deleteLabel.Visible = false;
        }
        private void doctorsNoticePictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (doctorsNoticePictureBox.Image != null)
            {
                Cursor = Cursors.Hand;
            }
        }
        private void doctorsNoticePictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (doctorsNoticePictureBox.Image != null)
            {
                Cursor = Cursors.Default;
            }
        }
        private void applyLeaveButton_Click(object sender, EventArgs e)
        {

            var checkedItems = reasonChecklist.CheckedItems;
            string checkboxListValue = string.Empty;
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                String addLeaveQuery = "INSERT INTO `leavetable`(`email`, `leaveStartDate`, `leaveFinishDate`, `leaveStartTime`, `leaveFinishTime`, `leaveApproved`, `leaveReason`, `leaveDescription`, `leaveBLOB`) "
                                    + "VALUES (@email, @leaveStartDate, @leaveFinishDate, @leaveStartTime, @leaveFinishTime, @leaveApproved, @leaveReason, @leaveDescription, @leaveBLOB)";
                bool checkChecker = false;
                foreach (int index in reasonChecklist.CheckedIndices) { checkChecker = true; break; }
                if (checkChecker)
                {
                    try
                    {
                        foreach (var reasonItem in checkedItems)
                        {
                            checkboxListValue += reasonItem.ToString();
                        }
                        using (MySqlCommand addLeaveCommand = new MySqlCommand(addLeaveQuery, databaseConnection))
                        {
                            isApproved =  1;
                            databaseConnection.Open();
                            addLeaveCommand.Parameters.AddWithValue("@email", adminCode);
                            addLeaveCommand.Parameters.AddWithValue("@leaveStartDate", leaveStartDatePicker.Value);
                            addLeaveCommand.Parameters.AddWithValue("@leaveFinishDate", leaveFinishDatePicker.Value);
                            addLeaveCommand.Parameters.AddWithValue("@leaveStartTime", leaveStartTimePicker.Value);
                            addLeaveCommand.Parameters.AddWithValue("@leaveFinishTime", leaveFinishTimePicker.Value);
                            addLeaveCommand.Parameters.AddWithValue("@leaveApproved", isApproved);
                            addLeaveCommand.Parameters.AddWithValue("@leaveReason", checkboxListValue);
                            addLeaveCommand.Parameters.AddWithValue("@leaveDescription", reasonDescriptionTextbox.Text);
                            if (doctorsNoticePictureBox != null)
                            {
                                addLeaveCommand.Parameters.Add("@leaveBLOB", MySqlDbType.MediumBlob).Value = imageBytes;
                            }
                            else
                            {
                                addLeaveCommand.Parameters.AddWithValue("@leaveBLOB", MySqlDbType.MediumBlob).Value = DBNull.Value;
                            }

                            addLeaveCommand.ExecuteNonQuery();
                            databaseConnection.Close();
                            if (isApproved == 0)
                            {
                                MessageBox.Show("Nieobecność zgłoszona, oczekuje na potwierdzenie");
                                calculateLeaveDays();
                            }
                            else if (isApproved == 1)
                            {
                                MessageBox.Show("Nieobecność potwierdzona");
                                calculateLeaveDays();
                            }
                        }
                    }
                    catch { MessageBox.Show("Błąd dodawania nieobecności"); }
                }
            }
        }
        private void calculateLeaveDays()
        {
            DateTime leaveStart = leaveStartDatePicker.Value;
            DateTime leaveFinish = leaveFinishDatePicker.Value;

            if(oneDayLeave.Checked)
            {
                dayCount = 1;
            }
            else
            {
                dayCount = (leaveFinish - leaveStart).Days;
            }
            dayCountPaid = dayCountPaid - dayCount;
            dayCountOnDemand = dayCountOnDemand - dayCount;

            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
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
                        updateLeaveCommand.Parameters.AddWithValue("@email", adminCode);
                        updateLeaveCommand.Parameters.AddWithValue("@leftLeaveDays", dayCountPaid);
                        int result = updateLeaveCommand.ExecuteNonQuery();
                    }
                }
                
            }
            else if(adminNum == 2)
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
                        updateLeaveCommand.Parameters.AddWithValue("@email", adminCode);
                        updateLeaveCommand.Parameters.AddWithValue("@leftLeaveOnDemandDays", dayCountOnDemand);
                        int result = updateLeaveCommand.ExecuteNonQuery();
                    }
                }
            }
            
        }




        // ============================ LEAVE PANEL FINISH ===============================


        // ============================ ADMIN PANEL START ================================
        private void manageUsers_Click(object sender, EventArgs e)
        {
            manageUsersPanel.Visible = true;
            approveLeavePanel.Visible = false;
            adminPanelBG.Visible = false;
        }
        private void leaveApproval_Click(object sender, EventArgs e)
        {
            approveLeavePanel.Visible = true;
            manageUsersPanel.Visible = false;
            adminPanelBG.Visible = false;
            displayAdminCalendar();
            displayNotApprovedEmails();
        }
        private void loadManagedUser_Click(object sender, EventArgs e)
        {
            clearAdminTextboxes();
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"

                MySqlCommand displayManagedUser = new MySqlCommand($"SELECT * FROM user WHERE email=@email"); //query to find name based on email
                displayManagedUser.Parameters.AddWithValue("@email", adminCode); //takes email from textbox
                displayManagedUser.CommandType = CommandType.Text; //makes command readable for the app
                displayManagedUser.Connection = databaseConnection; //does something?

                databaseConnection.Open(); //opens connection
                using (MySqlDataReader reader = displayManagedUser.ExecuteReader()) //executes command
                {
                    reader.Read(); //reads data recieved from query
                    adminUser = reader["userID"].ToString();
                    adminEmailTextbox.Text = reader["email"].ToString();
                    adminPasswordTextbox.Text = reader["password"].ToString();
                    int isAdminTMP = reader.GetInt32("isAdmin");
                    if (isAdminTMP == 1)
                    { isAdminCheckbox.Checked = true; }
                    else { isAdminCheckbox.Checked = false; }
                    adminNameTextbox.Text = reader["name"].ToString();
                    adminSurnameTextbox.Text = reader["surname"].ToString();
                    DateTime birthdate = reader.GetDateTime("birthdate");
                    adminBirthdateTextbox.Text = birthdate.ToString();
                    adminPeselTextbox.Text = reader["pesel"].ToString();
                    adminContractTextbox.Text = reader["contract"].ToString();
                    adminWageTextbox.Text = reader["wage"].ToString();
                    adminLeaveDaysTextbox.Text = reader["leaveDays"].ToString();
                    adminLeftLeaveDaysTextbox.Text = reader["leftLeaveDays"].ToString();
                    adminLeaveOnDemandDaysTextbox.Text = reader["leaveOnDemandDays"].ToString();
                    adminLeftOnDemandLeaveDaysTextbox.Text = reader["leftLeaveOnDemandDays"].ToString();
                    
                    databaseConnection.Close(); //stops the connection
                }
            }
        }
        private void clearAdminTextboxes()
        {
            adminNameTextbox.Text = "";
            adminSurnameTextbox.Text = "";
            adminBirthdateTextbox.Text = "";
            adminPeselTextbox.Text = "";
            adminContractTextbox.Text = "";
            adminWageTextbox.Text = "";
            adminLeaveDaysTextbox.Text = "";
            adminLeftLeaveDaysTextbox.Text = "";
            adminLeaveOnDemandDaysTextbox.Text = "";
            adminLeftOnDemandLeaveDaysTextbox.Text = "";
        }

        private void adminPasswordTextbox_MouseEnter(object sender, EventArgs e)
        {
            adminPasswordTextbox.PasswordChar = '\0';
        }

        private void adminPasswordTextbox_MouseLeave(object sender, EventArgs e)
        {
            adminPasswordTextbox.PasswordChar = '*';
        }

        private void adminSaveUser_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"

                MySqlCommand saveUserQuery = new MySqlCommand($"SELECT email FROM user WHERE email=@email"); //query to find name based on email
                saveUserQuery.Parameters.AddWithValue("@email", adminEmailTextbox.Text); //takes email from textbox
                saveUserQuery.CommandType = CommandType.Text; //makes command readable for the app
                saveUserQuery.Connection = databaseConnection;
                databaseConnection.Open(); //opens connection
                MySqlDataReader reader = saveUserQuery.ExecuteReader();//does something?
                try
                {
                    if (reader.HasRows)
                    {
                        updateUserOnSave();
                        MessageBox.Show("Dane użytkownika zmodyfikowane");
                    }
                    else if (adminEmailTextbox.Text == "")
                    {
                        MessageBox.Show("Wypełnij formularz");
                        databaseConnection.Close();
                    }
                    else
                    {
                        addUserOnSave();
                        MessageBox.Show("Nowy użytkownik dodany");
                    }
                }
                catch { MessageBox.Show("Błąd zapisu danych"); }
            }
        }
        
        private void updateUserOnSave()
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"
                databaseConnection.Open();
                String updateUserQuery = "UPDATE `user` SET `email`=@email,`password`=@password,`isAdmin`=@isAdmin,`name`=@name,`surname`=@surname,`birthdate`=@birthdate,`pesel`=@pesel,`contract`=@contract,`wage`=@wage,`leaveDays`=@leaveDays,`leaveOnDemandDays`=@leaveOnDemandDays WHERE userID=@userID";
                using (MySqlCommand updateUserCommand = new MySqlCommand(updateUserQuery, databaseConnection))
                {
                    updateUserCommand.Parameters.AddWithValue("@userID", adminUser);
                    updateUserCommand.Parameters.AddWithValue("@email", adminEmailTextbox.Text);
                    updateUserCommand.Parameters.AddWithValue("@password", adminPasswordTextbox.Text);
                    updateUserCommand.Parameters.AddWithValue("@isAdmin", isAdminCheckbox.Checked);
                    updateUserCommand.Parameters.AddWithValue("@name", adminNameTextbox.Text);
                    updateUserCommand.Parameters.AddWithValue("@surname", adminSurnameTextbox.Text);
                    string birthdateString = adminBirthdateTextbox.Text;
                    DateTime.TryParse(birthdateString, out DateTime birthdate);
                    updateUserCommand.Parameters.AddWithValue("@birthdate", birthdate);
                    updateUserCommand.Parameters.AddWithValue("@pesel", adminPeselTextbox.Text);
                    updateUserCommand.Parameters.AddWithValue("@contract", adminContractTextbox.Text);
                    updateUserCommand.Parameters.AddWithValue("@wage", adminWageTextbox.Text);
                    updateUserCommand.Parameters.AddWithValue("@leaveDays", adminLeaveDaysTextbox.Text);
                    updateUserCommand.Parameters.AddWithValue("@leaveOnDemandDays", adminLeaveOnDemandDaysTextbox.Text);

                    int result = updateUserCommand.ExecuteNonQuery();
                }
                databaseConnection.Close();
            }
        }
        private void addUserOnSave()
        {
            adminLeftLeaveDaysTextbox.Text = adminLeaveDaysTextbox.Text;
            adminLeftOnDemandLeaveDaysTextbox.Text = adminLeaveOnDemandDaysTextbox.Text;
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"
                databaseConnection.Open();
                string insertUserQuery = "INSERT INTO `user`(`email`, `password`, `isAdmin`, `name`, `surname`, `birthdate`, `pesel`, `contract`, `wage`, `leaveDays`, `leftLeaveDays`, `leaveOnDemandDays`, `leftLeaveOnDemandDays`) VALUES (@email, @password, @isAdmin, @name, @surname, @birthdate, @pesel, @contract, @wage, @leaveDays, @leftLeaveDays, @leaveOnDemandDays, @leftLeaveOnDemandDays)";
                using (MySqlCommand insertUserCommand = new MySqlCommand(insertUserQuery, databaseConnection))
                {
                    insertUserCommand.Parameters.AddWithValue("@email", adminEmailTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@password", adminPasswordTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@isAdmin", isAdminCheckbox.Checked);
                    insertUserCommand.Parameters.AddWithValue("@name", adminNameTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@surname", adminSurnameTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@birthdate", adminBirthdateTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@pesel", adminPeselTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@contract", adminContractTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@wage", adminWageTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@leaveDays", adminLeaveDaysTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@leftleaveDays", adminLeftLeaveDaysTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@leaveOnDemandDays", adminLeaveOnDemandDaysTextbox.Text);
                    insertUserCommand.Parameters.AddWithValue("@leftleaveOnDemandDays", adminLeftOnDemandLeaveDaysTextbox.Text);

                    int result = insertUserCommand.ExecuteNonQuery();
                }
                databaseConnection.Close();
                selectManagedUsers();
            }
        }
        private void displayNotApprovedEmails()
        {
            adminEmailListbox.Items.Clear();
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //renames connection :)
                string selectNotApprovedLeavesQuery = "SELECT DISTINCT user.email FROM user LEFT JOIN leavetable ON user.email = leavetable.email WHERE leavetable.leaveApproved = 0";
                using (MySqlConnection databaseConnection = new MySqlConnection(connection))
                {
                    MySqlCommand selectNotApprovedLeavesCommand = new MySqlCommand(selectNotApprovedLeavesQuery, databaseConnection);
                    databaseConnection.Open();
                    MySqlDataReader reader = selectNotApprovedLeavesCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string email = reader["email"].ToString();
                        string displayText = email;
                        adminEmailListbox.Items.Add(displayText);
                    }

                    reader.Close();
                }
            }
        }
        private void displayAdminCalendar()
        {
            adminCalendarLayoutPanel.Controls.Clear(); //first and formost it clears up the "main" calendar panel
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1); //sets the first time of the month
            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth); //then counts how many days are in said month
            int dayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; //sets the first day of the month and makes sure the week start with monday
            adminApproveMonthLabel.Text = firstDayOfMonth.ToString("MMMM", polishCulture); // changes monthLabel to correct month in polish
            adminApproveYearLabel.Text = firstDayOfMonth.ToString("yyyy" + ","); //sets yearLabel to correct year
            for (int i = 0; i < dayOfWeek; i++) //this is where magic begins
            { //first, it finds how many days, from monday happend in previous month, if i is smaller then int of firstDayOfMonth
                EmptyUserControl emptyUserControl = new EmptyUserControl(); //sets usercontrol as usercontrol
                adminCalendarLayoutPanel.Controls.Add(emptyUserControl); //and populates one plot in tabelPanel
            } //repeat if necessary
            for (int day = 1; day <= daysInMonth; day++)
            {
                AdminCalendarUserControl dayControl = new AdminCalendarUserControl();
                dayControl.SetDay(day, new DateTime(currentYear, currentMonth, day));
                adminCalendarLayoutPanel.Controls.Add(dayControl);
            }
        }

        private void adminEmailListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (adminEmailListbox.SelectedItem != null)
            {
                string selectedItem = adminEmailListbox.SelectedItem.ToString();
                adminCode = selectedItem;
                displayAdminCalendar();
            }
        }

        private void adminEmailListbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds); }
            else { e.Graphics.FillRectangle(Brushes.White, e.Bounds); }
            e.Graphics.DrawString(adminEmailListbox.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void adminPreviousMonthButton_Click(object sender, EventArgs e)
        {
            if (currentMonth == 1) //simple maths really, if currentMonth is equal to one(january)
            {
                currentMonth = 12; //then sets currentMonth to twelve(december)
                currentYear--; //and decreses year
            }
            else
            {
                currentMonth--; //else just decreses month by one
            }
            displayAdminCalendar();
        }

        private void adminNextMonthButton_Click(object sender, EventArgs e)
        {
            if (currentMonth == 12) //same thing here, but in reverse, if month is 12(december)
            {
                currentMonth = 1; //then sets currenMonth to one(January)
                currentYear++; //and increses year by one
            }
            else
            {
                currentMonth++; //else just increses month by one
            }
            displayAdminCalendar();
        }

        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                String deleteUserQuery = "DELETE FROM user WHERE email=@email AND password=@password";
                using (MySqlCommand deleteUserCommand = new MySqlCommand(deleteUserQuery, databaseConnection))
                {
                    deleteUserCommand.Parameters.AddWithValue("@email", adminEmailTextbox.Text);
                    deleteUserCommand.Parameters.AddWithValue("@password", adminPasswordTextbox.Text);

                    databaseConnection.Open();
                    int queryFeedback = deleteUserCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
            }
            MessageBox.Show("Użytkownik " + adminEmailTextbox.Text + " został usunięty");
            clearAdminTextboxes();
            adminEmailTextbox.Text = "";
            adminPasswordTextbox.Text = "";
        }




        // ============================ ADMIN PANEL FINISH ===============================
    }
}