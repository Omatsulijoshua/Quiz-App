using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Admin_Logincs : BaseForm
    {
        public static string fk_ad;
        private Panel heroPanel;
        private Label heroEyebrowLabel;
        private Label heroTitleLabel;
        private Label heroCopyLabel;
        private bool layoutEventsAttached;

        public Admin_Logincs()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;
            string userCount;
            string passwordFromDatabase;

            return_class dataReader = new return_class();
            userCount = dataReader.scalerReturn("select COUNT(ad_id) from tbl_admin where ad_name = '" + userName + "' ");

            if (userCount.Equals("0"))
            {
                MessageBox.Show("Invalid Username");
            }
            else
            {
                passwordFromDatabase = dataReader.scalerReturn("select ad_password from tbl_admin where ad_name = '" + userName + "' ");

                if (passwordFromDatabase.Equals(password))
                {
                    fk_ad = dataReader.scalerReturn("select ad_id from tbl_admin where ad_name ='" + userName + "'");

                    Form2 dashboardForm = new Form2();
                    dashboardForm.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Password");
                }
            }
        }

        private void Admin_Logincs_Load(object sender, EventArgs e)
        {
            ModernUi.ScaleForScreen(this);
            ApplyPreferredWindowSize();
            ApplyResponsiveBounds(70, 60);
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(9, 15, 29), Color.FromArgb(20, 32, 52));
            AttachLayoutEvents();
            BuildAdminLoginLayout();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form1 roleSelectionForm = new Form1();
            roleSelectionForm.Show();
            Hide();
        }

        private void BuildAdminLoginLayout()
        {
            SuspendLayout();

            BackColor = Color.FromArgb(9, 15, 29);
            FormBorderStyle = FormBorderStyle.None;

            int sidePadding = Math.Max(24, ClientSize.Width / 26);
            int topPadding = Math.Max(56, ClientSize.Height / 9);
            int contentGap = Math.Max(20, ClientSize.Width / 32);
            int availableWidth = ClientSize.Width - (sidePadding * 2) - contentGap;
            int heroWidth = Math.Max(180, Math.Min(240, (int)(availableWidth * 0.36f)));
            int panelHeight = Math.Max(360, ClientSize.Height - topPadding - 44);
            int loginWidth = Math.Max(300, availableWidth - heroWidth);

            if (heroPanel == null)
            {
                heroPanel = ModernUi.CreateCard(new Rectangle(sidePadding, topPadding, heroWidth, panelHeight));
                Controls.Add(heroPanel);
                heroPanel.SendToBack();
            }
            else
            {
                heroPanel.Bounds = new Rectangle(sidePadding, topPadding, heroWidth, panelHeight);
            }

            groupBox1.BackColor = Color.Transparent;
            groupBox1.ForeColor = ModernUi.Ink;
            groupBox1.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Text = "Administrator Sign In";
            groupBox1.Location = new Point(heroPanel.Right + contentGap, topPadding);
            groupBox1.Size = new Size(Math.Max(320, loginWidth), panelHeight);

            label1.Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = ModernUi.Ink;
            label1.Text = "Username";
            label1.Location = new Point(30, 88);

            label2.Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = ModernUi.Ink;
            label2.Text = "Password";
            label2.Location = new Point(30, 182);

            ModernUi.StyleTextInput(textBox1);
            textBox1.Location = new Point(34, 118);
            textBox1.Size = new Size(groupBox1.Width - 68, 34);

            ModernUi.StyleTextInput(textBox2);
            textBox2.Location = new Point(34, 212);
            textBox2.Size = new Size(groupBox1.Width - 68, 34);
            textBox2.UseSystemPasswordChar = true;

            ModernUi.StylePrimaryButton(button1);
            button1.Text = "Open Admin Workspace";
            button1.Location = new Point(34, Math.Min(groupBox1.Height - 86, 292));
            button1.Size = new Size(groupBox1.Width - 68, 48);

            if (heroEyebrowLabel == null)
            {
                heroEyebrowLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point), ModernUi.Accent, Point.Empty, Size.Empty, ContentAlignment.MiddleLeft);
                heroEyebrowLabel.Parent = heroPanel;
            }

            heroEyebrowLabel.Text = "Secure access";
            heroEyebrowLabel.Location = new Point(22, 34);
            heroEyebrowLabel.Size = new Size(heroPanel.Width - 44, 24);

            if (heroTitleLabel == null)
            {
                heroTitleLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point), ModernUi.Ink, Point.Empty, Size.Empty, ContentAlignment.TopLeft);
                heroTitleLabel.Parent = heroPanel;
            }

            heroTitleLabel.Text = "Admin\ncontrol\nmade cleaner.";
            heroTitleLabel.Location = new Point(22, 76);
            heroTitleLabel.Size = new Size(heroPanel.Width - 44, 160);

            if (heroCopyLabel == null)
            {
                heroCopyLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point), ModernUi.MutedInk, Point.Empty, Size.Empty, ContentAlignment.TopLeft);
                heroCopyLabel.Parent = heroPanel;
            }

            heroCopyLabel.Text = "Manage students, question banks, exam settings, and results from one polished workspace.";
            heroCopyLabel.Location = new Point(22, Math.Min(heroPanel.Height - 150, 214));
            heroCopyLabel.Size = new Size(heroPanel.Width - 44, 120);

            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Location = new Point(ClientSize.Width - pictureBox3.Width - 18, 12);
            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.Cursor = Cursors.Hand;
            pictureBox4.Location = new Point(14, 14);

            ResumeLayout();
        }

        private void AttachLayoutEvents()
        {
            if (layoutEventsAttached)
            {
                return;
            }

            layoutEventsAttached = true;
            Shown += (sender, e) => BuildAdminLoginLayout();
            Resize += (sender, e) =>
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    BuildAdminLoginLayout();
                }
            };
        }

        private void ApplyPreferredWindowSize()
        {
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int width = Math.Min(workingArea.Width - 70, 860);
            int height = Math.Min(workingArea.Height - 70, 560);

            ClientSize = new Size(
                Math.Max(680, width),
                Math.Max(460, height));
        }
    }
}

