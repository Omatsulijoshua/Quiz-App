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
    public partial class Set_Exam_Duration : BaseForm
    {
        public Set_Exam_Duration()
        {
            InitializeComponent(); // ? This is required to load your form controls
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
        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            if (comboBoxExams.SelectedValue == null)
            {
                MessageBox.Show("Please select an exam first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedExamId = Convert.ToInt32(comboBoxExams.SelectedValue);
            int duration = (int)numericUpDownDuration.Value;

            try
            {
                insertclass ins = new insertclass();
                ins.UpsertExamDuration(selectedExamId, duration);

                MessageBox.Show("Objective exam duration saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving exam duration:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Set_Exam_Duration_Load(object sender, EventArgs e)
        {
            ScaleForm(this);
            ModernUi.StyleComboBox(comboBoxExams);
            ModernUi.StyleComboBox(comboBox1);
            ModernUi.StyleNumericUpDown(numericUpDownDuration);
            ModernUi.StyleNumericUpDown(numericUpDown1);
            ModernUi.StylePrimaryButton(btnSaveDuration);
            saveTheoryDuration.FillColor = ModernUi.AccentAlt;
            saveTheoryDuration.ForeColor = Color.White;
            dataGridView1.AutoGenerateColumns = true;
            LoadGrid();
            comboBoxExams.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
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
                con.Open();
                string theoryColumnName = ResolveTheoryDurationColumn(con);
                string theoryColumn = string.IsNullOrWhiteSpace(theoryColumnName)
                    ? "CAST(NULL AS INT)"
                    : "s." + theoryColumnName;

                string query = @"
                SELECT e.ex_id, e.ex_name,
                       s.duration_minutes,
                       " + theoryColumn + @" AS theory_duration_value
                FROM tbl_exams e
                LEFT JOIN tbl_exam_settings s ON e.ex_id = s.ex_id
                ORDER BY e.ex_name ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // ? ComboBox for Objective
                comboBoxExams.DataSource = dt.Copy();
                comboBoxExams.DisplayMember = "ex_name";
                comboBoxExams.ValueMember = "ex_id";

                // ? ComboBox for Theory
                comboBox1.DataSource = dt.Copy();
                comboBox1.DisplayMember = "ex_name";
                comboBox1.ValueMember = "ex_id";

                // ? DataGridView setup
                dataGridView1.DataSource = dt;
                if (dataGridView1.Columns.Contains("ex_id"))
                    dataGridView1.Columns["ex_id"].Visible = false;

                if (dataGridView1.Columns.Contains("ex_name"))
                    dataGridView1.Columns["ex_name"].HeaderText = "Exam Name";

                if (dataGridView1.Columns.Contains("duration_minutes"))
                    dataGridView1.Columns["duration_minutes"].HeaderText = "Objective Duration (mins)";

                if (dataGridView1.Columns.Contains("theory_duration_value"))
                    dataGridView1.Columns["theory_duration_value"].HeaderText = "Theory Duration (mins)";

                ModernUi.StyleDataGridView(dataGridView1);
                dataGridView1.ClearSelection();
            }
        }


        private void saveTheoryDuration_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select an exam first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedExamId = Convert.ToInt32(comboBox1.SelectedValue);
            int theoryDuration = (int)numericUpDown1.Value;

            try
            {
                insertclass ins = new insertclass();
                ins.UpsertTheoryDuration(selectedExamId, theoryDuration);

                MessageBox.Show("Theory exam duration saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving theory duration:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpsertExamDuration(int examId, int duration)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = @"
        IF EXISTS (SELECT 1 FROM tbl_exam_settings WHERE ex_id = @ex_id)
            UPDATE tbl_exam_settings SET duration_minutes = @duration WHERE ex_id = @ex_id
        ELSE
            INSERT INTO tbl_exam_settings (ex_id, duration_minutes) VALUES (@ex_id, @duration)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ex_id", examId);
                    cmd.Parameters.AddWithValue("@duration", duration);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpsertTheoryDuration(int examId, int theoryDuration)
        {
            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();

                string theoryColumn = ResolveTheoryDurationColumn(conn);

                if (string.IsNullOrWhiteSpace(theoryColumn))
                {
                    MessageBox.Show("Theory duration column was not found in tbl_exam_settings.", "Schema Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = @"
        IF EXISTS (SELECT 1 FROM tbl_exam_settings WHERE ex_id = @examId)
            UPDATE tbl_exam_settings
            SET " + theoryColumn + @" = @theoryDuration
            WHERE ex_id = @examId;
        ELSE
            INSERT INTO tbl_exam_settings (ex_id, " + theoryColumn + @")
            VALUES (@examId, @theoryDuration);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    cmd.Parameters.AddWithValue("@theoryDuration", theoryDuration);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool ColumnExists(SqlConnection connection, string tableName, string columnName)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME = @column",
                connection))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private string ResolveTheoryDurationColumn(SqlConnection connection)
        {
            if (ColumnExists(connection, "tbl_exam_settings", "theory_duration_minutes"))
            {
                return "theory_duration_minutes";
            }

            if (ColumnExists(connection, "tbl_exam_settings", "theory_duration"))
            {
                return "theory_duration";
            }

            return null;
        }


    }
}
