using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class multi_GPA : BaseForm
    {
        private bool _loadingFilters;

        public multi_GPA()
        {
            InitializeComponent();
        }

        private void ApplyModernLayout()
        {
            ModernUi.ApplyTheme(this);
            ModernUi.StyleComboBox(batchFilterCombo);
            ModernUi.StyleComboBox(examFilterCombo);
            ModernUi.StyleComboBox(studentFilterCombo);
            ModernUi.StyleDataGridView(dataGridView1);
            ModernUi.StylePrimaryButton(button1);
            ModernUi.StyleSecondaryButton(btnClear);
            ModernUi.StyleSecondaryButton(btnRefresh);
            ModernUi.StyleSecondaryButton(button2);

            labelTitle.ForeColor = ModernUi.Ink;
            labelTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);

            labelBatch.ForeColor = ModernUi.MutedInk;
            labelExam.ForeColor = ModernUi.MutedInk;
            labelStudent.ForeColor = ModernUi.MutedInk;

            labelResult.ForeColor = Color.FromArgb(92, 240, 195);
            labelResult.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            labelResult.TextAlign = ContentAlignment.MiddleCenter;

            ArrangeResponsiveLayout();
        }

        private void ArrangeResponsiveLayout()
        {
            int sidePadding = 32;
            int topPadding = 24;
            int headerButtonWidth = 120;
            int headerButtonHeight = 42;
            int titleTop = topPadding + 4;
            int filterLabelTop = 82;
            int filterTop = 104;
            int filterGap = 32;
            int gridTop = 176;
            int bottomButtonHeight = 52;
            int bottomPadding = 28;
            int bottomRowTop = ClientSize.Height - bottomButtonHeight - bottomPadding;
            int gridHeight = Math.Max(220, bottomRowTop - gridTop - 18);
            int comboWidth = (ClientSize.Width - (sidePadding * 2) - (filterGap * 2)) / 3;

            btnClear.SetBounds(sidePadding, topPadding, headerButtonWidth, headerButtonHeight);
            btnRefresh.SetBounds(ClientSize.Width - sidePadding - 150, topPadding, 150, headerButtonHeight);

            labelTitle.AutoSize = true;
            labelTitle.Location = new Point((ClientSize.Width - labelTitle.PreferredWidth) / 2, titleTop);

            labelBatch.Location = new Point(sidePadding, filterLabelTop);
            batchFilterCombo.SetBounds(sidePadding, filterTop, comboWidth, 36);

            labelExam.Location = new Point(batchFilterCombo.Right + filterGap, filterLabelTop);
            examFilterCombo.SetBounds(batchFilterCombo.Right + filterGap, filterTop, comboWidth, 36);

            labelStudent.Location = new Point(examFilterCombo.Right + filterGap, filterLabelTop);
            studentFilterCombo.SetBounds(examFilterCombo.Right + filterGap, filterTop, comboWidth, 36);

            dataGridView1.SetBounds(sidePadding, gridTop, ClientSize.Width - (sidePadding * 2), gridHeight);

            button1.SetBounds(sidePadding, bottomRowTop, 180, bottomButtonHeight);
            button2.SetBounds(ClientSize.Width - sidePadding - 190, bottomRowTop, 190, bottomButtonHeight);

            labelResult.AutoSize = false;
            labelResult.SetBounds((ClientSize.Width / 2) - 185, bottomRowTop + 10, 370, 30);
        }

        private void multi_GPA_Load(object sender, EventArgs e)
        {
            ApplyModernLayout();
            Resize += (s, args) => ArrangeResponsiveLayout();
            LoadBatchFilter();
            ResetView();
        }

        private void ResetView()
        {
            _loadingFilters = true;
            try
            {
                batchFilterCombo.SelectedIndex = -1;
                examFilterCombo.DataSource = null;
                studentFilterCombo.DataSource = null;
                examFilterCombo.Items.Clear();
                studentFilterCombo.Items.Clear();
                examFilterCombo.Text = string.Empty;
                studentFilterCombo.Text = string.Empty;
                examFilterCombo.Enabled = false;
                studentFilterCombo.Enabled = false;
                dataGridView1.DataSource = null;
                labelResult.Text = "CGPA = ?";
            }
            finally
            {
                _loadingFilters = false;
            }
        }

        private void LoadBatchFilter()
        {
            _loadingFilters = true;
            try
            {
                DataTable table = new DataTable();
                using (SqlConnection conn = connection_class.GetConnection())
                using (SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT DISTINCT std_batch_code FROM student_record WHERE std_batch_code IS NOT NULL AND LTRIM(RTRIM(std_batch_code)) <> '' ORDER BY std_batch_code", conn))
                {
                    adapter.Fill(table);
                }

                batchFilterCombo.DataSource = table;
                batchFilterCombo.DisplayMember = "std_batch_code";
                batchFilterCombo.ValueMember = "std_batch_code";
                batchFilterCombo.SelectedIndex = -1;
                batchFilterCombo.Text = "Select Batch";
            }
            finally
            {
                _loadingFilters = false;
            }
        }

        private void LoadExamFilter(string batchCode)
        {
            _loadingFilters = true;
            try
            {
                DataTable table = new DataTable();
                using (SqlConnection conn = connection_class.GetConnection())
                using (SqlDataAdapter adapter = new SqlDataAdapter(@"
                    SELECT DISTINCT e.ex_id, e.ex_name
                    FROM score s
                    INNER JOIN tbl_exams e ON s.exam_fk_id = e.ex_id
                    INNER JOIN student_record sr ON s.percentagestud_fk_id = sr.std_id
                    WHERE sr.std_batch_code = @batchCode
                    ORDER BY e.ex_name", conn))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@batchCode", batchCode);
                    adapter.Fill(table);
                }

                examFilterCombo.DataSource = table;
                examFilterCombo.DisplayMember = "ex_name";
                examFilterCombo.ValueMember = "ex_id";
                examFilterCombo.SelectedIndex = -1;
                examFilterCombo.Text = "Select Exam";
                examFilterCombo.Enabled = table.Rows.Count > 0;
            }
            finally
            {
                _loadingFilters = false;
            }
        }

        private void LoadStudentFilter(string batchCode, int examId)
        {
            _loadingFilters = true;
            try
            {
                DataTable table = new DataTable();
                using (SqlConnection conn = connection_class.GetConnection())
                using (SqlDataAdapter adapter = new SqlDataAdapter(@"
                    SELECT DISTINCT
                        sr.std_id,
                        sr.std_name + ' (' + CAST(sr.std_id AS NVARCHAR(20)) + ')' AS student_display
                    FROM score s
                    INNER JOIN student_record sr ON s.percentagestud_fk_id = sr.std_id
                    WHERE sr.std_batch_code = @batchCode
                      AND s.exam_fk_id = @examId
                    ORDER BY student_display", conn))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@batchCode", batchCode);
                    adapter.SelectCommand.Parameters.AddWithValue("@examId", examId);
                    adapter.Fill(table);
                }

                studentFilterCombo.DataSource = table;
                studentFilterCombo.DisplayMember = "student_display";
                studentFilterCombo.ValueMember = "std_id";
                studentFilterCombo.SelectedIndex = -1;
                studentFilterCombo.Text = "Select Student";
                studentFilterCombo.Enabled = table.Rows.Count > 0;
            }
            finally
            {
                _loadingFilters = false;
            }
        }

        private void LoadCgpaResult(int studentId, int examId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Student ID");
            table.Columns.Add("Student Name");
            table.Columns.Add("Batch");
            table.Columns.Add("Course");
            table.Columns.Add("Grade");
            table.Columns.Add("Grade Point", typeof(int));
            table.Columns.Add("Credit Unit", typeof(int));
            table.Columns.Add("Quality Point", typeof(int));

            string studentName = studentFilterCombo.Text;
            string batchCode = batchFilterCombo.Text;

            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT e.ex_name, s.percentage, es.unit
                    FROM score s
                    INNER JOIN tbl_exams e ON s.exam_fk_id = e.ex_id
                    INNER JOIN tbl_exam_settings es ON e.ex_id = es.ex_id
                    WHERE s.percentagestud_fk_id = @studentId
                      AND s.exam_fk_id = @examId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    cmd.Parameters.AddWithValue("@examId", examId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string course = reader["ex_name"].ToString();
                            double percentage = reader["percentage"] != DBNull.Value ? Convert.ToDouble(reader["percentage"]) : 0;
                            int unit = reader["unit"] != DBNull.Value ? Convert.ToInt32(reader["unit"]) : 0;
                            string grade = GetGrade(percentage);
                            int gradePoint = GetGradePoint(grade);
                            int qualityPoint = gradePoint * unit;

                            table.Rows.Add(studentId, studentName, batchCode, course, grade, gradePoint, unit, qualityPoint);
                        }
                    }
                }
            }

            dataGridView1.DataSource = table;

            int totalQualityPoints = table.AsEnumerable().Sum(r => r.Field<int?>("Quality Point") ?? 0);
            int totalUnits = table.AsEnumerable().Sum(r => r.Field<int?>("Credit Unit") ?? 0);
            double cgpa = totalUnits > 0 ? (double)totalQualityPoints / totalUnits : 0;
            labelResult.Text = $"CGPA = {cgpa:F2}";
        }

        private static string GetGrade(double percentage)
        {
            if (percentage >= 70) return "A";
            if (percentage >= 60) return "B";
            if (percentage >= 50) return "C";
            if (percentage >= 45) return "D";
            if (percentage >= 40) return "E";
            return "F";
        }

        private static int GetGradePoint(string grade)
        {
            switch (grade)
            {
                case "A": return 5;
                case "B": return 4;
                case "C": return 3;
                case "D": return 2;
                case "E": return 1;
                default: return 0;
            }
        }

        private bool TryGetSelectedBatch(out string batchCode)
        {
            batchCode = batchFilterCombo.SelectedValue?.ToString();
            return !string.IsNullOrWhiteSpace(batchCode);
        }

        private bool TryGetSelectedExam(out int examId)
        {
            examId = 0;
            return examFilterCombo.SelectedValue != null && int.TryParse(examFilterCombo.SelectedValue.ToString(), out examId);
        }

        private bool TryGetSelectedStudent(out int studentId)
        {
            studentId = 0;
            return studentFilterCombo.SelectedValue != null && int.TryParse(studentFilterCombo.SelectedValue.ToString(), out studentId);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetView();
            LoadBatchFilter();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (TryGetSelectedStudent(out int studentId) && TryGetSelectedExam(out int examId))
            {
                LoadCgpaResult(studentId, examId);
                return;
            }

            MessageBox.Show("Select Batch, Exam, and Student first.", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedStudent(out int studentId) || !TryGetSelectedExam(out int examId))
            {
                MessageBox.Show("Select Batch, Exam, and Student first.", "Load CGPA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadCgpaResult(studentId, examId);
        }

        private void batchFilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingFilters || !TryGetSelectedBatch(out string batchCode))
            {
                return;
            }

            dataGridView1.DataSource = null;
            labelResult.Text = "CGPA = ?";
            studentFilterCombo.DataSource = null;
            studentFilterCombo.Items.Clear();
            studentFilterCombo.Enabled = false;
            LoadExamFilter(batchCode);
        }

        private void examFilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingFilters || !TryGetSelectedBatch(out string batchCode) || !TryGetSelectedExam(out int examId))
            {
                return;
            }

            dataGridView1.DataSource = null;
            labelResult.Text = "CGPA = ?";
            LoadStudentFilter(batchCode, examId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = "StudentCGPA.xlsx"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        DataTable dt = new DataTable();
                        foreach (DataGridViewColumn col in dataGridView1.Columns)
                        {
                            dt.Columns.Add(col.HeaderText);
                        }

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow)
                            {
                                continue;
                            }

                            DataRow dataRow = dt.NewRow();
                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                dataRow[i] = row.Cells[i].Value?.ToString();
                            }

                            dt.Rows.Add(dataRow);
                        }

                        wb.Worksheets.Add(dt, "CGPA Report");
                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Export Successful!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
