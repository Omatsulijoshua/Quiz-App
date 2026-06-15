using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Form3 : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private Panel pastQuestionsCard;
        private Panel examCard;
        private Label titleLabel;
        private bool layoutEventsAttached;

        public Form3()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            studentlogin examLoginForm = new studentlogin();
            examLoginForm.Show();
            Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            studentlogin2 pastQuestionsLoginForm = new studentlogin2();
            pastQuestionsLoginForm.Show();
            Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form1 roleSelectionForm = new Form1();
            roleSelectionForm.Show();
            Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ModernUi.ScaleForScreen(this);
            ApplyPreferredWindowSize();
            ApplyResponsiveBounds(70, 60);
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(10, 14, 28), Color.FromArgb(27, 37, 59));
            AttachLayoutEvents();
            BuildStudentSelectionLayout();
        }

        private void BuildStudentSelectionLayout()
        {
            SuspendLayout();

            int sidePadding = Math.Max(28, ClientSize.Width / 24);
            int topPadding = Math.Max(30, ClientSize.Height / 16);

            if (titleLabel == null)
            {
                titleLabel = ModernUi.CreateLabel(
                    "Student Workspace",
                    new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point),
                    ModernUi.Ink,
                    Point.Empty,
                    Size.Empty,
                    ContentAlignment.MiddleCenter);
                Controls.Add(titleLabel);
            }

            titleLabel.Location = new Point(sidePadding, topPadding);
            titleLabel.Size = new Size(ClientSize.Width - (sidePadding * 2), 42);
            titleLabel.BringToFront();

            int cardGap = Math.Max(18, ClientSize.Width / 30);
            int cardWidth = Math.Max(180, (ClientSize.Width - (sidePadding * 2) - cardGap) / 2);
            int cardHeight = Math.Max(170, Math.Min(220, ClientSize.Height - (topPadding + 96) - 26));
            int cardTop = Math.Max(topPadding + 64, ClientSize.Height - cardHeight - 28);

            pastQuestionsCard = BuildCard(
                pastQuestionsCard,
                pictureBox1,
                label2,
                "PAST QUESTIONS",
                "Review past question sets and practise with confidence.",
                new Rectangle(sidePadding, cardTop, cardWidth, cardHeight));

            examCard = BuildCard(
                examCard,
                pictureBox2,
                label3,
                "LIVE EXAM",
                "Enter the active exam flow and continue into your session.",
                new Rectangle(sidePadding + cardWidth + cardGap, cardTop, cardWidth, cardHeight));

            pictureBox3.Cursor = Cursors.Hand;
            pictureBox4.Cursor = Cursors.Hand;
            pictureBox3.Location = new Point(ClientSize.Width - pictureBox3.Width - 18, 12);
            pictureBox4.Location = new Point(14, 14);

            ResumeLayout();
        }

        private Panel BuildCard(Panel existingCard, PictureBox pictureBox, Label headingLabel, string heading, string description, Rectangle bounds)
        {
            Panel card = existingCard ?? ModernUi.CreateCard(bounds);
            card.Bounds = bounds;

            if (!Controls.Contains(card))
            {
                Controls.Add(card);
            }

            card.Cursor = Cursors.Hand;
            ModernUi.WireHoverLift(card, 6);

            pictureBox.Parent = card;
            pictureBox.BackColor = Color.Transparent;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            int iconSize = Math.Max(70, Math.Min(92, card.Height / 2 - 10));
            pictureBox.Size = new Size(iconSize, iconSize);
            pictureBox.Location = new Point((card.Width - pictureBox.Width) / 2, 22);
            pictureBox.Cursor = Cursors.Hand;

            headingLabel.Parent = card;
            headingLabel.BackColor = Color.Transparent;
            headingLabel.ForeColor = ModernUi.Accent;
            headingLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            headingLabel.Text = heading;
            headingLabel.Size = new Size(card.Width - 16, 28);
            headingLabel.Location = new Point(8, Math.Max(104, pictureBox.Bottom + 8));
            headingLabel.TextAlign = ContentAlignment.MiddleCenter;

            Label detailLabel = card.Controls.OfType<Label>().FirstOrDefault(label => Convert.ToString(label.Tag) == "detail");
            if (detailLabel == null)
            {
                detailLabel = ModernUi.CreateLabel(
                    description,
                    new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                    ModernUi.MutedInk,
                    new Point(14, 138),
                    new Size(card.Width - 28, 28),
                    ContentAlignment.TopCenter);
                detailLabel.Tag = "detail";
                card.Controls.Add(detailLabel);
            }
            else
            {
                detailLabel.Text = description;
            }

            detailLabel.Location = new Point(14, headingLabel.Bottom + 4);
            detailLabel.Size = new Size(card.Width - 28, Math.Max(28, card.Height - detailLabel.Top - 14));

            return card;
        }

        private void AttachLayoutEvents()
        {
            if (layoutEventsAttached)
            {
                return;
            }

            layoutEventsAttached = true;
            Shown += (sender, e) => BuildStudentSelectionLayout();
            Resize += (sender, e) =>
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    BuildStudentSelectionLayout();
                }
            };
        }

        private void ApplyPreferredWindowSize()
        {
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int width = Math.Min(workingArea.Width - 80, 840);
            int height = Math.Min(workingArea.Height - 80, 470);

            ClientSize = new Size(
                Math.Max(ClientSize.Width, width),
                Math.Max(ClientSize.Height, height));
        }
    }
}

