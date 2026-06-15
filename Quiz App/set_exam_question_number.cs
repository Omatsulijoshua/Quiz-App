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
    public partial class set_exam_question_number : BaseForm
    {
        public set_exam_question_number()
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
        return_class rc = new return_class();
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

        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text.Trim(), out int setLimit) || setLimit <= 0)
            {
                MessageBox.Show("Please enter a valid number greater than 0.");
                return;
            }

            // Get exam ID from selected combo box
            if (!int.TryParse(comboBoxExams.SelectedValue?.ToString(), out int examId))
            {
                MessageBox.Show("Please select a valid exam.");
                return;
            }

            int availableCount = 0;

            // ? Get total available questions (tbl_questions + tbl_shortanswer)
            using (SqlConnection con = connection_class.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT 
            (SELECT COUNT(*) FROM tbl_questions WHERE ex_id_fk = @examId)
          + (SELECT COUNT(*) FROM tbl_shortanswer WHERE exam_id = @examId) AS TotalCount;", con))
            {
                cmd.Parameters.AddWithValue("@examId", examId);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    availableCount = Convert.ToInt32(result);
            }

            // Validate against available question count
            if (setLimit > availableCount)
            {
                DialogResult confirm = MessageBox.Show(
                    $"Only {availableCount} questions are available for this exam.\n" +
                    $"Do you want to use {availableCount} as the total?",
                    "Insufficient Questions", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    setLimit = availableCount;
                }
                else
                {
                    return;
                }
            }

            // ? Check if setting exists
            bool exists = false;
            using (SqlConnection con = connection_class.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_exam_settings WHERE ex_id = @examId", con))
            {
                cmd.Parameters.AddWithValue("@examId", examId);
                con.Open();
                exists = (int)cmd.ExecuteScalar() > 0;
            }

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                if (exists)
                {
                    // ? Update if record exists
                    string updateQuery = "UPDATE tbl_exam_settings SET total_questions = @total WHERE ex_id = @examId";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@total", setLimit);
                        cmd.Parameters.AddWithValue("@examId", examId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // ? Insert with duration_minutes fetched from tbl_exams (or default 0)
                    int durationMinutes = 0;

                    string fetchDuration = "SELECT ISNULL(duration_minutes, 60) FROM tbl_exam_settings WHERE ex_id = @examId";
                    using (SqlCommand cmd = new SqlCommand(fetchDuration, con))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        object dur = cmd.ExecuteScalar();
                        if (dur != null && dur != DBNull.Value)
                            durationMinutes = Convert.ToInt32(dur);
                    }

                    string insertQuery = "INSERT INTO tbl_exam_settings (ex_id, total_questions, duration_minutes) VALUES (@examId, @total, @duration)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        cmd.Parameters.AddWithValue("@total", setLimit);
                        cmd.Parameters.AddWithValue("@duration", durationMinutes);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Question limit saved successfully.");

            // ? Refresh DataGridView after save
            set_exam_question_number_Load(sender, e);
        }


        private void set_exam_question_number_Load(object sender, EventArgs e)
        {

            set_exam_question_number.ScaleForm(this);

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                // ? Fetch ALL exams (with or without settings)
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT e.ex_id, 
                   e.ex_name, 
                   ISNULL(s.total_questions, 0) AS total_questions
            FROM tbl_exams e
            LEFT JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
            ORDER BY e.ex_name ASC;", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                // ? Fill ComboBox (show exam name, use exam id internally)
                comboBoxExams.DataSource = dt;
                comboBoxExams.DisplayMember = "ex_name"; // what user sees
                comboBoxExams.ValueMember = "ex_id";     // what app uses
                comboBoxExams.SelectedIndex = -1;        // no pre-selection

                // ? Fill DataGridView with exam name + duration
                dataGridView1.DataSource = dt;

                // Optional: Hide ID, format headers
                if (dataGridView1.Columns.Contains("ex_id"))
                    dataGridView1.Columns["ex_id"].Visible = false;

                if (dataGridView1.Columns.Contains("ex_name"))
                    dataGridView1.Columns["ex_name"].HeaderText = "Exam Name";

                if (dataGridView1.Columns.Contains("total_questions"))
                    dataGridView1.Columns["total_questions"].HeaderText = "Total Questions";
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}

