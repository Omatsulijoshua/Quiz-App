using DocumentFormat.OpenXml.Office.CustomXsn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Quiz_App
{
    public partial class messageform2 : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private int examId;
        private int studentId;
        private int score;
        private int totalQuestions;
        private bool _layoutEventsAttached;
        private void pictureBox4_Click(object sender, EventArgs e)
        {

            studentlogin w = new studentlogin();
            w.Show();
            this.Hide();
        }

        private void messageform2_Load(object sender, EventArgs e)
        {
            ApplyPreferredWindowSize();
            ApplyResponsiveBounds(70, 60);
            AttachLayoutEvents();
            ApplyModernTheoryStartLayout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!IsTheoryExamEnabled(examId))
            {
                MessageBox.Show("Theory exam is not enabled for this exam yet. Contact the administrator.", "Theory Disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get the theory exam duration from the database
            int durationMinutes = new return_class().GetTheoryDuration(examId);

            // Open the theory test form
            Test3 theoryExam = new Test3(examId, studentId, score, totalQuestions, durationMinutes);
            theoryExam.Show();
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
                        "IF OBJECT_ID(N'dbo.tbl_exam_settings', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.tbl_exam_settings', N'theory_exam_enabled') IS NULL ALTER TABLE dbo.tbl_exam_settings ADD theory_exam_enabled BIT NOT NULL CONSTRAINT DF_tbl_exam_settings_theory_exam_enabled_messageform2 DEFAULT (1);",
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



        public messageform2(int examId, int studentId, int score, int totalQuestions)
        {
            InitializeComponent();
            this.examId = examId;
            this.studentId = studentId;
            this.score = score;
            this.totalQuestions = totalQuestions;
        }

        private void ApplyModernTheoryStartLayout()
        {
            SuspendLayout();
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(10, 16, 29), Color.FromArgb(22, 41, 63));
            BackColor = Color.FromArgb(12, 19, 34);

            int margin = Math.Max(34, ClientSize.Width / 24);
            int contentWidth = ClientSize.Width - (margin * 2);
            int titleTop = Math.Max(34, ClientSize.Height / 10);
            int buttonTop = ClientSize.Height - 110;

            label1.Text = "Objective Exam Completed";
            label1.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(margin, titleTop);
            label1.Size = new Size(contentWidth, 42);

            Label subtitle = Controls.OfType<Label>().FirstOrDefault(l => l.Name == "runtimeTheorySubtitle");
            if (subtitle == null)
            {
                subtitle = new Label
                {
                    Name = "runtimeTheorySubtitle",
                    Parent = this,
                    BackColor = Color.Transparent
                };
                Controls.Add(subtitle);
            }

            subtitle.Text = "Theory exam is available for this paper. Continue when you are ready.";
            subtitle.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            subtitle.ForeColor = ModernUi.MutedInk;
            subtitle.Location = new Point(margin, titleTop + 70);
            subtitle.Size = new Size(contentWidth, 54);

            ModernUi.StylePrimaryButton(button2);
            button2.Text = "Start Theory Exam";
            button2.Location = new Point(margin, buttonTop);
            button2.Size = new Size(250, 50);

            pictureBox4.Location = new Point(ClientSize.Width - 76, 24);
            pictureBox4.Size = new Size(32, 32);
            pictureBox4.BackColor = Color.Transparent;
            ResumeLayout();
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
            int width = Math.Min(workingArea.Width - 90, 860);
            int height = Math.Min(workingArea.Height - 90, 420);
            Size = new Size(Math.Max(680, width), Math.Max(360, height));
            StartPosition = FormStartPosition.Manual;
        }
    }
}

