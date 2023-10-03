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
    public partial class f2_conSettings : Form
    {
        public f2_conSettings()
        {
            InitializeComponent();
            PathFactory pathFactory = new PathFactory();
            using (StreamReader streamReaderSource = new StreamReader(pathFactory.sourceString))
            {
                sourceTextbox.Text = streamReaderSource.ReadToEnd();
            }
            using (StreamReader streamReaderPort = new StreamReader(pathFactory.portString))
            {
                portTextbox.Text = streamReaderPort.ReadToEnd();
            }
            using (StreamReader streamReaderUsername = new StreamReader(pathFactory.usernameString))
            {
                usernameTextbox.Text = streamReaderUsername.ReadToEnd();
            }
            using (StreamReader streamReaderPassword = new StreamReader(pathFactory.passwordString))
            {
                passwordTextbox.Text = streamReaderPassword.ReadToEnd();
            }
            using (StreamReader streamReaderDatabase = new StreamReader(pathFactory.databaseString))
            {
                databaseTextbox.Text = streamReaderDatabase.ReadToEnd();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            PathFactory pathFactory = new PathFactory();
            using (StreamWriter connectionWriter = new StreamWriter(pathFactory.connString))
            {
                connectionWriter.Write($"datasource=" + sourceTextbox.Text + "; port=" + portTextbox.Text + "; username=" + usernameTextbox.Text + "; password=" + passwordTextbox.Text + "; database=" + databaseTextbox.Text);
            }
            using (StreamWriter sourceWriter = new StreamWriter(pathFactory.sourceString))
            {
                sourceWriter.Write(sourceTextbox.Text);
            }
            using (StreamWriter portWriter = new StreamWriter(pathFactory.portString))
            {
                portWriter.Write(portTextbox.Text);
            }
            using (StreamWriter usernameWriter = new StreamWriter(pathFactory.usernameString))
            {
                usernameWriter.Write(usernameTextbox.Text);
            }
            using (StreamWriter passwordWriter = new StreamWriter(pathFactory.passwordString))
            {
                passwordWriter.Write(passwordTextbox.Text);
            }
            using (StreamWriter databaseWriter = new StreamWriter(pathFactory.databaseString))
            {
                databaseWriter.Write(databaseTextbox.Text);
            }
            this.Close();
        }
    }
}
