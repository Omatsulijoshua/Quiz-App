using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Exam_Settings : BaseForm
    {
        private Panel settingsCard;

        public Exam_Settings()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            Setexams se = new Setexams();
            se.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Set_Exam_Duration se = new Set_Exam_Duration();
            se.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Exam_Shuffle se = new Exam_Shuffle();
            se.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form2 w = new Form2();
            w.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Exam_Settings_Load(object sender, EventArgs e)
        {
            ModernUi.ScaleForScreen(this);
            BuildExamSettingsLayout();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            show_calculator_scorecs se = new show_calculator_scorecs();
            se.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            show_result cs = new show_result();
            cs.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            set_exam_question_number c = new set_exam_question_number();
            c.Show();
            this.Hide();
        }

        private void BuildExamSettingsLayout()
        {
            BackColor = Color.FromArgb(9, 15, 29);
            FormBorderStyle = FormBorderStyle.None;

            if (settingsCard == null)
            {
                settingsCard = ModernUi.CreateCard(new Rectangle(42, 82, 1090, 470));
                Controls.Add(settingsCard);
                settingsCard.SendToBack();
            }

            label4.Parent = settingsCard;
            label4.BackColor = Color.Transparent;
            label4.Text = "Exam Configuration Hub";
            label4.ForeColor = ModernUi.Ink;
            label4.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(32, 22);
            label4.Size = new Size(560, 42);
            label4.TextAlign = ContentAlignment.MiddleLeft;

            ConfigureSettingsTile(pictureBox2, label1, "Allocate Exams", "Assign exams to students", new Rectangle(38, 98, 200, 150));
            ConfigureSettingsTile(pictureBox1, label2, "Question Limit", "Set number of exam questions", new Rectangle(296, 98, 200, 150));
            ConfigureSettingsTile(pictureBox4, label5, "Results", "Control result visibility", new Rectangle(554, 98, 200, 150));
            ConfigureSettingsTile(pictureBox9, label7, "Duration", "Define exam time limits", new Rectangle(38, 282, 200, 150));
            ConfigureSettingsTile(pictureBox3, label3, "Shuffle", "Configure exam randomization", new Rectangle(296, 282, 200, 150));
            ConfigureSettingsTile(pictureBox5, label6, "Calculator & Score", "Manage calculator and score display", new Rectangle(554, 282, 200, 150));

            pictureBox7.Cursor = Cursors.Hand;
            pictureBox8.Cursor = Cursors.Hand;
            pictureBox7.BringToFront();
            pictureBox8.BringToFront();
        }

        private void ConfigureSettingsTile(PictureBox icon, Label caption, string title, string description, Rectangle bounds)
        {
            Panel card = ModernUi.CreateCard(bounds);
            settingsCard.Controls.Add(card);

            icon.Parent = card;
            icon.BackColor = Color.Transparent;
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            icon.Location = new Point((card.Width - 82) / 2, 18);
            icon.Size = new Size(82, 70);
            icon.Cursor = Cursors.Hand;

            caption.Parent = card;
            caption.BackColor = Color.Transparent;
            caption.ForeColor = ModernUi.Accent;
            caption.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            caption.Text = title;
            caption.Location = new Point(12, 96);
            caption.Size = new Size(card.Width - 24, 26);
            caption.TextAlign = ContentAlignment.MiddleCenter;

            Label descriptionLabel = ModernUi.CreateLabel(
                description,
                new Font("Segoe UI", 9.2F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(12, 122),
                new Size(card.Width - 24, 22),
                ContentAlignment.TopCenter);
            descriptionLabel.Parent = card;
        }
    }
}

