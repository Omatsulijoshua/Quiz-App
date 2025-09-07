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
    public partial class past_questions_settings : Form
    {
        public past_questions_settings()
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

        private void past_questions_settings_Load(object sender, EventArgs e)
        {
            LoadExams();
            LoadGrid();
        }


        private void comboBoxExams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxExams.SelectedIndex != -1)
            {
                int selectedExamId = Convert.ToInt32(comboBoxExams.SelectedValue);
                //labelExamId.Text = selectedExamId.ToString(); // assuming you use a hidden label
            }
        }

        private void LoadExams()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                // ✅ Fetch exams in alphabetical order
                string query = "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBoxExams.DataSource = dt;
                comboBoxExams.DisplayMember = "ex_name";  // what user sees
                comboBoxExams.ValueMember = "ex_id";      // hidden value
                comboBoxExams.SelectedIndex = -1;         // no selection initially
            }
        }

        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            if (comboBoxExams.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an exam first.");
                return;
            }

            int examId = Convert.ToInt32(comboBoxExams.SelectedValue);

            // ✅ Determine value from radio buttons
            int enabledValue = radioButton1.Checked ? 1 : 0; // 1 if Yes selected, 0 if No

            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = "UPDATE tbl_exam_settings SET past_questions_enabled = @enabled WHERE ex_id = @examId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@enabled", enabledValue);
                    cmd.Parameters.AddWithValue("@examId", examId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Past question settings updated successfully.");

            // ✅ Refresh DataGridView immediately
            LoadGrid();
        }

        private void LoadGrid()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = @"
            SELECT e.ex_id, 
                   e.ex_name AS [Exam Name], 
                   CASE WHEN s.past_questions_enabled = 1 THEN 'Yes' ELSE 'No' END AS [Past Questions Enabled]
            FROM tbl_exams e
            LEFT JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
            ORDER BY e.ex_name ASC";   // ✅ Alphabetical order

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                // Hide exam_id column (optional)
                if (dataGridView1.Columns.Contains("ex_id"))
                    dataGridView1.Columns["ex_id"].Visible = false;
            }
        }
    }
}
