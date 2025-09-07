// ReportCardForm.cs
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class ReportCardForm : Form
    {
        private int studentId;
        private string studentName;
        private string studentPosition;

        public ReportCardForm(int stdId, string stdName, string position)
        {
            InitializeComponent();
            studentId = stdId;
            studentName = stdName;
            studentPosition = position;

            lblStudent.Text = $"Report Card for: {studentName}";
            labelPosition.Text = $"Position: {studentPosition}";

            LoadStudentScores();
        }

        private void LoadStudentScores()
        {
            try
            {
                using (SqlConnection con = connection_class.GetConnection())
                {
                    con.Open();

                    string query = @"
                SELECT ex.ex_name, sc.score, sc.percentage
                FROM score sc
                INNER JOIN tbl_exams ex ON sc.exam_fk_id = ex.ex_id
                WHERE sc.stud_fk_id = @studId";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@studId", studentId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No exam records found for this student.");
                    }

                    dataGridViewReport.DataSource = dt;

                    float totalPercentage = 0;
                    int totalScore = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        totalPercentage += Convert.ToSingle(row["percentage"]);
                        totalScore += Convert.ToInt32(row["score"]);
                    }

                    float averagePercentage = dt.Rows.Count > 0 ? totalPercentage / dt.Rows.Count : 0;

                    labelAverage.Text = $"Average Percentage: {averagePercentage:F2}%";
                    labelRemark.Text = $"Remark: {GetRemark(averagePercentage)}";
                    labelGrade.Text = $"Grade: {GetGrade(averagePercentage)}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading scores: " + ex.Message);
            }
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string filename = $"{downloads}\\ReportCard_{studentName}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            Document document = new Document();
            try
            {
                PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                document.Add(new Paragraph($"Report Card for {studentName}", titleFont));
                document.Add(new Paragraph($"Position: {studentPosition}", regularFont));
                document.Add(new Paragraph(" ", regularFont));

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.AddCell("Exam");
                table.AddCell("Score");
                table.AddCell("Percentage");

                foreach (DataGridViewRow row in dataGridViewReport.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        table.AddCell(row.Cells[0].Value?.ToString() ?? "");
                        table.AddCell(row.Cells[1].Value?.ToString() ?? "");
                        table.AddCell(row.Cells[2].Value?.ToString() + "%" ?? "");
                    }
                }

                document.Add(table);
                document.Add(new Paragraph(" ", regularFont));
                document.Add(new Paragraph(labelAverage.Text, regularFont));
                document.Add(new Paragraph(labelGrade.Text, regularFont));
                document.Add(new Paragraph(labelRemark.Text, regularFont));

                MessageBox.Show("PDF exported successfully to Downloads folder.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to export PDF: " + ex.Message);
            }
            finally
            {
                document.Close();
            }
        }

        private string GetRemark(float percentage)
        {
            if (percentage <= 40) return "Bad. Try better next term.";
            else if (percentage <= 50) return "Pass. You can do better.";
            else if (percentage <= 60) return "Fair. Keep improving.";
            else if (percentage <= 70) return "Good. Nice work.";
            else if (percentage <= 80) return "Very Good. Keep it up!";
            else if (percentage <= 90) return "Excellent Performance!";
            else return "Outstanding! You're a star!";
        }

        private string GetGrade(float percentage)
        {
            if (percentage >= 75) return "A1";
            else if (percentage >= 70) return "B2";
            else if (percentage >= 65) return "B3";
            else if (percentage >= 60) return "C4";
            else if (percentage >= 55) return "C5";
            else if (percentage >= 50) return "C6";
            else if (percentage >= 45) return "D7";
            else if (percentage >= 40) return "E8";
            else return "F9";
        }
    }
}
