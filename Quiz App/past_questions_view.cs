using ClosedXML.Excel;
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
using System.IO;


namespace Quiz_App
{
    public partial class past_questions_view : Form
    {
        public past_questions_view()
        {
            InitializeComponent();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form4 stt = new Form4();
            stt.Show();
            this.Hide();
        }

        private void past_questions_view_Load(object sender, EventArgs e)
        {
            
        }


        private void BindPastQuestionsGrid()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                string query = @"
SELECT 
    pq.ques_id AS [Question ID],
    pq.q_title AS [Question Title],
    pq.q_opA AS [Option A],
    pq.q_opB AS [Option B],
    pq.q_opC AS [Option C],
    pq.q_opD AS [Option D],
    pq.q_correctOpn AS [Correct Option],
    pq.q_correctDate AS [Date Added],
    e.ex_name AS [Exam Name]
FROM tbl_past_questions pq
LEFT JOIN tbl_exams e ON pq.ex_id_fk = e.ex_id
WHERE 1=1";

                SqlCommand cmd = new SqlCommand(query, con);

                // Filter by exam ID if label8 has text
                if (!string.IsNullOrEmpty(label8.Text))
                {
                    cmd.CommandText += " AND pq.ex_id_fk = @ExamID";
                    cmd.Parameters.AddWithValue("@ExamID", label8.Text);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                // Optional: format the columns
                if (dataGridView1.Columns["Date Added"] != null)
                {
                    dataGridView1.Columns["Date Added"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                    dataGridView1.Columns["Date Added"].Width = 100;
                }

                if (dataGridView1.Columns["Question ID"] != null)
                {
                    dataGridView1.Columns["Question ID"].Width = 50;
                }

                dataGridView1.ClearSelection();
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                label8.Text = comboBox1.SelectedValue.ToString();
                BindPastQuestionsGrid();
            }
        }

        private void LoadExamsComboBox()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                // ✅ Order exams alphabetically by name
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No exams found in the database!");
                    return;
                }

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "ex_name"; // visible name
                comboBox1.ValueMember = "ex_id";     // underlying value
                comboBox1.SelectedIndex = -1;        // optional
            }

            // Re-wire event handler
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }


        private void past_questions_view_Load_1(object sender, EventArgs e)
        {
            LoadExamsComboBox();      // populate ComboBox once
            BindPastQuestionsGrid();  // load all questions initially
        }

        private void ExportToExcel(DataGridView dgv, string fileName)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = new DataTable("PastQuestions");

                // Add columns from DataGridView
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    dt.Columns.Add(col.HeaderText);
                }

                // Add rows
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                // Convert DateTime to string to preserve formatting
                                if (row.Cells[i].Value is DateTime dtValue)
                                    dr[i] = dtValue.ToString("dd-MMM-yyyy");
                                else
                                    dr[i] = row.Cells[i].Value;
                            }
                            else
                            {
                                dr[i] = DBNull.Value;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }

                wb.Worksheets.Add(dt, "Past Questions");

                // Save to Documents folder
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName + ".xlsx");
                wb.SaveAs(path);

                MessageBox.Show("✅ Data exported successfully to: " + path, "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1, "PastQuestions_" + DateTime.Now.ToString("yyyyMMdd_HHmm"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select an exam first.", "Delete Questions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm deletion
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete ALL questions for the selected exam?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            int selectedExamId = Convert.ToInt32(comboBox1.SelectedValue);

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    string deleteQuery = "DELETE FROM tbl_past_questions WHERE ex_id_fk = @ExamId";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ExamId", selectedExamId);
                        int rowsDeleted = cmd.ExecuteNonQuery();
                        transaction.Commit();

                        MessageBox.Show($"{rowsDeleted} question(s) deleted successfully!", "Delete Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh DataGridView
                        BindPastQuestionsGrid();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error deleting questions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select an exam first.", "Reverse Questions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to move ALL questions for the selected exam back to the active questions table?",
                "Confirm Reverse",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            int selectedExamId = Convert.ToInt32(comboBox1.SelectedValue);

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // 1️⃣ Insert questions back into tbl_questions
                    string insertQuery = @"
INSERT INTO tbl_questions
    (q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_correctDate, ad_id_fk, ex_id_fk, q_image)
SELECT q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_correctDate, ad_id_fk, ex_id_fk, q_image
FROM tbl_past_questions
WHERE ex_id_fk = @ExamId";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ExamId", selectedExamId);
                        cmd.ExecuteNonQuery();
                    }

                    // 2️⃣ Delete the questions from tbl_past_questions
                    string deleteQuery = "DELETE FROM tbl_past_questions WHERE ex_id_fk = @ExamId";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ExamId", selectedExamId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Questions reversed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh DataGridView
                    BindPastQuestionsGrid();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error reversing questions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
