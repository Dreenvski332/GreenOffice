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
using System.Collections.Generic;

namespace GreenOffice
{
    public partial class f4_userBody : Form
    {
        private int currentYear;
        private int currentMonth;
        private int currentDay;
        private readonly CultureInfo polishCulture;
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
        public static string calendarCode = "";

        // ============================ OVERALL BODY START ==================================


        public f4_userBody()
        {
            InitializeComponent();
            viewUserTextbox.Text = f1_login.email;
            timerPanel.Visible = false;
            mainCalendarPanel.Visible = false;
            currentYear = DateTime.Now.Year;
            currentMonth = DateTime.Now.Month;
            currentDay = DateTime.Now.Day;
            polishCulture = new CultureInfo("pl-PL");
            calendarJuicePanel.RowCount = rowCount;
            calendarJuicePanel.ColumnCount = columnCount;
            startTimePicker.CustomFormat = "hh:mm tt";
            finishTimePicker.CustomFormat = "hh:mm tt";
            leaveStartTimePicker.CustomFormat = "hh:mm tt";
            leaveFinishTimePicker.CustomFormat = "hh:mm tt";
            isApproved = 0;
            calendarCode = viewUserTextbox.Text;
            selectCalendarUsers();
            displayedCalendarAccount.DrawMode = DrawMode.OwnerDrawFixed;

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

            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand displayName = new MySqlCommand($"SELECT email, name, surname FROM user WHERE email=@email");
                displayName.Parameters.AddWithValue("@email", viewUserTextbox.Text);
                displayName.CommandType = CommandType.Text;
                displayName.Connection = databaseConnection;

                databaseConnection.Open();
                using (MySqlDataReader readerDisplayName = displayName.ExecuteReader())
                {
                    readerDisplayName.Read();
                    nameWelcomeTextbox.Text = readerDisplayName["name"].ToString() + "!";
                    workerName = readerDisplayName["name"].ToString();
                    workerSurname = readerDisplayName["surname"].ToString();
                    databaseConnection.Close();
                }
            }

        }
        private void f4_userBody_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void calendarButton_Click(object sender, EventArgs e)
        {
            timerPanel.Visible = false;
            welcomeGroupbox.Visible = false;
            mainCalendarPanel.Visible = true;
            subCalendarPanel.Visible = true;
            calendarJuicePanel.Visible = true;
            leavePanel.Visible = false;
            displayedAccountPanel.Visible = true;
            DisplayCurrentMonth();
        }
        private void timerPanelButton_Click(object sender, EventArgs e)
            { 
            timerPanel.Visible = true; 
            welcomeGroupbox.Visible = false;
            mainCalendarPanel.Visible = false;
            subCalendarPanel.Visible = false;
            calendarJuicePanel.Visible = false;
            leavePanel.Visible = false;
            displayedAccountPanel.Visible = false;
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1);
            codeMonthLabel.Text = firstDayOfMonth.ToString("MM");
            timerMonthLabel.Text = firstDayOfMonth.ToString("MMMM", polishCulture);
            timerStats();
        }
        private void timeoutButton_Click(object sender, EventArgs e)
        {
            timerPanel.Visible = false;
            mainCalendarPanel.Visible = false;
            subCalendarPanel.Visible = false;
            calendarJuicePanel.Visible = false;
            welcomeGroupbox.Visible = false;
            leavePanel.Visible = true;
            displayedAccountPanel.Visible = false;
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
                    MySqlCommand workStartChecker = new MySqlCommand($"SELECT startDate, username FROM timer WHERE startDate=@startDate AND username=@username");
                    workStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    workStartChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text);
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

            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                try
                {
                    MySqlCommand rowExistenceChecker = new MySqlCommand($"SELECT * FROM timer WHERE startDate=@startDate AND username=@username");
                    rowExistenceChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd")); 
                    rowExistenceChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text);
                    rowExistenceChecker.CommandType = CommandType.Text;
                    rowExistenceChecker.Connection = databaseConnection;
                    databaseConnection.Open();
                    MySqlDataReader readerRowExistence = rowExistenceChecker.ExecuteReader();
                    bool boolRowExistence = readerRowExistence.HasRows;
                    if (boolRowExistence == true)
                    { 
                        databaseConnection.Close();
                        MySqlCommand verifyWorkStartChecker = new MySqlCommand($"SELECT * FROM timer WHERE timer.startTime IS NOT NULL AND timer.finishTime IS NULL AND timer.startDate=@startDate AND timer.username=@username");
                        verifyWorkStartChecker.Parameters.AddWithValue("@startDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        verifyWorkStartChecker.Parameters.AddWithValue("@username", viewUserTextbox.Text);
                        verifyWorkStartChecker.CommandType = CommandType.Text;
                        verifyWorkStartChecker.Connection = databaseConnection;
                        databaseConnection.Open();
                        MySqlDataReader readerVerifyWorkStart = verifyWorkStartChecker.ExecuteReader();
                        bool boolVerifyWorkStart = readerVerifyWorkStart.HasRows;
                        if (boolVerifyWorkStart == true)
                        { 
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
        


        // ============================ OVERALL BODY END ====================================



        // ============================ TIMER PANEL START ===================================


        private void killTimerButton_Click(object sender, EventArgs e)
        {
            timerPanel.Visible = false;
            welcomeGroupbox.Visible = true;
        }
        private void styczeńToolStripMenuItem_Click(object sender, EventArgs e)
        { 
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
            displayDateTextbox.Text = "";
            displayFinishTimeTextbox.Text = "";
            displayStartTimeTextbox.Text = "";
            displayTimeSpanTextbox.Text = "";
            try
            {
                PathFactory pathFactory = new PathFactory();
                using (StreamReader streamReader = new StreamReader(pathFactory.connString))
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
                            string formattedDate = date.ToString("yyyy-MM-dd");
                            displayDateTextbox.AppendText(formattedDate + Environment.NewLine);
                        }
                        databaseConnection.Close();
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
                    databaseConnection.Close();
                    displayStartTimeTextbox.Text = stringBuilderStart.ToString();

                    MySqlCommand finishTimeQuery = new MySqlCommand($"SELECT finishTime FROM timer WHERE username=@login AND MONTH(startDate)=@month");
                    finishTimeQuery.Parameters.AddWithValue("@login", viewUserTextbox.Text);
                    finishTimeQuery.Parameters.AddWithValue("@month", codeMonthLabel.Text);
                    finishTimeQuery.CommandType = CommandType.Text;
                    finishTimeQuery.Connection = databaseConnection;
                    databaseConnection.Open();

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
                    databaseConnection.Close();
                    displayFinishTimeTextbox.Text = stringBuilderEnd.ToString();

                    string[] startTimes = displayStartTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    string[] finishTimes = displayFinishTimeTextbox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    TimeSpan totalDifference = TimeSpan.Zero;
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
            }
            catch
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
        private void selectCalendarUsers()
        {
            string selectedEmail = viewUserTextbox.Text;
            List<string> emails = new List<string>();
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                using (MySqlConnection databaseConnection = new MySqlConnection(connectionString))
                {
                    string displayedCalendarAccountQuery = $"SELECT email FROM user";
                    databaseConnection.Open();
                    MySqlCommand displayedCalendarAccountCommand = new MySqlCommand(displayedCalendarAccountQuery, databaseConnection);
                    MySqlDataReader reader = displayedCalendarAccountCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string email = reader.GetString("email");
                        emails.Add(email);
                    }
                }
            }
            if (emails.Contains(selectedEmail))
            {
                emails.Remove(selectedEmail);
                emails.Insert(0, selectedEmail);
            }

            displayedCalendarAccount.DataSource = emails;
        }
        private void displayedCalendarAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = displayedCalendarAccount.SelectedItem.ToString();
            calendarCode = selectedItem;
            DisplayCurrentMonth();
        }
        private void displayedCalendarAccount_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds); }
            else { e.Graphics.FillRectangle(Brushes.White, e.Bounds); }
            e.Graphics.DrawString(displayedCalendarAccount.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

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
        private void killCalendarButton_Click(object sender, EventArgs e)
        {
            mainCalendarPanel.Visible = false;
            welcomeGroupbox.Visible = true;
            displayedAccountPanel.Visible = false;
        }
        private void previousButton_Click(object sender, EventArgs e)
        {
            if (currentMonth == 1)
            {
                currentMonth = 12;
                currentYear--;
            }
            else
            {
                currentMonth--;
            }
            DisplayCurrentMonth();
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            if (currentMonth == 12)
            {
                currentMonth = 1;
                currentYear++;
            }
            else
            {
                currentMonth++;
            }
            DisplayCurrentMonth();
        }

        private void categoryChecklist_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < categoryChecklist.Items.Count; i++)
                {
                    if (i != e.Index) { categoryChecklist.SetItemChecked(i, false); }
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
        private void oneDayEventCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (oneDayEventCheckbox.Checked)
            {
                finishEventDatePicker.Enabled = false;
                finishEventDatePicker.Value = startEventDatePicker.Value;
                startEventDatePicker.ValueChanged += startEventDatePicker_ValueChanged;
            }
            else
            {
                finishEventDatePicker.Enabled = true;
                startEventDatePicker.ValueChanged -= startEventDatePicker_ValueChanged;
            }
        }

        private void startEventDatePicker_ValueChanged(object sender, EventArgs e)
        {
            finishEventDatePicker.Value = startEventDatePicker.Value;
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
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand calculateWageQuery = new MySqlCommand($"SELECT leftLeaveDays FROM user WHERE email=@email");
                calculateWageQuery.Parameters.AddWithValue("@email", viewUserTextbox.Text);
                calculateWageQuery.CommandType = CommandType.Text;
                calculateWageQuery.Connection = databaseConnection;
                databaseConnection.Open();
                using (MySqlDataReader reader = calculateWageQuery.ExecuteReader())
                {
                    reader.Read();
                    int leftLeaveDays = reader.GetInt32("leftLeaveDays");
                    databaseConnection.Close();
                    availableDaysTextBox.Text = leftLeaveDays.ToString();
                }
            }
        }
        private void collectavailableDaysOnDemand()
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReader = new StreamReader(pathFactory.connString))
            {
                string connection = streamReader.ReadToEnd();
                string connectionString = connection;
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                MySqlCommand calculateWageQuery = new MySqlCommand($"SELECT leftLeaveOnDemandDays FROM user WHERE email=@email");
                calculateWageQuery.Parameters.AddWithValue("@email", viewUserTextbox.Text);
                calculateWageQuery.CommandType = CommandType.Text;
                calculateWageQuery.Connection = databaseConnection;
                databaseConnection.Open();
                using (MySqlDataReader reader = calculateWageQuery.ExecuteReader())
                {
                    reader.Read();
                    int leftLeaveDays = reader.GetInt32("leftLeaveOnDemandDays");
                    databaseConnection.Close();
                    availableDaysTextBox.Text = leftLeaveDays.ToString();
                }
            }
        }
        private void oneDayLeave_CheckedChanged(object sender, EventArgs e)
        {
            if (oneDayLeave.Checked)
            {
                leaveFinishDatePicker.Enabled = false;
                leaveFinishDatePicker.Value = startEventDatePicker.Value;
                leaveStartDatePicker.ValueChanged += leaveStartDatePicker_ValueChanged;
            }
            else
            {
                leaveFinishDatePicker.Enabled = true;
                leaveStartDatePicker.ValueChanged -= leaveStartDatePicker_ValueChanged;
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

            openPictureDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.jfif)|*.jpg; *.jpeg; *.gif; *.bmp; *.jfif";
            openPictureDialog.Title = "Wybierz zdjęcie adnotacji od lekarza";
            if (openPictureDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openPictureDialog.FileName);
                if (fileInfo.Length > 1 * 1024 * 1024)
                {
                    MessageBox.Show("Plik przekracza maksymalną wielkość (1MB) proszę dodać mniejszy plik!");
                    return;
                }
                imageBytes = File.ReadAllBytes(openPictureDialog.FileName);
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
