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
    public partial class GradeTheoryAnswers : BaseForm
    {

        private int examId;
        private int studentId;
        private DataTable answerTable;
        private Label emptyStateLabel;
        private Guna.UI2.WinForms.Guna2Button btnDeleteTheoryAnswers;
        private Label questionPreviewLabel;
        private Label answerPreviewLabel;
        private RichTextBox questionPreviewBox;
        private RichTextBox answerPreviewBox;

        public GradeTheoryAnswers(int examId, int studentId)
        {
            InitializeComponent();
            this.examId = examId;
            this.studentId = studentId;
        }
        public GradeTheoryAnswers()
        {
            InitializeComponent();
        }

        // Base resolution (your dev machine)
        private const int BaseWidth = 1920;
        private const int BaseHeight = 1080;

        public static void ScaleForm(Form form)
        {
            // Get current screen resolution
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calculate scale factors
            float scaleX = (float)screenWidth / BaseWidth;
            float scaleY = (float)screenHeight / BaseHeight;

            // Apply scaling to form and controls
            form.Scale(new SizeF(scaleX, scaleY));

            // Adjust font scaling (optional, but makes UI balanced)
            foreach (Control c in form.Controls)
            {
                c.Font = new Font(c.Font.FontFamily, c.Font.Size * Math.Min(scaleX, scaleY));
            }

            // Center form
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        private void GradeTheoryAnswers_Load(object sender, EventArgs e)
        {
            GradeTheoryAnswers.ScaleForm(this);
            ModernUi.StyleComboBox(cmbExam);
            ModernUi.StyleComboBox(cmbBatch);
            ModernUi.StyleComboBox(cmbStudent);
            EnsureDeleteAnswersButton();
            cmbExam.ForeColor = Color.Black;
            cmbBatch.ForeColor = Color.Black;
            cmbStudent.ForeColor = Color.Black;
            btnLoadStudentAnswers.FillColor = ModernUi.Accent;
            btnLoadStudentAnswers.ForeColor = Color.FromArgb(8, 20, 28);
            btnLoadStudentAnswers.BorderRadius = 16;
            ModernUi.StylePrimaryButton(btnSaveScores);
            btnFinalizeGrades.FillColor = Color.FromArgb(28, 42, 63);
            btnFinalizeGrades.ForeColor = ModernUi.Ink;
            btnFinalizeGrades.BorderRadius = 16;
            btnSaveScores.ForeColor = Color.Black;
            if (richTextBox1 != null)
            {
                richTextBox1.ForeColor = Color.Black;
            }
            ModernUi.PrepareDataGridView(dgvGrading);
            dgvGrading.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dgvGrading.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            dgvGrading.RowTemplate.Height = 36;
            EnsurePreviewPanels();
            EnsureEmptyState();
            LoadBatches();
            LoadExams();

            cmbBatch.SelectedIndexChanged += (s, ev) =>
            {
                if (cmbBatch.SelectedIndex != -1)
                {
                    string selectedBatch = cmbBatch.SelectedValue.ToString();
                    LoadStudentsByBatch(selectedBatch);
                }
            };

            RefreshTheorySchemaState();
        }

        private void EnsurePreviewPanels()
        {
            if (questionPreviewBox != null && answerPreviewBox != null)
            {
                return;
            }

            dgvGrading.Height = 360;
            dgvGrading.Width = 790;

            if (label5 != null)
            {
                label5.AutoSize = true;
                label5.Location = new Point(dgvGrading.Right + 28, dgvGrading.Top + 18);
                label5.BringToFront();
            }

            if (richTextBox1 != null)
            {
                richTextBox1.Location = new Point(dgvGrading.Right + 28, (label5?.Bottom ?? dgvGrading.Top) + 10);
                richTextBox1.Size = new Size(170, 52);
                richTextBox1.BringToFront();
            }

            questionPreviewLabel = new Label
            {
                AutoSize = true,
                Text = "Question Preview",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = ModernUi.Ink,
                Location = new Point(dgvGrading.Left, dgvGrading.Bottom + 14)
            };

            questionPreviewBox = new RichTextBox
            {
                Location = new Point(dgvGrading.Left, questionPreviewLabel.Bottom + 8),
                Size = new Size(440, 105),
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point)
            };

            answerPreviewLabel = new Label
            {
                AutoSize = true,
                Text = "Student Answer Preview",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = ModernUi.Ink,
                Location = new Point(questionPreviewBox.Right + 24, dgvGrading.Bottom + 14)
            };

            answerPreviewBox = new RichTextBox
            {
                Location = new Point(questionPreviewBox.Right + 24, answerPreviewLabel.Bottom + 8),
                Size = new Size(326, 105),
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point)
            };

            Controls.Add(questionPreviewLabel);
            Controls.Add(questionPreviewBox);
            Controls.Add(answerPreviewLabel);
            Controls.Add(answerPreviewBox);

            questionPreviewLabel.BringToFront();
            questionPreviewBox.BringToFront();
            answerPreviewLabel.BringToFront();
            answerPreviewBox.BringToFront();

            dgvGrading.SelectionChanged -= dgvGrading_SelectionChanged;
            dgvGrading.SelectionChanged += dgvGrading_SelectionChanged;
        }

        private void EnsureDeleteAnswersButton()
        {
            if (btnDeleteTheoryAnswers != null)
            {
                return;
            }

            btnDeleteTheoryAnswers = new Guna.UI2.WinForms.Guna2Button
            {
                Name = "btnDeleteTheoryAnswers",
                Text = "Delete Saved Answers",
                Size = new Size(250, 45),
                Location = new Point(711, 249),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                FillColor = Color.FromArgb(120, 42, 42),
                ForeColor = Color.White,
                BorderRadius = 16
            };

            btnDeleteTheoryAnswers.Click += btnDeleteTheoryAnswers_Click;
            Controls.Add(btnDeleteTheoryAnswers);
            btnDeleteTheoryAnswers.BringToFront();
        }

        private void LoadAnswers()
        {
            if (cmbStudent.SelectedIndex == -1 || cmbExam.SelectedIndex == -1)
            {
                MessageBox.Show("Please select both exam and student.", "Select Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedStudentId = Convert.ToInt32(cmbStudent.SelectedValue);
            int selectedExamId = Convert.ToInt32(cmbExam.SelectedValue);

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    if (!TheorySchemaReady(conn))
                    {
                        ShowEmptyState("Theory grading tables are not available in this database yet.");
                        return;
                    }

                    string sql = @"
            SELECT 
                tq.theory_id,
                tq.question_number AS [Q#],
                tq.question_text AS [Question],
                ta.answer_text AS [Student's Answer],
                tq.model_answer AS [Model Answer],
                tq.mark AS [Max Mark],
                ta.score AS [Score Given],
                ta.answer_id
            FROM tbl_theory_answers ta
            INNER JOIN tbl_theory_questions tq 
                ON ta.theory_fk_id = tq.theory_id
            WHERE ta.exam_fk_id = @examId 
              AND ta.student_fk_id = @studentId
            ORDER BY tq.question_number";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@examId", selectedExamId);
                    cmd.Parameters.AddWithValue("@studentId", selectedStudentId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    answerTable = new DataTable();
                    da.Fill(answerTable);

                    dgvGrading.DataSource = answerTable;
                    ModernUi.StyleDataGridView(dgvGrading);
                    dgvGrading.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    dgvGrading.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
                    dgvGrading.RowTemplate.Height = 36;
                    dgvGrading.ClearSelection();

                    if (dgvGrading.Columns.Contains("Student's Answer"))
                    {
                        dgvGrading.Columns["Student's Answer"].DefaultCellStyle.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
                        dgvGrading.Columns["Student's Answer"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        dgvGrading.Columns["Student's Answer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    if (dgvGrading.Columns.Contains("Model Answer"))
                    {
                        dgvGrading.Columns["Model Answer"].DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
                        dgvGrading.Columns["Model Answer"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        dgvGrading.Columns["Model Answer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    if (answerTable.Rows.Count == 0)
                    {
                        ClearPreviewPanels();
                        UpdateDisplayedTotalScore();
                        ShowEmptyState("No theory answers were found for the selected student and exam.");
                    }
                    else
                    {
                        UpdateDisplayedTotalScore();
                        HideEmptyState();
                        if (dgvGrading.Rows.Count > 0)
                        {
                            dgvGrading.Rows[0].Selected = true;
                            PopulatePreviewPanelsFromRow(dgvGrading.Rows[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClearPreviewPanels();
                ShowEmptyState("Unable to load theory answers right now.");
                MessageBox.Show("Error loading answers: " + ex.Message);
            }
        }



        private void btnSaveScores_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    if (!TheoryAnswersTableExists(conn))
                    {
                        MessageBox.Show("Theory answers table is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (answerTable == null || answerTable.Rows.Count == 0)
                    {
                        MessageBox.Show("There are no loaded theory answers to save.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    foreach (DataRow row in answerTable.Rows)
                    {
                        int answerId = Convert.ToInt32(row["answer_id"]);
                        object scoreObj = row["Score Given"];
                        decimal? score = scoreObj == DBNull.Value ? (decimal?)null : Convert.ToDecimal(scoreObj);

                        string sql = "UPDATE tbl_theory_answers SET score = @score, graded_at = @now WHERE answer_id = @answerId";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@score", (object)score ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@now", DateTime.Now);
                            cmd.Parameters.AddWithValue("@answerId", answerId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    UpdateDisplayedTotalScore();
                    MessageBox.Show("All scores saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving scores: " + ex.Message);
            }

        }

        private void LoadBatches()
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT std_batch_code FROM student_record ORDER BY std_batch_code ASC";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbBatch.DataSource = dt;
                    cmbBatch.DisplayMember = "std_batch_code";
                    cmbBatch.ValueMember = "std_batch_code";
                    cmbBatch.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading batches: " + ex.Message);
            }
        }

        private void LoadStudentsByBatch(string batchCode)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT std_id, std_name FROM student_record WHERE std_batch_code = @batchCode ORDER BY std_name ASC";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@batchCode", batchCode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbStudent.DataSource = dt;
                    cmbStudent.DisplayMember = "std_name";
                    cmbStudent.ValueMember = "std_id";
                    cmbStudent.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
        }

        private void LoadExams()
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbExam.DataSource = dt;
                    cmbExam.DisplayMember = "ex_name";
                    cmbExam.ValueMember = "ex_id";
                    cmbExam.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading exams: " + ex.Message);
            }
        }




        private void btnLoadStudentAnswers_Click(object sender, EventArgs e)
        {
            LoadAnswers();
        }

        private void btnDeleteTheoryAnswers_Click(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedIndex == -1 || cmbExam.SelectedIndex == -1)
            {
                MessageBox.Show("Please select both exam and student before deleting theory answers.", "Select Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedStudentId = Convert.ToInt32(cmbStudent.SelectedValue);
            int selectedExamId = Convert.ToInt32(cmbExam.SelectedValue);

            DialogResult confirm = MessageBox.Show(
                "This will permanently delete all saved theory answers for the selected student and exam. Continue?",
                "Delete Theory Answers",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    if (!TheoryAnswersTableExists(conn))
                    {
                        MessageBox.Show("Theory answers table is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM tbl_theory_answers WHERE exam_fk_id = @examId AND student_fk_id = @studentId",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("@examId", selectedExamId);
                        cmd.Parameters.AddWithValue("@studentId", selectedStudentId);
                        int deletedRows = cmd.ExecuteNonQuery();

                        answerTable = null;
                        dgvGrading.DataSource = null;
                        ClearPreviewPanels();
                        UpdateDisplayedTotalScore();

                        if (deletedRows > 0)
                        {
                            ShowEmptyState("All saved theory answers for the selected student and exam have been deleted.");
                            MessageBox.Show("Saved theory answers deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            ShowEmptyState("No saved theory answers were found for the selected student and exam.");
                            MessageBox.Show("No saved theory answers were found to delete.", "Nothing Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting theory answers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyTheoryTotalsToScoreTable(int? examId = null)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    if (!StoredProcedureExists(conn, "usp_UpsertTheoryScores"))
                    {
                        MessageBox.Show("Theory score finalization procedure is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SqlCommand cmd = new SqlCommand("dbo.usp_UpsertTheoryScores", conn))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (examId.HasValue)
                        cmd.Parameters.AddWithValue("@ExamId", examId.Value);
                    else
                        cmd.Parameters.AddWithValue("@ExamId", DBNull.Value);

                    cmd.CommandTimeout = 120; // adjust if needed
                    cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Theory totals copied to score table successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying theory totals: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFinalizeGrades_Click(object sender, EventArgs e)
        {
            if (cmbExam.SelectedIndex == -1 && examId <= 0)
            {
                MessageBox.Show("Please select an exam before finalizing theory grades.", "Select Exam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSaveScores_Click(sender, e);

            int selectedExamId = examId > 0
                ? examId
                : Convert.ToInt32(cmbExam.SelectedValue);

            CopyTheoryTotalsToScoreTable(selectedExamId);
        }

        private void EnsureEmptyState()
        {
            if (emptyStateLabel != null)
            {
                return;
            }

            emptyStateLabel = new Label
            {
                Parent = dgvGrading.Parent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = ModernUi.MutedInk,
                BackColor = Color.FromArgb(11, 18, 31),
                Visible = false
            };

            emptyStateLabel.BringToFront();
            PositionEmptyState();
            dgvGrading.Parent.Resize += (s, e) => PositionEmptyState();
        }

        private void PositionEmptyState()
        {
            if (emptyStateLabel == null || dgvGrading.Parent == null)
            {
                return;
            }

            emptyStateLabel.Location = dgvGrading.Location;
            emptyStateLabel.Size = dgvGrading.Size;
        }

        private void ShowEmptyState(string message)
        {
            EnsureEmptyState();
            emptyStateLabel.Text = message;
            emptyStateLabel.Visible = true;
            dgvGrading.DataSource = null;
            dgvGrading.Visible = false;
        }

        private void HideEmptyState()
        {
            if (emptyStateLabel != null)
            {
                emptyStateLabel.Visible = false;
            }

            dgvGrading.Visible = true;
        }

        private void dgvGrading_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGrading.CurrentRow == null || dgvGrading.CurrentRow.IsNewRow)
            {
                ClearPreviewPanels();
                return;
            }

            PopulatePreviewPanelsFromRow(dgvGrading.CurrentRow);
        }

        private void PopulatePreviewPanelsFromRow(DataGridViewRow row)
        {
            if (questionPreviewBox == null || answerPreviewBox == null || row == null)
            {
                return;
            }

            bool hasQuestionColumn = row.DataGridView?.Columns.Contains("Question") == true;
            bool hasStudentAnswerColumn = row.DataGridView?.Columns.Contains("Student's Answer") == true;

            questionPreviewBox.Text = hasQuestionColumn
                ? Convert.ToString(row.Cells["Question"].Value)
                : string.Empty;

            answerPreviewBox.Text = hasStudentAnswerColumn
                ? Convert.ToString(row.Cells["Student's Answer"].Value)
                : string.Empty;
        }

        private void ClearPreviewPanels()
        {
            if (questionPreviewBox != null)
            {
                questionPreviewBox.Clear();
            }

            if (answerPreviewBox != null)
            {
                answerPreviewBox.Clear();
            }
        }

        private void UpdateDisplayedTotalScore()
        {
            if (richTextBox1 == null)
            {
                return;
            }

            if (answerTable == null || answerTable.Rows.Count == 0 || !answerTable.Columns.Contains("Score Given"))
            {
                richTextBox1.Text = "0";
                return;
            }

            decimal totalScore = 0m;

            foreach (DataRow row in answerTable.Rows)
            {
                object scoreObj = row["Score Given"];
                if (scoreObj == DBNull.Value || scoreObj == null)
                {
                    continue;
                }

                if (decimal.TryParse(scoreObj.ToString(), out decimal parsedScore))
                {
                    totalScore += parsedScore;
                }
            }

            richTextBox1.Text = totalScore % 1 == 0
                ? decimal.ToInt32(totalScore).ToString()
                : totalScore.ToString("0.##");
        }

        private void RefreshTheorySchemaState()
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    bool ready = TheorySchemaReady(conn);
                    btnLoadStudentAnswers.Enabled = ready;
                    btnSaveScores.Enabled = ready;
                    btnFinalizeGrades.Enabled = ready;
                    if (btnDeleteTheoryAnswers != null)
                    {
                        btnDeleteTheoryAnswers.Enabled = ready;
                    }

                    if (!ready)
                    {
                        ShowEmptyState("Theory grading is not ready yet because the required theory tables are missing from this database.");
                    }
                    else
                    {
                        HideEmptyState();
                    }
                }
            }
            catch
            {
                ShowEmptyState("Theory grading status could not be checked right now.");
            }
        }

        private bool TheorySchemaReady(SqlConnection connection)
        {
            return TheoryAnswersTableExists(connection) && TheoryQuestionsTableExists(connection);
        }

        private bool TheoryAnswersTableExists(SqlConnection connection)
        {
            return TableExists(connection, "tbl_theory_answers");
        }

        private bool TheoryQuestionsTableExists(SqlConnection connection)
        {
            return TableExists(connection, "tbl_theory_questions");
        }

        private bool TableExists(SqlConnection connection, string tableName)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table",
                connection))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool StoredProcedureExists(SqlConnection connection, string procedureName)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = @name",
                connection))
            {
                cmd.Parameters.AddWithValue("@name", procedureName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}

