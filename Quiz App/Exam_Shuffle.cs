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
    public partial class Exam_Shuffle : Form
    {
        public Exam_Shuffle()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

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

        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            int shuffle = radioButtonYes.Checked ? 1 : 0;
            int examId = Convert.ToInt32(comboBoxExams.SelectedValue);

            string query = "UPDATE tbl_exam_settings SET shuffle = @shuffle WHERE ex_id = @examId";

            using (SqlConnection con = connection_class.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@shuffle", shuffle);
                cmd.Parameters.AddWithValue("@examId", examId);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Shuffle setting saved successfully.", "Saved");

            // 🔄 Refresh grid here
            RefreshGrid();

        }

        private void Exam_Shuffle_Load(object sender, EventArgs e)
        {
            SqlConnection con = connection_class.GetConnection();

            // ✅ Fill ComboBox (exams)
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT ex_id, ex_name FROM tbl_exams", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataView dv = new DataView(dt);
                dv.Sort = "ex_name ASC";

                comboBoxExams.DataSource = dv;
                comboBoxExams.DisplayMember = "ex_name";
                comboBoxExams.ValueMember = "ex_id";
                comboBoxExams.SelectedIndex = -1;
            }

            // ✅ Fill DataGridView
            RefreshGrid();

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "shuffle" && e.Value != null)
            {
                e.Value = (Convert.ToInt32(e.Value) == 1) ? "Yes" : "No";
                e.FormattingApplied = true;
            }
        }
        private void RefreshGrid()
        {
            using (SqlConnection con = connection_class.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT e.ex_id, e.ex_name, 
                 CASE WHEN s.shuffle = 1 THEN 'Yes' ELSE 'No' END AS Shuffle
          FROM tbl_exam_settings s
          INNER JOIN tbl_exams e ON s.ex_id = e.ex_id", con))
            {
                DataTable dtGrid = new DataTable();
                da.Fill(dtGrid);
                dataGridView1.DataSource = dtGrid;
            }
        }


    }
}
