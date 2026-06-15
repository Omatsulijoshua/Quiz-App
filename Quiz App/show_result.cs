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
    public partial class show_result : BaseForm
    {
        public show_result()
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
        private void show_result_Load(object sender, EventArgs e)
        {

            show_result.ScaleForm(this);
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

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                // ? Check if record exists
                bool exists;
                using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tbl_exam_settings WHERE ex_id = @examId", con))
                {
                    checkCmd.Parameters.AddWithValue("@examId", examId);
                    exists = (int)checkCmd.ExecuteScalar() > 0;
                }

                if (exists)
                {
                    // ? Update if record exists
                    string updateQuery = "UPDATE tbl_exam_settings SET show_result = @showResult WHERE ex_id = @examId";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@showResult", showResult);
                        cmd.Parameters.AddWithValue("@examId", examId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // ? Insert a new record with default values for other NOT NULL fields
                    // fetch duration if needed, else default to 0
                    int durationMinutes = 0;
                    string fetchDuration = "SELECT ISNULL(duration_minutes, 60) FROM tbl_exam_settings WHERE ex_id = @examId";
                    using (SqlCommand durCmd = new SqlCommand(fetchDuration, con))
                    {
                        durCmd.Parameters.AddWithValue("@examId", examId);
                        object dur = durCmd.ExecuteScalar();
                        if (dur != null && dur != DBNull.Value)
                            durationMinutes = Convert.ToInt32(dur);
                    }

                    string insertQuery = "INSERT INTO tbl_exam_settings (ex_id, total_questions, duration_minutes, show_result) VALUES (@examId, 0, @duration, @showResult)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        cmd.Parameters.AddWithValue("@duration", durationMinutes);
                        cmd.Parameters.AddWithValue("@showResult", showResult);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Show result setting saved.", "Success");

            // ? Refresh DataGridView
            LoadGrid();
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

