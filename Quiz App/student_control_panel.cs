using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class student_control_panel : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private Panel settingsCard;
        private Panel summaryCard;
        private bool _layoutEventsAttached;

        public student_control_panel()
        {
            InitializeComponent();
        }

        public static class ExamPreferences
        {
            public static int SelectedExamId { get; set; }
            public static bool ShuffleEnabled { get; set; }
            public static int DurationMinutes { get; set; }
            public static int QuestionLimit { get; set; }
        }

        private void btnStartTest_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(studentlogin2.exam_id))
            {
                MessageBox.Show("No exam was selected from login.");
                return;
            }

            int examId = Convert.ToInt32(studentlogin2.exam_id);
            bool pastQuestionsEnabledExam = false;
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = "SELECT past_questions_enabled FROM tbl_exam_settings WHERE ex_id = @examId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        pastQuestionsEnabledExam = Convert.ToBoolean(result);
                    }
                }
            }

            if (!pastQuestionsEnabledExam)
            {
                MessageBox.Show("Past questions are not enabled for this exam. Contact the administrator.");
                return;
            }

            int enabledPastQuestions = 0;
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_past_questions WHERE ex_id_fk = @examId", con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    enabledPastQuestions += Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_past_shortanswer WHERE exam_id = @examId", con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    enabledPastQuestions += Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            if (enabledPastQuestions == 0)
            {
                MessageBox.Show("No past questions are currently available for this exam.");
                return;
            }

            int requestedLimit = (int)numericUpDownQuestionLimit.Value;
            if (requestedLimit > enabledPastQuestions)
            {
                MessageBox.Show($"Only {enabledPastQuestions} questions are available. Reduce the question limit and try again.");
                return;
            }

            ExamPreferences.ShuffleEnabled = radioButtonShuffle.Checked;
            ExamPreferences.SelectedExamId = examId;
            ExamPreferences.DurationMinutes = (int)numericUpDownDuration1.Value;
            ExamPreferences.QuestionLimit = requestedLimit;

            Test2.score = 0;
            Test2 testForm = new Test2();
            testForm.Show();
            Hide();
        }

        private void student_control_panel_Load(object sender, EventArgs e)
        {
            ApplyPreferredWindowSize();
            ApplyResponsiveBounds(70, 60);
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(9, 15, 29), Color.FromArgb(20, 32, 52));
            AttachLayoutEvents();
            BuildLayout();
            LoadExamAvailability();
            ModernUi.FadeIn(this);
        }

        private void BuildLayout()
        {
            SuspendLayout();

            Controls.Clear();
            BackColor = Color.FromArgb(9, 15, 29);
            FormBorderStyle = FormBorderStyle.None;

            int margin = Math.Max(28, ClientSize.Width / 26);
            int top = Math.Max(104, ClientSize.Height / 6);
            int gap = 28;
            int availableWidth = ClientSize.Width - (margin * 2);
            int settingsWidth = Math.Max(360, (int)(availableWidth * 0.56f));
            int summaryWidth = Math.Max(280, availableWidth - settingsWidth - gap);
            int cardHeight = Math.Max(360, ClientSize.Height - top - margin);

            if (settingsWidth + summaryWidth + gap > availableWidth)
            {
                summaryWidth = Math.Max(260, availableWidth - settingsWidth - gap);
            }

            settingsCard = ModernUi.CreateCard(new Rectangle(margin, top, settingsWidth, cardHeight));
            summaryCard = ModernUi.CreateCard(new Rectangle(settingsCard.Right + gap, top, summaryWidth, cardHeight));
            Controls.Add(settingsCard);
            Controls.Add(summaryCard);

            Label eyebrow = ModernUi.CreateLabel(
                "Practice setup",
                new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Warning,
                new Point(margin, 34),
                new Size(160, 24),
                ContentAlignment.MiddleLeft);
            Controls.Add(eyebrow);

            Label title = ModernUi.CreateLabel(
                "Customize your revision session",
                new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(margin, 60),
                new Size(Math.Min(620, ClientSize.Width - (margin * 2)), 38),
                ContentAlignment.MiddleLeft);
            Controls.Add(title);

            label3.Parent = settingsCard;
            label3.BackColor = Color.Transparent;
            label3.ForeColor = ModernUi.Ink;
            label3.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Text = "Session Settings";
            label3.Location = new Point(26, 22);
            label3.AutoSize = true;

            label1.Parent = settingsCard;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = ModernUi.Ink;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Text = "Shuffle questions";
            label1.Location = new Point(28, 78);
            label1.AutoSize = true;

            radioButtonShuffle.Parent = settingsCard;
            radioButtonNoShuffle.Parent = settingsCard;
            radioButtonShuffle.BackColor = Color.Transparent;
            radioButtonNoShuffle.BackColor = Color.Transparent;
            radioButtonShuffle.ForeColor = ModernUi.Ink;
            radioButtonNoShuffle.ForeColor = ModernUi.Ink;
            radioButtonShuffle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            radioButtonNoShuffle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            radioButtonShuffle.Location = new Point(32, 118);
            radioButtonNoShuffle.Location = new Point(132, 118);
            radioButtonNoShuffle.Checked = true;

            label4.Parent = settingsCard;
            label4.BackColor = Color.Transparent;
            label4.ForeColor = ModernUi.Ink;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Text = "Question limit";
            label4.Location = new Point(28, 182);
            label4.AutoSize = true;

            ModernUi.StyleNumericUpDown(numericUpDownQuestionLimit);
            numericUpDownQuestionLimit.Parent = settingsCard;
            numericUpDownQuestionLimit.Location = new Point(32, 220);
            numericUpDownQuestionLimit.Size = new Size(220, 32);

            label2.Parent = settingsCard;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = ModernUi.Ink;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Text = "Duration";
            label2.Location = new Point(28, 282);
            label2.AutoSize = true;

            labelDuration.Parent = settingsCard;
            labelDuration.BackColor = Color.Transparent;
            labelDuration.ForeColor = ModernUi.MutedInk;
            labelDuration.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            labelDuration.Text = "Minutes";
            labelDuration.Location = new Point(262, 324);
            labelDuration.AutoSize = true;

            ModernUi.StyleNumericUpDown(numericUpDownDuration1);
            numericUpDownDuration1.Parent = settingsCard;
            numericUpDownDuration1.Location = new Point(32, 318);
            numericUpDownDuration1.Size = new Size(220, 32);
            numericUpDownDuration1.Minimum = 5;
            numericUpDownDuration1.Maximum = 240;
            numericUpDownDuration1.Value = 30;

            ModernUi.StylePrimaryButton(btnStartTest);
            btnStartTest.Parent = settingsCard;
            btnStartTest.Text = "Start Practice Session";
            btnStartTest.Location = new Point(32, settingsCard.Height - 74);
            btnStartTest.Size = new Size(Math.Min(settingsCard.Width - 64, 280), 46);

            Label summaryTitle = ModernUi.CreateLabel(
                "Session Summary",
                new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(22, 22),
                new Size(220, 28),
                ContentAlignment.MiddleLeft);
            summaryTitle.Parent = summaryCard;

            Label summaryCopy = ModernUi.CreateLabel(
                "Pick how many questions to answer, set a time limit, and decide whether to shuffle the order before you begin.",
                new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(22, 66),
                new Size(310, 82),
                ContentAlignment.TopLeft);
            summaryCopy.Parent = summaryCard;

            label8.Parent = summaryCard;
            label8.BackColor = Color.Transparent;
            label8.ForeColor = ModernUi.Accent;
            label8.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(22, 186);
            label8.Size = new Size(300, 42);

            Label helper = ModernUi.CreateLabel(
                "The question limit updates based on the selected exam's available past questions.",
                new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(22, 238),
                new Size(summaryCard.Width - 44, 72),
                ContentAlignment.TopLeft);
            helper.Parent = summaryCard;

            ResumeLayout();
        }

        private void LoadExamAvailability()
        {
            if (string.IsNullOrEmpty(studentlogin2.exam_id))
            {
                return;
            }

            int examId = Convert.ToInt32(studentlogin2.exam_id);
            int totalQuestions = 0;

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_past_questions WHERE ex_id_fk = @examId", con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    totalQuestions += Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_past_shortanswer WHERE exam_id = @examId", con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    totalQuestions += Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            numericUpDownQuestionLimit.Maximum = Math.Max(1, totalQuestions);
            label8.Text = totalQuestions > 0 ? totalQuestions + " questions available" : "No questions available";

            if (totalQuestions == 0)
            {
                numericUpDownQuestionLimit.Enabled = false;
                btnStartTest.Enabled = false;
                btnStartTest.Text = "Questions Unavailable";
            }
            else
            {
                numericUpDownQuestionLimit.Enabled = true;
                btnStartTest.Enabled = true;
                btnStartTest.Text = "Start Practice Session";
                numericUpDownQuestionLimit.Value = Math.Min(10, totalQuestions);
            }
        }

        private void AttachLayoutEvents()
        {
            if (_layoutEventsAttached)
            {
                return;
            }

            _layoutEventsAttached = true;
            Shown += (sender, e) => BuildLayout();
            Resize += (sender, e) =>
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    BuildLayout();
                }
            };
        }

        private void ApplyPreferredWindowSize()
        {
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int width = Math.Min(workingArea.Width - 90, 980);
            int height = Math.Min(workingArea.Height - 90, 640);
            Size = new Size(Math.Max(760, width), Math.Max(560, height));
            StartPosition = FormStartPosition.Manual;
        }
    }
}

