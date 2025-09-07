using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class subscription_select : Form
    {
        public subscription_select()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            make_subscription ss = new make_subscription();
            ss.Show();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            subscription_history ss = new subscription_history();
            ss.Show();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            app_settings app_Settings = new app_settings();
            app_Settings.Show();
            this.Hide();    
        }
    }
}
