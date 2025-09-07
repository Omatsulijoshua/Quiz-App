using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Quiz_App.student_control_panel;


namespace Quiz_App
{
    public partial class Test2 : Form
    {
        public static int score = 0;
        private int examId;
        private int totalSeconds = 3600;
        private bool isShuffleMode = false;
        private int totalQuestionsLimit = 0;
        private int currentIndex = 0;
        private int currentQuesId = 0;

        private List<DataRow> shuffledQuestions = new List<DataRow>();
        private List<int> orderedQuestionIds = new List<int>();
        private List<int> visitedQuestionIds = new List<int>();
        private List<int> answeredQuestionIds = new List<int>();
        private Dictionary<int, (string Selected, string Correct)> selectedAnswers = new Dictionary<int, (string, string)>();

        // NEW: unified questions table + helpers
        private DataTable _unifiedQuestions;
        private int _currentIndex = 0;

        // helper accessors
        private DataRow CurrentRow => (_unifiedQuestions != null && _unifiedQuestions.Rows.Count > 0)
                                        ? _unifiedQuestions.Rows[_currentIndex]
                                        : null;
        private int CurrentId => CurrentRow == null ? -1 : Convert.ToInt32(CurrentRow["ques_id"]);
        private string CurrentType => CurrentRow?["q_type"]?.ToString(); // "MCQ" or "SA"



        private return_class rc = new return_class();
        private string correctop = "";
        



        public Test2()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;

            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += comboBox1_DrawItem;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void Test2_Load(object sender, EventArgs e)
        {
            score = 0;
            label4.Text = "Score: 0";
            label4.Visible = true;

            examId = ExamPreferences.SelectedExamId;
            isShuffleMode = ExamPreferences.ShuffleEnabled;
            totalQuestionsLimit = ExamPreferences.QuestionLimit;
            totalSeconds = ExamPreferences.DurationMinutes * 60;

            // NEW: hide SA UI at start
            groupBox3.Visible = false;       // contains txtboxAnswer
            txtShortAnswer.Text = string.Empty;

            UpdateTimerLabel();
            label10.Text = $"Questions: {totalQuestionsLimit}";

            // REPLACE your old LoadQuestions() call with this:
            BuildUnifiedQuestionsAndShowFirst();

            timer1.Start();
        }

        private void LoadQuestions()
        {
            DataTable questions = rc.GetDataTable($"SELECT TOP {totalQuestionsLimit} * FROM tbl_past_questions WHERE ex_id_fk = {examId}");

            if (isShuffleMode)
            {
                shuffledQuestions = questions.AsEnumerable().OrderBy(r => Guid.NewGuid()).ToList();
                currentIndex = 0;
                LoadQuestionFromShuffledList(currentIndex);
            }
            else
            {
                questions = questions.AsEnumerable().OrderBy(r => Convert.ToInt32(r["ques_id"])).CopyToDataTable();
                orderedQuestionIds.Clear();
                foreach (DataRow row in questions.Rows)
                {
                    int id = Convert.ToInt32(row["ques_id"]);
                    orderedQuestionIds.Add(id);
                    visitedQuestionIds.Add(id);
                    comboBox1.Items.Add($"Question {comboBox1.Items.Count + 1}");
                }

                currentIndex = 0;
                currentQuesId = orderedQuestionIds[currentIndex];
                LoadQuestionById(currentQuesId);
            }
        }

        private void LoadQuestionFromShuffledList(int index)
        {
            if (index < 0 || index >= shuffledQuestions.Count) return;

            var row = shuffledQuestions[index];
            currentQuesId = Convert.ToInt32(row["ques_id"]);

            label1.Text = row["q_title"].ToString();
            radioButton1.Text = row["q_opA"].ToString();
            radioButton2.Text = row["q_opB"].ToString();
            radioButton3.Text = row["q_opC"].ToString();
            radioButton4.Text = row["q_opD"].ToString();

            correctop = row["q_correctOpn"].ToString();

            // ✅ Load image if available
            if (row.Table.Columns.Contains("q_image") && row["q_image"] != DBNull.Value)
            {
                byte[] imgBytes = (byte[])row["q_image"];
                pictureBoxQuestion.Image = ByteArrayToImage(imgBytes);
                pictureBoxQuestion.Visible = true;
            }
            else
            {
                pictureBoxQuestion.Image = null;
                pictureBoxQuestion.Visible = false;
            }

            if (!visitedQuestionIds.Contains(currentQuesId))
            {
                visitedQuestionIds.Add(currentQuesId);
                comboBox1.Items.Add($"Question {visitedQuestionIds.Count}");
            }

            label8.Text = $"Question {currentIndex + 1}";
            ResetRadioButtons();
        }


        private void LoadQuestionById(int questionId)
        {
            DataRow row = GetDataRow($"SELECT * FROM tbl_past_questions WHERE ques_id = {questionId} AND ex_id_fk = {examId}");

            if (row == null) return;

            currentQuesId = questionId;

            label1.Text = row["q_title"].ToString();
            radioButton1.Text = row["q_opA"].ToString();
            radioButton2.Text = row["q_opB"].ToString();
            radioButton3.Text = row["q_opC"].ToString();
            radioButton4.Text = row["q_opD"].ToString();
            correctop = row["q_correctOpn"].ToString();

            // ✅ Load image if available
            if (row.Table.Columns.Contains("q_image") && row["q_image"] != DBNull.Value)
            {
                byte[] imgBytes = (byte[])row["q_image"];
                pictureBoxQuestion.Image = ByteArrayToImage(imgBytes);
                pictureBoxQuestion.Visible = true;
            }
            else
            {
                pictureBoxQuestion.Image = null;
                pictureBoxQuestion.Visible = false;
            }

            if (!visitedQuestionIds.Contains(questionId))
                visitedQuestionIds.Add(questionId);

            label8.Text = $"Question {currentIndex + 1}";
            ResetRadioButtons();
        }


        private void ResetRadioButtons()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;

            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            radioButton4.Enabled = true;

            // Apply previous answer if any
            if (selectedAnswers.ContainsKey(currentQuesId))
            {
                string ans = selectedAnswers[currentQuesId].Selected;  // ✅ now correct

                if (radioButton1.Text.Replace(" ✅", "").Replace(" ❌", "") == ans) radioButton1.Checked = true;
                else if (radioButton2.Text.Replace(" ✅", "").Replace(" ❌", "") == ans) radioButton2.Checked = true;
                else if (radioButton3.Text.Replace(" ✅", "").Replace(" ❌", "") == ans) radioButton3.Checked = true;
                else if (radioButton4.Text.Replace(" ✅", "").Replace(" ❌", "") == ans) radioButton4.Checked = true;

                // Lock options again since question was already answered
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
            }
        }


        private void SaveAnswer()
        {
            if (CurrentRow == null) return;

            bool isSA = string.Equals(CurrentType, "SA", StringComparison.OrdinalIgnoreCase);

            if (isSA)
            {
                string selected = (txtShortAnswer.Text ?? "").Trim();
                if (string.IsNullOrEmpty(selected)) return;

                string correct = (CurrentRow["correct_answer"]?.ToString() ?? "").Trim();

                selectedAnswers[CurrentId] = (selected, correct);

                if (!answeredQuestionIds.Contains(CurrentId))
                    answeredQuestionIds.Add(CurrentId);

                // Recalculate score across all answers
                score = selectedAnswers.Count(kvp =>
                    string.Equals(kvp.Value.Selected?.Trim(), kvp.Value.Correct?.Trim(), StringComparison.OrdinalIgnoreCase));

                label4.Text = $"Score: {score}";
                label4.Visible = true;

                // lock editing after answer
                txtShortAnswer.Enabled = false;
                comboBox1.Invalidate();
                return;
            }
            else
            {
                // MCQ (your existing logic)
                string selected = "";
                if (radioButton1.Checked) selected = radioButton1.Text.Replace(" ✅", "").Replace(" ❌", "");
                else if (radioButton2.Checked) selected = radioButton2.Text.Replace(" ✅", "").Replace(" ❌", "");
                else if (radioButton3.Checked) selected = radioButton3.Text.Replace(" ✅", "").Replace(" ❌", "");
                else if (radioButton4.Checked) selected = radioButton4.Text.Replace(" ✅", "").Replace(" ❌", "");

                if (string.IsNullOrEmpty(selected)) return;

                string sanitizedCorrect = (correctop ?? "").Replace(" ✅", "").Replace(" ❌", "");

                selectedAnswers[CurrentId] = (selected, sanitizedCorrect);

                if (!answeredQuestionIds.Contains(CurrentId))
                    answeredQuestionIds.Add(CurrentId);

                score = selectedAnswers.Count(kvp => kvp.Value.Selected == kvp.Value.Correct);

                label4.Text = $"Score: {score}";
                label4.Visible = true;

                // reset texts then mark ✅ / ❌
                radioButton1.Text = radioButton1.Text.Replace(" ✅", "").Replace(" ❌", "");
                radioButton2.Text = radioButton2.Text.Replace(" ✅", "").Replace(" ❌", "");
                radioButton3.Text = radioButton3.Text.Replace(" ✅", "").Replace(" ❌", "");
                radioButton4.Text = radioButton4.Text.Replace(" ✅", "").Replace(" ❌", "");

                if (radioButton1.Text == sanitizedCorrect) radioButton1.Text += " ✅";
                else if (radioButton1.Checked) radioButton1.Text += " ❌";

                if (radioButton2.Text == sanitizedCorrect) radioButton2.Text += " ✅";
                else if (radioButton2.Checked) radioButton2.Text += " ❌";

                if (radioButton3.Text == sanitizedCorrect) radioButton3.Text += " ✅";
                else if (radioButton3.Checked) radioButton3.Text += " ❌";

                if (radioButton4.Text == sanitizedCorrect) radioButton4.Text += " ✅";
                else if (radioButton4.Checked) radioButton4.Text += " ❌";

                radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = radioButton4.Enabled = false;
                comboBox1.Invalidate();
            }
        }


        private void btnSubmitAnswer_Click(object sender, EventArgs e)
        {
            SaveAnswer();
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
                MessageBox.Show("Time's up!", "Exam Finished");
                EndExam();
            }
        }

        private void UpdateTimerLabel()
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            timerLabel1.Text = time.ToString(@"hh\:mm\:ss");
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
           
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            
        }

        private void btnEndExam_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("End exam early?", "Confirm", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                EndExam();
            }
        }

        private void EndExam()
        {
            timer1.Stop();
            float percentage = ((float)score / totalQuestionsLimit) * 100;
            this.Hide();
            new messageform(score, totalQuestionsLimit).Show();
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= comboBox1.Items.Count) return;

            int quesId = (e.Index < visitedQuestionIds.Count) ? visitedQuestionIds[e.Index] : -1;
            bool isAnswered = answeredQuestionIds.Contains(quesId);

            string text = $"Question {e.Index + 1}";
            Brush brush = Brushes.Black;
            Font font = e.Font;

            if (quesId == -1) brush = Brushes.Gray;
            else if (isAnswered) { brush = Brushes.Green; font = new Font(e.Font, FontStyle.Bold); text += " ✅"; }
            else text += " ❌";

            e.DrawBackground();
            e.Graphics.DrawString(text, font, brush, e.Bounds);
            e.DrawFocusRectangle();
        }

       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = comboBox1.SelectedIndex;
            if (idx >= 0 && idx < _unifiedQuestions.Rows.Count)
            {
                _currentIndex = idx;
                PresentQuestionAt(_currentIndex);
            }
        }


        private DataRow GetDataRow(string query)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0];
                    else
                        return null;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_unifiedQuestions == null) return;

            if (_currentIndex > 0)
            {
                _currentIndex--;
                PresentQuestionAt(_currentIndex);
            }
            else
            {
                MessageBox.Show("This is the first question.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveAnswer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveAnswer();
            if (_unifiedQuestions == null) return;

            if (_currentIndex < _unifiedQuestions.Rows.Count - 1)
            {
                _currentIndex++;
                PresentQuestionAt(_currentIndex);
            }
            else
            {
                MessageBox.Show("This is the last question.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            calculator c = new calculator();
            c.Show();
        }

        private Image ByteArrayToImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void BuildUnifiedQuestionsAndShowFirst()
        {
            _unifiedQuestions = new DataTable();
            _unifiedQuestions.Columns.Add("ques_id", typeof(int));       // primary id
            _unifiedQuestions.Columns.Add("q_type", typeof(string));     // "MCQ" or "SA"
            _unifiedQuestions.Columns.Add("q_title", typeof(string));
            _unifiedQuestions.Columns.Add("q_opA", typeof(string));
            _unifiedQuestions.Columns.Add("q_opB", typeof(string));
            _unifiedQuestions.Columns.Add("q_opC", typeof(string));
            _unifiedQuestions.Columns.Add("q_opD", typeof(string));
            _unifiedQuestions.Columns.Add("q_correctOpn", typeof(string));  // MCQ correct option
            _unifiedQuestions.Columns.Add("correct_answer", typeof(string)); // SA correct answer
            _unifiedQuestions.Columns.Add("q_image", typeof(byte[]));        // image bytes

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                // 1) Load MCQs
                using (var cmd = new SqlCommand(@"
            SELECT ques_id, ex_id_fk, q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_image
            FROM tbl_past_questions
            WHERE ex_id_fk = @examId", con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var row = _unifiedQuestions.NewRow();
                            row["ques_id"] = r["ques_id"];
                            row["q_type"] = "MCQ";
                            row["q_title"] = r["q_title"]?.ToString();
                            row["q_opA"] = r["q_opA"]?.ToString();
                            row["q_opB"] = r["q_opB"]?.ToString();
                            row["q_opC"] = r["q_opC"]?.ToString();
                            row["q_opD"] = r["q_opD"]?.ToString();
                            row["q_correctOpn"] = r["q_correctOpn"]?.ToString();
                            row["correct_answer"] = DBNull.Value;
                            row["q_image"] = r["q_image"] == DBNull.Value ? null : (byte[])r["q_image"];
                            _unifiedQuestions.Rows.Add(row);
                        }
                    }
                }

                // 2) Load Short-Answer questions
                using (var cmd = new SqlCommand(@"
            SELECT sa_id, ex_id_fk, sa_question, sa_answer, sa_image
            FROM tbl_past_shortanswer
            WHERE ex_id_fk = @examId", con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var row = _unifiedQuestions.NewRow();
                            row["ques_id"] = r["sa_id"];
                            row["q_type"] = "SA";
                            row["q_title"] = r["sa_question"]?.ToString();
                            row["q_opA"] = "";
                            row["q_opB"] = "";
                            row["q_opC"] = "";
                            row["q_opD"] = "";
                            row["q_correctOpn"] = DBNull.Value;
                            row["correct_answer"] = r["sa_answer"]?.ToString();
                            row["q_image"] = r["sa_image"] == DBNull.Value ? null : (byte[])r["sa_image"];
                            _unifiedQuestions.Rows.Add(row);
                        }
                    }
                }
            }

            // Apply shuffle or ordered by ques_id
            IEnumerable<DataRow> rows = _unifiedQuestions.AsEnumerable();
            if (isShuffleMode)
                rows = rows.OrderBy(_ => Guid.NewGuid());
            else
                rows = rows.OrderBy(r => r.Field<int>("ques_id"));

            // Apply total question limit safely
            if (totalQuestionsLimit > 0 && totalQuestionsLimit < rows.Count())
                rows = rows.Take(totalQuestionsLimit);

            _unifiedQuestions = rows.Any() ? rows.CopyToDataTable() : _unifiedQuestions.Clone();

            // Reset tracking
            _currentIndex = 0;
            visitedQuestionIds.Clear();
            answeredQuestionIds.Clear();
            comboBox1.Items.Clear();
            selectedAnswers.Clear();

            // Show first question
            if (_unifiedQuestions.Rows.Count > 0)
                PresentQuestionAt(_currentIndex);
            else
                MessageBox.Show("No questions available for this exam.");
        }


        private void PresentQuestionAt(int index)
        {
            if (_unifiedQuestions == null || _unifiedQuestions.Rows.Count == 0) return;
            if (index < 0 || index >= _unifiedQuestions.Rows.Count) return;

            _currentIndex = index;
            var row = _unifiedQuestions.Rows[_currentIndex];

            // Common UI
            label1.Text = row["q_title"]?.ToString() ?? "";

            // Image
            if (row["q_image"] != DBNull.Value && row["q_image"] is byte[] bytes && bytes.Length > 0)
            {
                pictureBoxQuestion.Image = ByteArrayToImage(bytes);
                pictureBoxQuestion.Visible = true;
            }
            else
            {
                pictureBoxQuestion.Image = null;
                pictureBoxQuestion.Visible = false;
            }

            // Type-specific UI
            bool isSA = string.Equals(row["q_type"]?.ToString(), "SA", StringComparison.OrdinalIgnoreCase);

            // MCQ controls
            radioButton1.Visible = !isSA;
            radioButton2.Visible = !isSA;
            radioButton3.Visible = !isSA;
            radioButton4.Visible = !isSA;

            // SA controls
            groupBox3.Visible = isSA; // contains txtboxAnswer

            if (isSA)
            {
                // No options; ensure radios are reset & disabled
                radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
                radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = radioButton4.Enabled = false;

                // restore prior answer if any
                txtShortAnswer.Text = selectedAnswers.ContainsKey(CurrentId) ? selectedAnswers[CurrentId].Selected : "";
                txtShortAnswer.Enabled = !selectedAnswers.ContainsKey(CurrentId);

                correctop = row["correct_answer"]?.ToString() ?? "";
            }
            else
            {
                // Fill options and enable radios
                radioButton1.Text = (row["q_opA"]?.ToString() ?? "").Replace(" ✅", "").Replace(" ❌", "");
                radioButton2.Text = (row["q_opB"]?.ToString() ?? "").Replace(" ✅", "").Replace(" ❌", "");
                radioButton3.Text = (row["q_opC"]?.ToString() ?? "").Replace(" ✅", "").Replace(" ❌", "");
                radioButton4.Text = (row["q_opD"]?.ToString() ?? "").Replace(" ✅", "").Replace(" ❌", "");

                radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = radioButton4.Enabled = true;

                correctop = row["q_correctOpn"]?.ToString() ?? "";

                // restore prior answer if any (MCQ)
                ResetRadioButtons();
            }

            // visited tracking + combobox
            if (!visitedQuestionIds.Contains(CurrentId))
            {
                visitedQuestionIds.Add(CurrentId);
                comboBox1.Items.Add($"Question {visitedQuestionIds.Count}");
            }

            label8.Text = $"Question {_currentIndex + 1}";
            comboBox1.SelectedIndex = Math.Min(_currentIndex, comboBox1.Items.Count - 1);
        }





    }
}
