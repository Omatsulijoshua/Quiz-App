using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Table;




namespace Quiz_App
{
    public partial class pas_question_view2 : Form
    {
        public pas_question_view2()
        {
            InitializeComponent();
            this.comboBoxExams.SelectionChangeCommitted += new System.EventHandler(this.comboBoxExams_SelectionChangeCommitted);

        }

        private void pas_question_view2_Load(object sender, EventArgs e)
        {
            LoadExams();   // ✅ first load exams into comboBox
            label8.Text = ""; // clear exam id label at start
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadQuestions(int examId)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = @"
            SELECT sa_id, 
                   ques_title AS [Question], 
                   correct_answer AS [Correct Answer],
                   ques_image
            FROM tbl_past_shortanswer
            WHERE exam_id = @examId";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@examId", examId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Contains("ques_image"))
                    dataGridView1.Columns["ques_image"].Visible = false;

                if (dataGridView1.Columns.Contains("sa_id"))
                    dataGridView1.Columns["sa_id"].HeaderText = "ID";
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Only act if a valid row is clicked
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string question = row.Cells["Question"].Value?.ToString();
                string answer = row.Cells["Correct Answer"].Value?.ToString();

                // Example: show details in a MessageBox
                MessageBox.Show($"Question: {question}\nAnswer: {answer}", "Details");
            }
        } 

        private void LoadExams()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBoxExams.DataSource = dt;
                comboBoxExams.DisplayMember = "ex_name"; // show exam name
                comboBoxExams.ValueMember = "ex_id";     // keep exam id
                comboBoxExams.SelectedIndex = -1;        // no selection at first
            }
        }

        private void comboBoxExams_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxExams.SelectedValue != null)
            {
                int examId = Convert.ToInt32(comboBoxExams.SelectedValue);
                label8.Text = examId.ToString();
                LoadQuestions(examId);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ExportToCsv(dataGridView1, "PastQuestions");



        }

        private void ExportToCsv(DataGridView dgv, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            // Add column headers
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                sb.Append(dgv.Columns[i].HeaderText);
                if (i < dgv.Columns.Count - 1) sb.Append(",");
            }
            sb.AppendLine();

            // Add rows
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow)
                {
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        sb.Append(row.Cells[i].Value?.ToString().Replace(",", " "));
                        if (i < dgv.Columns.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine();
                }
            }

            // Save file
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName + ".csv");
            File.WriteAllText(path, sb.ToString());

            MessageBox.Show("✅ Data exported successfully to: " + path);
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a question to reverse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get values from the selected row
                DataGridViewRow row = dataGridView1.CurrentRow;

                string question = row.Cells["Question"].Value?.ToString();
                string answer = row.Cells["Correct Answer"].Value?.ToString();

                // exam id comes from label8 (you already set it when selecting exam)
                int examId = int.Parse(label8.Text);

                // Image (optional, check if column exists)
                byte[] imageBytes = null;
                if (row.Cells["ques_image"] != null && row.Cells["ques_image"].Value != DBNull.Value)
                {
                    imageBytes = (byte[])row.Cells["ques_image"].Value;
                }

                // Insert into tbl_shortanswer
                using (SqlConnection con = connection_class.GetConnection())
                {
                    string insertQuery = @"
                INSERT INTO tbl_shortanswer (exam_id, ques_title, correct_answer, ques_image)
                VALUES (@examId, @ques_title, @correct_answer, @ques_image)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        cmd.Parameters.AddWithValue("@ques_title", question);
                        cmd.Parameters.AddWithValue("@correct_answer", answer);

                        if (imageBytes != null)
                            cmd.Parameters.AddWithValue("@ques_image", imageBytes);
                        else
                            cmd.Parameters.AddWithValue("@ques_image", DBNull.Value);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                MessageBox.Show("✅ Question reversed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Reverse Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBoxExams.SelectedValue == null)
            {
                MessageBox.Show("Please select an exam first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int examId = Convert.ToInt32(comboBoxExams.SelectedValue);

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete all questions for this exam?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = connection_class.GetConnection())
                    {
                        string deleteQuery = "DELETE FROM tbl_shortanswer WHERE exam_id = @examId";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@examId", examId);

                            con.Open();
                            int rows = cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show($"{rows} question(s) deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    // Refresh the grid
                    LoadQuestions(examId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
