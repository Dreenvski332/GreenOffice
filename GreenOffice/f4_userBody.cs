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

namespace GreenOffice
{
    public partial class f4_userBody : Form
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
        private int dayCount;
        private int dayCountPaid;
        private int dayCountOnDemand;
        private int adminNum;
        private string workerName;
        private string workerSurname;

        // ============================ OVERALL BODY START ==================================


        public f4_userBody()
        {
            InitializeComponent(); //you know starts the whole ordeal
            viewUserTextbox.Text = f1_login.email; //sets user email, puts it into textbox - taken from login screen
            timerPanel.Visible = false; //makes scary Timer not appear at first
            mainCalendarPanel.Visible = false; //makes scary Calendar not appear at first
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

            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"

                MySqlCommand displayName = new MySqlCommand($"SELECT email, name, surname FROM user WHERE email=@email"); //query to find name based on email
                displayName.Parameters.AddWithValue("@email", viewUserTextbox.Text); //takes email from textbox
                displayName.CommandType = CommandType.Text; //makes command readable for the app
                displayName.Connection = databaseConnection; //does something?

                databaseConnection.Open(); //opens connection
                using (MySqlDataReader readerDisplayName = displayName.ExecuteReader()) //executes command
                {
                    readerDisplayName.Read(); //reads data recieved from query
                    nameWelcomeTextbox.Text = readerDisplayName["name"].ToString() + "!"; //shoots name into a textbox with "!" at the end
                    workerName = readerDisplayName["name"].ToString();
                    workerSurname = readerDisplayName["surname"].ToString();
                    databaseConnection.Close(); //stops the connection
                }
            }

        }
        private void f4_userBody_FormClosing(object sender, FormClosingEventArgs e)
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
        }
        private void timerStartButton_Click(object sender, EventArgs e) //starts workday timer
        {
            PathFactory pathFactory = new PathFactory(); //path to use pathFactory
            using (StreamReader streamReader = new StreamReader(pathFactory.connString)) //loads path from pathFactory - from file "connString"
            {
                string connection = streamReader.ReadToEnd(); //reads "connString" file
                string connectionString = connection; //and makes a connection
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //sets connection to database as "connectionString"
                try
                { //checks if there is a ROW in "timer" that contains current date and currently logged user
                    MySqlCommand workStartChecker = new MySqlCommand($"SELECT startDate, username FROM timer WHERE startDate=@startDate AND username=@username"); //a query
                    workStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); //sets startTime to current time
                    workStartChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text); //takes username from textbox
                    workStartChecker.CommandType = CommandType.Text; //makes it so the program knows what a command is
                    workStartChecker.Connection = databaseConnection; // ummmm

                    databaseConnection.Open(); //opens the connection
                    MySqlDataReader readerWorkStart = workStartChecker.ExecuteReader(); //executes reader, not in a kill way
                    bool boolWorkStart = readerWorkStart.HasRows; //checks if there actually are any matching rows in a DB
                    if (boolWorkStart == true) //if that row exists, that means the work has already started for the day
                    { 
                        databaseConnection.Close(); //so we need to stop the connection
                        MessageBox.Show("Praca dnia " + DateTime.Now.ToString("yyyy-MM-dd") + " została już rozpoczęta"); //and notify user about that
                    }
                    else //if it doesn't then starts work for the day
                    { 
                        databaseConnection.Close(); //still we neet to halt the connection
                        String addStartingTimeQuery = "INSERT INTO `timer`(`username`, `startDate`, `startTime`) VALUES (@username,@startDate,@startTime)"; //define new query to add data into the DB
                        try
                        {
                            using (MySqlCommand addStartingTimeCommand = new MySqlCommand(addStartingTimeQuery, databaseConnection)) //connects to DB
                            {
                                addStartingTimeCommand.Parameters.AddWithValue("@startTime", DateTime.Now.ToString("H:mm")); //sets startTime to current time
                                addStartingTimeCommand.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); //sets current date
                                addStartingTimeCommand.Parameters.AddWithValue("@username", viewUserTextbox.Text); //takes username from textbox
                                databaseConnection.Open(); //opens the connection
                                int queryFeedback = addStartingTimeCommand.ExecuteNonQuery(); //executes the command, again not in a kill way
                                databaseConnection.Close(); //stops the connection
                                MessageBox.Show("Praca rozpoczęta"); //time to notify user about that
                            }
                        }
                        catch { MessageBox.Show("Nieoczekiwany błąd zaczynania pracy"); } //error is latest query mucks up
                    }
                }
                catch { MessageBox.Show("Nieoczewkiwany błąd sprawdzania rejestru pracy na dzień " + DateTime.Now.ToString("yyyy-MM-dd")); } //error if something failed during checking whether there are any matching rows in the DB
            }
        }

        private void endTimerButton_Click(object sender, EventArgs e) //stops the timer for the day
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
                    rowExistenceChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); //take startDate as a current date
                    rowExistenceChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text); //username from textbox, we've been over this
                    rowExistenceChecker.CommandType = CommandType.Text; //program stupid, needs to know what a command is
                    rowExistenceChecker.Connection = databaseConnection; //ugh
                    databaseConnection.Open(); //opens the connection
                    MySqlDataReader readerRowExistence = rowExistenceChecker.ExecuteReader(); //makes command actually do something, in this case just sends it to DB to retrieve data
                    bool boolRowExistence = readerRowExistence.HasRows; //checks if there are any matching rows at all
                    if (boolRowExistence == true) //if row like that exists, then program checks whether the work has been ended for the day or not
                    { 
                        databaseConnection.Close(); //QUERY BELLOW is looking for a ROW where startTime is set, but the endTime isn't \/
                        MySqlCommand verifyWorkStartChecker = new MySqlCommand($"SELECT * FROM timer WHERE timer.startTime IS NOT NULL AND timer.finishTime IS NULL AND timer.startDate=@startDate AND timer.username=@username");
                        verifyWorkStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); //take startDate as a current date
                        verifyWorkStartChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text); //username from textbox, we've really been over this
                        verifyWorkStartChecker.CommandType = CommandType.Text; //program stupid, needs to know what a command is
                        verifyWorkStartChecker.Connection = databaseConnection; //hmpf
                        databaseConnection.Open(); //opens the connection
                        MySqlDataReader readerVerifyWorkStart = verifyWorkStartChecker.ExecuteReader(); //makes command actually do something, in this case just sends it to DB to retrieve data
                        bool boolVerifyWorkStart = readerVerifyWorkStart.HasRows; //checks if there are any matching rows at all
                        if (boolVerifyWorkStart == true) //if a row like that exists then program updates said row with endTime - that is time when button was pressed
                        { 
                            databaseConnection.Close(); //stops the connection
                            String addEndTimeQuery = "UPDATE timer SET finishTime=@finishTime WHERE startDate=@startDate AND username=@username"; //update query, that's a fresh one here
                            using (MySqlCommand addEndTimeCommand = new MySqlCommand(addEndTimeQuery, databaseConnection)) //makes the query into a command
                            {
                                addEndTimeCommand.Parameters.AddWithValue("@finishTime", DateTime.Now.ToString("H:mm")); //sets finishTime as a current time
                                addEndTimeCommand.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); //sets startDate as current date
                                addEndTimeCommand.Parameters.AddWithValue("@username", viewUserTextbox.Text); //we all know what this does

                                databaseConnection.Open(); //opens the connection
                                int queryFeedback = addEndTimeCommand.ExecuteNonQuery(); //makes connection work
                                databaseConnection.Close(); //ends the connection
                                MessageBox.Show("Praca zakończona"); //also a message not to leave user standing
                            }
                        } //if row where startTime is set, and endTime isn't doesn't exist, then the only other option is that startTime and endTime are both set
                        else { databaseConnection.Close(); MessageBox.Show("W dniu " + DateTime.Now.ToString("yyyy-MM-dd") + " zakończono już pracę"); }
                    }//this is because option where startTime isn't set was filtered earlier, and there is no way to set endTime without startTime
                    else { databaseConnection.Close(); MessageBox.Show("W dniu " + DateTime.Now.ToString("yyyy-MM-dd") + " nie rozpoczęto jeszcze pracy"); }
                }
                catch { MessageBox.Show("Nieoczekiwany błąd weryfikacji rozpoczęcia pracy"); } //the ultimate error
            }
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

                        MySqlCommand displayDateQuery = new MySqlCommand($"SELECT startDate FROM timer WHERE username=@login AND MONTH(startDate)=@month"); //an SQL query, kinda self explanatory
                        displayDateQuery.Parameters.AddWithValue("@login", viewUserTextbox.Text); //takes login from viewUserTextbox
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

                    MySqlCommand startingTimeQuery = new MySqlCommand($"SELECT startTime FROM timer WHERE username=@login AND MONTH(startDate)=@month"); //an SQL query, kinda self explanatory
                    startingTimeQuery.Parameters.AddWithValue("@login", viewUserTextbox.Text); //takes login from viewUserTextbox
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
                    MySqlCommand finishTimeQuery = new MySqlCommand($"SELECT finishTime FROM timer WHERE username=@login AND MONTH(startDate)=@month"); //an SQL query, kinda self explanatory
                    finishTimeQuery.Parameters.AddWithValue("@login", viewUserTextbox.Text); //takes login from viewUserTextbox
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

                    //this part right here counts how many hours an employee has on the clock
                    string[] startTimes = displayStartTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //creates array for startTime
                    string[] finishTimes = displayFinishTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //creates array for finishTime
                    TimeSpan totalDifference = TimeSpan.Zero; //makes timeSpan == 0
                    for (int i = 0; i < startTimes.Length; i++) // iterate through each pair of start and finish times
                    {
                        if (DateTime.TryParse(startTimes[i], out DateTime startTime) && // parse the strings to DateTime
                            DateTime.TryParse(finishTimes[i], out DateTime finishTime))
                        {
                            totalDifference += finishTime - startTime;// subtract finish time from start time and add to total difference
                            string formattedDifference = $"{(int)totalDifference.TotalHours}:{totalDifference.Minutes:D2}"; //make it into a string
                            displayTimeSpanTextbox.Text = formattedDifference.ToString(); //because we need a string to display it here
                        }
                        else { MessageBox.Show($"Błąd w przekazywaniu danych w rzędzie: {i + 1}"); return; } //just an error
                    }
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
                bool fileExists = File.Exists(pdfFile);
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

                // Close the document
                document.Close();
                MessageBox.Show("Raport wygenerowany");
            }
            catch { MessageBox.Show("Błąd generowania raportu"); }
        }


        // ============================ TIMER PANEL END =====================================



        // ============================ CALENDAR PANEL START ================================


        public void DisplayCurrentMonth() //this bad boy is a function called in other parts of calendar
        {
            calendarJuicePanel.Controls.Clear(); //first and formost it clears up the "main" calendar panel
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1); //sets the first time of the month
            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth); //then counts how many days are in said month
            int dayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; //sets the first day of the month and makes sure the week start with monday
            monthLabel.Text = firstDayOfMonth.ToString("MMMM", polishCulture); // changes monthLabel to correct month in polish
            yearLabel.Text = firstDayOfMonth.ToString("yyyy" + ","); //sets yearLabel to correct year
            for (int i = 0; i < dayOfWeek; i++) //this is where magic begins
            { //first, it finds how many days, from monday happend in previous month, if i is smaller then int of firstDayOfMonth
                EmptyUserControl emptyUserControl = new EmptyUserControl(); //sets usercontrol as usercontrol
                calendarJuicePanel.Controls.Add(emptyUserControl); //and populates one plot in tabelPanel
            } //repeat if necessary
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
                startTimePicker.Enabled= true;
                finishTimePicker.Enabled= true;
                startTimePicker.ValueChanged += StartTimePicker_ValueChanged;
            }
        }

        private void StartTimePicker_ValueChanged(object sender, EventArgs e)
        {
            finishTimePicker.Value= startTimePicker.Value;
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
                    foreach(int index in categoryChecklist.CheckedIndices) { checkChecker = true; break; }
                    if (checkChecker)
                    {
                        try
                        {
                            foreach(var categoryItem in checkedItems)
                            {
                                checkboxListValue += categoryItem.ToString();
                            }
                            using (MySqlCommand addEventCommand = new MySqlCommand(addEventQuery, databaseConnection))
                            {
                                addEventCommand.Parameters.AddWithValue("@email", viewUserTextbox.Text);
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
                        isApproved = 0;
                        collectavailableDaysPaid();
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
                        isApproved = 0;
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
                        isApproved = 1;
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
                        isApproved = 1;
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
                        isApproved = 0;
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
                        isApproved = 0;
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
                        isApproved = 1;
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
                        collectavailableDaysOnDemand();
                        isApproved = 1;
                        adminNum = 2;
                    }
                    break;
            }
        }
        private void collectavailableDaysPaid()
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd(); //let's read the connection first. Literally that
                string connectionString = connection; //create connection string
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //that's used here, it's to create connection with DB

                MySqlCommand calculateWageQuery = new MySqlCommand($"SELECT leftLeaveDays FROM user WHERE email=@email"); //an SQL query, kinda self explanatory
                calculateWageQuery.Parameters.AddWithValue("@email", viewUserTextbox.Text); //takes login from viewUserTextbox
                calculateWageQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                calculateWageQuery.Connection = databaseConnection; //idk connects with DB or something
                databaseConnection.Open(); //starts the actual connection
                using (MySqlDataReader reader = calculateWageQuery.ExecuteReader()) //executes command
                {
                    reader.Read(); //reads data recieved from query
                    int leftLeaveDays = reader.GetInt32("leftLeaveDays");
                    databaseConnection.Close(); //stops the connection
                    availableDaysTextBox.Text = leftLeaveDays.ToString();
                }
            }
        }
        private void collectavailableDaysOnDemand()
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd(); //let's read the connection first. Literally that
                string connectionString = connection; //create connection string
                MySqlConnection databaseConnection = new MySqlConnection(connectionString); //that's used here, it's to create connection with DB

                MySqlCommand calculateWageQuery = new MySqlCommand($"SELECT leftLeaveOnDemandDays FROM user WHERE email=@email"); //an SQL query, kinda self explanatory
                calculateWageQuery.Parameters.AddWithValue("@email", viewUserTextbox.Text); //takes login from viewUserTextbox
                calculateWageQuery.CommandType = CommandType.Text; //makes sure the program can read the query
                calculateWageQuery.Connection = databaseConnection; //idk connects with DB or something
                databaseConnection.Open(); //starts the actual connection
                using (MySqlDataReader reader = calculateWageQuery.ExecuteReader()) //executes command
                {
                    reader.Read(); //reads data recieved from query
                    int leftLeaveDays = reader.GetInt32("leftLeaveOnDemandDays");
                    databaseConnection.Close(); //stops the connection
                    availableDaysTextBox.Text = leftLeaveDays.ToString();
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
                            databaseConnection.Open();
                            addLeaveCommand.Parameters.AddWithValue("@email", viewUserTextbox.Text);
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

            if (oneDayLeave.Checked)
            {
                dayCount = 1;
            }
            else
            {
                dayCount = (leaveFinish - leaveStart).Days;
            }
            dayCountPaid = dayCountPaid - dayCount;
            dayCountOnDemand = dayCountOnDemand - dayCount;
            if(dayCount > dayCountPaid || dayCount >  dayCountOnDemand)
            {
                MessageBox.Show("Wybrana nieobecność przekracza ilość dostępnych dni wolnych");
            }
            else
            {
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
                            updateLeaveCommand.Parameters.AddWithValue("@email", viewUserTextbox.Text);
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
                            updateLeaveCommand.Parameters.AddWithValue("@email", viewUserTextbox.Text);
                            updateLeaveCommand.Parameters.AddWithValue("@leftLeaveOnDemandDays", dayCountOnDemand);
                            int result = updateLeaveCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }


        // ============================ LEAVE PANEL END ===================================
    }
}
