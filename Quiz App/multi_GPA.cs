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
using ClosedXML.Excel;
using System.IO;

namespace Quiz_App
{
    public partial class multi_GPA : Form
    {
        public multi_GPA()
        {
            InitializeComponent();
        }

        private void multi_GPA_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = connection_class.GetConnection())
            {
                string query = "SELECT DISTINCT std_batch_code FROM student_record";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["std_batch_code"].ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a batch first!", "Batch Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string batchCode = comboBox1.SelectedItem.ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("Student ID");
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Batch Code");
            dt.Columns.Add("GPA");

            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();

                // Get all students in this batch
                string query = "SELECT std_id, std_name, std_batch_code FROM student_record WHERE std_batch_code = @batch";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@batch", batchCode);

                SqlDataReader reader = cmd.ExecuteReader();
                List<(int, string, string)> students = new List<(int, string, string)>();

                while (reader.Read())
                {
                    students.Add((Convert.ToInt32(reader["std_id"]), reader["std_name"].ToString(), reader["std_batch_code"].ToString()));
                }
                reader.Close();

                foreach (var student in students)
                {
                    double totalQualityPoints = 0;
                    double totalCreditUnits = 0;

                    // Get student's scores
                    string scoreQuery = @"SELECT s.score, ex.ex_id, ex.ex_name, ts.unit 
                                  FROM score s
                                  INNER JOIN tbl_exams ex ON s.exam_fk_id = ex.ex_id
                                  INNER JOIN tbl_exam_settings ts ON ex.ex_id = ts.ex_id
                                  WHERE s.percentagestud_fk_id = @stdId";

                    SqlCommand scoreCmd = new SqlCommand(scoreQuery, conn);
                    scoreCmd.Parameters.AddWithValue("@stdId", student.Item1);
                    SqlDataReader scoreReader = scoreCmd.ExecuteReader();

                    while (scoreReader.Read())
                    {
                        int percentage = Convert.ToInt32(scoreReader["score"]);
                        int creditUnit = Convert.ToInt32(scoreReader["set_course_credit"]);
                        int gradePoint = GetGradePoint(percentage);
                        int qualityPoint = gradePoint * creditUnit;

                        totalQualityPoints += qualityPoint;
                        totalCreditUnits += creditUnit;
                    }
                    scoreReader.Close();

                    double gpa = totalCreditUnits > 0 ? totalQualityPoints / totalCreditUnits : 0;

                    dt.Rows.Add(student.Item1, student.Item2, student.Item3, gpa.ToString("0.00"));
                }
            }

            dataGridView1.DataSource = dt;
        }

        private int GetGradePoint(int percentage)
        {
            if (percentage >= 70) return 5;   // A
            else if (percentage >= 60) return 4; // B
            else if (percentage >= 50) return 3; // C
            else if (percentage >= 45) return 2; // D
            else if (percentage >= 40) return 1; // E
            else return 0; // F
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get selected batch code (if none, fallback to "All")
            string batchCode = comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "All";

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = $"Batch_{batchCode}_GPA_Report.xlsx"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            DataTable dt = new DataTable();

                            // Create columns from DataGridView
                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                dt.Columns.Add(col.HeaderText);
                            }

                            // Add rows
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    DataRow dRow = dt.NewRow();
                                    for (int i = 0; i < row.Cells.Count; i++)
                                    {
                                        dRow[i] = row.Cells[i].Value?.ToString();
                                    }
                                    dt.Rows.Add(dRow);
                                }
                            }

                            // Add worksheet
                            wb.Worksheets.Add(dt, "GPA Report");
                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show($"Export Successful!\nFile saved as {Path.GetFileName(sfd.FileName)}",
                                        "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

}
