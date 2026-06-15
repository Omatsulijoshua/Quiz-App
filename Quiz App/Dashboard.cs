using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Dashboard : BaseForm
    {
        private Panel summaryCard;
        private Panel progressCard;
        private Panel actionsCard;

        public Dashboard()
        {
            InitializeComponent();
            Load += Dashboard_Load;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            ModernUi.ApplyTheme(this);
            BackColor = ModernUi.Surface;
            BuildDashboard();
        }

        private void BuildDashboard()
        {
            SuspendLayout();

            Controls.Clear();

            Label eyebrow = ModernUi.CreateLabel(
                "Admin overview",
                new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Accent,
                new Point(42, 26),
                new Size(160, 24),
                ContentAlignment.MiddleLeft);
            Controls.Add(eyebrow);

            Label title = ModernUi.CreateLabel(
                "Run your CBT workspace from one place.",
                new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(38, 54),
                new Size(680, 46),
                ContentAlignment.MiddleLeft);
            Controls.Add(title);

            Label subtitle = ModernUi.CreateLabel(
                "Use the left navigation to manage exams, questions, students, reports, and settings.",
                new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(42, 98),
                new Size(760, 28),
                ContentAlignment.MiddleLeft);
            Controls.Add(subtitle);

            summaryCard = ModernUi.CreateCard(new Rectangle(42, 154, 380, 220));
            progressCard = ModernUi.CreateCard(new Rectangle(448, 154, 380, 220));
            actionsCard = ModernUi.CreateCard(new Rectangle(42, 398, 786, 170));

            Controls.Add(summaryCard);
            Controls.Add(progressCard);
            Controls.Add(actionsCard);

            PopulateCard(
                summaryCard,
                "Today’s focus",
                "Question banks, exam settings, and grading are all one click away.",
                "Use the side menu to jump into setup or review work."
            );

            PopulateCard(
                progressCard,
                "Modernized shell",
                "The dashboard now uses a cleaner navigation surface and embedded content area.",
                "New modules loaded here inherit the shared theme automatically."
            );

            PopulateActionCard();

            ResumeLayout();
        }

        private void PopulateCard(Panel card, string heading, string body, string footnote)
        {
            Label headingLabel = ModernUi.CreateLabel(
                heading,
                new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(24, 26),
                new Size(card.Width - 48, 32),
                ContentAlignment.MiddleLeft);
            headingLabel.Parent = card;

            Label bodyLabel = ModernUi.CreateLabel(
                body,
                new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(24, 76),
                new Size(card.Width - 48, 62),
                ContentAlignment.TopLeft);
            bodyLabel.Parent = card;

            Label footnoteLabel = ModernUi.CreateLabel(
                footnote,
                new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Accent,
                new Point(24, card.Height - 52),
                new Size(card.Width - 48, 24),
                ContentAlignment.MiddleLeft);
            footnoteLabel.Parent = card;
        }

        private void PopulateActionCard()
        {
            Label headingLabel = ModernUi.CreateLabel(
                "Suggested next steps",
                new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(24, 22),
                new Size(actionsCard.Width - 48, 32),
                ContentAlignment.MiddleLeft);
            headingLabel.Parent = actionsCard;

            Label bullets = ModernUi.CreateLabel(
                "Add students and courses.\nSet exam duration and question limits.\nReview results or grade theory responses.",
                new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(24, 62),
                new Size(actionsCard.Width - 48, 78),
                ContentAlignment.TopLeft);
            bullets.Parent = actionsCard;
        }
    }
}

