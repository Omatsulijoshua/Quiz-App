using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Quiz_App
{
    public partial class messageform : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private int score;
        private int totalQuestions;
        private int examId;
        private int studentId;
        private bool _layoutEventsAttached;

        public messageform(int score, int totalQuestions, int examId, int studentId)
        {
            InitializeComponent();
            this.score = score;
            this.totalQuestions = totalQuestions;
            this.examId = examId;
            this.studentId = studentId;
        }

        private void messageform_Load(object sender, EventArgs e)
        {
            ApplyPreferredWindowSize();
            ApplyResponsiveBounds(70, 60);
            AttachLayoutEvents();
            float percentage = (totalQuestions > 0)
       ? ((float)score / totalQuestions) * 100
       : 0;

            label3.Text = $"Your Score is {score}";
            label5.Text = $"Your Percentage = {percentage:F2} %";

            string remark = GetRemark(percentage);
            label2.Text = remark;
            ApplyModernTheoryStartLayout();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form1 w = new Form1();
            w.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Print_Screen P = new Print_Screen();
            P.Show();
            this.Hide();
        }
        private string GetRemark(float percentage)
        {
            if (percentage <= 40)
                return "Bad. Try better.";
            else if (percentage <= 50)
                return "Pass. You can do better.";
            else if (percentage <= 60)
                return "Fair. Keep improving.";
            else if (percentage <= 70)
                return "Good. Nice work.";
            else if (percentage <= 80)
                return "Very Good. Keep it up!";
            else if (percentage <= 90)
                return "Excellent Performance!";
            else // 91 - 100
                return "Outstanding! You're a star!";
        }

        private void ApplyModernTheoryStartLayout()
        {
            SuspendLayout();
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(10, 16, 29), Color.FromArgb(22, 41, 63));
            BackColor = Color.FromArgb(12, 19, 34);

            int margin = Math.Max(34, ClientSize.Width / 24);
            int contentWidth = ClientSize.Width - (margin * 2);
            int titleTop = Math.Max(34, ClientSize.Height / 12);
            int buttonTop = ClientSize.Height - 108;

            label1.Text = "Objective Exam Completed";
            label1.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(margin, titleTop);
            label1.Size = new Size(contentWidth, 42);

            label3.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = ModernUi.Accent;
            label3.Location = new Point(margin, titleTop + 86);
            label3.Size = new Size(contentWidth, 42);

            label5.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = ModernUi.MutedInk;
            label5.Location = new Point(margin, titleTop + 138);
            label5.Size = new Size(contentWidth, 32);

            label2.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(255, 210, 120);
            label2.Location = new Point(margin, titleTop + 188);
            label2.Size = new Size(contentWidth, 56);

            ModernUi.StyleSecondaryButton(button1);
            button1.Text = "Print Result";
            button1.Location = new Point(margin, buttonTop);
            button1.Size = new Size(200, 50);

            ModernUi.StylePrimaryButton(button2);
            button2.Text = "Start Theory Exam";
            button2.Location = new Point(button1.Right + 18, buttonTop);
            button2.Size = new Size(230, 50);

            pictureBox4.Location = new Point(ClientSize.Width - 78, 28);
            pictureBox4.Size = new Size(32, 32);
            pictureBox4.BackColor = Color.Transparent;
            ResumeLayout();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!IsTheoryExamEnabled(examId))
            {
                MessageBox.Show("Theory exam is not enabled for this exam yet. Contact the administrator.", "Theory Disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get theory exam duration from tbl_exam_settings
            int durationMinutes = new return_class().GetTheoryDuration(examId);

            // Start theory exam
            Test3 theory = new Test3(examId, studentId, score, totalQuestions, durationMinutes);
            theory.Show();
            this.Close();
        }

        private bool IsTheoryExamEnabled(int selectedExamId)
        {
            try
            {
                using (SqlConnection connection = connection_class.GetConnection())
                {
                    connection.Open();
                    using (SqlCommand ensureCommand = new SqlCommand(
                        "IF OBJECT_ID(N'dbo.tbl_exam_settings', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.tbl_exam_settings', N'theory_exam_enabled') IS NULL ALTER TABLE dbo.tbl_exam_settings ADD theory_exam_enabled BIT NOT NULL CONSTRAINT DF_tbl_exam_settings_theory_exam_enabled_messageform DEFAULT (1);",
                        connection))
                    {
                        ensureCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand("SELECT theory_exam_enabled FROM tbl_exam_settings WHERE ex_id = @examId", connection))
                    {
                        command.Parameters.AddWithValue("@examId", selectedExamId);
                        object result = command.ExecuteScalar();
                        return result == null || result == DBNull.Value || Convert.ToBoolean(result);
                    }
                }
            }
            catch
            {
                return true;
            }
        }

        public partial class MessageForm : BaseForm
        {
            private int score;
            private int totalQuestions;
            private int examId;
            private int studentId;  
        }

        private void AttachLayoutEvents()
        {
            if (_layoutEventsAttached)
            {
                return;
            }

            _layoutEventsAttached = true;
            Shown += (sender, e) => ApplyModernTheoryStartLayout();
            Resize += (sender, e) =>
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    ApplyModernTheoryStartLayout();
                }
            };
        }

        private void ApplyPreferredWindowSize()
        {
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int width = Math.Min(workingArea.Width - 90, 880);
            int height = Math.Min(workingArea.Height - 90, 520);
            Size = new Size(Math.Max(720, width), Math.Max(440, height));
            StartPosition = FormStartPosition.Manual;
        }
    
    }
}

