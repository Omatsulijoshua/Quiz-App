using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Threading;
using System.Runtime.InteropServices;



namespace Quiz_App
{
    public partial class Test : Form
    {
        private bool allowScoreDisplay = false;
        private int examId;
        public static int score = 0;
        private int totalSeconds = 3600;
        private List<int> visitedQuestionIds = new List<int>();
        private int currentIndex = 0;
        private Dictionary<int, string> answeredQuestions = new Dictionary<int, string>();
        private Dictionary<int, string> selectedAnswers = new Dictionary<int, string>();
        private List<int> answeredQuestionIds = new List<int>();
        private List<int> allQuestionIds = new List<int>();
        private int totalQuestionsLimit = 0;
        private int currentQuesId = 0;

        private int _endCalled = 0;




        public static class ExamSettings
        {
            public static int DurationInMinutes { get; set; } = 60;
        }

        public static int exam_id;
        private List<DataRow> shuffledQuestions;
        private bool isShuffleMode = false;
        int C = 0;
        string correctop;
        string s, selectedvalue;
        int i;




        private List<int> orderedQuestionIds = new List<int>();

        private bool examEnded = false;  // 🔑 flag to prevent multiple triggers
        public Test(int selectedExamId)
        {
            InitializeComponent();
            examId = selectedExamId;
        }

        public Test()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            label4.Text = "Score: 0";
            label4.Text = $"Score: {score}";


            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += comboBox1_DrawItem;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;


            // hook events
            this.FormClosing += Test_FormClosing;
           // this.FormClosed += Test_FormClosed;
            this.Deactivate += Test_Deactivate;   // 🔑 triggers if user switches app


        }
        return_class rc = new return_class();

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateTimerLabel()
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            timerLabel1.Text = time.ToString(@"hh\:mm\:ss");
        }

        private Image ByteArrayToImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }


       

        private void LoadQuestionFromShuffledList(int index)
        {
            DataRow row = shuffledQuestions[index];

            label1.Text = row["q_title"].ToString();
            radioButton1.Text = row["q_opA"].ToString();
            radioButton2.Text = row["q_opB"].ToString();
            radioButton3.Text = row["q_opC"].ToString();
            radioButton4.Text = row["q_opD"].ToString();

            // ✅ store correct option here
            correctop = row["q_correctOpn"].ToString();

            if (row["q_image"] != DBNull.Value)
            {
                byte[] imgBytes = (byte[])row["q_image"];
                pictureBox1.Image = ByteArrayToImage(imgBytes);
            }
            else
            {
                pictureBox1.Image = null;
            }
        }




        private void LoadQuestionById(int questionId)
        {
            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();
                string query = "SELECT q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_image FROM tbl_questions WHERE ques_id=@id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", questionId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label1.Text = reader["q_title"].ToString();
                            radioButton1.Text = reader["q_opA"].ToString();
                            radioButton2.Text = reader["q_opB"].ToString();
                            radioButton3.Text = reader["q_opC"].ToString();
                            radioButton4.Text = reader["q_opD"].ToString();

                            // ✅ store correct option here
                            correctop = reader["q_correctOpn"].ToString();

                            if (reader["q_image"] != DBNull.Value)
                            {
                                byte[] imgBytes = (byte[])reader["q_image"];
                                pictureBox1.Image = ByteArrayToImage(imgBytes);
                            }
                            else
                            {
                                pictureBox1.Image = null;
                            }
                        }
                    }
                }
            }
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            DataRow currentRow = shuffledQuestions[i];
            string qtype = currentRow["qtype"].ToString();
            string selectedValue = "";

            if (qtype == "MCQ")
            {
                if (radioButton1.Checked) selectedValue = radioButton1.Text;
                else if (radioButton2.Checked) selectedValue = radioButton2.Text;
                else if (radioButton3.Checked) selectedValue = radioButton3.Text;
                else if (radioButton4.Checked) selectedValue = radioButton4.Text;
            }
            else if (qtype == "SHORT")
            {
                selectedValue = txtShortAnswer.Text.Trim();
            }

            int currentQid = Convert.ToInt32(currentRow["qid"]);

            // scoring logic (unchanged)
            if (selectedAnswers.ContainsKey(currentQid))
            {
                string prev = selectedAnswers[currentQid];

                if (prev != selectedValue)
                {
                    if (!string.IsNullOrEmpty(prev) && prev.Equals(correctop, StringComparison.OrdinalIgnoreCase))
                        score--;

                    if (!string.IsNullOrEmpty(selectedValue) && selectedValue.Equals(correctop, StringComparison.OrdinalIgnoreCase))
                        score++;
                }

                selectedAnswers[currentQid] = selectedValue;
            }
            else
            {
                if (!string.IsNullOrEmpty(selectedValue) && selectedValue.Equals(correctop, StringComparison.OrdinalIgnoreCase))
                    score++;

                selectedAnswers.Add(currentQid, selectedValue);
            }

            if (allowScoreDisplay) label4.Text = $"Score: {score}";

            // Move next (do NOT add ComboBox items here)
            i++;
            if (i < shuffledQuestions.Count)
            {
                currentQuesId = Convert.ToInt32(shuffledQuestions[i]["qid"]);

                // mark visited now
                if (!visitedQuestionIds.Contains(currentQuesId))
                    visitedQuestionIds.Add(currentQuesId);

                currentIndex = i;
                LoadUnifiedQuestion(shuffledQuestions[i]);
                label8.Text = $"Question {currentIndex + 1} of {totalQuestionsLimit}";

                comboBox1.SelectedIndex = currentIndex; // keep combobox in sync
                comboBox1.Invalidate();
            }
            else
            {
                EndExamOnce();
            }

        }


        private void ApplyExamSettings(int examId)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = "SELECT show_score, show_calculator FROM tbl_exam_settings WHERE ex_id = @exid";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@exid", examId);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool showScore = reader["show_score"] != DBNull.Value && Convert.ToBoolean(reader["show_score"]);
                            bool showCalculator = reader["show_calculator"] != DBNull.Value && Convert.ToBoolean(reader["show_calculator"]);

                            allowScoreDisplay = showScore;
                            label4.Visible = showScore;
                            label3.Visible = showScore;
                            button2.Visible = showCalculator;
                            label6.Visible = showCalculator;
                        }
                    }
                }
            }
        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (totalSeconds > 0)
            {
                totalSeconds--;
                UpdateTimerLabel();
            }
            else
            {
                timer1.Stop();
                timerLabel1.Text = "Time Up!";
                MessageBox.Show("You didn't finish on time.", "Sorry!");
                SaveScoreAndShowResult();
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {

            calculator c = new calculator();
            c.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (isShuffleMode)
            {
                if (i > 0)
                {
                    i--;
                    LoadQuestionFromShuffledList(i);
                }
                else
                {
                    MessageBox.Show("This is the first question.");
                }
            }
            else
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    int prevId = visitedQuestionIds[currentIndex];
                    if (isShuffleMode)
                        LoadQuestionFromShuffledList(currentIndex);
                    else
                        LoadQuestionById(prevId);
                }
                else
                {
                    MessageBox.Show("This is the first question.");
                }

            }

            radiobtn();
        }


        public void radiobtn()
        {
            // Clear all first
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            txtShortAnswer.Text = "";

            // Restore previous answer if exists
            if (selectedAnswers.ContainsKey(currentQuesId))
            {
                string savedAnswer = selectedAnswers[currentQuesId];

                if (radioButton1.Text == savedAnswer) radioButton1.Checked = true;
                else if (radioButton2.Text == savedAnswer) radioButton2.Checked = true;
                else if (radioButton3.Text == savedAnswer) radioButton3.Checked = true;
                else if (radioButton4.Text == savedAnswer) radioButton4.Checked = true;
                else txtShortAnswer.Text = savedAnswer; // for SHORT answers
            }
        }




        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                int indexToJump = comboBox1.SelectedIndex;
                int questionId = visitedQuestionIds[indexToJump];

                currentIndex = indexToJump;
                LoadQuestionById(questionId);
                label8.Text = $"Question {currentIndex + 1}";
            }
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || shuffledQuestions == null || e.Index >= shuffledQuestions.Count)
                return;

            DataRow row = shuffledQuestions[e.Index];
            int quesId = Convert.ToInt32(row["qid"]);
            string savedAnswer = selectedAnswers.ContainsKey(quesId) ? selectedAnswers[quesId] : "";
            bool isVisited = visitedQuestionIds.Contains(quesId);
            bool isAnswered = !string.IsNullOrEmpty(savedAnswer);

            string displayText = $"Question {e.Index + 1}";
            Brush brush = Brushes.Black;
            Font font = e.Font;

            if (!isVisited)
            {
                brush = Brushes.Gray;
                displayText += " 🔒";   // not visited
            }
            else if (isAnswered)
            {
                // 👇 Regardless of correctness, show the same "attempted" icon
                brush = Brushes.Blue;
                font = new Font(e.Font, FontStyle.Bold);
                displayText += " ✅";   // attempted
            }
            else
            {
                brush = Brushes.Orange;
                displayText += " ⏭";   // visited but unanswered
            }

            e.DrawBackground();
            e.Graphics.DrawString(displayText, font, brush, e.Bounds);
            e.DrawFocusRectangle();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0 || shuffledQuestions == null || comboBox1.SelectedIndex >= shuffledQuestions.Count)
                return;

            int selectedIndex = comboBox1.SelectedIndex;

            // Keep indices in sync
            currentIndex = selectedIndex;
            i = currentIndex; // your "i" is used elsewhere
            currentQuesId = Convert.ToInt32(shuffledQuestions[i]["qid"]);

            // Mark visited immediately (so draw shows unlocked)
            if (!visitedQuestionIds.Contains(currentQuesId))
                visitedQuestionIds.Add(currentQuesId);

            // Load the question using unified loader
            LoadUnifiedQuestion(shuffledQuestions[i]);

            label8.Text = $"Question {currentIndex + 1} of {totalQuestionsLimit}";

            // redraw combobox so icons reflect the new visited/answered state
            comboBox1.Invalidate();
        }




        private void ComboBoxQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBox1.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= shuffledQuestions.Count) return;

            currentIndex = selectedIndex;
            i = currentIndex;
            currentQuesId = Convert.ToInt32(shuffledQuestions[i]["qid"]);

            LoadUnifiedQuestion(shuffledQuestions[i]);

            // mark as visited if not already
            if (!visitedQuestionIds.Contains(currentQuesId))
                visitedQuestionIds.Add(currentQuesId);

            label8.Text = $"Question {currentIndex + 1} of {totalQuestionsLimit}";
            radiobtn();
        }





        private void SaveScoreAndShowResult()
        {
            if (totalQuestionsLimit <= 0)
            {
                totalQuestionsLimit = Convert.ToInt32(rc.scalerReturn(
                    $"SELECT COUNT(*) FROM tbl_questions WHERE ex_id_fk = {studentlogin.exam_id}"));
            }

            float per = ((float)score / totalQuestionsLimit) * 100;

            insertclass ic = new insertclass();
            ic.insert_score(score.ToString(), studentlogin.studentid, studentlogin.exam_id, per.ToString("F2"));

            string resultSetting = rc.scalerReturn("SELECT show_result FROM tbl_exam_settings WHERE ex_id = " + studentlogin.exam_id);
            int showResult = int.TryParse(resultSetting, out int res) ? res : 0;

            this.Enabled = false;
            this.Hide();

            if (showResult == 1)
                new messageform(score, totalQuestionsLimit).Show();
            else
                new messageform2().Show();
        }


        private DataTable allQuestionsTable;

        private void Test_Load(object sender, EventArgs e)
        {
            score = 0;
            label4.Text = $"Score: {score}";
            label3.Visible = false;
            label4.Visible = false;
            label6.Visible = false;
            button2.Visible = false;

            ApplyExamSettings(int.Parse(studentlogin.exam_id));

            string query = $@"
        SELECT ques_id AS qid, q_title AS title, q_opA, q_opB, q_opC, q_opD, q_correctOpn AS correctAns, q_image, 'MCQ' AS qtype
        FROM tbl_questions
        WHERE ex_id_fk = {studentlogin.exam_id}
        UNION ALL
        SELECT sa_id AS qid, ques_title AS title, NULL AS q_opA, NULL AS q_opB, NULL AS q_opC, NULL AS q_opD, correct_answer AS correctAns, ques_image, 'SHORT' AS qtype
        FROM tbl_shortanswer
        WHERE exam_id = {studentlogin.exam_id};
    ";

            allQuestionsTable = rc.GetDataTable(query);

            // shuffle or sequential
            if (isShuffleMode)
                shuffledQuestions = allQuestionsTable.AsEnumerable().OrderBy(row => Guid.NewGuid()).ToList();
            else
                shuffledQuestions = allQuestionsTable.AsEnumerable().ToList();

            totalQuestionsLimit = shuffledQuestions.Count;
            label10.Text = $"Total Questions: {totalQuestionsLimit}";

            // Fill ComboBox once with ALL question entries
            comboBox1.Items.Clear();
            for (int j = 0; j < shuffledQuestions.Count; j++)
            {
                comboBox1.Items.Add($"Question {j + 1}");
            }

            // start at first question
            i = 0;
            currentQuesId = Convert.ToInt32(shuffledQuestions[i]["qid"]);
            visitedQuestionIds.Clear();
            visitedQuestionIds.Add(currentQuesId);
            currentIndex = 0;

            LoadUnifiedQuestion(shuffledQuestions[i]);
            label8.Text = $"Question {currentIndex + 1} of {totalQuestionsLimit}";

            int duration = rc.GetExamDuration(int.Parse(studentlogin.exam_id));
            totalSeconds = (duration > 0 ? duration : 60) * 60;
            UpdateTimerLabel();
            timer1.Start();
        }



        private void btnEndExam_Click_1(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
          "Are you sure you want to end the exam early?",
          "End Exam",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                EndExamOnce();
            }
        }

        private void EndExamOnce(string reason = null, bool showMessage = false)
        {
            // ensure this runs only once, thread-safe
            if (Interlocked.Exchange(ref _endCalled, 1) != 0) return;

            try
            {
                // stop timer safely
                timer1?.Stop();

                // unsubscribe to avoid re-triggers
                try { this.Deactivate -= Test_Deactivate; } catch { }
                try { this.FormClosing -= Test_FormClosing; } catch { }

                // persist & navigate (your method already hides and shows result/message forms)
                SaveScoreAndShowResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ending exam: " + ex.Message);
            }
            finally
            {
                // Optionally notify once AFTER saving (never from Deactivate)
                if (showMessage && !string.IsNullOrWhiteSpace(reason))
                {
                    MessageBox.Show(reason, "Exam Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void LoadUnifiedQuestion(DataRow row)
        {
            if (row == null) return;

            string qtype = row["qtype"]?.ToString() ?? "MCQ";
            label1.Text = row["title"]?.ToString() ?? "";
            correctop = row["correctAns"]?.ToString() ?? "";

            // Reset controls
            radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
            txtShortAnswer.Text = "";

            // Ensure both groupboxes visibility set explicitly
            if (qtype.Equals("MCQ", StringComparison.OrdinalIgnoreCase))
            {
                // Show MCQ group and hide short-answer group
                if (groupBox2 != null) { groupBox2.Visible = true; groupBox2.BringToFront(); }
                if (groupBox3 != null) groupBox3.Visible = false;

                txtShortAnswer.Visible = false;
                label16.Visible = false;

                // Ensure radios visible and populate texts (use safe null-coalescing)
                radioButton1.Visible = radioButton2.Visible = radioButton3.Visible = radioButton4.Visible = true;
                radioButton1.Text = row["q_opA"]?.ToString() ?? "";
                radioButton2.Text = row["q_opB"]?.ToString() ?? "";
                radioButton3.Text = row["q_opC"]?.ToString() ?? "";
                radioButton4.Text = row["q_opD"]?.ToString() ?? "";
            }
            else // SHORT
            {
                if (groupBox2 != null) groupBox2.Visible = false;
                if (groupBox3 != null) { groupBox3.Visible = true; groupBox3.BringToFront(); }

                // Hide MCQ radios
                radioButton1.Visible = radioButton2.Visible = radioButton3.Visible = radioButton4.Visible = false;

                // Show short answer textbox
                txtShortAnswer.Visible = true;
                label16.Visible = true;
                txtShortAnswer.Text = ""; // reset
            }

            // Picture
            try
            {
                if (row["q_image"] != DBNull.Value && row["q_image"] != null)
                {
                    byte[] imgBytes = (byte[])row["q_image"];
                    pictureBox1.Image = ByteArrayToImage(imgBytes);
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
            catch
            {
                pictureBox1.Image = null;
            }

            // After loading, restore any previous answer for this question
            // (currentQuesId should already be set by caller)
            radiobtn();
        }


        private void Test_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If already ended, allow close
            if (Volatile.Read(ref _endCalled) != 0) return;

            var dr = MessageBox.Show(
                "Leaving this exam will automatically end it. Do you want to continue?",
                "End Exam",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                // End quietly; SaveScoreAndShowResult will hide this form / show result
                e.Cancel = true;           // cancel default close; we control navigation
                EndExamOnce();
            }
            else
            {
                e.Cancel = true;           // stay in exam
            }
        }

        private void Test_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If they somehow force the form closed, still end exam
            EndExamOnce();
        }


       

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (shuffledQuestions == null || shuffledQuestions.Count == 0) return;

            if (i > 0)
            {
                i--;
                currentIndex = i;
                currentQuesId = Convert.ToInt32(shuffledQuestions[i]["qid"]);

                if (!visitedQuestionIds.Contains(currentQuesId))
                    visitedQuestionIds.Add(currentQuesId);

                LoadUnifiedQuestion(shuffledQuestions[i]);
                label8.Text = $"Question {currentIndex + 1} of {totalQuestionsLimit}";

                // sync combobox
                comboBox1.SelectedIndex = currentIndex;
                comboBox1.Invalidate();
            }
            else
            {
                MessageBox.Show("This is the first question.");
            }

            radiobtn();
        }

        private void Test_Deactivate(object sender, EventArgs e)
        {
            // if already ended, ignore
            if (_endCalled != 0) return;

            // Check if this form is still the active window
            if (GetForegroundWindow() == this.Handle)
            {
                // Still active → ignore (like when using ComboBox dropdown)
                return;
            }

            // If another app is active → end exam
            EndExamOnce("You left the exam window. The exam has ended.", true);
        }


    }
}