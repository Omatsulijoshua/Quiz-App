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
    public partial class Set_Exam_Duration : Form
    {
        public Set_Exam_Duration()
        {
            InitializeComponent(); // ← This is required to load your form controls
        }



        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            int selectedExamId = Convert.ToInt32(comboBoxExams.SelectedValue);
            int duration = (int)numericUpDownDuration.Value;

            insertclass ins = new insertclass();
            ins.UpsertExamDuration(selectedExamId, duration);

            MessageBox.Show("Exam duration saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 🔄 Refresh DataGridView
            LoadGrid();

        }

        private void Set_Exam_Duration_Load(object sender, EventArgs e)
        {
            LoadGrid();
            comboBoxExams.SelectedIndex = -1; // optional: no pre-selection
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Exam_Settings w = new Exam_Settings();
            w.Show();
            this.Hide();
        }

        private void LoadGrid()
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = @"
            SELECT e.ex_id, e.ex_name, s.duration_minutes
            FROM tbl_exams e
            INNER JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
            ORDER BY e.ex_name ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // ✅ Fill ComboBox
                comboBoxExams.DataSource = dt.Copy();
                comboBoxExams.DisplayMember = "ex_name";
                comboBoxExams.ValueMember = "ex_id";

                // ✅ Fill DataGridView
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["ex_id"].Visible = false;
                dataGridView1.Columns["ex_name"].HeaderText = "Exam Name";
                dataGridView1.Columns["duration_minutes"].HeaderText = "Duration (mins)";
            }
        }

    }

}
