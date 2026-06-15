using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;



namespace Quiz_App
{
    public partial class Test : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private bool allowScoreDisplay = false;
        private int examId;
        public static int score = 0;
        private int totalSeconds = 3600;
        private List<int> visitedQuestionIds = new List<int>();
        private int currentIndex = 0;
        private Dictionary<int, string> answeredQuestions = new Dictionary<int, string>();
        private Dictionary<int, string> selectedAnswers = new Dictionary<int, string>();
        private Dictionary<int, bool> questionScoredCorrect = new Dictionary<int, bool>();
        private List<int> answeredQuestionIds = new List<int>();
        private List<int> allQuestionIds = new List<int>();
        private int totalQuestionsLimit = 0;
        private int currentQuesId = 0;
        private Panel imageWrapperPanel;
        private Label shortAnswerCautionLabel;
        private int layoutMargin = 26;
        private int layoutHeaderHeight = 128;
        private int layoutQuestionHeight = 210;
        private int layoutContentWidth;
        private int layoutQuestionWidth;
        private int layoutImageWidth;
        private Panel[] optionCardPanels;
        private bool suppressRadioSync;

        private int _endCalled = 0;


        private int exam_fk_id => int.Parse(studentlogin.exam_id);
        private int student_id => int.Parse(studentlogin.studentid);



        public static class ExamSettings
        {
            public static int DurationInMinutes { get; set; } = 60;
        }

        public static int exam_id;
        private List<DataRow> shuffledQuestions;
        private bool isShuffleMode = false;
        string correctop;
        int i;
        public Test(int selectedExamId)
        {
            InitializeComponent();
            examId = selectedExamId;
            //isShuffleMode = shuffleMode;
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

            radioButton1.CheckedChanged += RadioButton_CheckedChanged;
            radioButton2.CheckedChanged += RadioButton_CheckedChanged;
            radioButton3.CheckedChanged += RadioButton_CheckedChanged;
            radioButton4.CheckedChanged += RadioButton_CheckedChanged;


            // hook events
            this.FormClosing += Test_FormClosing;
           // this.FormClosed += Test_FormClosed;
            this.Deactivate += Test_Deactivate;   // ?? triggers if user switches app


        }

        private void BuildExamLayout()
        {
            SuspendLayout();
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(8, 12, 22), Color.FromArgb(18, 30, 48));

            BackColor = Color.FromArgb(7, 11, 21);
            AutoScaleMode = AutoScaleMode.None;

            int margin = layoutMargin;
            int headerHeight = layoutHeaderHeight;
            int questionHeight = layoutQuestionHeight;
            int baseOptionsTop = margin + headerHeight + questionHeight + 44;
            int contentWidth = ClientSize.Width - (margin * 2);
            int questionWidth = (int)(contentWidth * 0.72f);
            int imageWidth = contentWidth - questionWidth - 20;
            int timerBlockLeft = 28;
            int timerBlockWidth = 230;
            int scoreBlockLeft = timerBlockLeft + timerBlockWidth;
            int centerBlockWidth = 320;
            int centerBlockLeft = Math.Max(scoreBlockLeft + 28, (contentWidth - centerBlockWidth) / 2);
            layoutContentWidth = contentWidth;
            layoutQuestionWidth = questionWidth;
            layoutImageWidth = imageWidth;
            int optionGap = 18;
            int optionWidth = (contentWidth - optionGap) / 2;
            int optionsTop = Math.Max(baseOptionsTop, (ClientSize.Height / 2) + 20);
            if (!pictureBox1.Visible)
            {
                int questionTop = margin + headerHeight + 12;
                questionHeight = Math.Max(180, optionsTop - questionTop - 18);
            }
            int availableAnswerHeight = Math.Max(220, ClientSize.Height - optionsTop - margin - 12);
            int mcqSectionHeight = availableAnswerHeight;
            int shortAnswerSectionHeight = availableAnswerHeight;

            Controls.SetChildIndex(label7, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(pictureBox1, 0);
            Controls.SetChildIndex(groupBox2, 0);
            Controls.SetChildIndex(groupBox3, 0);

            label7.Text = "Computer-Based Examination";
            label7.Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold, GraphicsUnit.Point);
            label7.ForeColor = ModernUi.Ink;
            label7.Location = new Point(margin, 18);
            label7.Size = new Size(720, 40);

            groupBox1.Text = string.Empty;
            groupBox1.BackColor = Color.FromArgb(20, 29, 46);
            groupBox1.Location = new Point(margin, 58);
            groupBox1.Size = new Size(contentWidth, headerHeight);

            groupBox2.Text = string.Empty;
            groupBox2.BackColor = Color.FromArgb(20, 29, 46);
            groupBox2.Location = new Point(margin, optionsTop);
            groupBox2.Size = new Size(contentWidth, mcqSectionHeight);
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            groupBox3.Text = string.Empty;
            groupBox3.BackColor = Color.FromArgb(20, 29, 46);
            groupBox3.Location = new Point(margin, optionsTop);
            groupBox3.Size = new Size(contentWidth, shortAnswerSectionHeight);
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            label1.BackColor = Color.FromArgb(20, 29, 46);
            label1.Location = new Point(margin, margin + headerHeight + 12);
            label1.Size = new Size(questionWidth, questionHeight);
            label1.Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = ModernUi.Ink;
            label1.Padding = new Padding(20, 14, 20, 12);
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            pictureBox1.BackColor = Color.FromArgb(20, 29, 46);
            pictureBox1.Size = new Size(imageWidth, questionHeight);
            pictureBox1.Padding = new Padding(14);
            EnsureImageWrapper();
            imageWrapperPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UpdateQuestionSurfaceLayout(true);

            label2.Text = "Exam session is active";
            label2.ForeColor = ModernUi.MutedInk;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(timerBlockLeft, 94);
            label2.Size = new Size(220, 20);

            label3.ForeColor = ModernUi.Warning;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(scoreBlockLeft, 18);
            label3.Size = new Size(64, 24);
            label3.Text = "Score:";

            label4.ForeColor = ModernUi.Accent;
            label4.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(scoreBlockLeft + 68, 14);
            label4.Size = new Size(128, 32);

            label5.ForeColor = ModernUi.MutedInk;
            label5.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(timerBlockLeft, 18);
            label5.Size = new Size(94, 24);
            label5.Text = "Time Left";

            label6.ForeColor = ModernUi.MutedInk;
            label6.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(contentWidth - 748, 68);
            label6.Size = new Size(74, 22);

            label8.ForeColor = ModernUi.Ink;
            label8.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(centerBlockLeft, 28);
            label8.Size = new Size(centerBlockWidth, 30);
            label8.TextAlign = ContentAlignment.MiddleCenter;

            label10.ForeColor = ModernUi.MutedInk;
            label10.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(centerBlockLeft, 62);
            label10.Size = new Size(centerBlockWidth, 26);
            label10.TextAlign = ContentAlignment.MiddleCenter;

            label11.ForeColor = ModernUi.MutedInk;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(contentWidth - 420, 14);
            label11.Size = new Size(300, 22);

            label12.ForeColor = ModernUi.Accent;
            label13.ForeColor = ModernUi.Accent;
            label14.ForeColor = ModernUi.Accent;
            label15.ForeColor = ModernUi.Accent;
            foreach (Label optionLabel in new[] { label12, label13, label14, label15 })
            {
                optionLabel.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
                optionLabel.BackColor = Color.FromArgb(14, 24, 38);
                optionLabel.Size = new Size(44, 44);
                optionLabel.TextAlign = ContentAlignment.MiddleCenter;
            }

            label16.ForeColor = ModernUi.MutedInk;
            label16.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label16.Location = new Point(24, 26);
            label16.Size = new Size(340, 26);
            timerLabel1.ForeColor = ModernUi.Warning;
            timerLabel1.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            timerLabel1.Location = new Point(timerBlockLeft, 46);
            timerLabel1.Size = new Size(190, 40);

            timerLabel2.Visible = false;

            ModernUi.StyleComboBox(comboBox1);
            comboBox1.BackColor = Color.FromArgb(18, 26, 42);
            comboBox1.ForeColor = ModernUi.Ink;
            comboBox1.Location = new Point(contentWidth - 420, 36);
            comboBox1.Size = new Size(230, 36);

            ModernUi.StylePrimaryButton(button1);
            button1.Text = "Save & Next";
            button1.Location = new Point(contentWidth - 290, 78);
            button1.Size = new Size(118, 38);

            ModernUi.StyleSecondaryButton(button4);
            button4.Text = "Previous";
            button4.Location = new Point(contentWidth - 420, 78);
            button4.Size = new Size(118, 38);

            ModernUi.StyleSecondaryButton(button2);
            button2.Text = "Calculator";
            button2.BackgroundImage = null;
            button2.Location = new Point(contentWidth - 550, 78);
            button2.Size = new Size(118, 38);

            btnClock.Visible = false;
            btnTimer.Visible = false;

            ModernUi.StyleDangerButton(btnEndExam);
            btnEndExam.Text = "End Exam";
            btnEndExam.Location = new Point(contentWidth - 150, 28);
            btnEndExam.Size = new Size(126, 88);

            EnsureOptionCards();
            foreach (RadioButton radioButton in new[] { radioButton1, radioButton2, radioButton3, radioButton4 })
            {
                radioButton.ForeColor = ModernUi.Ink;
                radioButton.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
                radioButton.Padding = new Padding(8, 0, 8, 0);
                radioButton.CheckAlign = ContentAlignment.MiddleLeft;
                radioButton.TextAlign = ContentAlignment.MiddleLeft;
                radioButton.AutoEllipsis = true;
            }

            panel1.Visible = false;
            panel2.Visible = false;
            LayoutOptionCards(optionWidth, optionGap);

            ModernUi.StyleTextInput(txtShortAnswer);
            txtShortAnswer.CharacterCasing = CharacterCasing.Upper;
            txtShortAnswer.Multiline = true;
            txtShortAnswer.ScrollBars = ScrollBars.Vertical;
            txtShortAnswer.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            txtShortAnswer.Location = new Point(24, 68);
            txtShortAnswer.Size = new Size(contentWidth - 48, shortAnswerSectionHeight - 96);
            txtShortAnswer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            EnsureShortAnswerCautionLabel();

            pictureBox1.BackColor = Color.FromArgb(14, 20, 32);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            ResumeLayout();
        }

        private void EnsureOptionCards()
        {
            if (optionCardPanels != null)
            {
                return;
            }

            optionCardPanels = new Panel[4];
            RadioButton[] radios = { radioButton1, radioButton2, radioButton3, radioButton4 };
            Label[] badges = { label12, label13, label14, label15 };

            for (int index = 0; index < optionCardPanels.Length; index++)
            {
                Panel card = new Panel
                {
                    BackColor = Color.FromArgb(14, 20, 32)
                };

                ModernUi.AddPanelChrome(card);
                groupBox2.Controls.Add(card);
                card.BringToFront();

                badges[index].Parent = card;
                radios[index].Parent = card;
                optionCardPanels[index] = card;
            }
        }

        private void LayoutOptionCards(int optionWidth, int optionGap)
        {
            if (optionCardPanels == null)
            {
                return;
            }

            RadioButton[] radios = { radioButton1, radioButton2, radioButton3, radioButton4 };
            Label[] badges = { label12, label13, label14, label15 };
            int cardLeft = 22;
            int rowGap = 18;
            int verticalPadding = 22;
            int optionHeight = Math.Max(72, (groupBox2.Height - (verticalPadding * 2) - rowGap) / 2);
            int topRow = verticalPadding;
            int bottomRow = topRow + optionHeight + rowGap;

            Point[] positions =
            {
                new Point(cardLeft, topRow),
                new Point(cardLeft, bottomRow),
                new Point(cardLeft + optionWidth + optionGap, topRow),
                new Point(cardLeft + optionWidth + optionGap, bottomRow)
            };

            for (int index = 0; index < optionCardPanels.Length; index++)
            {
                Panel card = optionCardPanels[index];
                card.Location = positions[index];
                card.Size = new Size(optionWidth - 4, optionHeight);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                Label badge = badges[index];
                badge.Location = new Point(14, (card.Height - 42) / 2);
                badge.Size = new Size(40, 42);
                badge.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);

                RadioButton radio = radios[index];
                radio.BackColor = card.BackColor;
                radio.Location = new Point(52, 4);
                radio.Size = new Size(card.Width - 68, card.Height - 8);
                radio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }
        }

        private void EnsureImageWrapper()
        {
            if (imageWrapperPanel != null)
            {
                return;
            }

            imageWrapperPanel = new Panel
            {
                BackColor = Color.FromArgb(20, 29, 46)
            };

            Controls.Add(imageWrapperPanel);
            pictureBox1.Parent = imageWrapperPanel;
            ModernUi.AddPanelChrome(imageWrapperPanel);
        }

        private void UpdateQuestionSurfaceLayout(bool showImage)
        {
            int margin = layoutMargin;
            int gap = 20;
            int headerBottom = groupBox1.Bottom;
            int optionsTop = groupBox2.Top;
            int availableGap = Math.Max(0, optionsTop - headerBottom);
            int centeredOffset = (availableGap - layoutQuestionHeight) / 2;
            int top = headerBottom + Math.Max(28, centeredOffset);
            top = Math.Min(top, optionsTop - layoutQuestionHeight - 20);

            if (showImage)
            {
                label1.Location = new Point(margin, top);
                label1.Size = new Size(layoutQuestionWidth, layoutQuestionHeight);

                imageWrapperPanel.Visible = true;
                imageWrapperPanel.Location = new Point(margin + layoutQuestionWidth + gap, top);
                imageWrapperPanel.Size = new Size(layoutImageWidth, layoutQuestionHeight);
                pictureBox1.Location = new Point(14, 14);
                pictureBox1.Size = new Size(imageWrapperPanel.Width - 28, imageWrapperPanel.Height - 28);
                imageWrapperPanel.BringToFront();
                pictureBox1.BringToFront();
                label1.BringToFront();
                groupBox1.BringToFront();
            }
            else
            {
                imageWrapperPanel.Visible = false;
                label1.Location = new Point(margin, top);
                label1.Size = new Size(layoutContentWidth, layoutQuestionHeight);
            }
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
                using (Image source = Image.FromStream(ms))
                {
                    return new Bitmap(source);
                }
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

            // ? store correct option here
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

                            // ? store correct option here
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
            string correctAnswer = currentRow["correctAns"]?.ToString()?.Trim() ?? string.Empty;

            if (qtype == "MCQ")
            {
                if (radioButton1.Checked) selectedValue = radioButton1.Text;
                else if (radioButton2.Checked) selectedValue = radioButton2.Text;
                else if (radioButton3.Checked) selectedValue = radioButton3.Text;
                else if (radioButton4.Checked) selectedValue = radioButton4.Text;
            }
            else if (qtype.Equals("SHORT", StringComparison.OrdinalIgnoreCase))
            {
                selectedValue = txtShortAnswer.Text.Trim();
            }

            int currentQid = Convert.ToInt32(currentRow["qid"]);
            selectedValue = selectedValue.Trim();
            selectedAnswers[currentQid] = selectedValue;

            bool wasCorrect = questionScoredCorrect.ContainsKey(currentQid) && questionScoredCorrect[currentQid];
            bool isNowCorrect = !string.IsNullOrEmpty(selectedValue)
                && selectedValue.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase);

            if (wasCorrect && !isNowCorrect)
            {
                score = Math.Max(0, score - 1);
            }
            else if (!wasCorrect && isNowCorrect)
            {
                score++;
            }

            questionScoredCorrect[currentQid] = isNowCorrect;

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
                MessageBox.Show("This is the last question.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                i = shuffledQuestions.Count - 1; // stay on last question
                currentIndex = i;
                currentQuesId = Convert.ToInt32(shuffledQuestions[i]["qid"]);
                LoadUnifiedQuestion(shuffledQuestions[i]);
                label8.Text = $"Question {currentIndex + 1} of {totalQuestionsLimit}";
                comboBox1.SelectedIndex = currentIndex;
                comboBox1.Invalidate();
            }

        }


        private void ApplyExamSettings(int examId)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = "SELECT show_score, show_calculator, shuffle FROM tbl_exam_settings WHERE ex_id = @exid";

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
                            bool shuffleMode = reader["shuffle"] != DBNull.Value && Convert.ToBoolean(reader["shuffle"]);

                            allowScoreDisplay = showScore;
                            label4.Visible = showScore;
                            label3.Visible = showScore;
                            button2.Visible = showCalculator;
                            label6.Visible = false;

                            isShuffleMode = shuffleMode;  // ? set shuffle mode here
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
                MessageBox.Show("Time has expired. Your exam will now be submitted.", "Time Up");
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
                    MessageBox.Show("You're already at the first question.");
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
                    MessageBox.Show("You're already at the first question.");
                }

            }

            radiobtn();
        }


        public void radiobtn()
        {
            suppressRadioSync = true;
            // Clear all first
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton1.TabStop = false;
            radioButton2.TabStop = false;
            radioButton3.TabStop = false;
            radioButton4.TabStop = false;
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

            if (radioButton1.Checked) radioButton1.TabStop = true;
            if (radioButton2.Checked) radioButton2.TabStop = true;
            if (radioButton3.Checked) radioButton3.TabStop = true;
            if (radioButton4.Checked) radioButton4.TabStop = true;
            suppressRadioSync = false;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressRadioSync)
            {
                return;
            }

            RadioButton changed = sender as RadioButton;
            if (changed == null || !changed.Checked)
            {
                return;
            }

            suppressRadioSync = true;

            foreach (RadioButton radio in new[] { radioButton1, radioButton2, radioButton3, radioButton4 })
            {
                if (!ReferenceEquals(radio, changed))
                {
                    radio.Checked = false;
                    radio.TabStop = false;
                }
            }

            changed.TabStop = true;
            suppressRadioSync = false;
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
            Color textColor = ModernUi.Ink;
            Font font = e.Font;

            if (!isVisited)
            {
                textColor = ModernUi.MutedInk;
                displayText += " - locked";
            }
            else if (isAnswered)
            {
                textColor = ModernUi.AccentAlt;
                font = new Font(e.Font, FontStyle.Bold);
                displayText += " - answered";
            }
            else
            {
                textColor = ModernUi.Warning;
                displayText += " - review";
            }

            e.DrawBackground();
            using (Brush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(displayText, font, brush, e.Bounds);
            }
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
                new messageform(score, totalQuestionsLimit, exam_fk_id, student_id).Show();
            else
                new messageform2(exam_fk_id, student_id, score, totalQuestionsLimit).Show();



        }


        private DataTable allQuestionsTable;

        private void Test_Load(object sender, EventArgs e)
        {
            
            this.FormBorderStyle = FormBorderStyle.None;   // remove close/min/max buttons
            this.WindowState = FormWindowState.Maximized;  // maximize to fill screen
            //this.TopMost = true;                           // keep exam window on top
            this.Resize -= Test_Resize;
            this.Resize += Test_Resize;

            BuildExamLayout();
            score = 0;
            label4.Text = $"Score: {score}";
            label3.Visible = false;
            label4.Visible = false;
            label6.Visible = false;
            button2.Visible = false;

            ApplyExamSettings(int.Parse(studentlogin.exam_id));

            string query = $@"
SELECT 
    ques_id AS qid, 
    q_title AS q_title, 
    q_opA, q_opB, q_opC, q_opD, 
    q_correctOpn AS correctAns, 
    q_image AS q_image, 
    'MCQ' AS qtype
FROM tbl_questions
WHERE ex_id_fk = {studentlogin.exam_id}

UNION ALL

SELECT 
    sa_id AS qid, 
    ques_title AS q_title, 
    NULL AS q_opA, NULL AS q_opB, NULL AS q_opC, NULL AS q_opD, 
    correct_answer AS correctAns, 
    ques_image AS q_image, 
    'SHORT' AS qtype
FROM tbl_shortanswer
WHERE exam_id = {studentlogin.exam_id};";


            allQuestionsTable = rc.GetDataTable(query);

            // shuffle or sequential
            if (isShuffleMode)
                shuffledQuestions = allQuestionsTable.AsEnumerable().OrderBy(row => Guid.NewGuid()).ToList();
            else
                shuffledQuestions = allQuestionsTable.AsEnumerable().ToList();

            totalQuestionsLimit = shuffledQuestions.Count;
            label10.Text = $"Total Questions: {totalQuestionsLimit}";

            if (totalQuestionsLimit <= 0)
            {
                timer1.Stop();
                label8.Text = "No questions available";
                label1.Text = "This exam does not have any questions yet. Add objective or short-answer questions before starting the test.";
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
                groupBox2.Visible = false;
                groupBox3.Visible = false;
                button1.Enabled = false;
                button4.Enabled = false;
                comboBox1.Enabled = false;
                btnEndExam.Text = "Close";
                btnEndExam.Click -= btnEndExam_Click_1;
                btnEndExam.Click += (s, args) => Close();
                MessageBox.Show(
                    "No questions were found for this exam. Please add questions first.",
                    "Empty Exam",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

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
            radiobtn();
            label8.Text = $"Question {currentIndex + 1} of {totalQuestionsLimit}";

            int duration = rc.GetExamDuration(int.Parse(studentlogin.exam_id));
            totalSeconds = (duration > 0 ? duration : 60) * 60;
            UpdateTimerLabel();
            timer1.Start();
            ModernUi.FadeIn(this);
        }

        private void Test_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                BuildExamLayout();
            }
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

            radiobtn();

            string qtype = row["qtype"]?.ToString() ?? "MCQ";

            // ? Always use the unified names
            label1.Text = row["q_title"]?.ToString() ?? "";
            correctop = row["correctAns"]?.ToString() ?? "";

            bool hasImage = false;

            // ? Show question image if available
            if (row["q_image"] != DBNull.Value && !string.IsNullOrEmpty(row["q_image"].ToString()))
            {
                try
                {
                    byte[] imgBytes = (byte[])row["q_image"];
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = ByteArrayToImage(imgBytes);
                    hasImage = true;
                }
                catch
                {
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = null;
                    hasImage = false;
                }
            }
            else
            {
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = null;
            }

            pictureBox1.Visible = hasImage;
            if (imageWrapperPanel != null)
            {
                imageWrapperPanel.Visible = hasImage;
            }

            ApplyQuestionModeLayout(qtype);
            UpdateQuestionSurfaceLayout(hasImage);

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

                txtShortAnswer.Visible = false;
                label16.Visible = false;
                radiobtn();
            }
            else // SHORT
            {
                if (groupBox2 != null) groupBox2.Visible = false;
                if (groupBox3 != null) { groupBox3.Visible = true; groupBox3.BringToFront(); }

                // Hide MCQ radios
                radioButton1.Visible = radioButton2.Visible = radioButton3.Visible = radioButton4.Visible = false;
                groupBox3.BringToFront();

                txtShortAnswer.Visible = true;
                txtShortAnswer.BringToFront();
                label16.Visible = true;
                label16.BringToFront();


                txtShortAnswer.Text = "";
            }


        }

        private void ApplyQuestionModeLayout(string qtype)
        {
            bool isShort = qtype.Equals("SHORT", StringComparison.OrdinalIgnoreCase);

            if (groupBox2 != null)
            {
                groupBox2.Visible = !isShort;
            }

            if (groupBox3 != null)
            {
                groupBox3.Visible = isShort;
            }

            if (isShort)
            {
                label16.Text = "Enter Your Answer Below";
                label16.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
                txtShortAnswer.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
                if (shortAnswerCautionLabel != null)
                {
                    shortAnswerCautionLabel.Visible = true;
                    shortAnswerCautionLabel.BringToFront();
                }
            }
            else if (shortAnswerCautionLabel != null)
            {
                shortAnswerCautionLabel.Visible = false;
            }
        }

        private void EnsureShortAnswerCautionLabel()
        {
            if (shortAnswerCautionLabel == null)
            {
                shortAnswerCautionLabel = new Label
                {
                    Parent = groupBox3,
                    AutoSize = false,
                    BackColor = Color.Transparent
                };
            }

            shortAnswerCautionLabel.Text = "Caution: Answer must be in ALL CAPITAL LETTERS.";
            shortAnswerCautionLabel.ForeColor = ModernUi.Warning;
            shortAnswerCautionLabel.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            shortAnswerCautionLabel.Location = new Point(24, 34);
            shortAnswerCautionLabel.Size = new Size(Math.Max(340, groupBox3.Width - 48), 24);
            shortAnswerCautionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            shortAnswerCautionLabel.Visible = false;

            label16.Location = new Point(24, 62);
            label16.Size = new Size(420, 26);
            txtShortAnswer.Location = new Point(24, 96);
            txtShortAnswer.Size = new Size(layoutContentWidth - 48, groupBox3.Height - 124);
        }

            // After loading, restore any previous answer for this question


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
                MessageBox.Show("You're already at the first question.");
            }

            radiobtn();
        }

        private void groupBox3_Enter_1(object sender, EventArgs e)
        {

        }

        private void Test_Deactivate(object sender, EventArgs e)
        {
            // if already ended, ignore
            if (_endCalled != 0) return;

            // Check if this form is still the active window
            if (GetForegroundWindow() == this.Handle)
            {
                // Still active ? ignore (like when using ComboBox dropdown)
                return;
            }

            // If another app is active ? end exam
            EndExamOnce("You left the exam window. The exam has ended.", true);
        }


    }
}

