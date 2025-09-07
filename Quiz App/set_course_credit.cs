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
    public partial class set_course_credit : Form
    {
        public set_course_credit()
        {
            InitializeComponent();
        }

        private void set_course_credit_Load(object sender, EventArgs e)
        {
            LoadCourses();
            LoadDataGridView();
        }

        private void LoadCourses()
        {
            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();

                // ✅ Fetch exams in alphabetical order
                string query = "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "ex_name";
                comboBox1.ValueMember = "ex_id";

                // Prevent SelectedIndexChanged from firing too early
                comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                comboBox1.SelectedIndex = -1; // No selection initially
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            }
        }

        private void LoadDataGridView()
        {
            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();

                // ✅ Order exams alphabetically
                string query = @"
            SELECT e.ex_id, e.ex_name, s.unit
            FROM tbl_exams e
            LEFT JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
            ORDER BY e.ex_name ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                // Format grid
                dataGridView1.Columns["ex_id"].Visible = false;
                dataGridView1.Columns["ex_name"].HeaderText = "Course Name";
                dataGridView1.Columns["unit"].HeaderText = "Unit";
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    int examId = Convert.ToInt32(comboBox1.SelectedValue);
                    int unit = (int)numericUpDownDuration1.Value;

                    // Check if unit already set
                    string checkQuery = "SELECT COUNT(*) FROM tbl_exam_settings WHERE ex_id = @examId";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@examId", examId);
                    int exists = (int)checkCmd.ExecuteScalar();

                    string query;
                    if (exists > 0)
                    {
                        // Update existing
                        query = "UPDATE tbl_exam_settings SET unit = @unit WHERE ex_id = @examId";
                    }
                    else
                    {
                        // Insert new
                        query = "INSERT INTO tbl_exam_settings (ex_id, unit) VALUES (@examId, @unit)";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@examId", examId);
                    cmd.Parameters.AddWithValue("@unit", unit);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Course unit saved successfully!");

                    // Refresh DataGridView to show updated values
                    LoadDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
                return;

            if (!int.TryParse(comboBox1.SelectedValue.ToString(), out int examId))
                return;

            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();
                string query = "SELECT unit FROM tbl_exam_settings WHERE ex_id = @examId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@examId", examId);

                object result = cmd.ExecuteScalar();
                numericUpDownDuration1.Value = (result != null && result != DBNull.Value)
                    ? Convert.ToInt32(result)
                    : 0;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    

        private void LoadDataGrid()
        {
            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();

                // ✅ Fetch exams in alphabetical order
                string query = "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "ex_name";
                comboBox1.ValueMember = "ex_id";

                // Prevent SelectedIndexChanged from firing too early
                comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                comboBox1.SelectedIndex = -1; // No selection initially
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            }
        }

      

       

    }
}