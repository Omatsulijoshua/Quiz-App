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
    public partial class app_settings : Form
    {
        public app_settings()
        {
            InitializeComponent();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form2 stt = new Form2();
            stt.Show();
            this.Hide();
        }

        private void app_settings_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            set_course_credit stt = new set_course_credit();
            stt.Show();
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            mastersheet_mode stt = new mastersheet_mode();
            stt.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            subscription_select stt = new subscription_select();
            stt.Show();
            this.Hide();
        }
    }
}
