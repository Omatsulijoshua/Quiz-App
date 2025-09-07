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
    public partial class show_calculator_scorecs : Form
    {
        public show_calculator_scorecs()
        {
            InitializeComponent();
        }

        return_class rc = new return_class();

        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            int showScore = rbScoreYes.Checked ? 1 : 0;

            if (cmbShowScoreExam.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid subject.");
                return;
            }

            int selectedSubjectId = Convert.ToInt32(cmbShowScoreExam.SelectedValue);

            SqlConnection con = connection_class.GetConnection();
            {
                string query = "UPDATE tbl_exam_settings SET show_score = @show_score WHERE ex_id = @ex_id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@show_score", showScore);
                    cmd.Parameters.AddWithValue("@ex_id", selectedSubjectId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            MessageBox.Show("Score display setting updated.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int showCalc = rbCalcYes.Checked ? 1 : 0;

            if (cmbShowCalcExam.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid subject.");
                return;
            }

            int selectedSubjectId = Convert.ToInt32(cmbShowCalcExam.SelectedValue);

            SqlConnection con = connection_class.GetConnection();
            {
                string query = "UPDATE tbl_exam_settings SET show_calculator = @show_calc WHERE ex_id = @ex_id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@show_calc", showCalc);
                    cmd.Parameters.AddWithValue("@ex_id", selectedSubjectId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            MessageBox.Show("Calculator display setting updated.");
        }

        private void show_calculator_scorecs_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGrid(); // just call the method
                cmbShowScoreExam.SelectedIndex = -1;
                cmbShowCalcExam.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading exams: " + ex.Message);
            }

        }

        private void radioButtonYes_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Exam_Settings w = new Exam_Settings();
            w.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadGrid()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = @"
            SELECT e.ex_id, e.ex_name, 
                   CASE WHEN ISNULL(s.show_score, 0) = 1 THEN 'Yes' ELSE 'No' END AS show_score,
                   CASE WHEN ISNULL(s.show_calculator, 0) = 1 THEN 'Yes' ELSE 'No' END AS show_calculator
            FROM tbl_exams e
            LEFT JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
            ORDER BY e.ex_name ASC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // ✅ Bind to DataGridView
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["ex_id"].Visible = false;
                dataGridView1.Columns["ex_name"].HeaderText = "Exam Name";
                dataGridView1.Columns["show_score"].HeaderText = "Show Score";
                dataGridView1.Columns["show_calculator"].HeaderText = "Show Calculator";

                // ✅ Re-bind ComboBoxes
                cmbShowScoreExam.DataSource = dt.Copy();
                cmbShowScoreExam.DisplayMember = "ex_name";
                cmbShowScoreExam.ValueMember = "ex_id";

                cmbShowCalcExam.DataSource = dt.Copy();
                cmbShowCalcExam.DisplayMember = "ex_name";
                cmbShowCalcExam.ValueMember = "ex_id";
            }
        }

    }
}
