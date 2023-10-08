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
    public partial class f3_adminBody : Form
    {
        public f3_adminBody()
        {
            InitializeComponent();
            timerGroupbox.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e) //timer button
        {
            timerGroupbox.Visible = true;
        }
    }
}
