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
    public partial class add_theory_questions : BaseForm
    {
        private PictureBox theoryImagePreview;
        private Button btnSelectImage;
        private Button btnClearImage;
        private byte[] selectedQuestionImageBytes;
        private Label modelAnswerHintLabel;

        public add_theory_questions()
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

            // Adjust font scaling (recursive, handles nested controls)
            ScaleControlFontsRecursive(form, Math.Min(scaleX, scaleY));

            // Center form
            form.StartPosition = FormStartPosition.CenterScreen;
        }
        private static void ScaleControlFontsRecursive(System.Windows.Forms.Control parent, float fontScale)
        {
            foreach (System.Windows.Forms.Control c in parent.Controls)
            {
                if (c.Font != null)
                {
                    c.Font = new System.Drawing.Font(
                        c.Font.FontFamily,
                        c.Font.Size * fontScale,
                        c.Font.Style
                    );
                }

                if (c.HasChildren)
                {
                    ScaleControlFontsRecursive(c, fontScale);
                }
            }
        }

        private void add_theory_questions_Load(object sender, EventArgs e)
        {
            LoadExams();
            this.FormBorderStyle = FormBorderStyle.None;   // remove close/min/max buttons
            this.WindowState = FormWindowState.Maximized;  // maximize to fill screen
            this.TopMost = true;                           // keep exam window on top
            add_theory_questions.ScaleForm(this);
            ModernUi.StyleComboBox(cmbExam);
            ModernUi.StyleNumericUpDown(numQuestionNo);
            ModernUi.StyleNumericUpDown(numMark);
            ModernUi.StylePrimaryButton(btnSave);
            ModernUi.StyleSecondaryButton(btnClear);
            ModernUi.StyleSecondaryButton(btnUpdate);
            ModernUi.PrepareDataGridView(dgvQuestions);
            dgvQuestions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQuestions.MultiSelect = true;
            dgvQuestions.AllowUserToAddRows = false;
            dgvQuestions.ReadOnly = false;
            EnsureTheoryImageControls();
            EnsureModelAnswerHint();
            txtModelAnswer.TextChanged -= txtModelAnswer_TextChanged;
            txtModelAnswer.TextChanged += txtModelAnswer_TextChanged;
            btnUpdate.Enabled = false;
        }
        private void LoadExams()
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbExam.DataSource = dt;
                    cmbExam.DisplayMember = "ex_name";
                    cmbExam.ValueMember = "ex_id";

                    if (dt.Rows.Count > 0)
                    {
                        cmbExam.SelectedIndex = 0; // programmatic selection
                        int firstId = Convert.ToInt32(dt.Rows[0]["ex_id"]);
                        exam_Id.Text = firstId.ToString();
                        LoadQuestions(firstId);     // load first exam's rows immediately
                    }
                    else
                    {
                        dgvQuestions.DataSource = null;
                        exam_Id.Text = "No Exam";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading exams: " + ex.Message);
            }
        }





        private void LoadQuestions(int examId)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    if (!TheoryQuestionsTableExists(conn))
                    {
                        dgvQuestions.DataSource = null;
                        exam_Id.Text = "Theory table missing";
                        btnSave.Enabled = false;
                        btnUpdate.Enabled = false;
                        return;
                    }

                    string query = @"
                SELECT 
                    tq.theory_id,
                    tq.question_number,
                    tq.question_text,
                    tq.mark,
                    tq.model_answer,
                    tq.question_image,
                    e.ex_id AS exam_id,
                    e.ex_name AS exam_name
                FROM tbl_theory_questions tq
                INNER JOIN tbl_exams e ON tq.exam_fk_id = e.ex_id
                WHERE tq.exam_fk_id = @exam_fk_id
                ORDER BY tq.question_number";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@exam_fk_id", examId);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataTable displayTable = new DataTable();
                    displayTable.Columns.Add("exam_name", typeof(string));
                    displayTable.Columns.Add("exam_id", typeof(int));
                    displayTable.Columns.Add("theory_id", typeof(int));
                    displayTable.Columns.Add("question_number", typeof(int));
                    displayTable.Columns.Add("question_text", typeof(string));
                    displayTable.Columns.Add("mark", typeof(int));
                    displayTable.Columns.Add("model_answer", typeof(string));
                    displayTable.Columns.Add("question_image_preview", typeof(Image));
                    displayTable.Columns.Add("question_image", typeof(byte[]));

                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow displayRow = displayTable.NewRow();
                        displayRow["exam_name"] = row["exam_name"];
                        displayRow["exam_id"] = row["exam_id"];
                        displayRow["theory_id"] = row["theory_id"];
                        displayRow["question_number"] = row["question_number"];
                        displayRow["question_text"] = row["question_text"];
                        displayRow["mark"] = row["mark"];
                        displayRow["model_answer"] = row["model_answer"];

                        if (row["question_image"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])row["question_image"];
                            displayRow["question_image"] = imageBytes;
                            displayRow["question_image_preview"] = ByteArrayToImage(imageBytes);
                        }
                        else
                        {
                            displayRow["question_image"] = DBNull.Value;
                            displayRow["question_image_preview"] = DBNull.Value;
                        }

                        displayTable.Rows.Add(displayRow);
                    }

                    // Rebuild columns so mapping is always correct for the selected exam
                    dgvQuestions.AutoGenerateColumns = false;
                    dgvQuestions.Columns.Clear();

                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "exam_name", HeaderText = "Exam Name", Name = "exam_name" });
                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "exam_id", HeaderText = "Exam ID", Name = "exam_id" });

                    // hide internal ID if you prefer: show for debugging
                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "theory_id", HeaderText = "Q ID", Name = "theory_id", Visible = false });

                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "question_number", HeaderText = "Q No", Name = "question_number", Width = 60 });
                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "question_text", HeaderText = "Question", Name = "question_text", Width = 400 });
                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "mark", HeaderText = "Mark", Name = "mark", Width = 70 });
                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "model_answer", HeaderText = "Model Answer", Name = "model_answer", Width = 200 });
                    dgvQuestions.Columns.Add(new DataGridViewImageColumn
                    {
                        DataPropertyName = "question_image_preview",
                        HeaderText = "Image",
                        Name = "question_image_preview",
                        Width = 120,
                        ImageLayout = DataGridViewImageCellLayout.Zoom
                    });
                    dgvQuestions.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "question_image", HeaderText = "Image Raw", Name = "question_image", Visible = false });

                    // Add Edit/Delete buttons (names used by your CellContentClick handler)
                    DataGridViewButtonColumn editButton = new DataGridViewButtonColumn
                    {
                        Name = "Edit",
                        HeaderText = "",
                        Text = "Edit",
                        UseColumnTextForButtonValue = true,
                        Width = 60
                    };
                    dgvQuestions.Columns.Add(editButton);

                    DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn
                    {
                        Name = "Delete",
                        HeaderText = "",
                        Text = "Delete",
                        UseColumnTextForButtonValue = true,
                        Width = 70
                    };
                    dgvQuestions.Columns.Add(deleteButton);

                    // bind data
                    dgvQuestions.DataSource = displayTable;

                    // optional UI niceties
                    dgvQuestions.RowTemplate.Height = 74;
                    ModernUi.StyleDataGridView(dgvQuestions);
                    dgvQuestions.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading questions: " + ex.Message);
            }
        }



        private void ClearForm()
        {
            txtQuestion.Clear();
            txtModelAnswer.Clear();
            numMark.Value = 10;
            numQuestionNo.Value = 1;
            txtQuestion.Tag = null;
            selectedQuestionImageBytes = null;
            ShowTheoryImagePreview(null);
            btnUpdate.Enabled = false; // disable update mode
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbExam.SelectedValue == null || string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                MessageBox.Show("Please select an exam and enter a question.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    if (!TheoryQuestionsTableExists(conn))
                    {
                        MessageBox.Show("Theory questions table is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = @"INSERT INTO tbl_theory_questions 
                                    (exam_fk_id, question_text, mark, question_number, model_answer, question_image)
                                    VALUES (@exam_fk_id, @question_text, @mark, @question_number, @model_answer, @question_image)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@exam_fk_id", cmbExam.SelectedValue);
                    cmd.Parameters.AddWithValue("@question_text", txtQuestion.Text.Trim());
                    cmd.Parameters.AddWithValue("@mark", numMark.Value);
                    cmd.Parameters.AddWithValue("@question_number", numQuestionNo.Value);
                    cmd.Parameters.AddWithValue("@model_answer", txtModelAnswer.Text.Trim().ToUpperInvariant());
                    cmd.Parameters.AddWithValue("@question_image", (object)selectedQuestionImageBytes ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Theory question added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadQuestions();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving question: " + ex.Message);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            question_type w = new question_type();
            w.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DeleteQuestion(int id)
        {
            if (MessageBox.Show("Are you sure you want to delete this question?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = connection_class.GetConnection())
                    {
                        conn.Open();
                        if (!TheoryQuestionsTableExists(conn))
                        {
                            MessageBox.Show("Theory questions table is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string query = "DELETE FROM tbl_theory_questions WHERE theory_id = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Question deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadQuestions();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting question: " + ex.Message);
                }
            }
        }

        private void LoadQuestionForEdit(int id)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    if (!TheoryQuestionsTableExists(conn))
                    {
                        MessageBox.Show("Theory questions table is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = "SELECT * FROM tbl_theory_questions WHERE theory_id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        numQuestionNo.Value = Convert.ToInt32(reader["question_number"]);
                        txtQuestion.Text = reader["question_text"].ToString();
                        numMark.Value = Convert.ToInt32(reader["mark"]);
                        txtModelAnswer.Text = reader["model_answer"].ToString();
                        selectedQuestionImageBytes = reader["question_image"] == DBNull.Value
                            ? null
                            : (byte[])reader["question_image"];
                        ShowTheoryImagePreview(selectedQuestionImageBytes);
                        txtQuestion.Tag = id;
                        btnUpdate.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading question for edit: " + ex.Message);
            }
        }

        private void dgvQuestions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // ignore header clicks

            if (dgvQuestions.Columns[e.ColumnIndex].Name == "Edit")
            {
                int theoryId = Convert.ToInt32(dgvQuestions.Rows[e.RowIndex].Cells["theory_id"].Value);
                string question = dgvQuestions.Rows[e.RowIndex].Cells["question_text"].Value.ToString();
                decimal mark = Convert.ToDecimal(dgvQuestions.Rows[e.RowIndex].Cells["mark"].Value);
                decimal number = Convert.ToDecimal(dgvQuestions.Rows[e.RowIndex].Cells["question_number"].Value);
                string modelAnswer = dgvQuestions.Rows[e.RowIndex].Cells["model_answer"].Value.ToString();
                object imageValue = dgvQuestions.Rows[e.RowIndex].Cells["question_image"].Value;

                txtQuestion.Text = question;
                numMark.Value = mark;
                numQuestionNo.Value = number;
                txtModelAnswer.Text = modelAnswer;
                selectedQuestionImageBytes = imageValue == null || imageValue == DBNull.Value
                    ? null
                    : (byte[])imageValue;
                ShowTheoryImagePreview(selectedQuestionImageBytes);

                txtQuestion.Tag = theoryId;  // store the ID for update
                btnUpdate.Enabled = true;    // enable update button
            }
            else if (dgvQuestions.Columns[e.ColumnIndex].Name == "Delete")
            {
                List<int> theoryIds = dgvQuestions.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Where(row => !row.IsNewRow && row.Cells["theory_id"].Value != null)
                    .Select(row => Convert.ToInt32(row.Cells["theory_id"].Value))
                    .Distinct()
                    .ToList();

                if (theoryIds.Count == 0)
                {
                    theoryIds.Add(Convert.ToInt32(dgvQuestions.Rows[e.RowIndex].Cells["theory_id"].Value));
                }

                string confirmMessage = theoryIds.Count == 1
                    ? "Are you sure you want to delete this question?"
                    : $"Are you sure you want to delete {theoryIds.Count} selected questions?";

                if (MessageBox.Show(confirmMessage, "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection conn = connection_class.GetConnection())
                    {
                        conn.Open();
                        if (!TheoryQuestionsTableExists(conn))
                        {
                            MessageBox.Show("Theory questions table is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (int theoryId in theoryIds)
                                {
                                    using (SqlCommand cmd = new SqlCommand("DELETE FROM tbl_theory_questions WHERE theory_id = @theory_id", conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@theory_id", theoryId);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Error deleting question(s): " + ex.Message);
                                return;
                            }
                        }
                    }

                    LoadQuestions(); // refresh grid
                }
            }
        }
        private void AddEditDeleteButtons()
        {
            // Prevent adding duplicate button columns
            if (!dgvQuestions.Columns.Contains("Edit") && !dgvQuestions.Columns.Contains("Delete"))
            {
                // Add Edit button
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.HeaderText = "Edit";
                editButton.Text = "Edit";
                editButton.Name = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dgvQuestions.Columns.Add(editButton);

                // Add Delete button
                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.HeaderText = "Delete";
                deleteButton.Text = "Delete";
                deleteButton.Name = "Delete";
                deleteButton.UseColumnTextForButtonValue = true;
                dgvQuestions.Columns.Add(deleteButton);
            }
        }

       

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtQuestion.Tag == null)
            {
                MessageBox.Show("Please select a question to update from the list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    if (!TheoryQuestionsTableExists(conn))
                    {
                        MessageBox.Show("Theory questions table is not available in this database yet.", "Schema Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = @"UPDATE tbl_theory_questions
                             SET question_text = @question_text,
                                 mark = @mark,
                                 question_number = @question_number,
                                 model_answer = @model_answer,
                                 question_image = @question_image
                             WHERE theory_id = @theory_id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@theory_id", Convert.ToInt32(txtQuestion.Tag)); // ? FIX
                    cmd.Parameters.AddWithValue("@question_text", txtQuestion.Text.Trim());
                    cmd.Parameters.AddWithValue("@mark", numMark.Value);
                    cmd.Parameters.AddWithValue("@question_number", numQuestionNo.Value);
                    cmd.Parameters.AddWithValue("@model_answer", txtModelAnswer.Text.Trim().ToUpperInvariant());
                    cmd.Parameters.AddWithValue("@question_image", (object)selectedQuestionImageBytes ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Question updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadQuestions();
                    ClearForm();
                    btnUpdate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating question: " + ex.Message);
            }
        }

        private int? GetSelectedExamId()
        {
            if (cmbExam.SelectedItem == null) return null;

            // If SelectedValue is DataRowView (happens during binding), read from SelectedItem
            if (cmbExam.SelectedItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("ex_id") && drv["ex_id"] != DBNull.Value)
                    return Convert.ToInt32(drv["ex_id"]);
                return null;
            }

            // If SelectedValue is the actual id (int / string)
            if (cmbExam.SelectedValue != null && int.TryParse(cmbExam.SelectedValue.ToString(), out int id))
                return id;

            return null;
        }

        private void LoadQuestions()
        {
            int? examId = GetSelectedExamId();
            if (!examId.HasValue)
            {
                // No exam selected yet — clear grid
                dgvQuestions.DataSource = null;
                return;
            }

            // Forward to the main implementation
            LoadQuestions(examId.Value);
        }

        private void cmbExam_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // avoid firing while DataSource is being set
            int? examId = GetSelectedExamId();
            if (!examId.HasValue) return;

            try
            {
                exam_Id.Text = examId.Value.ToString();
                LoadQuestions(examId.Value); // load only questions for the selected exam
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading selected exam: " + ex.Message);
            }
        }

        private bool TheoryQuestionsTableExists(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbl_theory_questions'",
                connection))
            {
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void EnsureTheoryImageControls()
        {
            if (theoryImagePreview != null)
            {
                return;
            }

            theoryImagePreview = new PictureBox
            {
                Name = "theoryImagePreview",
                BackColor = Color.FromArgb(18, 26, 42),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(1049, 185),
                Size = new Size(320, 220)
            };

            btnSelectImage = new Button
            {
                Name = "btnSelectImage",
                Text = "Upload Image",
                Location = new Point(1049, 430),
                Size = new Size(150, 40)
            };

            btnClearImage = new Button
            {
                Name = "btnClearImage",
                Text = "Clear Image",
                Location = new Point(1219, 430),
                Size = new Size(150, 40)
            };

            ModernUi.StyleSecondaryButton(btnSelectImage);
            ModernUi.StyleSecondaryButton(btnClearImage);

            btnSelectImage.Click += BtnSelectImage_Click;
            btnClearImage.Click += BtnClearImage_Click;

            Controls.Add(theoryImagePreview);
            Controls.Add(btnSelectImage);
            Controls.Add(btnClearImage);

            theoryImagePreview.BringToFront();
            btnSelectImage.BringToFront();
            btnClearImage.BringToFront();
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                dialog.Title = "Select Theory Question Image";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedQuestionImageBytes = File.ReadAllBytes(dialog.FileName);
                    ShowTheoryImagePreview(selectedQuestionImageBytes);
                }
            }
        }

        private void BtnClearImage_Click(object sender, EventArgs e)
        {
            selectedQuestionImageBytes = null;
            ShowTheoryImagePreview(null);
        }

        private void ShowTheoryImagePreview(byte[] imageBytes)
        {
            if (theoryImagePreview == null)
            {
                return;
            }

            if (theoryImagePreview.Image != null)
            {
                Image oldImage = theoryImagePreview.Image;
                theoryImagePreview.Image = null;
                oldImage.Dispose();
            }

            if (imageBytes == null || imageBytes.Length == 0)
            {
                return;
            }

            using (MemoryStream stream = new MemoryStream(imageBytes))
            using (Image source = Image.FromStream(stream))
            {
                theoryImagePreview.Image = new Bitmap(source);
            }
        }

        private Image ByteArrayToImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            using (MemoryStream stream = new MemoryStream(imageBytes))
            using (Image source = Image.FromStream(stream))
            {
                return new Bitmap(source);
            }
        }

        private void txtModelAnswer_TextChanged(object sender, EventArgs e)
        {
            string current = txtModelAnswer.Text;
            string upper = current.ToUpperInvariant();
            if (current == upper)
            {
                return;
            }

            int selectionStart = txtModelAnswer.SelectionStart;
            int selectionLength = txtModelAnswer.SelectionLength;
            txtModelAnswer.Text = upper;
            txtModelAnswer.SelectionStart = Math.Min(selectionStart, txtModelAnswer.TextLength);
            txtModelAnswer.SelectionLength = selectionLength;
        }

        private void EnsureModelAnswerHint()
        {
            if (modelAnswerHintLabel == null)
            {
                modelAnswerHintLabel = new Label
                {
                    AutoSize = false,
                    BackColor = Color.Transparent
                };
                Controls.Add(modelAnswerHintLabel);
            }

            modelAnswerHintLabel.Text = "Caution: Model answer must be in ALL CAPITAL LETTERS.";
            modelAnswerHintLabel.ForeColor = ModernUi.Warning;
            modelAnswerHintLabel.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            modelAnswerHintLabel.Location = new Point(txtModelAnswer.Left, txtModelAnswer.Bottom + 8);
            modelAnswerHintLabel.Size = new Size(Math.Max(360, txtModelAnswer.Width), 24);
            modelAnswerHintLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            modelAnswerHintLabel.BringToFront();
        }
    }
}

