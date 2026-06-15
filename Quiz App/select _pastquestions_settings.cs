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
    public partial class select__pastquestions_settings : BaseForm
    {
        public select__pastquestions_settings()
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
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form4 w = new Form4();
            w.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            past_questions_settings w = new past_questions_settings();
            w.Show();
            this.Hide();
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

        private void select__pastquestions_settings_Load(object sender, EventArgs e)
        {
            select__pastquestions_settings.ScaleForm(this);
        }
    }
}

