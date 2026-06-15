using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Form1 : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private Panel adminCard;
        private Panel studentCard;
        private Label titleLabel;
        private Label subtitleLabel;
        private bool layoutEventsAttached;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form3 studentOptionsForm = new Form3();
            studentOptionsForm.Show();
            Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Admin_Logincs adminLoginForm = new Admin_Logincs();
            adminLoginForm.Show();
            Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ModernUi.ScaleForScreen(this);
            ApplyPreferredWindowSize();
            ApplyResponsiveBounds(70, 60);
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(10, 14, 28), Color.FromArgb(27, 37, 59));
            AttachLayoutEvents();
            BuildRoleSelectionLayout();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Welcome welcomeForm = new Welcome();
            welcomeForm.Show();
            Hide();
        }

        private void BuildRoleSelectionLayout()
        {
            SuspendLayout();

            BackColor = Color.FromArgb(10, 14, 28);
            FormBorderStyle = FormBorderStyle.None;

            int sidePadding = Math.Max(24, ClientSize.Width / 24);
            int topPadding = Math.Max(28, ClientSize.Height / 18);
            int headerWidth = Math.Max(280, ClientSize.Width - (sidePadding * 2));
            int headerLeft = (ClientSize.Width - headerWidth) / 2;

            if (titleLabel == null)
            {
                titleLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point), ModernUi.Ink, Point.Empty, Size.Empty, ContentAlignment.MiddleCenter);
                Controls.Add(titleLabel);
            }

            titleLabel.Text = "Choose Your Workspace";
            titleLabel.Location = new Point(headerLeft, topPadding + 2);
            titleLabel.Size = new Size(headerWidth, 50);
            titleLabel.BringToFront();

            if (subtitleLabel == null)
            {
                subtitleLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI", 11.5F, FontStyle.Regular, GraphicsUnit.Point), ModernUi.MutedInk, Point.Empty, Size.Empty, ContentAlignment.MiddleCenter);
                Controls.Add(subtitleLabel);
            }

            subtitleLabel.Text = "Jump in as an administrator or continue as a student.";
            subtitleLabel.Location = new Point(headerLeft + 10, topPadding + 54);
            subtitleLabel.Size = new Size(Math.Max(260, headerWidth - 20), 28);
            subtitleLabel.BringToFront();

            int cardGap = Math.Max(18, ClientSize.Width / 30);
            int availableWidth = ClientSize.Width - (sidePadding * 2) - cardGap;
            int cardWidth = Math.Max(170, availableWidth / 2);
            int cardHeight = Math.Max(160, Math.Min(220, ClientSize.Height - (topPadding + 130) - 26));
            int cardTop = Math.Max(topPadding + 94, ClientSize.Height - cardHeight - 26);
            int firstCardLeft = (ClientSize.Width - ((cardWidth * 2) + cardGap)) / 2;

            adminCard = BuildRoleCard(adminCard, pictureBox1, label2, "ADMIN", "Manage exams, students, results, and settings.", new Rectangle(firstCardLeft, cardTop, cardWidth, cardHeight));
            studentCard = BuildRoleCard(studentCard, pictureBox2, label3, "STUDENT", "Start exams and continue into your assigned test flow.", new Rectangle(firstCardLeft + cardWidth + cardGap, cardTop, cardWidth, cardHeight));

            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Location = new Point(ClientSize.Width - pictureBox3.Width - 18, 12);
            pictureBox3.BringToFront();

            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.Cursor = Cursors.Hand;
            pictureBox4.Location = new Point(14, 14);
            pictureBox4.BringToFront();

            ResumeLayout();
        }

        private Panel BuildRoleCard(Panel existingCard, PictureBox pictureBox, Label footerLabel, string heading, string description, Rectangle bounds)
        {
            Panel card = existingCard ?? ModernUi.CreateCard(bounds);
            card.Bounds = bounds;

            if (!Controls.Contains(card))
            {
                Controls.Add(card);
            }

            card.BringToFront();
            card.Cursor = Cursors.Hand;
            ModernUi.WireHoverLift(card, 6);

            pictureBox.Parent = card;
            pictureBox.BackColor = Color.Transparent;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            int iconSize = Math.Max(66, Math.Min(90, card.Height / 2 - 10));
            pictureBox.Size = new Size(iconSize, iconSize);
            pictureBox.Location = new Point((card.Width - pictureBox.Width) / 2, 18);
            pictureBox.Cursor = Cursors.Hand;

            footerLabel.Parent = card;
            footerLabel.BackColor = Color.Transparent;
            footerLabel.ForeColor = ModernUi.Accent;
            footerLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            footerLabel.Text = heading;
            footerLabel.Size = new Size(card.Width - 24, 28);
            footerLabel.Location = new Point(12, Math.Max(92, pictureBox.Bottom + 8));
            footerLabel.TextAlign = ContentAlignment.MiddleCenter;
            footerLabel.Cursor = Cursors.Hand;

            Label detailLabel = card.Controls.OfType<Label>().FirstOrDefault(label => Convert.ToString(label.Tag) == "detail");
            if (detailLabel == null)
            {
                detailLabel = ModernUi.CreateLabel(
                    description,
                    new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                    ModernUi.MutedInk,
                    new Point(16, 150),
                    new Size(card.Width - 32, 30),
                    ContentAlignment.TopCenter);
                detailLabel.Tag = "detail";
                card.Controls.Add(detailLabel);
            }
            
            detailLabel.Text = description;
            detailLabel.Location = new Point(16, footerLabel.Bottom + 4);
            detailLabel.Size = new Size(card.Width - 32, Math.Max(28, card.Height - detailLabel.Top - 14));
            detailLabel.Cursor = Cursors.Hand;

            EventHandler clickHandler = pictureBox == pictureBox1
                ? new EventHandler(pictureBox1_Click)
                : new EventHandler(pictureBox2_Click);

            WireRoleCardClicks(card, clickHandler, pictureBox, footerLabel, detailLabel);

            return card;
        }

        private void WireRoleCardClicks(Panel card, EventHandler clickHandler, params Control[] clickableChildren)
        {
            card.Click -= clickHandler;
            card.Click += clickHandler;

            foreach (Control child in clickableChildren)
            {
                child.Click -= clickHandler;
                child.Click += clickHandler;
                child.Cursor = Cursors.Hand;
            }
        }

        private void AttachLayoutEvents()
        {
            if (layoutEventsAttached)
            {
                return;
            }

            layoutEventsAttached = true;
            Shown += (sender, e) => BuildRoleSelectionLayout();
            Resize += (sender, e) =>
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    BuildRoleSelectionLayout();
                }
            };
        }

        private void ApplyPreferredWindowSize()
        {
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int width = Math.Min(workingArea.Width - 80, 820);
            int height = Math.Min(workingArea.Height - 80, 470);

            ClientSize = new Size(
                Math.Max(ClientSize.Width, width),
                Math.Max(ClientSize.Height, height));
        }
    }
}

