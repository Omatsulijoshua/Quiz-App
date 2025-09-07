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
    public partial class single_GPA : Form
    {
        public single_GPA()
        {
            InitializeComponent();
        }
        private DataTable dt; // ✅ make it class-level
        private void btnClear_Click(object sender, EventArgs e)
        {
            dataGridViewMaster.DataSource = null;
            textBox1.Clear();
            label2.Text = "GPA = ?";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter a Student ID");
                return;
            }

            string studentId = textBox1.Text.Trim();
            LoadStudentResult(studentId);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Enter Student ID first.");
                return;
            }

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    string query = @"
    SELECT x.ex_name, s.percentage, e.unit
    FROM score s
    INNER JOIN tbl_exam_settings e 
        ON s.exam_fk_id = e.ex_id
    INNER JOIN tbl_exams x
        ON e.ex_id = x.ex_id
    WHERE s.percentagestud_fk_id = @StudentId";


                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", textBox1.Text);

                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Course");
                    dt.Columns.Add("Grade");
                    dt.Columns.Add("Grade Point", typeof(int));
                    dt.Columns.Add("Credit Unit", typeof(int));
                    dt.Columns.Add("Quality Point", typeof(int));

                    while (reader.Read())
                    {
                        string course = reader["ex_name"].ToString();
                        double score = Convert.ToDouble(reader["score"]);
                        int unit = Convert.ToInt32(reader["unit"]);

                        string grade = GetGrade(score);
                        int gradePoint = GetGradePoint(grade);
                        int qualityPoint = gradePoint * unit;

                        dt.Rows.Add(course, grade, gradePoint, unit, qualityPoint);
                    }

                    reader.Close();
                    dataGridViewMaster.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private string GetGrade(double percentage)
        {
            if (percentage >= 70)
                return "A";
            else if (percentage >= 60)
                return "B";
            else if (percentage >= 50)
                return "C";
            else if (percentage >= 45)
                return "D";
            else if (percentage >= 40)
                return "E";
            else
                return "F";
        }

        private int GetGradePoint(string grade)
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

        private void LoadStudentResult(string studentId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Student ID");
            dt.Columns.Add("Course");
            dt.Columns.Add("Grade");
            dt.Columns.Add("Grade Point", typeof(int));
            dt.Columns.Add("Credit Unit", typeof(int));
            dt.Columns.Add("Quality Point", typeof(int));

            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();

                // join score with exams and exam_settings to get percentage + course name + credit unit
                string query = @"
            SELECT e.ex_name, s.percentage, es.unit
            FROM score s
            INNER JOIN tbl_exams e ON s.exam_fk_id = e.ex_id
            INNER JOIN tbl_exam_settings es ON e.ex_id = es.ex_id
            WHERE s.percentagestud_fk_id = @studentId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", textBox1.Text.Trim());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string course = reader["ex_name"].ToString();

                            // safely handle NULLs
                            double percentage = reader["percentage"] != DBNull.Value ? Convert.ToDouble(reader["percentage"]) : 0;
                            int unit = reader["unit"] != DBNull.Value ? Convert.ToInt32(reader["unit"]) : 0;

                            // convert percentage → grade + gradePoint
                            string grade = GetGrade(percentage);
                            int gradePoint = GetGradePoint(grade);

                            // calculate quality point = gradePoint × unit
                            int qualityPoint = gradePoint * unit;

                            // add row to datatable
                            dt.Rows.Add(course, grade, gradePoint, unit, qualityPoint);
                        }
                    }
                }
            }

            // bind to grid
            dataGridViewMaster.DataSource = dt;

            // calculate GPA safely (nulls = 0)
            int totalQualityPoints = dt.AsEnumerable()
                .Sum(r => r.Field<int?>("Quality Point") ?? 0);

            int totalUnits = dt.AsEnumerable()
                .Sum(r => r.Field<int?>("Credit Unit") ?? 0);

            double gpa = totalUnits > 0 ? (double)totalQualityPoints / totalUnits : 0;

            label2.Text = $"GPA = {gpa:F2}";

    }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewMaster.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog()
                { Filter = "Excel Workbook|*.xlsx", FileName = "StudentGPA.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var workbook = new ClosedXML.Excel.XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("GPA Records");

                            // Add headers
                            for (int i = 0; i < dataGridViewMaster.Columns.Count; i++)
                            {
                                worksheet.Cell(1, i + 1).Value = dataGridViewMaster.Columns[i].HeaderText;
                            }

                            // Add data
                            for (int i = 0; i < dataGridViewMaster.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataGridViewMaster.Columns.Count; j++)
                                {
                                    worksheet.Cell(i + 2, j + 1).Value =
                                        dataGridViewMaster.Rows[i].Cells[j].Value?.ToString();
                                }
                            }

                            // Add GPA at the bottom
                            worksheet.Cell(dataGridViewMaster.Rows.Count + 3, 1).Value = "Calculated GPA:";
                            worksheet.Cell(dataGridViewMaster.Rows.Count + 3, 2).Value = label2.Text;

                            workbook.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            GPA w = new GPA();
            w.Show();
            this.Hide();
        }
    }
}
