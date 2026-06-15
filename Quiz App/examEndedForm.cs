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
    public partial class examEndedForm : BaseForm
    {
        public examEndedForm()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            studentlogin sl = new studentlogin();
            sl.Show();
            this.Hide();
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

        private void examEndedForm_Load(object sender, EventArgs e)
        {
            examEndedForm.ScaleForm(this);
            ApplyModernEndedLayout();
        }

        private void ApplyModernEndedLayout()
        {
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(10, 16, 29), Color.FromArgb(28, 20, 24));
            BackColor = Color.FromArgb(12, 19, 34);

            guna2HtmlLabel1.Text = "Exam Ended";
            guna2HtmlLabel1.Font = new Font("Segoe UI Semibold", 34F, FontStyle.Bold, GraphicsUnit.Point);
            guna2HtmlLabel1.ForeColor = Color.White;
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Location = new Point(118, 122);

            Label subtitle = Controls.OfType<Label>().FirstOrDefault(l => l.Name == "runtimeEndedSubtitle");
            if (subtitle == null)
            {
                subtitle = new Label
                {
                    Name = "runtimeEndedSubtitle",
                    Parent = this,
                    BackColor = Color.Transparent
                };
                Controls.Add(subtitle);
            }

            subtitle.Text = "Your theory responses have been captured. Return to the login screen when you are done.";
            subtitle.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            subtitle.ForeColor = ModernUi.MutedInk;
            subtitle.Location = new Point(124, 204);
            subtitle.Size = new Size(ClientSize.Width - 248, 54);

            guna2Button1.Text = "Return To Login";
            guna2Button1.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            guna2Button1.FillColor = ModernUi.Accent;
            guna2Button1.ForeColor = Color.FromArgb(8, 20, 28);
            guna2Button1.BorderRadius = 18;
            guna2Button1.Location = new Point(124, 304);
            guna2Button1.Size = new Size(ClientSize.Width - 248, 60);
        }
    }
}

