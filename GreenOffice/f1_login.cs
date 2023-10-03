using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenOffice
{
    public partial class f1_login : Form
    {
        public f1_login()
        {
            InitializeComponent();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
                f2_conSettings Open_f2_conSettings = new f2_conSettings();
                Open_f2_conSettings.ShowDialog();
        }
    }
}
