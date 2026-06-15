using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Quiz_App
{
    public partial class view_scores : BaseForm
    {
        private const int BaseWidth = 1920;
        private const int BaseHeight = 1080;

        private Label summaryLabel;
        private Label batchLabel;
        private Label examFilterLabel;
        private Label studentLabel;
        private ComboBox batchFilterCombo;
        private ComboBox examFilterCombo;
        private ComboBox studentFilterCombo;
        private bool suppressFilterEvents;

        public view_scores()
        {
            InitializeComponent();
        }

        public string score { get; set; }

        public static void ScaleForm(Form form)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            float scaleX = (float)screenWidth / BaseWidth;
            float scaleY = (float)screenHeight / BaseHeight;

            form.Scale(new SizeF(scaleX, scaleY));

            foreach (Control c in form.Controls)
            {
                c.Font = new Font(c.Font.FontFamily, c.Font.Size * Math.Min(scaleX, scaleY));
            }

            form.StartPosition = FormStartPosition.CenterScreen;
        }

        private void view_scores_Load(object sender, EventArgs e)
        {
            view_scores.ScaleForm(this);
            dataGridView2.AutoGenerateColumns = true;
            ConfigureModernScoreWorkspace();
            InitializeScoreFilters();
            LoadBatchFilter();
            BindEmptyGrid();
        }

        private void BindData(string batchCode = null, int? examId = null, int? studentId = null)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                try
                {
                    con.Open();

                    bool hasTheoryScore = ColumnExists(con, "score", "theory_score");
                    bool hasCombinedScore = ColumnExists(con, "score", "combined_score");
                    bool hasTheoryDetails = ColumnExists(con, "score", "theory_details");

                    string sqlQuery = @"
SELECT 
    s.SCORE_ID AS [Score ID],
    sr.std_name AS [Student Name],
    sr.std_batch_code AS [Batch Code],
    e.ex_name AS [Exam Name],
    s.score AS [Objective Score],
    s.percentage AS [Objective Percentage], " +
    (hasTheoryScore ? "s.theory_score" : "CAST(NULL AS INT)") + @" AS [Theory Score],
    " + (hasCombinedScore ? "s.combined_score" : "s.score") + @" AS [Combined Score],
    " + (hasTheoryDetails ? "s.theory_details" : "CAST(NULL AS NVARCHAR(MAX))") + @" AS [Theory Details],
    s.stud_fk_id AS [Student ID],
    s.exam_fk_id AS [Exam ID]
FROM score s
LEFT JOIN student_record sr ON s.stud_fk_id = sr.std_id
LEFT JOIN tbl_exams e ON s.exam_fk_id = e.ex_id
WHERE 1 = 1";

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;

                        if (!string.IsNullOrWhiteSpace(batchCode) && !string.Equals(batchCode, "All Batches", StringComparison.OrdinalIgnoreCase))
                        {
                            sqlQuery += " AND sr.std_batch_code = @batchCode";
                            cmd.Parameters.AddWithValue("@batchCode", batchCode);
                        }

                        if (examId.HasValue)
                        {
                            sqlQuery += " AND s.exam_fk_id = @examId";
                            cmd.Parameters.AddWithValue("@examId", examId.Value);
                        }

                        if (studentId.HasValue)
                        {
                            sqlQuery += " AND s.stud_fk_id = @studentId";
                            cmd.Parameters.AddWithValue("@studentId", studentId.Value);
                        }

                        sqlQuery += " ORDER BY s.SCORE_ID DESC";
                        cmd.CommandText = sqlQuery;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dataGridView2.DataSource = table;
                        ApplyScoresGridFormatting();
                        UpdateScoreSummary(table.Rows.Count, !string.IsNullOrWhiteSpace(batchCode) || examId.HasValue);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading score data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select one or more score rows to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<int> scoreIds = new List<int>();
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                if (row.Cells["Score ID"]?.Value != null && int.TryParse(row.Cells["Score ID"].Value.ToString(), out int scoreId))
                {
                    scoreIds.Add(scoreId);
                }
            }

            if (scoreIds.Count == 0)
            {
                MessageBox.Show("The selected rows do not contain valid score IDs.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DeleteScores(scoreIds, $"Are you sure you want to delete {scoreIds.Count} selected score record(s)?");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string scoreId = null;
            if (dataGridView2.CurrentRow != null)
            {
                scoreId = Convert.ToString(dataGridView2.CurrentRow.Cells["Score ID"]?.Value);
            }

            if (string.IsNullOrWhiteSpace(scoreId))
            {
                MessageBox.Show("Type or select a Score ID to print.", "Missing Score ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                string query = "SELECT * FROM score WHERE SCORE_ID = @ScoreID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ScoreID", scoreId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("No record found with the given score ID.");
                            return;
                        }

                        string objectiveScore = reader["score"].ToString();
                        string percentage = reader["percentage"].ToString();

                        Print_Screen printScreen = new Print_Screen();
                        printScreen.UpdateData(scoreId, objectiveScore, percentage);
                        printScreen.Show();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Select a score row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(Convert.ToString(dataGridView2.CurrentRow.Cells["Score ID"]?.Value), out int scoreId))
            {
                MessageBox.Show("The selected row does not contain a valid score ID.");
                return;
            }

            DeleteScores(new[] { scoreId }, $"Are you sure you want to delete the score with ID {scoreId}?");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ApplySelectedFilters();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView2, "ManageScoresExport");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ResetScoreFilters();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
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

        private void ApplyScoresGridFormatting()
        {
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = true;

            if (dataGridView2.Columns.Contains("Objective Percentage"))
            {
                dataGridView2.Columns["Objective Percentage"].DefaultCellStyle.Format = "0.00'%'";
            }

            if (dataGridView2.Columns.Contains("Theory Details"))
            {
                dataGridView2.Columns["Theory Details"].Visible = false;
            }

            if (dataGridView2.Columns.Contains("Exam ID"))
            {
                dataGridView2.Columns["Exam ID"].Visible = false;
            }

            if (dataGridView2.Columns.Contains("Student ID"))
            {
                dataGridView2.Columns["Student ID"].Visible = false;
            }

            ModernUi.StyleDataGridView(dataGridView2);
            dataGridView2.ClearSelection();
        }

        private void ExportToExcel(DataGridView targetGrid, string fileName)
        {
            if (targetGrid.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            using (XLWorkbook workbook = new XLWorkbook())
            {
                DataTable table = new DataTable("Sheet1");

                foreach (DataGridViewColumn column in targetGrid.Columns)
                {
                    if (column.Visible)
                    {
                        table.Columns.Add(column.HeaderText);
                    }
                }

                foreach (DataGridViewRow row in targetGrid.Rows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    DataRow dataRow = table.NewRow();
                    int visibleColumnIndex = 0;

                    for (int i = 0; i < targetGrid.Columns.Count; i++)
                    {
                        if (!targetGrid.Columns[i].Visible)
                        {
                            continue;
                        }

                        dataRow[visibleColumnIndex++] = row.Cells[i].Value ?? DBNull.Value;
                    }

                    table.Rows.Add(dataRow);
                }

                workbook.Worksheets.Add(table, "Export");

                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    fileName + ".xlsx");

                workbook.SaveAs(path);
                MessageBox.Show("Export successful!\nSaved at: " + path);
            }
        }

        private void ConfigureModernScoreWorkspace()
        {
            BackColor = Color.FromArgb(11, 18, 31);

            label3.Text = "Manage Scores";
            label3.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.White;
            label3.Location = new Point(44, 28);
            label3.Size = new Size(360, 44);

            if (summaryLabel == null)
            {
                summaryLabel = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = ModernUi.MutedInk,
                    Location = new Point(48, 78)
                };
                Controls.Add(summaryLabel);
                summaryLabel.BringToFront();
            }

            EnsureFilterLabels();

            label1.Visible = false;
            label2.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            button1.Visible = false;
            dataGridView1.Visible = false;
            textBox4.Visible = false;

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            comboBox1.Visible = false;

            batchLabel.Location = new Point(48, 132);
            examFilterLabel.Location = new Point(560, 132);
            studentLabel.Location = new Point(1072, 132);

            dataGridView2.Location = new Point(48, 282);
            dataGridView2.Size = new Size(1434, 560);
            dataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dataGridView2.CellClick -= dataGridView2_CellClick;
            dataGridView2.CellClick += dataGridView2_CellClick;

            ConfigureActionButton(button5, "Load Scores", new Point(48, 220), new Size(210, 44), ModernUi.Accent, Color.FromArgb(8, 20, 28));
            ConfigureActionButton(button7, "Reset View", new Point(274, 220), new Size(190, 44), Color.FromArgb(32, 51, 79), Color.White);
            ConfigureActionButton(button2, "Delete Selected", new Point(964, 220), new Size(170, 44), Color.FromArgb(134, 46, 46), Color.White);
            ConfigureActionButton(button4, "Delete Record", new Point(1148, 220), new Size(170, 44), Color.FromArgb(95, 56, 42), Color.White);
            ConfigureActionButton(button3, "Print Record", new Point(1332, 220), new Size(150, 44), Color.FromArgb(32, 51, 79), Color.White);
            ConfigureActionButton(button6, "Export Excel", new Point(1272, 860), new Size(210, 46), Color.FromArgb(32, 51, 79), Color.White);
            button6.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        }

        private void EnsureFilterLabels()
        {
            if (examFilterLabel != null)
            {
                return;
            }

            examFilterLabel = CreateFilterLabel("Select Exam");
            batchLabel = CreateFilterLabel("Batch");
            studentLabel = CreateFilterLabel("Select Student");

            Controls.Add(examFilterLabel);
            Controls.Add(batchLabel);
            Controls.Add(studentLabel);
        }

        private Label CreateFilterLabel(string text)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = ModernUi.MutedInk,
                BackColor = Color.Transparent
            };
        }

        private void ConfigureActionButton(Button button, string text, Point location, Size size, Color backColor, Color foreColor)
        {
            button.Visible = true;
            button.Text = text;
            button.Location = location;
            button.Size = size;
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            button.Cursor = Cursors.Hand;
        }

        private void InitializeScoreFilters()
        {
            if (examFilterCombo != null)
            {
                return;
            }

            batchFilterCombo = CreateFilterCombo(new Point(48, 162), new Size(450, 38));
            examFilterCombo = CreateFilterCombo(new Point(560, 162), new Size(450, 38));
            studentFilterCombo = CreateFilterCombo(new Point(1072, 162), new Size(410, 38));

            examFilterCombo.SelectedIndexChanged += examFilterCombo_SelectedIndexChanged;
            batchFilterCombo.SelectedIndexChanged += batchFilterCombo_SelectedIndexChanged;
            studentFilterCombo.SelectedIndexChanged += studentFilterCombo_SelectedIndexChanged;

            Controls.Add(examFilterCombo);
            Controls.Add(batchFilterCombo);
            Controls.Add(studentFilterCombo);
        }

        private ComboBox CreateFilterCombo(Point location, Size size)
        {
            ComboBox combo = new ComboBox
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(18, 26, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            return combo;
        }

        private void LoadBatchFilter()
        {
            suppressFilterEvents = true;
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(
                    @"SELECT DISTINCT std_batch_code
                      FROM student_record
                      WHERE std_batch_code IS NOT NULL AND LTRIM(RTRIM(std_batch_code)) <> ''
                      ORDER BY std_batch_code", connection_class.GetConnection()))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    DataRow promptRow = table.NewRow();
                    promptRow["std_batch_code"] = "Select Batch";
                    table.Rows.InsertAt(promptRow, 0);

                    batchFilterCombo.DataSource = table;
                    batchFilterCombo.DisplayMember = "std_batch_code";
                    batchFilterCombo.ValueMember = "std_batch_code";
                }

                examFilterCombo.DataSource = null;
                examFilterCombo.Items.Clear();
                examFilterCombo.Items.Add("Select Exam");
                examFilterCombo.SelectedIndex = 0;
                examFilterCombo.Enabled = false;

                studentFilterCombo.DataSource = null;
                studentFilterCombo.Items.Clear();
                studentFilterCombo.Items.Add("Select Student");
                studentFilterCombo.SelectedIndex = 0;
                studentFilterCombo.Enabled = false;
            }
            finally
            {
                suppressFilterEvents = false;
            }
        }

        private void LoadExamFilter(string batchCode)
        {
            suppressFilterEvents = true;
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(
                    @"SELECT ex_id, ex_name
                      FROM tbl_exams
                      ORDER BY ex_name", connection_class.GetConnection()))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    DataRow promptRow = table.NewRow();
                    promptRow["ex_id"] = DBNull.Value;
                    promptRow["ex_name"] = "Select Exam";
                    table.Rows.InsertAt(promptRow, 0);

                    examFilterCombo.DataSource = table;
                    examFilterCombo.DisplayMember = "ex_name";
                    examFilterCombo.ValueMember = "ex_id";
                    examFilterCombo.Enabled = true;
                }

                studentFilterCombo.DataSource = null;
                studentFilterCombo.Items.Clear();
                studentFilterCombo.Items.Add("Select Student");
                studentFilterCombo.SelectedIndex = 0;
                studentFilterCombo.Enabled = false;
            }
            finally
            {
                suppressFilterEvents = false;
            }
        }

        private void LoadStudentFilter(string batchCode, int? examId)
        {
            suppressFilterEvents = true;
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(
                    @"SELECT DISTINCT
                          sr.std_id,
                          sr.std_name,
                          sr.std_name + ' (' + CAST(sr.std_id AS NVARCHAR(20)) + ')' AS student_display
                      FROM student_record sr
                      LEFT JOIN score s ON s.stud_fk_id = sr.std_id
                      WHERE (@batchCode = 'All Batches' OR sr.std_batch_code = @batchCode)
                        AND (@examId IS NULL OR s.exam_fk_id = @examId)
                      ORDER BY sr.std_name", connection_class.GetConnection()))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@batchCode", batchCode ?? "All Batches");
                    adapter.SelectCommand.Parameters.AddWithValue("@examId", (object)examId ?? DBNull.Value);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    DataRow allRow = table.NewRow();
                    allRow["std_id"] = DBNull.Value;
                    allRow["student_display"] = "All Students";
                    table.Rows.InsertAt(allRow, 0);

                    studentFilterCombo.DataSource = table;
                    studentFilterCombo.DisplayMember = "student_display";
                    studentFilterCombo.ValueMember = "std_id";
                    studentFilterCombo.Enabled = true;
                }
            }
            finally
            {
                suppressFilterEvents = false;
            }
        }

        private void BindEmptyGrid()
        {
            DataTable emptyTable = new DataTable();
            emptyTable.Columns.Add("Score ID");
            emptyTable.Columns.Add("Student Name");
            emptyTable.Columns.Add("Batch Code");
            emptyTable.Columns.Add("Exam Name");
            emptyTable.Columns.Add("Objective Score");
            emptyTable.Columns.Add("Objective Percentage");
            emptyTable.Columns.Add("Theory Score");
            emptyTable.Columns.Add("Combined Score");
            emptyTable.Columns.Add("Theory Details");
            emptyTable.Columns.Add("Student ID");
            emptyTable.Columns.Add("Exam ID");

            dataGridView2.DataSource = emptyTable;
            ApplyScoresGridFormatting();
            UpdateScoreSummary(0, false);
        }

        private void UpdateScoreSummary(int rowCount, bool examChosen)
        {
            if (summaryLabel == null)
            {
                return;
            }

            if (!examChosen)
            {
                summaryLabel.Text = "Choose a batch first. Then exam and student can narrow the score records further.";
                return;
            }

            if (rowCount == 0)
            {
                summaryLabel.Text = "No score records were found for the selected exam, batch, and student.";
                return;
            }

            summaryLabel.Text = rowCount == 1
                ? "1 score record loaded for the selected filters."
                : $"{rowCount} score records loaded for the selected filters.";
        }

        private void DeleteScores(IEnumerable<int> scoreIds, string confirmationMessage)
        {
            List<int> ids = scoreIds.Distinct().ToList();
            if (ids.Count == 0)
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                confirmationMessage,
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (int scoreId in ids)
                    {
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM score WHERE SCORE_ID = @ScoreId", conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ScoreId", scoreId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show(ids.Count == 1 ? "Score deleted successfully." : $"{ids.Count} score records deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ApplySelectedFilters();
                    dataGridView2.ClearSelection();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred while deleting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView2.Rows[e.RowIndex].IsNewRow)
            {
                return;
            }

            object scoreId = dataGridView2.Rows[e.RowIndex].Cells["Score ID"]?.Value;
            if (scoreId != null && scoreId != DBNull.Value)
            {
                dataGridView2.Rows[e.RowIndex].Selected = true;
            }
        }

        private string GetSelectedBatchCode()
        {
            if (batchFilterCombo?.SelectedValue == null || batchFilterCombo.SelectedValue == DBNull.Value)
            {
                return null;
            }

            return Convert.ToString(batchFilterCombo.SelectedValue);
        }

        private int? GetSelectedExamId()
        {
            return examFilterCombo?.SelectedValue == null || examFilterCombo.SelectedValue == DBNull.Value
                ? (int?)null
                : Convert.ToInt32(examFilterCombo.SelectedValue);
        }

        private int? GetSelectedStudentId()
        {
            return studentFilterCombo?.SelectedValue == null || studentFilterCombo.SelectedValue == DBNull.Value
                ? (int?)null
                : Convert.ToInt32(studentFilterCombo.SelectedValue);
        }

        private void ApplySelectedFilters()
        {
            string batchCode = GetSelectedBatchCode();
            if (string.IsNullOrWhiteSpace(batchCode) || string.Equals(batchCode, "Select Batch", StringComparison.OrdinalIgnoreCase))
            {
                BindEmptyGrid();
                return;
            }

            BindData(batchCode, GetSelectedExamId(), GetSelectedStudentId());
        }

        private void ResetScoreFilters()
        {
            LoadBatchFilter();
            BindEmptyGrid();
        }

        private void batchFilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressFilterEvents)
            {
                return;
            }

            string batchCode = GetSelectedBatchCode();
            if (string.IsNullOrWhiteSpace(batchCode) || string.Equals(batchCode, "Select Batch", StringComparison.OrdinalIgnoreCase))
            {
                ResetScoreFilters();
                return;
            }

            LoadExamFilter(batchCode);
            LoadStudentFilter(batchCode, null);
            ApplySelectedFilters();
        }

        private void examFilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressFilterEvents)
            {
                return;
            }

            string batchCode = GetSelectedBatchCode() ?? "All Batches";
            LoadStudentFilter(batchCode, GetSelectedExamId());
            ApplySelectedFilters();
        }

        private void studentFilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressFilterEvents)
            {
                return;
            }

            ApplySelectedFilters();
        }
    }
}
