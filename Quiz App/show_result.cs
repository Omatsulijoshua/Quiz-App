using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Quiz_App
{
    public partial class show_result : Form
    {
        public show_result()
        {
            InitializeComponent();
        }

        private void show_result_Load(object sender, EventArgs e)
        {
            SqlConnection con = connection_class.GetConnection();

            try
            {
                // Fill ComboBox (exams)
                using (SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxExams.DataSource = dt;
                    comboBoxExams.DisplayMember = "ex_name";
                    comboBoxExams.ValueMember = "ex_id";
                    comboBoxExams.SelectedIndex = -1;
                }

                // Load DataGridView
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            // Check which radio button is selected
            int showResult = radioButtonYes.Checked ? 1 : 0;

            // Get selected exam ID from ComboBox
            if (comboBoxExams.SelectedValue == null)
            {
                MessageBox.Show("Please select an exam.");
                return;
            }

            int examId = Convert.ToInt32(comboBoxExams.SelectedValue);

            // Update show_result in the database
            string query = "UPDATE tbl_exam_settings SET show_result = @showResult WHERE ex_id = @examId";

            SqlConnection con = connection_class.GetConnection();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@showResult", showResult);
                cmd.Parameters.AddWithValue("@examId", examId);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Show result setting saved.", "Success");

            

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

            Exam_Settings w = new Exam_Settings();
            w.Show();
            this.Hide();
        }
        private void LoadGrid()
        {
            SqlConnection con = connection_class.GetConnection();
            using (SqlDataAdapter da = new SqlDataAdapter(@"
        SELECT 
            e.ex_id, 
            e.ex_name, 
           
            CASE WHEN s.show_result = 1 THEN 'Yes' ELSE 'No' END AS ShowResult
        FROM tbl_exams e
        LEFT JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
        ORDER BY e.ex_name ASC", con))
            {
                DataTable dtGrid = new DataTable();
                da.Fill(dtGrid);

                dataGridView1.DataSource = dtGrid;

                // Hide and rename columns
                dataGridView1.Columns["ex_id"].Visible = false;
                dataGridView1.Columns["ex_name"].HeaderText = "Exam Name";
                //dataGridView1.Columns["Shuffle"].HeaderText = "Shuffle Enabled";
                dataGridView1.Columns["ShowResult"].HeaderText = "Show Result Enabled";
            }
        }

    }
}
