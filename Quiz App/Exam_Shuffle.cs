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
    public partial class Exam_Shuffle : BaseForm
    {
        public Exam_Shuffle()
        {
            InitializeComponent();
        }

        private const int BaseWidth = 1920;
        private const int BaseHeight = 1080;

        public static void ScaleForm(Form form)
        {
            // Get current screen resolution
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calculate scale factors
            float scaleX = (float)screenWidth / BaseWidth;
            float scaleY = (float)screenHeight / BaseHeight;

            // Apply scaling to form and controls
            form.Scale(new SizeF(scaleX, scaleY));

            // Adjust font scaling (optional, but makes UI balanced)
            foreach (Control c in form.Controls)
            {
                c.Font = new Font(c.Font.FontFamily, c.Font.Size * Math.Min(scaleX, scaleY));
            }

            // Center form
            form.StartPosition = FormStartPosition.CenterScreen;
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

            // ?? Refresh grid here
            RefreshGrid();

        }

        private void Exam_Shuffle_Load(object sender, EventArgs e)
        {
            Exam_Shuffle.ScaleForm(this);
            SqlConnection con = connection_class.GetConnection();

            // ? Fill ComboBox (exams)
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

            // ? Fill DataGridView
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
          INNER JOIN tbl_exams e ON s.ex_id = e.ex_id
          ORDER BY e.ex_name ASC", con))  // ? Alphabetical order
            {
                DataTable dtGrid = new DataTable();
                da.Fill(dtGrid);
                dataGridView1.DataSource = dtGrid;

                // Optional: make headers look better
                dataGridView1.Columns["ex_id"].Visible = false;
                dataGridView1.Columns["ex_name"].HeaderText = "Exam Name";
                dataGridView1.Columns["Shuffle"].HeaderText = "Shuffle Enabled";
            }
        }


    }
}

