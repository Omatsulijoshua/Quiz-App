using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class student_control_panel : Form
    {
        public student_control_panel()
        {
            InitializeComponent();
        }


        private void pictureBox7_Click(object sender, EventArgs e)
        {
            studentlogin2 w = new studentlogin2();
            w.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

            // ✅ Check if past questions are enabled for this exam
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
                        pastQuestionsEnabledExam = Convert.ToBoolean(result);
                }
            }

            if (!pastQuestionsEnabledExam)
            {
                MessageBox.Show("Past questions not enabled for this exam. Contact admin.",
                                "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Check if there are any questions marked as past-question-enabled
            int enabledPastQuestions = 0;
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM tbl_exam_settings WHERE ex_id = @examId AND past_questions_enabled = 1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    con.Open();
                    enabledPastQuestions = (int)cmd.ExecuteScalar();
                }
            }

            if (enabledPastQuestions == 0)
            {
                MessageBox.Show("No questions are enabled as past questions for this exam.",
                                "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Validate question limit against available enabled questions
            int requestedLimit = (int)numericUpDownQuestionLimit.Value;

            if (requestedLimit > enabledPastQuestions)
            {
                MessageBox.Show($"Only {enabledPastQuestions} questions are available for this exam. Please reduce the question limit.",
                    "Insufficient Questions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Handle shuffle option
            if (radioButtonShuffle.Checked)
                ExamPreferences.ShuffleEnabled = true;
            else if (radioButtonNoShuffle.Checked)
                ExamPreferences.ShuffleEnabled = false;
            else
            {
                MessageBox.Show("Please select whether to shuffle questions or not.");
                return;
            }

            // ✅ Save exam preferences
            ExamPreferences.SelectedExamId = examId;
            ExamPreferences.DurationMinutes = (int)numericUpDownDuration1.Value;
            ExamPreferences.QuestionLimit = requestedLimit;

            Test2.score = 0;

            // ✅ Open Test Form
            Test2 testForm = new Test2();
            testForm.Show();
            this.Hide();
        }



        private void student_control_panel_Load(object sender, EventArgs e)
        {
            radioButtonNoShuffle.Checked = true;

            // Populate numericUpDownQuestionLimit maximum based on available past questions
            if (!string.IsNullOrEmpty(studentlogin2.exam_id))
            {
                int examId = Convert.ToInt32(studentlogin2.exam_id);
                int totalQuestions = 0;

                using (SqlConnection con = connection_class.GetConnection())
                {
                    con.Open();

                    // Count multiple-choice past questions (ensure correct FK column)
                    string queryMCQ = "SELECT COUNT(*) FROM tbl_past_questions WHERE ex_id_fk = @examId";
                    using (SqlCommand cmd = new SqlCommand(queryMCQ, con))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        totalQuestions += Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Count short-answer past questions (ensure correct FK column)
                    string querySAQ = "SELECT COUNT(*) FROM tbl_past_shortanswer WHERE exam_id = @examId";
                    using (SqlCommand cmd = new SqlCommand(querySAQ, con))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        totalQuestions += Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                // Set numericUpDownQuestionLimit max value
                numericUpDownQuestionLimit.Maximum = totalQuestions;

                if (totalQuestions == 0)
                {
                    MessageBox.Show("No past questions are available for this exam.",
                                    "No Questions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numericUpDownQuestionLimit.Enabled = false;
                }
                else
                {
                    numericUpDownQuestionLimit.Enabled = true;
                    numericUpDownQuestionLimit.Value = Math.Min(10, totalQuestions); // default value
                }
            }
        }
        

    }
}
