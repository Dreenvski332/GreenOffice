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
    public partial class DayUserControl : UserControl
    {
        public DayUserControl()
        {
            InitializeComponent();
        }
        private int _dayNumber;
        public int DayNumber
        {
            get { return _dayNumber; }
            set
            {
                _dayNumber = value;
                dayLabel.Text = _dayNumber.ToString();
            }
        }
    }
}
