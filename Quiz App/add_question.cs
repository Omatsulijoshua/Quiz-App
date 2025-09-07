using ClosedXML.Excel;
using ExcelDataReader;
using OfficeOpenXml; // Add at the top of your file
using OfficeOpenXml.Style;
using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Quiz_App
{
    public partial class add_question : Form
    {
        public add_question()
        {
            InitializeComponent();
        }
        private int selectedQuestionId = -1;  // class level variable
        private void add_question_Load(object sender, EventArgs e)
        {
            this.tbl_examsTableAdapter.Fill(this.quizAppDataSet1.tbl_exams);

            // ✅ Sort exams alphabetically by exam_name
            DataView dv = new DataView(this.quizAppDataSet1.tbl_exams);
            dv.Sort = "ex_name ASC"; // <-- make sure your column name is correct

            comboBox1.DataSource = dv;
            comboBox1.DisplayMember = "ex_name"; // what user sees
            comboBox1.ValueMember = "ex_id";       // primary key (ID)

            // Load admins
            this.tbl_adminTableAdapter.Fill(this.quizAppDataSet.tbl_admin);

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            BindData();


        }
        //private void comboBoxQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (comboBoxQuestionType.SelectedItem != null)
        //    {
        //        string selectedType = comboBoxQuestionType.SelectedItem.ToString();

        //        if (selectedType == "Multiple Choice")
        //        {
        //            panelMCQ.Visible = true;
        //            panelShortAnswer.Visible = false;
        //        }
        //        else if (selectedType == "Short Answer")
        //        {
        //            panelMCQ.Visible = false;
        //            panelShortAnswer.Visible = true;
        //        }
        //    }
        //}






        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                label8.Text = comboBox1.SelectedValue.ToString();
            }
            BindData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ques_id q = new ques_id();

            q.q_title = textBox1.Text;
            q.q_opA = textBox2.Text;
            q.q_opB = textBox3.Text;
            q.q_opC = textBox4.Text;
            q.q_opD = textBox5.Text;
            q.q_correctOpn = textBox6.Text;
            q.q_correctDate = DateTime.Now.ToShortDateString();
            q.ad_id_fk = Admin_Logincs.fk_ad;
            q.ex_id_fk = comboBox1.SelectedValue.ToString();

            // save to DB
            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();
                string query = @"
            INSERT INTO tbl_questions
            (q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_correctDate, ad_id_fk, ex_id_fk, q_image)
            VALUES
            (@title, @a, @b, @c, @d, @correct, @date, @admin, @exam, @image)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", q.q_title);
                    cmd.Parameters.AddWithValue("@a", q.q_opA);
                    cmd.Parameters.AddWithValue("@b", q.q_opB);
                    cmd.Parameters.AddWithValue("@c", q.q_opC);
                    cmd.Parameters.AddWithValue("@d", q.q_opD);
                    cmd.Parameters.AddWithValue("@correct", q.q_correctOpn);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@admin", q.ad_id_fk);
                    cmd.Parameters.AddWithValue("@exam", q.ex_id_fk);

                    if (questionImageBytes != null)
                        cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = questionImageBytes;
                    else
                        cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = DBNull.Value;

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Question added successfully!");
            BindData();


        }



        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            question_type w = new question_type();
            w.Show();
            this.Hide();
        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            SqlConnection conn = connection_class.GetConnection();
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to delete the selected question(s)?",
                            "Confirm Delete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result != DialogResult.Yes)
                            return;

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                int questionId = Convert.ToInt32(row.Cells[0].Value); // Column index 0

                                using (SqlCommand cmd = new SqlCommand("DELETE FROM tbl_questions WHERE ques_id = @QuestionId", conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@QuestionId", questionId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Selected question(s) deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Please select question(s) to delete.");
                        transaction.Rollback();
                        return;
                    }

                    // Refresh DataGridView
                    BindData();
                    dataGridView1.ClearSelection();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred while deleting: " + ex.Message);
                }
            }
        }






        private void btnImportQuestions_Click_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls;*.xlsx;";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                        });

                        DataTable dt = result.Tables[0];

                        using (SqlConnection con = connection_class.GetConnection())
                        {
                            con.Open();

                            foreach (DataRow row in dt.Rows)
                            {
                                string title = row["q_title"].ToString().Trim();
                                string opA = row["q_opA"].ToString().Trim();
                                string opB = row["q_opB"].ToString().Trim();
                                string opC = row["q_opC"].ToString().Trim();
                                string opD = row["q_opD"].ToString().Trim();
                                string correct = row["q_correctOpn"].ToString().Trim();

                                // ✅ Image file path from Excel
                                string imagePath = row["q_image"].ToString().Trim();
                                byte[] imageBytes = null;

                                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                {
                                    imageBytes = File.ReadAllBytes(imagePath); // Convert to byte[]
                                }

                                string insertQuery = @"
                            INSERT INTO tbl_questions 
                            (q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_correctDate, ad_id_fk, ex_id_fk, q_image)
                            VALUES 
                            (@title, @a, @b, @c, @d, @correct, @date, @admin, @exam, @image)";

                                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                {
                                    cmd.Parameters.AddWithValue("@title", title);
                                    cmd.Parameters.AddWithValue("@a", opA);
                                    cmd.Parameters.AddWithValue("@b", opB);
                                    cmd.Parameters.AddWithValue("@c", opC);
                                    cmd.Parameters.AddWithValue("@d", opD);
                                    cmd.Parameters.AddWithValue("@correct", correct);
                                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
                                    cmd.Parameters.AddWithValue("@admin", Admin_Logincs.fk_ad); // your admin ID
                                    cmd.Parameters.AddWithValue("@exam", comboBox1.SelectedValue.ToString());

                                    // ✅ Image parameter (null if no image)
                                    if (imageBytes != null)
                                        cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = imageBytes;
                                    else
                                        cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = DBNull.Value;

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            BindData();
                            MessageBox.Show("✅ Questions (with images) imported successfully!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Import failed: " + ex.Message);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SqlConnection con = connection_class.GetConnection();
                {
                    if (MessageBox.Show("Are you sure to delete?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {


                        con.Open();

                        // Create the SQL command with parameters
                        string sqlQuery = "DELETE FROM tbl_questions WHERE q_title = @q_title";

                        SqlCommand command = new SqlCommand(sqlQuery, con);
                        command.Parameters.AddWithValue("@q_title", textBox1.Text);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successfully Deleted");
                            BindData();
                        }
                        else
                        {
                            MessageBox.Show("No rows were deleted. The question name may not exist.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please type in the question to be deleted.");
            }
        }
        private byte[] questionImageBytes = null; // store selected image as byte[]

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            questionImageBytes = null;
            pictureBoxQuestion.Image = null;
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                questionImageBytes = File.ReadAllBytes(ofd.FileName); // read as byte[]
                pictureBoxQuestion.Image = Image.FromFile(ofd.FileName); // preview
            }
        }

        private void BindData()
        {
            if (comboBox1.SelectedValue == null)
                return; // No exam selected yet

            int selectedExamId = Convert.ToInt32(comboBox1.SelectedValue);

            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();
                string query = @"SELECT ques_id, q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_correctDate, q_image 
                 FROM tbl_questions 
                 WHERE ex_id_fk = @ExamId";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@ExamId", selectedExamId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                // Prepare display table
                DataTable displayTable = new DataTable();
                displayTable.Columns.Add("ques_id", typeof(int));
                displayTable.Columns.Add("q_title", typeof(string));
                displayTable.Columns.Add("q_opA", typeof(string));
                displayTable.Columns.Add("q_opB", typeof(string));
                displayTable.Columns.Add("q_opC", typeof(string));
                displayTable.Columns.Add("q_opD", typeof(string));
                displayTable.Columns.Add("q_correctOpn", typeof(string));
                displayTable.Columns.Add("q_correctDate", typeof(DateTime)); // <-- Add date column
                displayTable.Columns.Add("q_image", typeof(Image));

                foreach (DataRow row in dt.Rows)
                {
                    DataRow newRow = displayTable.NewRow();
                    newRow["ques_id"] = row["ques_id"];
                    newRow["q_title"] = row["q_title"];
                    newRow["q_opA"] = row["q_opA"];
                    newRow["q_opB"] = row["q_opB"];
                    newRow["q_opC"] = row["q_opC"];
                    newRow["q_opD"] = row["q_opD"];
                    newRow["q_correctOpn"] = row["q_correctOpn"];
                    newRow["q_correctDate"] = row["q_correctDate"]; // <-- assign date

                    if (row["q_image"] != DBNull.Value)
                    {
                        byte[] imgBytes = (byte[])row["q_image"];
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            newRow["q_image"] = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        newRow["q_image"] = null;
                    }

                    displayTable.Rows.Add(newRow);
                }

                // Bind to DataGridView
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = displayTable;

                // Add text columns
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ques_id", HeaderText = "ID" });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "q_title", HeaderText = "Question" });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "q_opA", HeaderText = "Option A" });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "q_opB", HeaderText = "Option B" });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "q_opC", HeaderText = "Option C" });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "q_opD", HeaderText = "Option D" });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "q_correctOpn", HeaderText = "Correct" });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "q_correctDate", HeaderText = "Date Added", DefaultCellStyle = { Format = "dd-MMM-yyyy" } }); // <-- add column

                // Add image column
                DataGridViewImageColumn imgCol = new DataGridViewImageColumn
                {
                    DataPropertyName = "q_image",
                    HeaderText = "Image",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dataGridView1.Columns.Add(imgCol);

                // Set row height
                dataGridView1.RowTemplate.Height = 60;
            }
        }




        private void btnDownloadSample_Click_1(object sender, EventArgs e)
        {

            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Download Sample Excel File";
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.FileName = "question_sample.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourcePath = Path.Combine(Application.StartupPath, "Resources", "question_sample.xlsx");
                        File.Copy(sourcePath, saveFileDialog.FileName, true);
                        MessageBox.Show("Sample Excel file downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select an exam first.");
                return;
            }

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    int examId = Convert.ToInt32(comboBox1.SelectedValue);

                    // Step 1: Insert into tbl_past_questions
                    string insertQuery = @"
                INSERT INTO tbl_past_questions 
                    (q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_correctDate, ad_id_fk, ex_id_fk, q_image)
                SELECT q_title, q_opA, q_opB, q_opC, q_opD, q_correctOpn, q_correctDate, ad_id_fk, ex_id_fk, q_image
                FROM tbl_questions
                WHERE ex_id_fk = @examId";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 2: Delete from tbl_questions
                    string deleteQuery = "DELETE FROM tbl_questions WHERE ex_id_fk = @examId";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@examId", examId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show("Selected exam questions moved to past questions successfully!");

                    // ✅ Refresh DataGridView after moving
                    BindData();
                    dataGridView1.ClearSelection();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error while moving questions: " + ex.Message);
                }
            }
        }


        private void ExportToExcel(DataGridView dataGridView1, string fileName)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = new DataTable("Sheet1");

                // Add columns
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    dt.Columns.Add(col.HeaderText);
                }

                // Add rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            dr[i] = row.Cells[i].Value ?? DBNull.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                }

                wb.Worksheets.Add(dt, "Questions");

                // Save to Documents folder
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName + ".xlsx");
                wb.SaveAs(path);

                MessageBox.Show("✅ Data exported successfully to: " + path);
            }
        }



        private void button6_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1, "QuestionsExport");
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1, "QuestionsExport");
        }

      

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ignore header clicks
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Assuming your columns are: 0 = std_id, 1 = std_name, 2 = std_batch_code, 3 = std_password
                textBox1.Text = row.Cells[1].Value.ToString(); // std_name
                textBox2.Text = row.Cells[2].Value.ToString(); // std_batch_code
                textBox3.Text = row.Cells[3].Value.ToString(); // std_password
                textBox4.Text = row.Cells[4].Value.ToString(); // std_password
                textBox5.Text = row.Cells[5].Value.ToString(); // std_password
                textBox6.Text = row.Cells[6].Value.ToString(); // std_password
             //   pictureBoxQuestion.Image = row.Cells[].Value.ToString(); // std_password




            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    string query = @"
                UPDATE tbl_questions
                SET q_opA = @a,
                    q_opB = @b,
                    q_opC = @c,
                    q_opD = @d,
                    q_correctOpn = @correct,
                    q_correctDate = @date,
                    ad_id_fk = @admin,
                    ex_id_fk = @exam,
                    q_image = @image
                WHERE q_title = @title";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", textBox1.Text.Trim());   // q_title
                        cmd.Parameters.AddWithValue("@a", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@b", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@c", textBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@d", textBox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@correct", textBox6.Text.Trim());
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@admin", Admin_Logincs.fk_ad);
                        cmd.Parameters.AddWithValue("@exam", comboBox1.SelectedValue.ToString());

                        if (questionImageBytes != null)
                            cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = questionImageBytes;
                        else
                            cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = DBNull.Value;

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Question updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No record found with this question title!");
                        }
                    }
                }

                // Refresh the DataGridView
                BindData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating question: " + ex.Message);
            }
        }

       


        private void LoadQuestions(string subjectId)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT question_id, question_text, question_type, correct_option, correct_answer_text " +
                    "FROM tbl_questions WHERE subject_fk_id = @subjectId", con);

                cmd.Parameters.AddWithValue("@subjectId", subjectId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        


       
    }
}



