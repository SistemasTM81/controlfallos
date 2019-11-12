using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            label1.Left = (this.Width - label1.Width) / 2;
            pictureBox1.Left = (this.Width -  pictureBox1.Width)/2;
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
