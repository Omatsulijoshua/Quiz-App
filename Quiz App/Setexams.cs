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
    using ExcelDataReader;
    using ClosedXML.Excel;
    using OfficeOpenXml; // Add at the top of your file
    using OfficeOpenXml.Style;

    namespace Quiz_App
    {
        public partial class Setexams : BaseForm
        {
                public Setexams()
                {
                    InitializeComponent();
                }
            //OpenFileDialog ofd = new OpenFileDialog();

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
        private void Setexams_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;

            Setexams.ScaleForm(this);

            // Load student records only once
            string q = "select * from student_record";
            viewclass vc = new viewclass(q);

            DataTable records = vc.showrecord(); // ?? call once

            dataGridView1.DataSource = records;
            ModernUi.StyleDataGridView(dataGridView1);

            dataGridView2.DataSource = records.Copy(); // use a copy if needed
            ModernUi.StyleDataGridView(dataGridView2);
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = true;
            dataGridView2.ReadOnly = true;

            // Fill Batch Code ComboBox
            SqlDataAdapter dadapter = new SqlDataAdapter(
                "select distinct std_batch_code from student_record ORDER BY std_batch_code ASC",
                connection_class.GetConnection());
            DataSet dset = new DataSet();
            dadapter.Fill(dset);
            comboBox1.DataSource = dset.Tables[0];
            comboBox1.DisplayMember = "std_batch_code";
            comboBox1.ValueMember = "std_batch_code";

            // Fill Exams ComboBox
            SqlDataAdapter dadapter2 = new SqlDataAdapter(
                "select ex_id, ex_name from tbl_exams ORDER BY ex_name ASC",
                connection_class.GetConnection());
            DataSet dset2 = new DataSet();
            dadapter2.Fill(dset2);
            comboBox2.DataSource = dset2.Tables[0];
            comboBox2.DisplayMember = "ex_name";
            comboBox2.ValueMember = "ex_id";

            ConfigureModernAllocateWorkspace();
        }

        private void ConfigureModernAllocateWorkspace()
        {
            BackColor = Color.FromArgb(11, 18, 31);
            label5.Text = "Allocate Exam To Students";
            label5.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label5.ForeColor = ModernUi.Ink;
            label5.AutoSize = false;
            label5.TextAlign = ContentAlignment.MiddleCenter;
            label5.SetBounds(0, 16, ClientSize.Width, 42);

            label1.Text = "Batch Code or Class";
            label2.Text = "Student ID";
            label3.Text = "Exam Name";

            foreach (Label label in new[] { label1, label2, label3 })
            {
                label.ForeColor = ModernUi.Ink;
                label.Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point);
                label.AutoSize = false;
                label.Size = new Size(180, 28);
            }

            label6.ForeColor = ModernUi.MutedInk;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Text = "Tip: fetch a batch, choose an exam, allocate one student manually or use Excel for bulk allocation.";
            label6.AutoSize = false;
            label6.Size = new Size(620, 54);

            ModernUi.StyleComboBox(comboBox1);
            ModernUi.StyleComboBox(comboBox2);
            ModernUi.StyleTextInput(textBox1);
            ModernUi.StyleDataGridView(dataGridView1);
            ModernUi.StyleDataGridView(dataGridView2);

            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView2.MultiSelect = true;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.ReadOnly = true;

            ModernUi.StylePrimaryButton(button1);
            ModernUi.StylePrimaryButton(button2);
            ModernUi.StyleSecondaryButton(button5);
            ModernUi.StyleSecondaryButton(button3);
            ModernUi.StyleDangerButton(buttonDeleteMultiple);

            button1.Text = "Fetch Students";
            button2.Text = "Allocate Student";
            button5.Text = "Allocate With Excel";
            button3.Text = "Load Allocated";
            buttonDeleteMultiple.Text = "Delete Selected";

            button4.ForeColor = ModernUi.AccentAlt;
            button4.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            button4.AutoSize = false;
            button4.TextAlign = ContentAlignment.MiddleLeft;

            int margin = 36;
            int contentWidth = ClientSize.Width - (margin * 2);
            int leftWidth = (contentWidth - 28) / 2;
            int rightWidth = contentWidth - leftWidth - 28;
            int leftX = margin;
            int rightX = leftX + leftWidth + 28;

            label1.Location = new Point(leftX, 88);
            comboBox1.SetBounds(leftX + 190, 82, leftWidth - 190, 40);
            button1.SetBounds(leftX + 190, 136, 180, 46);

            label3.Location = new Point(leftX, 214);
            comboBox2.SetBounds(leftX + 190, 208, leftWidth - 190, 40);

            label2.Location = new Point(leftX, 290);
            textBox1.SetBounds(leftX + 190, 284, leftWidth - 190, 40);
            button2.SetBounds(leftX + 190, 344, 180, 50);

            label6.Location = new Point(leftX, 408);

            int gridTop = 494;
            int gridHeight = Math.Max(260, ClientSize.Height - gridTop - 88);

            dataGridView1.SetBounds(leftX, gridTop, leftWidth, gridHeight);
            dataGridView2.SetBounds(rightX, gridTop, rightWidth, gridHeight - 134);

            button5.SetBounds(rightX, gridTop + gridHeight - 118, 180, 50);
            buttonDeleteMultiple.SetBounds(rightX + 196, gridTop + gridHeight - 118, 180, 50);
            button3.SetBounds(rightX, gridTop + gridHeight - 58, 180, 50);
            button4.SetBounds(rightX + 196, gridTop + gridHeight - 52, rightWidth - 196, 28);

            Panel leftCard = EnsureAllocateCard("__allocateLeftCard", new Rectangle(leftX - 12, gridTop - 12, leftWidth + 24, gridHeight + 24));
            Panel rightCard = EnsureAllocateCard("__allocateRightCard", new Rectangle(rightX - 12, gridTop - 12, rightWidth + 24, gridHeight - 110));
            leftCard.SendToBack();
            rightCard.SendToBack();
            dataGridView1.BringToFront();
            dataGridView2.BringToFront();

            button7.Visible = false;
        }

        private Panel EnsureAllocateCard(string name, Rectangle bounds)
        {
            Panel card = Controls.Find(name, false).OfType<Panel>().FirstOrDefault();
            if (card == null)
            {
                card = ModernUi.CreateCard(bounds);
                card.Name = name;
                Controls.Add(card);
            }
            else
            {
                card.Bounds = bounds;
                card.Invalidate();
            }

            return card;
        }
        private void RefreshSetExamTables()
            {
                SqlConnection con = connection_class.GetConnection();
                {
                    con.Open();

                    string query = "SELECT * FROM set_exam ORDER BY set_exam_date ASC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());

                        dataGridView1.DataSource = dt.Copy(); // Load into Grid 1
                        dataGridView2.DataSource = dt;        // Load into Grid 2
                    }
                }

                // Optional UI polish
                dataGridView1.AutoResizeColumns();
                dataGridView1.ClearSelection();

                dataGridView2.AutoResizeColumns();
                dataGridView2.ClearSelection();
            }
            private void button1_Click(object sender, EventArgs e)
            {
                string q = "select * from student_record where std_id in (select std_id from set_exam) and std_batch_code ='" + comboBox1.SelectedValue.ToString() + "' ";
                viewclass vc = new viewclass(q);
                dataGridView1.DataSource = vc.showrecord();
                ModernUi.StyleDataGridView(dataGridView1);


                dataGridView2.DataSource = vc.showrecord();
                ModernUi.StyleDataGridView(dataGridView2);

                dataGridView1.DataSource = vc.showrecord();
            }

            private void button2_Click(object sender, EventArgs e)
            {
                insertclass ic = new insertclass();
                ic.insert_setexam(System.DateTime.Now.ToShortDateString(), textBox1.Text, comboBox2.SelectedValue.ToString());


                string q = "select * from set_exam";
                viewclass vc = new viewclass(q);

                dataGridView2.DataSource = vc.showrecord();

                q = "select * from student_record where std_id not in (select std_id_fk from set_exam) and std_batch_code ='" + comboBox1.SelectedValue.ToString() + "' ";
                viewclass vc2 = new viewclass(q);

                dataGridView1.DataSource = vc.showrecord();
                ModernUi.StyleDataGridView(dataGridView1);

                dataGridView2.DataSource = vc.showrecord();
                ModernUi.StyleDataGridView(dataGridView2);




            }

            private void pictureBox3_Click(object sender, EventArgs e)
            {
                Application.Exit();
            }



            private void button3_Click(object sender, EventArgs e)
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    SqlConnection con = connection_class.GetConnection();
                    {
                        if (MessageBox.Show("Are you sure to delete this Student?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {


                            con.Open();

                            // Create the SQL command with parameters
                            string sqlQuery = "DELETE FROM set_exam WHERE set_exam_id = @set_exam_id";

                            SqlCommand command = new SqlCommand(sqlQuery, con);
                            command.Parameters.AddWithValue("@set_exam_id", textBox1.Text);

                            // Execute the command
                            int rowsAffected = command.ExecuteNonQuery();

                            // Check if any rows were affected
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Course Allocation Successfully Deleted");
                                //BindData(); // Refresh the DataGridView
                            }
                            else
                            {
                                MessageBox.Show("No rows were deleted. The student ID name may not exist.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please type in the student ID to be deleted.");
                }
            }

            private void button4_Click(object sender, EventArgs e)
            {
               

            }

            private void btnAddWithExcel_Click(object sender, EventArgs e)
            {

            }

            private void button5_Click(object sender, EventArgs e)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Excel Files|*.xls;*.xlsx;";
                ofd.Title = "Select Excel File";

                if (ofd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(ofd.FileName))
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

                            SqlConnection con = connection_class.GetConnection();
                            {
                                con.Open();
                                foreach (DataRow row in dt.Rows)
                                {
                                    string stuId = row["stu_id_fk"].ToString().Trim();  // Make sure column name matches Excel
                                    string examId = comboBox2.SelectedValue.ToString();
                                    string date = DateTime.Now.ToShortDateString();

                                    if (!string.IsNullOrWhiteSpace(stuId))
                                    {
                                        // Check if student exists
                                        string checkQuery = "SELECT COUNT(*) FROM student_record WHERE std_id = @stuId";
                                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                                        {
                                            checkCmd.Parameters.AddWithValue("@stuId", stuId);
                                            int exists = (int)checkCmd.ExecuteScalar();

                                            if (exists > 0)
                                            {
                                                string insertQuery = "INSERT INTO set_exam (set_exam_date, exam_id_fk, stud_id_fk) VALUES (@date, @exam, @stu)";
                                                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                                {
                                                    cmd.Parameters.AddWithValue("@date", date);
                                                    cmd.Parameters.AddWithValue("@exam", examId);
                                                    cmd.Parameters.AddWithValue("@stu", stuId);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show($"Student ID '{stuId}' does not exist in student_record. Skipping this row.");
                                            }
                                        }
                                    }

                                }

                                MessageBox.Show("Students allocated to exam successfully!");
                                RefreshSetExamTables();

                            }
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Please close the Excel file before importing.", "File In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No file selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }



            }

            private void pictureBox4_Click(object sender, EventArgs e)
            {
                Exam_Settings w = new Exam_Settings();
                w.Show();
                this.Hide();
            }

            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {

            }

            private void buttonDeleteMultiple_Click(object sender, EventArgs e)
            {
                DeleteSelectedSetExamRecords();
            }

            private void DeleteSelectedSetExamRecords()
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete the selected record(s)?",
                                                          "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        SqlConnection con = connection_class.GetConnection();
                        {
                            con.Open();

                            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                            {
                                if (row.Cells[0].Value != null) // Accessing by column index 0
                                {
                                    int setExamId = Convert.ToInt32(row.Cells[0].Value); // set_exam_id in first column

                                    string deleteQuery = "DELETE FROM set_exam WHERE set_exam_id = @id";
                                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                                    {
                                        cmd.Parameters.AddWithValue("@id", setExamId);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            con.Close();
                        }

                        MessageBox.Show("Selected record(s) deleted successfully.");
                        RefreshSetExamTables(); // Reload your datagrids
                    }
                }
                else
                {
                    MessageBox.Show("Please select one or more rows to delete.");
                }
            }



            private void button3_Click_1(object sender, EventArgs e)
            {
                {
                    string query = @"
            SELECT set_exam_id,
                   set_exam_date,
                   exam_id_fk,
                   stud_id_fk
            FROM set_exam
            ORDER BY set_exam_id ASC";  // Ensures top-to-bottom loading

                    SqlConnection con = connection_class.GetConnection();
                    {
                        SqlDataAdapter da = new SqlDataAdapter(query, con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridView2.DataSource = dt;
                    }
                }
            }

        private void button4_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Download Sample Excel File";
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.FileName = "Allocate Exam.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Make sure the SetExamTemplate.xlsx is located in Resources folder of your application
                    string sourcePath = Path.Combine(Application.StartupPath, "Resources", "Allocate Exam.xlsx");

                    // Copy the file to the selected download path
                    File.Copy(sourcePath, saveFileDialog.FileName, true);

                    MessageBox.Show("Allocate Exam downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    }

