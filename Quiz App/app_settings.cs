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
    public partial class app_settings : BaseForm
    {
        public app_settings()
        {
            InitializeComponent();
        }

        private const int BaseWidth = 1920;
        private const int BaseHeight = 1080;

        public static void ScaleForm(Form form)
        {
            // Get current screen resolution
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calculate scale factors
            float scaleX = (float)screenWidth / BaseWidth;
            float scaleY = (float)screenHeight / BaseHeight;

            // Apply scaling to form and controls
            form.Scale(new SizeF(scaleX, scaleY));

            // Adjust font scaling (optional, but makes UI balanced)
            foreach (Control c in form.Controls)
            {
                c.Font = new Font(c.Font.FontFamily, c.Font.Size * Math.Min(scaleX, scaleY));
            }

            // Center form
            form.StartPosition = FormStartPosition.CenterScreen;
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
            app_settings.ScaleForm(this);
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

