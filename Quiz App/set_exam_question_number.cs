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
    public partial class set_exam_question_number : Form
    {
        public set_exam_question_number()
        {
            InitializeComponent();
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

            // ✅ Get total available questions (tbl_questions + tbl_short_questions) in one query
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

            // ✅ Update the setting in the database
            string query = "UPDATE tbl_exam_settings SET total_questions = @total WHERE ex_id = @examId";
            using (SqlConnection con = connection_class.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@total", setLimit);
                cmd.Parameters.AddWithValue("@examId", examId);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Question limit saved successfully.");
        }


        private void set_exam_question_number_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                // ✅ Fetch exam name + duration from tbl_exams and tbl_exam_settings
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT e.ex_id, e.ex_name, s.total_questions
            FROM tbl_exams e
            INNER JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
            ORDER BY e.ex_name ASC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                // ✅ Fill ComboBox (show exam name, use exam id internally)
                comboBoxExams.DataSource = dt;
                comboBoxExams.DisplayMember = "ex_name"; // what user sees
                comboBoxExams.ValueMember = "ex_id";     // what app uses
                comboBoxExams.SelectedIndex = -1;        // no pre-selection

                // ✅ Fill DataGridView with exam name + duration
                dataGridView1.DataSource = dt;

                // Optional: Hide ID, format headers
                dataGridView1.Columns["ex_id"].Visible = false;
                dataGridView1.Columns["ex_name"].HeaderText = "Exam Name";
                dataGridView1.Columns["total_questions"].HeaderText = "Total Qestions)";
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
