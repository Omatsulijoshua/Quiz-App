using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Test3 : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private readonly int _examId;
        private readonly int _studentId;
        private readonly int _initialScore;
        private readonly int _initialTotalQuestions;
        private DataTable _questions;
        private int _currentIndex = 0;
        private PictureBox _questionImageBox;
        private Panel _questionImagePanel;
        private Button _calculatorButton;
        private calculator _calculatorForm;
        private int _endCalled = 0;
        private bool _openingCalculator;
        private bool _calculatorSessionActive;
        private bool _calculatorActivatedOnce;
        private DateTime _calculatorLaunchUtc = DateTime.MinValue;

        private System.Windows.Forms.Timer _autoSaveTimer;
        private System.Windows.Forms.Timer _countdownTimer;
        private TimeSpan _timeLeft;
        // constructor: pass examId and studentId; durationMinutes = 0 for untimed
        //public Test3(int examId, int studentId, int score = 0, int totalQuestions = 0, int durationMinutes = 0)
        //{
        //    InitializeComponent();

        //    _examId = examId;
        //    _studentId = studentId;
        //    _initialScore = score;
        //    _initialTotalQuestions = totalQuestions;

        //    // autosave every 30 seconds
        //    _autoSaveTimer = new System.Windows.Forms.Timer { Interval = 30_000 };
        //    _autoSaveTimer.Tick += (s, e) => SaveDraftForCurrentQuestion();

        //    if (durationMinutes > 0)
        //    {
        //        _timeLeft = TimeSpan.FromMinutes(durationMinutes);
        //        _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        //        _countdownTimer.Tick += CountdownTimer_Tick;
        //    }

        //    // wire up buttons if not wired in designer (safe to re-wire)
        //    btnPrev.Click -= BtnPrev_Click;
        //    btnPrev.Click += BtnPrev_Click;

          

        //    btnSaveDraft.Click -= BtnSaveDraft_Click;
        //    btnSaveDraft.Click += BtnSaveDraft_Click;

        //    btnSubmitAll.Click -= BtnSubmitAll_Click;
        //    btnSubmitAll.Click += BtnSubmitAll_Click;

        //    this.FormClosing -= Test3_FormClosing;
        //    this.FormClosing += Test3_FormClosing;
        //}

        public Test3(int examId, int studentId, int score = 0, int totalQuestions = 0, int durationMinutes = 0)
        {
            InitializeComponent();

            _examId = examId;
            _studentId = studentId;
            _initialScore = score;
            _initialTotalQuestions = totalQuestions;

            // Load duration if not passed
            if (durationMinutes == 0)
                durationMinutes = GetTheoryDuration(_examId);

            if (durationMinutes <= 0)
            {
                durationMinutes = 60;
            }

            // Initialize autosave
            _autoSaveTimer = new System.Windows.Forms.Timer { Interval = 30_000 };
            _autoSaveTimer.Tick += (s, e) => SaveDraftForCurrentQuestion();

            // Initialize timer if duration exists
            if (durationMinutes > 0)
            {
                _timeLeft = TimeSpan.FromMinutes(durationMinutes);
                _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
                _countdownTimer.Tick += CountdownTimer_Tick;
            }

            // Wire buttons safely
            btnPrev.Click -= BtnPrev_Click;
            btnPrev.Click += BtnPrev_Click;

            btnNext.Click -= btnNext_Click;
            btnNext.Click += btnNext_Click;

            btnSaveDraft.Click -= BtnSaveDraft_Click;
            btnSaveDraft.Click += BtnSaveDraft_Click;

            btnSubmitAll.Click -= BtnSubmitAll_Click;
            btnSubmitAll.Click += BtnSubmitAll_Click;

            this.FormClosing -= Test3_FormClosing;
            this.FormClosing += Test3_FormClosing;
            this.Deactivate -= Test3_Deactivate;
            this.Deactivate += Test3_Deactivate;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (_timeLeft == TimeSpan.Zero) return;

            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));

            if (lblTimer != null)
                lblTimer.Text = _timeLeft.ToString(@"hh\:mm\:ss");

            if (_timeLeft <= TimeSpan.Zero)
            {
                _countdownTimer?.Stop();
                _autoSaveTimer?.Stop();

                SaveDraftForCurrentQuestion();
                SubmitAllAnswers();
            }
        }

        // ? Function to get theory exam duration from tbl_exam_settings
        private int GetTheoryDuration(int examId)
        {
            int duration = 0;
            try
            {
                using (SqlConnection con = connection_class.GetConnection())
                {
                    con.Open();
                    string theoryColumn = ResolveTheoryDurationColumn(con);
                    if (string.IsNullOrWhiteSpace(theoryColumn))
                    {
                        return 0;
                    }

                    string query = "SELECT " + theoryColumn + " FROM tbl_exam_settings WHERE ex_id = @examId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@examId", examId);

                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int minutes))
                        duration = minutes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting theory duration: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return duration;
        }
        private void Test3_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Resize -= Test3_Resize;
            this.Resize += Test3_Resize;
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(8, 12, 22), Color.FromArgb(18, 30, 48));
            BackColor = Color.FromArgb(8, 12, 22);

            if (lblTimer != null)
            {
                lblTimer.Text = _timeLeft.TotalSeconds > 0
                    ? _timeLeft.ToString(@"hh\:mm\:ss")
                    : "01:00:00";
            }

            LoadQuestionsFromDb();

            if (_questions == null || _questions.Rows.Count == 0)
            {
                MessageBox.Show("No theory questions found for this exam.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNext.Enabled = btnPrev.Enabled = btnSaveDraft.Enabled = btnSubmitAll.Enabled = false;
                return;
            }

            if (_questions.Columns.Contains("exam_name"))
                lblExamTitle.Text = _questions.Rows[0]["exam_name"].ToString();

            EnsureQuestionImageSurface();
            EnsureCalculatorButton();
            ArrangeTheoryWorkspace(false);
            ShowQuestion(0);

            _autoSaveTimer?.Start();
            _countdownTimer?.Start();
        }




        private void LoadQuestionsFromDb()
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    if (!TheoryQuestionsTableExists(conn))
                    {
                        _questions = new DataTable();
                        return;
                    }

                    string sql = @"
                        SELECT tq.theory_id, tq.question_number, tq.question_text, tq.mark, tq.model_answer, tq.question_image,
                               e.ex_id AS exam_id, e.ex_name AS exam_name
                        FROM tbl_theory_questions tq
                        INNER JOIN tbl_exams e ON tq.exam_fk_id = e.ex_id
                        WHERE tq.exam_fk_id = @examId
                        ORDER BY tq.question_number";

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@examId", _examId);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        _questions = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading questions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowQuestion(int index)
        {
            if (_questions == null || _questions.Rows.Count == 0) return;
            if (index < 0 || index >= _questions.Rows.Count) return;

            // Save previous automatically when switching (optional)
            // SaveDraftForCurrentQuestion();

            _currentIndex = index;
            DataRow row = _questions.Rows[index];

            lblQuestionIndex.Text = $"Q {index + 1} / {_questions.Rows.Count}";
            lblQuestionMark.Text = $"Mark: {row["mark"]}";
            rtbQuestion.Text = row["question_text"].ToString();
            rtbQuestion.ReadOnly = true;
            ShowTheoryQuestionImage(row["question_image"] == DBNull.Value ? null : (byte[])row["question_image"]);

            int theoryId = Convert.ToInt32(row["theory_id"]);
            rtbAnswer.Text = LoadSavedAnswerText(theoryId) ?? string.Empty;

            btnPrev.Enabled = index > 0;
            btnNext.Enabled = index < _questions.Rows.Count - 1;

            if (progressBarQuestions != null)
            {
                progressBarQuestions.Maximum = Math.Max(1, _questions.Rows.Count);
                progressBarQuestions.Value = Math.Min(progressBarQuestions.Maximum, index + 1);
            }
        }

        private string LoadSavedAnswerText(int theoryId)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT answer_text FROM tbl_theory_answers 
                                   WHERE theory_fk_id = @theoryId AND student_fk_id = @studentId AND exam_fk_id = @examId";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@theoryId", theoryId);
                        cmd.Parameters.AddWithValue("@studentId", _studentId);
                        cmd.Parameters.AddWithValue("@examId", _examId);

                        object result = cmd.ExecuteScalar();
                        return result == null || result == DBNull.Value ? null : result.ToString();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private void SaveDraftForCurrentQuestion()
        {
            if (_questions == null || _questions.Rows.Count == 0) return;

            DataRow row = _questions.Rows[_currentIndex];
            int theoryId = Convert.ToInt32(row["theory_id"]);
            string answerText = (rtbAnswer?.Text ?? string.Empty).Trim();

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    string chkSql = @"SELECT answer_id FROM tbl_theory_answers 
                                      WHERE theory_fk_id = @theoryId AND student_fk_id = @studentId AND exam_fk_id = @examId";
                    using (SqlCommand chkCmd = new SqlCommand(chkSql, conn))
                    {
                        chkCmd.Parameters.AddWithValue("@theoryId", theoryId);
                        chkCmd.Parameters.AddWithValue("@studentId", _studentId);
                        chkCmd.Parameters.AddWithValue("@examId", _examId);

                        object existing = chkCmd.ExecuteScalar();

                        if (existing == null)
                        {
                            string insertSql = @"
                                INSERT INTO tbl_theory_answers (theory_fk_id, student_fk_id, exam_fk_id, answer_text, last_saved_at, is_submitted)
                                VALUES (@theoryId, @studentId, @examId, @answerText, @now, 0)";
                            using (SqlCommand ins = new SqlCommand(insertSql, conn))
                            {
                                ins.Parameters.AddWithValue("@theoryId", theoryId);
                                ins.Parameters.AddWithValue("@studentId", _studentId);
                                ins.Parameters.AddWithValue("@examId", _examId);
                                ins.Parameters.AddWithValue("@answerText", string.IsNullOrEmpty(answerText) ? (object)DBNull.Value : answerText);
                                ins.Parameters.AddWithValue("@now", DateTime.Now);
                                ins.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string updateSql = @"
                                UPDATE tbl_theory_answers
                                SET answer_text = @answerText, last_saved_at = @now
                                WHERE answer_id = @answerId";
                            using (SqlCommand upd = new SqlCommand(updateSql, conn))
                            {
                                upd.Parameters.AddWithValue("@answerText", string.IsNullOrEmpty(answerText) ? (object)DBNull.Value : answerText);
                                upd.Parameters.AddWithValue("@now", DateTime.Now);
                                upd.Parameters.AddWithValue("@answerId", Convert.ToInt32(existing));
                                upd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // fail silently - don't block the user; optionally log
            }
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            // save current
            SaveDraftForCurrentQuestion();

            if (_currentIndex > 0)
                ShowQuestion(_currentIndex - 1);
        }

      

        private void BtnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveDraftForCurrentQuestion();
            MessageBox.Show("Answer saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSubmitAll_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Are you sure you want to submit? You cannot undo this action.",
        "Confirm Submission",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    );

            if (result == DialogResult.Yes)
            {
                // ? Save or finalize the exam here
               // SaveExamResults();  // (optional – your existing saving logic)

                // ? Move to exam ended form
                SaveDraftForCurrentQuestion();
                SubmitAllAnswers();
            }
            else
            {
                // ? User cancelled
                MessageBox.Show("Submission cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EndTheoryExamOnce(string reason = null, bool showMessage = false)
        {
            if (Interlocked.Exchange(ref _endCalled, 1) != 0)
            {
                return;
            }

            try
            {
                SaveDraftForCurrentQuestion();
                SubmitAllAnswers();
            }
            finally
            {
                if (showMessage && !string.IsNullOrWhiteSpace(reason))
                {
                    MessageBox.Show(reason, "Exam Ended", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void SubmitAllAnswers()
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow q in _questions.Rows)
                            {
                                int theoryId = Convert.ToInt32(q["theory_id"]);

                                string checkSql = @"SELECT answer_id FROM tbl_theory_answers 
                                                    WHERE theory_fk_id = @theoryId AND student_fk_id = @studentId AND exam_fk_id = @examId";
                                using (SqlCommand chk = new SqlCommand(checkSql, conn, tx))
                                {
                                    chk.Parameters.AddWithValue("@theoryId", theoryId);
                                    chk.Parameters.AddWithValue("@studentId", _studentId);
                                    chk.Parameters.AddWithValue("@examId", _examId);

                                    object existing = chk.ExecuteScalar();

                                    if (existing == null)
                                    {
                                        string ins = @"INSERT INTO tbl_theory_answers
                                            (theory_fk_id, student_fk_id, exam_fk_id, answer_text, is_submitted, submitted_at, last_saved_at)
                                            VALUES (@theoryId, @studentId, @examId, @answerText, 1, @now, @now)";
                                        using (SqlCommand ci = new SqlCommand(ins, conn, tx))
                                        {
                                            ci.Parameters.AddWithValue("@theoryId", theoryId);
                                            ci.Parameters.AddWithValue("@studentId", _studentId);
                                            ci.Parameters.AddWithValue("@examId", _examId);
                                            ci.Parameters.AddWithValue("@answerText", DBNull.Value);
                                            ci.Parameters.AddWithValue("@now", DateTime.Now);
                                            ci.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        string upd = @"UPDATE tbl_theory_answers 
                                                       SET is_submitted = 1, submitted_at = @now
                                                       WHERE answer_id = @answerId";
                                        using (SqlCommand uu = new SqlCommand(upd, conn, tx))
                                        {
                                            uu.Parameters.AddWithValue("@now", DateTime.Now);
                                            uu.Parameters.AddWithValue("@answerId", Convert.ToInt32(existing));
                                            uu.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            tx.Commit();

                            _autoSaveTimer?.Stop();
                            _countdownTimer?.Stop();
                            FinalizeTheoryTimers();

                            examEndedForm endedForm = new examEndedForm();
                            endedForm.Show();
                            Close();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            Interlocked.Exchange(ref _endCalled, 0);
                            MessageBox.Show("Submit failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Interlocked.Exchange(ref _endCalled, 0);
                MessageBox.Show("Submit failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Test3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Volatile.Read(ref _endCalled) != 0)
            {
                FinalizeTheoryTimers();
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Leaving this theory exam will automatically end it. Do you want to continue?",
                "End Theory Exam",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                e.Cancel = true;
                EndTheoryExamOnce();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // save current
            SaveDraftForCurrentQuestion();
            if (_currentIndex < _questions.Rows.Count - 1)
                ShowQuestion(_currentIndex + 1);
        }

        private string ResolveTheoryDurationColumn(SqlConnection connection)
        {
            if (ColumnExists(connection, "tbl_exam_settings", "theory_duration_minutes"))
            {
                return "theory_duration_minutes";
            }

            if (ColumnExists(connection, "tbl_exam_settings", "theory_duration"))
            {
                return "theory_duration";
            }

            return null;
        }

        private bool TheoryQuestionsTableExists(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbl_theory_questions'",
                connection))
            {
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void EnsureQuestionImageSurface()
        {
            if (_questionImagePanel != null)
            {
                return;
            }

            _questionImagePanel = new Panel
            {
                BackColor = Color.FromArgb(18, 26, 42),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(rtbQuestion.Right + 30, rtbQuestion.Top),
                Size = new Size(260, 250),
                Visible = false
            };

            _questionImageBox = new PictureBox
            {
                Parent = _questionImagePanel,
                BackColor = Color.FromArgb(14, 20, 32),
                Location = new Point(12, 12),
                Size = new Size(_questionImagePanel.Width - 24, _questionImagePanel.Height - 24),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            Controls.Add(_questionImagePanel);
            _questionImagePanel.BringToFront();
        }

        private void ShowTheoryQuestionImage(byte[] imageBytes)
        {
            EnsureQuestionImageSurface();

            if (_questionImageBox.Image != null)
            {
                Image oldImage = _questionImageBox.Image;
                _questionImageBox.Image = null;
                oldImage.Dispose();
            }

            if (imageBytes == null || imageBytes.Length == 0)
            {
                _questionImagePanel.Visible = false;
                ArrangeTheoryWorkspace(false);
                return;
            }

            using (MemoryStream stream = new MemoryStream(imageBytes))
            using (Image source = Image.FromStream(stream))
            {
                _questionImageBox.Image = new Bitmap(source);
            }

            _questionImagePanel.Visible = true;
            ArrangeTheoryWorkspace(true);
        }

        private void EnsureCalculatorButton()
        {
            if (_calculatorButton != null)
            {
                return;
            }

            _calculatorButton = new Button
            {
                Name = "btnTheoryCalculator",
                Text = "Calculator",
                Size = new Size(190, 48)
            };

            ModernUi.StyleSecondaryButton(_calculatorButton);
            _calculatorButton.Click += (s, e) =>
            {
                if (_calculatorForm == null || _calculatorForm.IsDisposed)
                {
                    _openingCalculator = true;
                    _calculatorSessionActive = true;
                    _calculatorActivatedOnce = false;
                    _calculatorLaunchUtc = DateTime.UtcNow;
                    this.Deactivate -= Test3_Deactivate;
                    _calculatorForm = new calculator
                    {
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    _calculatorForm.Deactivate += CalculatorForm_Deactivate;
                    _calculatorForm.Activated += (sender, args) =>
                    {
                        _openingCalculator = false;
                        _calculatorActivatedOnce = true;
                    };
                    _calculatorForm.FormClosed += (sender, args) =>
                    {
                        _calculatorForm = null;
                        _openingCalculator = false;
                        _calculatorSessionActive = false;
                        _calculatorActivatedOnce = false;
                        this.Deactivate -= Test3_Deactivate;
                        this.Deactivate += Test3_Deactivate;
                    };
                    _calculatorForm.ShowDialog(this);
                }
                else
                {
                    _calculatorSessionActive = true;
                    _calculatorForm.BringToFront();
                    _calculatorForm.WindowState = FormWindowState.Normal;
                    _calculatorForm.Activate();
                }
            };

            Controls.Add(_calculatorButton);
            _calculatorButton.BringToFront();
        }

        private void ArrangeTheoryWorkspace(bool showImage)
        {
            int margin = Math.Max(24, ClientSize.Width / 50);
            int totalWidth = ClientSize.Width - (margin * 2);
            int centerGap = Math.Max(18, totalWidth / 45);
            int headerTop = Math.Max(28, ClientSize.Height / 22);
            int topTitleY = headerTop;
            int contentTop = Math.Max(270, ClientSize.Height / 3);
            int contentBottom = Math.Max(contentTop + 220, ClientSize.Height - 170);
            int boxHeight = Math.Max(250, contentBottom - (contentTop + 46));
            int columnWidth = showImage
                ? Math.Max(250, (totalWidth - (centerGap * 2)) / 3)
                : Math.Max(420, (totalWidth - centerGap) / 2);
            int textWidth = columnWidth;
            int imageWidth = showImage ? columnWidth : 0;
            int questionLeft = margin;
            int imageLeft = questionLeft + textWidth + centerGap;
            int answerLeft = showImage ? imageLeft + imageWidth + centerGap : questionLeft + textWidth + centerGap;

            lblExamTitle.Location = new Point(Math.Max(margin, (ClientSize.Width - 420) / 2), topTitleY);
            lblExamTitle.Size = new Size(420, 48);

            lblQuestionIndex.Location = new Point(margin, topTitleY + 110);
            lblQuestionIndex.Size = new Size(220, 42);
            lblQuestionMark.Location = new Point(margin, topTitleY + 168);
            lblQuestionMark.Size = new Size(220, 42);

            label4.Location = new Point(ClientSize.Width - margin - 320, topTitleY + 40);
            label4.Size = new Size(130, 32);
            lblTimer.Location = new Point(ClientSize.Width - margin - 180, topTitleY + 34);
            lblTimer.Size = new Size(170, 36);

            int labelY = contentTop;
            int boxY = contentTop + 56;

            label1.Location = new Point(questionLeft, labelY);
            label1.Size = new Size(textWidth, 34);
            rtbQuestion.Location = new Point(questionLeft, boxY);
            rtbQuestion.Size = new Size(textWidth, boxHeight);

            if (showImage)
            {
                _questionImagePanel.Location = new Point(imageLeft, boxY);
                _questionImagePanel.Size = new Size(imageWidth, boxHeight);
                _questionImageBox.Location = new Point(12, 12);
                _questionImageBox.Size = new Size(_questionImagePanel.Width - 24, _questionImagePanel.Height - 24);
                _questionImagePanel.Visible = true;
                _questionImagePanel.BringToFront();
            }
            else
            {
                _questionImagePanel.Visible = false;
            }

            label2.Location = new Point(answerLeft, labelY);
            label2.Size = new Size(textWidth, 34);
            rtbAnswer.Location = new Point(answerLeft, boxY);
            rtbAnswer.Size = new Size(textWidth, boxHeight);

            progressBarQuestions.Location = new Point(margin, ClientSize.Height - 150);
            progressBarQuestions.Size = new Size(ClientSize.Width - (margin * 2), 38);

            int buttonTop = ClientSize.Height - 92;
            btnPrev.Location = new Point(margin, buttonTop);
            btnSaveDraft.Location = new Point((ClientSize.Width / 2) - (btnSaveDraft.Width / 2), buttonTop);
            btnSubmitAll.Location = new Point(ClientSize.Width - margin - btnSubmitAll.Width, buttonTop);
            btnNext.Location = new Point(btnSubmitAll.Left - btnNext.Width - 24, buttonTop);

            if (_calculatorButton != null)
            {
                _calculatorButton.Location = new Point(ClientSize.Width - margin - _calculatorButton.Width, topTitleY + 104);
                _calculatorButton.BringToFront();
            }
        }

        private void FinalizeTheoryTimers()
        {
            try { _autoSaveTimer?.Stop(); } catch { }
            try { _countdownTimer?.Stop(); } catch { }
            try { _autoSaveTimer?.Dispose(); } catch { }
            try { _countdownTimer?.Dispose(); } catch { }
        }

        private void Test3_Resize(object sender, EventArgs e)
        {
            ArrangeTheoryWorkspace(_questionImagePanel != null && _questionImagePanel.Visible);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private void Test3_Deactivate(object sender, EventArgs e)
        {
            if (_endCalled != 0 || _openingCalculator || _calculatorSessionActive)
            {
                return;
            }

            if (_calculatorForm != null && !_calculatorForm.IsDisposed && _calculatorForm.Visible)
            {
                return;
            }

            if ((DateTime.UtcNow - _calculatorLaunchUtc).TotalMilliseconds < 800)
            {
                return;
            }

            IntPtr foreground = GetForegroundWindow();
            if (foreground == this.Handle)
            {
                return;
            }

            if (_calculatorForm != null && !_calculatorForm.IsDisposed && foreground == _calculatorForm.Handle)
            {
                return;
            }

            EndTheoryExamOnce("You left the theory exam window. The exam has ended.", true);
        }

        private async void CalculatorForm_Deactivate(object sender, EventArgs e)
        {
            if (_endCalled != 0 || _openingCalculator)
            {
                return;
            }

            if (!_calculatorActivatedOnce)
            {
                return;
            }

            if ((DateTime.UtcNow - _calculatorLaunchUtc).TotalMilliseconds < 3000)
            {
                return;
            }

            await Task.Delay(300);

            if (_endCalled != 0 || _openingCalculator)
            {
                return;
            }

            IntPtr foreground = GetForegroundWindow();
            if (foreground == this.Handle)
            {
                return;
            }

            if (_calculatorForm != null && !_calculatorForm.IsDisposed && foreground == _calculatorForm.Handle)
            {
                return;
            }

            _calculatorSessionActive = false;

            try
            {
                _calculatorForm?.Close();
            }
            catch { }

            EndTheoryExamOnce("You left the theory exam window. The exam has ended.", true);
        }

        private bool ColumnExists(SqlConnection connection, string tableName, string columnName)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME = @column",
                connection))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }


    }

}

