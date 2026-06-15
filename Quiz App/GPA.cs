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
    public partial class GPA : BaseForm
    {
        public GPA()
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
            single_GPA stt = new single_GPA();
            stt.Show();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            multi_GPA gg = new multi_GPA();
            gg.Show();
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            Form2 w = new Form2();
            w.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GPA_Load(object sender, EventArgs e)
        {
            GPA.ScaleForm(this);
        }
    }
}

