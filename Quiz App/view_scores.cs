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
    public partial class view_scores : Form
    {
        public view_scores()
        {
            InitializeComponent();
        }

        public string score { get; set; }


        private void view_scores_Load(object sender, EventArgs e)
        {
            BindData();         // This populates dataGridView2 (scores)
            BindStudentGrid();  // This populates and styles dataGridView1 (students)



            // TODO: This line of code loads data into the 'quizAppDataSet9.set_exam' table. You can move, or remove it, as needed.
            // this.set_examTableAdapter.Fill(this.quizAppDataSet9.set_exam);
            // TODO: This line of code loads data into the 'quizAppDataSet2.student_record' table. You can move, or remove it, as needed.
            // this.student_recordTableAdapter.Fill(this.quizAppDataSet2.student_record);

            BindStudentGrid(); // Already correctly written

            SqlDataAdapter dadapter = new SqlDataAdapter("select distinct std_batch_code from student_record", connection_class.GetConnection());
            DataSet dset = new DataSet();
            dadapter.Fill(dset);
            comboBox1.DataSource = dset.Tables[0];
            comboBox1.DisplayMember = "std_batch_code";
            comboBox1.ValueMember = "std_batch_code";

            // 🔁 Attach event handler after setting data
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;


        }

        void BindData()
        {
            SqlConnection con = connection_class.GetConnection();
            {
                con.Open();

                string sqlQuery = "SELECT * FROM score";
                SqlCommand command = new SqlCommand(sqlQuery, con);

                SqlDataAdapter sd = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sd.Fill(dt);

                dataGridView2.DataSource = dt;
                dataGridView2.BackgroundColor = Color.LightYellow;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView2.Font, FontStyle.Bold);
                dataGridView2.DefaultCellStyle.BackColor = Color.LightCyan;
                dataGridView2.DefaultCellStyle.ForeColor = Color.DarkBlue;
                dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridView2.EnableHeadersVisualStyles = false;

                dataGridView2.ClearSelection();
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = connection_class.GetConnection();
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Case 1: Delete selected rows if any are selected
                    if (dataGridView2.SelectedRows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to delete the selected scores?",
                            "Confirm Delete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result != DialogResult.Yes) return;

                        foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                        {
                            if (row.Cells["sCOREIDDataGridViewTextBoxColumn"].Value != null)
                            {
                                int scoreId = Convert.ToInt32(row.Cells["sCOREIDDataGridViewTextBoxColumn"].Value);
                                string deleteQuery = "DELETE FROM score WHERE SCORE_ID = @ScoreId";

                                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ScoreId", scoreId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Selected scores deleted successfully.");
                    }
                    // Case 2: No selection, but delete by typed score ID
                    else if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        int scoreId;
                        if (int.TryParse(textBox1.Text.Trim(), out scoreId))
                        {
                            DialogResult result = MessageBox.Show(
                                $"Are you sure you want to delete the score with ID {scoreId}?",
                                "Confirm Delete",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (result != DialogResult.Yes) return;

                            string deleteQuery = "DELETE FROM score WHERE SCORE_ID = @ScoreId";

                            using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ScoreId", scoreId);
                                int rows = cmd.ExecuteNonQuery();

                                if (rows == 0)
                                {
                                    MessageBox.Show("No score found with the provided ID.");
                                    transaction.Rollback();
                                    return;
                                }
                            }

                            transaction.Commit();
                            MessageBox.Show($"Score with ID {scoreId} deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid numeric Score ID.");
                            transaction.Rollback();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select rows in the table or type a Score ID to delete.");
                        transaction.Rollback();
                        return;
                    }

                    // Refresh DataGridView after deletion
                    // Refresh DataGridView after deletion
                    BindData(); // ✅ Correct refresh
                    dataGridView2.ClearSelection(); // ✅ Optional visual reset
                    textBox1.Clear(); // ✅ Clear Score ID input box


                    // Clear selection so highlight only appears when user selects rows manually
                    dataGridView2.ClearSelection();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred while deleting: " + ex.Message);
                }
            }
        }


        private void BindStudentGrid()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                string query = @"
            SELECT 
                sr.std_id AS [Student ID],
                sr.std_name AS [Student Name],
                sr.std_batch_code AS [Batch Code],
                e.ex_name AS [Exam Name],
                sc.score AS [Score],
                sc.percentage AS [Percentage]
            FROM student_record sr
            LEFT JOIN score sc ON sr.std_id = sc.stud_fk_id
            LEFT JOIN tbl_exams e ON sc.exam_fk_id = e.ex_id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns.Contains("Percentage"))
                    {
                        dataGridView1.Columns["Percentage"].DefaultCellStyle.Format = "0.00'%'";
                    }


                    // Apply styles
                    dataGridView1.BackgroundColor = Color.LightYellow;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                    dataGridView1.DefaultCellStyle.BackColor = Color.LightCyan;
                    dataGridView1.DefaultCellStyle.ForeColor = Color.DarkBlue;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.ClearSelection();
                }
            }
        }



        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form2 w = new Form2();
            w.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string scoreID = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(scoreID))
            {
                SqlConnection con = connection_class.GetConnection();
                {
                    con.Open();

                    string query = "SELECT * FROM score WHERE SCORE_ID = @ScoreID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ScoreID", scoreID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string score = reader["score"].ToString();
                                string percentage = reader["percentage"].ToString();

                                Print_Screen printScreen = new Print_Screen();
                                printScreen.UpdateData(scoreID, score, percentage);
                                printScreen.Show();
                            }
                            else
                            {
                                MessageBox.Show("No record found with the given score ID.");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please type in the score ID to be printed.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string scoreIDText = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(scoreIDText))
            {
                MessageBox.Show("Please enter a Score ID to delete.");
                return;
            }

            if (!int.TryParse(scoreIDText, out int scoreId))
            {
                MessageBox.Show("Score ID must be a valid number.");
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the score with ID {scoreId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            SqlConnection conn = connection_class.GetConnection();
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string deleteQuery = "DELETE FROM score WHERE SCORE_ID = @ScoreId";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ScoreId", scoreId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("No score found with the provided ID.");
                            transaction.Rollback();
                            return;
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Score deleted successfully.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred while deleting: " + ex.Message);
                }
            }

            // Refresh UI
            BindData();
            dataGridView2.ClearSelection();
            textBox1.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string searchBatchOrID = textBox2.Text.Trim();
            string searchExamName = textBox3.Text.Trim();   // now search by exam name
            string searchStudentName = textBox4.Text.Trim();

            // Base query
            string query = @"
SELECT 
    sr.std_id AS [Student ID],
    sr.std_name AS [Student Name],
    sr.std_batch_code AS [Batch Code],
    e.ex_name AS [Exam Name],
    sc.score AS [Score],
    sc.percentage AS [Percentage]
FROM student_record sr
LEFT JOIN score sc ON sr.std_id = sc.stud_fk_id
LEFT JOIN tbl_exams e ON sc.exam_fk_id = e.ex_id
WHERE 1 = 1";

            // Add filters if textboxes are not empty
            if (!string.IsNullOrEmpty(searchBatchOrID))
            {
                query += $" AND (sr.std_id LIKE '%{searchBatchOrID}%' OR sr.std_batch_code LIKE '%{searchBatchOrID}%')";
            }

            if (!string.IsNullOrEmpty(searchExamName))
            {
                query += $" AND e.ex_name LIKE '%{searchExamName.Replace("'", "''")}%'"; // prevent single quote errors
            }

            if (!string.IsNullOrEmpty(searchStudentName))
            {
                query += $" AND sr.std_name LIKE '%{searchStudentName.Replace("'", "''")}%'"; // prevent single quote errors
            }

            using (SqlConnection con = connection_class.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                // Optional: reapply formatting if needed
                if (dataGridView1.Columns.Contains("Percentage"))
                {
                    dataGridView1.Columns["Percentage"].DefaultCellStyle.Format = "0.00'%'";
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                string selectedBatch = comboBox1.SelectedValue.ToString();

                string query = @"
            SELECT 
                sr.std_id AS [Student ID],
                sr.std_name AS [Student Name],
                sr.std_batch_code AS [Batch Code],
                e.ex_name AS [Exam Name],
                sc.score AS [Score],
                sc.percentage AS [Percentage]
            FROM student_record sr
            LEFT JOIN score sc ON sr.std_id = sc.stud_fk_id
            LEFT JOIN tbl_exams e ON sc.exam_fk_id = e.ex_id
            WHERE sr.std_batch_code = @BatchCode";

                using (SqlConnection con = connection_class.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@BatchCode", selectedBatch);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Optional: Format percentage again
                    if (dataGridView1.Columns.Contains("Percentage"))
                    {
                        dataGridView1.Columns["Percentage"].DefaultCellStyle.Format = "0.00'%'";
                    }
                }
            }
        }


        private void ExportToExcel(DataGridView dataGridView1, string fileName)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = new DataTable("Sheet1");

                // Add headers
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    dt.Columns.Add(col.HeaderText);
                }

                // Add rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            dr[i] = row.Cells[i].Value ?? DBNull.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                }

                wb.Worksheets.Add(dt, "Export");

                // Save file to Documents
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    fileName + ".xlsx"
                );

                wb.SaveAs(path);

                MessageBox.Show("✅ Export successful!\nSaved at: " + path);
            }
        }

        private void ExportToExcel_2(DataGridView dataGridView2, string fileName)
        {
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = new DataTable("Sheet1");

                // Add headers
                foreach (DataGridViewColumn col in dataGridView2.Columns)
                {
                    dt.Columns.Add(col.HeaderText);
                }

                // Add rows
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < dataGridView2.Columns.Count; i++)
                        {
                            dr[i] = row.Cells[i].Value ?? DBNull.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                }

                wb.Worksheets.Add(dt, "Export");

                // Save file to Documents
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    fileName + ".xlsx"
                );

                wb.SaveAs(path);

                MessageBox.Show("✅ Export successful!\nSaved at: " + path);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ExportToExcel_2(dataGridView2, "ScoresExport1");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1, "ScoresExport2");
        }
    }
}
