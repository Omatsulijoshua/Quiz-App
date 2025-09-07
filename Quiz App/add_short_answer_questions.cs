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
using System.IO;


namespace Quiz_App
{
    public partial class add_short_answer_questions : Form
    {
        public add_short_answer_questions()
        {
            InitializeComponent();
        }

        private void add_short_answer_questions_Load(object sender, EventArgs e)
        {
            label9.Text = "";
            LoadExams();
            LoadShortAnswers(); // 
        }
        // class-level variables (so all buttons can use them)
        private byte[] imageData = null;       // holds the image in memory
        private int selectedQuestionId = -1;   // used for update/delete
        private int selectedSaId = -1;  // keep track of the row's primary key


        private void ClearFields()
        {
            txtQuestion.Clear();
            txtShortAnswer.Clear();
            pictureBox1.Image = null;
            imageData = null;
            selectedQuestionId = -1;
        }

        private void add_question_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestion.Text) || string.IsNullOrWhiteSpace(txtShortAnswer.Text) || string.IsNullOrWhiteSpace(label9.Text))
            {
                MessageBox.Show("Please fill all fields and select an exam.");
                return;
            }

            using (SqlConnection conn = connection_class.GetConnection())
            {
                string query = "INSERT INTO tbl_shortanswer (exam_id, ques_title, correct_answer, ques_image) " +
                               "VALUES (@examId, @title, @answer, @img)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@examId", int.Parse(label9.Text));
                cmd.Parameters.AddWithValue("@title", txtQuestion.Text);
                cmd.Parameters.AddWithValue("@answer", txtShortAnswer.Text);

                if (imageData != null)
                    cmd.Parameters.AddWithValue("@img", imageData);
                else
                    cmd.Parameters.Add("@img", SqlDbType.VarBinary).Value = DBNull.Value;

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Question Added Successfully!");
            ClearFields();

            // refresh the grid based on the currently selected exam
            if (int.TryParse(label9.Text, out int examId))
            {
                LoadShortAnswers(examId);
            }
            else
            {
                LoadShortAnswers(); // fallback to load all
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (selectedSaId == -1)
            {
                MessageBox.Show("Please select a record first.");
                return;
            }

            int examId = Convert.ToInt32(comboBox2.SelectedValue);
            string question = txtQuestion.Text.Trim();
            string answer = txtShortAnswer.Text.Trim();

            byte[] imgData = null;
            if (pictureBox1.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    imgData = ms.ToArray();
                }
            }

            using (SqlConnection conn = connection_class.GetConnection())
            {
                string query = "UPDATE tbl_shortanswer SET exam_id=@exam_id, ques_title=@title, correct_answer=@answer, ques_image=@img WHERE sa_id=@id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@exam_id", examId);
                    cmd.Parameters.AddWithValue("@title", question);
                    cmd.Parameters.AddWithValue("@answer", answer);

                    if (imgData == null)
                        cmd.Parameters.AddWithValue("@img", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@img", imgData);

                    cmd.Parameters.AddWithValue("@id", selectedSaId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            MessageBox.Show("Record updated successfully.");
            LoadShortAnswers(); // reload grid
            selectedSaId = -1; // reset selection
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (selectedQuestionId <= 0)
            {
                MessageBox.Show("Please select a question to delete.");
                return;
            }

            using (SqlConnection conn = connection_class.GetConnection())
            {
                string query = "DELETE FROM tbl_shortanswer WHERE sa_id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedQuestionId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Question Deleted Successfully!");
            ClearFields();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            imageData = null;
            pictureBox1.Image = null;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imageData = File.ReadAllBytes(ofd.FileName);
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }




       



        private void LoadExams()
        {
            using (SqlConnection conn = connection_class.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name", conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                // prevent event from firing mid-bind
                comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "ex_name";
                comboBox2.ValueMember = "ex_id";
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

                comboBox2.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;

                comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            }

            // set label once after initial bind
            UpdateExamIdLabel();
        }
        private void UpdateExamIdLabel()
        {
            // Handle all cases (DataRowView during binding, normal SelectedValue after)
            if (comboBox2.SelectedItem is DataRowView drv && drv.Row.Table.Columns.Contains("ex_id"))
            {
                label9.Text = Convert.ToString(drv["ex_id"]);
            }
            else if (comboBox2.SelectedValue != null && comboBox2.SelectedValue != DBNull.Value)
            {
                label9.Text = comboBox2.SelectedValue.ToString();
            }
            else
            {
                label9.Text = "";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateExamIdLabel();

            if (int.TryParse(label9.Text, out int examId))
            {
                LoadShortAnswers(examId); // reload grid with filter
            }
        }


        private void LoadShortAnswers(int? examId = null)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    string query = "SELECT sa_id, exam_id, ques_title, correct_answer, ques_image FROM tbl_shortanswer";

                    if (examId.HasValue) // filter by exam if provided
                        query += " WHERE exam_id = @examId";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    if (examId.HasValue)
                        da.SelectCommand.Parameters.AddWithValue("@examId", examId.Value);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Create a new table with Image type for ques_image
                    DataTable displayTable = new DataTable();
                    displayTable.Columns.Add("sa_id", typeof(int));
                    displayTable.Columns.Add("exam_id", typeof(int));
                    displayTable.Columns.Add("ques_title", typeof(string));
                    displayTable.Columns.Add("correct_answer", typeof(string));
                    displayTable.Columns.Add("ques_image", typeof(Image));

                    foreach (DataRow dr in dt.Rows)
                    {
                        Image img = null;
                        if (dr["ques_image"] != DBNull.Value)
                        {
                            byte[] bytes = (byte[])dr["ques_image"];
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                img = Image.FromStream(ms);
                            }
                        }
                        displayTable.Rows.Add(
                            dr["sa_id"],
                            dr["exam_id"],
                            dr["ques_title"],
                            dr["correct_answer"],
                            img
                        );
                    }

                    dataGridView1.DataSource = displayTable;

                    // Convert image column style
                    if (dataGridView1.Columns["ques_image"] is DataGridViewImageColumn imgCol)
                    {
                        imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom; // thumbnail style
                    }

                    // Adjust headers
                    dataGridView1.Columns["sa_id"].HeaderText = "ID";
                    dataGridView1.Columns["exam_id"].HeaderText = "Exam ID";
                    dataGridView1.Columns["ques_title"].HeaderText = "Question";
                    dataGridView1.Columns["correct_answer"].HeaderText = "Answer";

                    // Make rows tall enough for thumbnail
                    dataGridView1.RowTemplate.Height = 80;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }


        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Save the sa_id for update
                if (row.Cells["sa_id"].Value != null)
                {
                    selectedSaId = Convert.ToInt32(row.Cells["sa_id"].Value);
                }

                // Copy text fields
                txtQuestion.Text = row.Cells["ques_title"].Value?.ToString();
                txtShortAnswer.Text = row.Cells["correct_answer"].Value?.ToString();
                label9.Text = row.Cells["exam_id"].Value?.ToString(); // exam_id

                // Copy image safely
                if (row.Cells["ques_image"].Value != null && row.Cells["ques_image"].Value != DBNull.Value)
                {
                    if (row.Cells["ques_image"].Value is Image)
                    {
                        pictureBox1.Image = (Image)row.Cells["ques_image"].Value;
                    }
                    else if (row.Cells["ques_image"].Value is byte[])
                    {
                        byte[] imgBytes = (byte[])row.Cells["ques_image"].Value;
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            question_type se = new question_type();
            se.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Please select an exam first.");
                return;
            }

            int examId = Convert.ToInt32(comboBox2.SelectedValue);

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    // 1️⃣ Insert all questions for this exam into tbl_past_shortanswer
                    string insertQuery = @"
                INSERT INTO tbl_past_shortanswer (exam_id, ques_title, correct_answer, ques_image)
                SELECT exam_id, ques_title, correct_answer, ques_image
                FROM tbl_shortanswer
                WHERE exam_id = @examId";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        int rowsMoved = cmd.ExecuteNonQuery();

                        MessageBox.Show($"✅ {rowsMoved} question(s) moved to Past Questions.");
                    }

                    // 2️⃣ (Optional) Delete from tbl_shortanswer after moving
                    string deleteQuery = "DELETE FROM tbl_shortanswer WHERE exam_id = @examId";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@examId", examId);
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                // Refresh your DataGridView so the user sees the update
                //LoadQuestionsForExam(examId);
               // LoadExams();
                LoadShortAnswers(examId); // 
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error moving questions: " + ex.Message);
            }
        }

        private void btnDownloadSample_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "Sample_ShortAnswer.csv",
                Title = "Save Sample Short Answer File"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();

                // Add headers
                sb.AppendLine("ques_title,correct_answer,ques_image");

                // Add sample rows
                sb.AppendLine("What is 2 + 2?,4,");
                sb.AppendLine("Capital of France,Paris,");
                sb.AppendLine("Identify this object,Answer goes here,image_path_or_leave_blank");

                File.WriteAllText(saveFileDialog.FileName, sb.ToString());

                MessageBox.Show("✅ Sample file created: " + saveFileDialog.FileName, "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnImportQuestions_Click_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                Title = "Select Excel/CSV File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(openFileDialog.FileName);

                    if (lines.Length <= 1)
                    {
                        MessageBox.Show("The file is empty or missing data.");
                        return;
                    }

                    // Skip header row
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] values = lines[i].Split(',');

                        if (values.Length < 2) continue;

                        string ques_title = values[0].Trim();
                        string correct_answer = values[1].Trim();
                        string ques_image = values.Length > 2 ? values[2].Trim() : null;

                        using (SqlConnection con = connection_class.GetConnection())
                        {
                            con.Open();
                            string query = "INSERT INTO tbl_shortanswer (ques_title, correct_answer, ques_image) VALUES (@title, @answer, @image)";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@title", ques_title);
                                cmd.Parameters.AddWithValue("@answer", correct_answer);
                                cmd.Parameters.AddWithValue("@image", (object)ques_image ?? DBNull.Value);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    MessageBox.Show("✅ Questions added successfully from Excel/CSV!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Error importing file: " + ex.Message);
                }
            }
        }
    }
    }




   
    



