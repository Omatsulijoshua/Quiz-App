// MasterSheetForm.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class MasterSheetForm : Form
    {
        private string selectedBatchCode;

        public MasterSheetForm(string batchCode)
        {
            InitializeComponent();
            selectedBatchCode = batchCode;
        }

        private void MasterSheetForm_Load(object sender, EventArgs e)
        {
            LoadMasterSheet();
            this.dataGridViewMaster.CellDoubleClick += dataGridViewMaster_CellDoubleClick;
        }

        private void LoadMasterSheet()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Student ID");
            table.Columns.Add("Name");
            table.Columns.Add("Batch");
            table.Columns.Add("Total Score");
            table.Columns.Add("Average (%)");
            table.Columns.Add("Remark");
            table.Columns.Add("Grade");
            table.Columns.Add("Position");

            var rankedStudents = new List<(int Id, string Name, string Batch, float AvgPercentage, int TotalScore)>();

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                string studentQuery = "SELECT std_id, std_name, std_batch_code FROM student_record WHERE std_batch_code = @batch";
                SqlCommand cmd = new SqlCommand(studentQuery, con);
                cmd.Parameters.AddWithValue("@batch", selectedBatchCode);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable students = new DataTable();
                adapter.Fill(students);

                foreach (DataRow row in students.Rows)
                {
                    int id = Convert.ToInt32(row["std_id"]);
                    string name = row["std_name"].ToString();
                    string batch = row["std_batch_code"].ToString();

                    float totalPercentage = 0;
                    int totalScore = 0;
                    int examCount = 0;

                    using (SqlCommand scoreCmd = new SqlCommand("SELECT score, percentage FROM score WHERE stud_fk_id = @id", con))
                    {
                        scoreCmd.Parameters.AddWithValue("@id", id);
                        SqlDataAdapter scoreAdapter = new SqlDataAdapter(scoreCmd);
                        DataTable scores = new DataTable();
                        scoreAdapter.Fill(scores);

                        foreach (DataRow scoreRow in scores.Rows)
                        {
                            totalPercentage += Convert.ToSingle(scoreRow["percentage"]);
                            totalScore += Convert.ToInt32(scoreRow["score"]);
                            examCount++;
                        }
                    }

                    float avgPercentage = examCount > 0 ? totalPercentage / examCount : 0;
                    rankedStudents.Add((id, name, batch, avgPercentage, totalScore));
                }
            }

            var sorted = rankedStudents.OrderByDescending(s => s.AvgPercentage).ToList();
            for (int pos = 0; pos < sorted.Count; pos++)
            {
                var s = sorted[pos];
                string remark = GetRemark(s.AvgPercentage);
                string grade = GetGrade(s.AvgPercentage);
                string positionStr = GetOrdinal(pos + 1);

                table.Rows.Add(s.Id, s.Name, s.Batch, s.TotalScore, s.AvgPercentage.ToString("F2"), remark, grade, positionStr);
            }

            dataGridViewMaster.DataSource = table;
            dataGridViewMaster.ClearSelection();
            dataGridViewMaster.Refresh();
            ApplyMasterSheetColor();
        }

        private void ApplyMasterSheetColor()
        {
            foreach (DataGridViewRow row in dataGridViewMaster.Rows)
            {
                row.DefaultCellStyle.BackColor = row.Index % 2 == 0 ? Color.LightBlue : Color.White;
                row.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
            dataGridViewMaster.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;
            dataGridViewMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewMaster.MultiSelect = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMasterSheet();
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

        private string GetOrdinal(int number)
        {
            if (number % 100 >= 11 && number % 100 <= 13) return number + "th";

            switch (number % 10)
            {
                case 1: return number + "st";
                case 2: return number + "nd";
                case 3: return number + "rd";
                default: return number + "th";
            }
        }

        private void dataGridViewMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridViewMaster.Rows[e.RowIndex].Cells["Student ID"].Value);
                string name = dataGridViewMaster.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                string position = dataGridViewMaster.Rows[e.RowIndex].Cells["Position"].Value.ToString();

                ReportCardForm report = new ReportCardForm(id, name, position);
                report.Show();
            }
        }
    }
}
