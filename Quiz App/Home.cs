using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Home : BaseForm
    {
        private Panel splashCard;
        private Label footerCreditLabel;
        private Label splashEyebrowLabel;

        public Home()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Width += Math.Max(10, panel2.Width / 50);
            label1.Text = "Loading platform";

            if (panel1.Width >= panel2.Width)
            {
                timer1.Stop();
                Welcome welcomeForm = new Welcome();
                welcomeForm.Show();
                Hide();
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            ModernUi.ScaleForScreen(this);
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(8, 12, 22), Color.FromArgb(24, 40, 70));
            BuildSplashLayout();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void BuildSplashLayout()
        {
            SuspendLayout();

            BackColor = Color.FromArgb(8, 12, 22);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            Opacity = 0.98D;

            int splashWidth = Math.Min(780, ClientSize.Width - 56);
            int splashHeight = 380;
            int splashLeft = (ClientSize.Width - splashWidth) / 2;
            int splashTop = Math.Max(40, (ClientSize.Height - splashHeight) / 2 - 8);

            panel2.BackColor = Color.FromArgb(34, 52, 82);
            panel2.Height = 12;
            panel2.Width = Math.Max(420, splashWidth - 180);
            panel2.Left = (ClientSize.Width - panel2.Width) / 2;
            panel2.Top = splashTop + splashHeight + 24;

            panel1.BackColor = ModernUi.Accent;
            panel1.Location = new Point(0, 0);
            panel1.Height = panel2.Height;
            panel1.Width = 0;

            if (splashCard == null)
            {
                splashCard = ModernUi.CreateCard(new Rectangle(splashLeft, splashTop, splashWidth, splashHeight));
                Controls.Add(splashCard);
                splashCard.SendToBack();
            }
            else
            {
                splashCard.Bounds = new Rectangle(splashLeft, splashTop, splashWidth, splashHeight);
            }

            EnsureSplashChrome();

            splashEyebrowLabel.Parent = splashCard;
            splashEyebrowLabel.Location = new Point((splashCard.Width - splashEyebrowLabel.Width) / 2, 22);

            pictureBox1.Parent = splashCard;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Size = new Size(280, 150);
            pictureBox1.Location = new Point((splashCard.Width - pictureBox1.Width) / 2, 66);

            label2.Parent = splashCard;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = ModernUi.Ink;
            label2.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Text = "Moses & Grace College CBT";
            label2.AutoSize = false;
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Size = new Size(660, 52);
            label2.Location = new Point((splashCard.Width - label2.Width) / 2, 224);

            label3.Parent = splashCard;
            label3.BackColor = Color.Transparent;
            label3.ForeColor = ModernUi.MutedInk;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Text = "Preparing a secure and focused exam workspace.";
            label3.AutoSize = false;
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Size = new Size(540, 30);
            label3.Location = new Point((splashCard.Width - label3.Width) / 2, 276);

            label1.ForeColor = ModernUi.Ink;
            label1.Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Text = "Loading platform";
            label1.AutoSize = false;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Width = panel2.Width;
            label1.Left = panel2.Left;
            label1.Top = panel2.Top - 30;

            footerCreditLabel.Left = (ClientSize.Width - footerCreditLabel.Width) / 2;
            footerCreditLabel.Top = panel2.Bottom + 12;

            ResumeLayout();
        }

        private void EnsureSplashChrome()
        {
            if (splashEyebrowLabel == null)
            {
                splashEyebrowLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(220, 24),
                    Text = "COMPUTER-BASED TESTING",
                    Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = ModernUi.Accent,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                splashCard.Controls.Add(splashEyebrowLabel);
            }

            if (footerCreditLabel == null)
            {
                footerCreditLabel = new Label
                {
                    Parent = this,
                    AutoSize = true,
                    Text = "Designed by Emerald Code Studio",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = ModernUi.MutedInk,
                    BackColor = Color.Transparent
                };
                Controls.Add(footerCreditLabel);
            }
        }
    }
}

