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
    public partial class add_student : Form
    {
        public static string fk_ad;


        public add_student()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form2 w = new Form2();
            w.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            // Create a SqlConnection object with connection string
            SqlConnection con = connection_class.GetConnection();
            {
                // Open the connection
                con.Open();


                // Create the SQL command with parameters to prevent SQL injection
                string sqlQuery = "INSERT INTO student_record (std_name, std_batch_code, std_password) " +
                                  "VALUES (@std_name, @std_batch_code, @std_password)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    // Add parameters to the SQL command
                    // cmd.Parameters.AddWithValue("@std_id", int.Parse(textBox1.Text));
                    cmd.Parameters.AddWithValue("@std_name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@std_batch_code", textBox2.Text);
                    cmd.Parameters.AddWithValue("@std_password", textBox3.Text);
                    //fk_id.std_id_fk = studentlogin.fk_ad;





                    // Execute the command
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if any rows were affected
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Student Successfully Added");
                        BindData(); // Refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Failed to add student");
                    }
                }

                // Close the connection
                con.Close();
                BindData();
            }

        }
        void BindData()
        {
            SqlConnection con = connection_class.GetConnection();
            {
                con.Open();
                string sqlQuery = "SELECT * FROM student_record";
                SqlCommand command = new SqlCommand(sqlQuery, con);
                SqlDataAdapter sd = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                con.Close();

                // **Set DataSource here**
                dataGridView1.DataSource = dt;
               // foreach (DataGridViewColumn col in dataGridView1.Columns)
              //  {
              //      MessageBox.Show(col.Name); // Shows each column name
              //  }


                // Then your color settings:
                dataGridView1.BackgroundColor = Color.LightYellow;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                dataGridView1.DefaultCellStyle.BackColor = Color.LightCyan;
                dataGridView1.DefaultCellStyle.ForeColor = Color.DarkBlue;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.EnableHeadersVisualStyles = false;

                dataGridView1.ClearSelection();
            }
        }


        private void add_student_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'quizAppDataSet6.student_record' table. You can move, or remove it, as needed.
            this.student_recordTableAdapter2.Fill(this.quizAppDataSet6.student_record);
            BindData();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = true;
            dataGridView1.ReadOnly = true; // Optional: prevents accidental edits
            dataGridView1.AllowUserToAddRows = false; // Cleaner display


        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = connection_class.GetConnection();
            {
                conn.Open();

                // Start a SQL transaction
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        if (dataGridView1.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Please select student(s) to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to delete the selected student(s)?",
                            "Confirm Delete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result != DialogResult.Yes)
                            return;

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            if (row.Cells[0].Value != null) // Assuming std_id is in column 0
                            {
                                int studentId = Convert.ToInt32(row.Cells[0].Value);

                                // 1. Delete from child table (set_exam)
                                using (SqlCommand cmd1 = new SqlCommand("DELETE FROM set_exam WHERE stud_id_fk = @StudentId", conn, transaction))
                                {
                                    cmd1.Parameters.AddWithValue("@StudentId", studentId);
                                    cmd1.ExecuteNonQuery();
                                }

                                // 2. Delete from parent table (student_record)
                                using (SqlCommand cmd2 = new SqlCommand("DELETE FROM student_record WHERE std_id = @StudentId", conn, transaction))
                                {
                                    cmd2.Parameters.AddWithValue("@StudentId", studentId);
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Selected student(s) deleted successfully.");

                        // ✅ Refresh DataGridView manually after deletion
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM student_record", conn))
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }

                        dataGridView1.ClearSelection();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("An error occurred while deleting students: " + ex.Message);
                    }
                }
            }




        }

        private void button3_Click(object sender, EventArgs e)
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

                        SqlConnection con = connection_class.GetConnection();
                        {
                            con.Open();

                            foreach (DataRow row in dt.Rows)
                            {
                                string name = row["std_name"].ToString().Trim();
                                string batch = row["std_batch_code"].ToString().Trim();
                                string password = row["std_password"].ToString().Trim();

                                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(password))
                                {
                                    string insertQuery = "INSERT INTO student_record (std_name, std_batch_code, std_password) VALUES (@name, @batch, @pass)";
                                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                    {
                                        cmd.Parameters.AddWithValue("@name", name);
                                        cmd.Parameters.AddWithValue("@batch", batch);
                                        cmd.Parameters.AddWithValue("@pass", password);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            MessageBox.Show("Students imported successfully!");
                            BindData(); // Refresh the student DataGridView
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

        }

        private void button4_Click(object sender, EventArgs e)
        {
           

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SqlConnection con = connection_class.GetConnection();
                {
                    if (MessageBox.Show("Are you sure to delete this Student?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {


                        con.Open();

                        // Create the SQL command with parameters
                        string sqlQuery = "DELETE FROM student_record WHERE std_name = @std_name";

                        SqlCommand command = new SqlCommand(sqlQuery, con);
                        command.Parameters.AddWithValue("@std_name", textBox1.Text);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student Successfully Deleted");
                            BindData(); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No rows were deleted. The student name may not exist.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please type in the student to be deleted.");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Download Sample Excel File";
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.FileName = "student_sample.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourcePath = Path.Combine(Application.StartupPath, "Resources", "student_sample.xlsx");
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;

        }

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            SqlConnection con = connection_class.GetConnection();
            {
                con.Open();

                // Build dynamic query depending on what the user typed
                string sqlQuery = "SELECT * FROM student_record WHERE 1=1"; // Always true, easy to append conditions

                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    sqlQuery += " AND std_name LIKE @name";
                }

                if (!string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    sqlQuery += " AND std_id = @id";
                }

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    if (!string.IsNullOrWhiteSpace(textBox4.Text))
                    {
                        cmd.Parameters.AddWithValue("@name", "%" + textBox4.Text.Trim() + "%"); // LIKE search
                    }

                    if (!string.IsNullOrWhiteSpace(textBox5.Text))
                    {
                        int id;
                        if (int.TryParse(textBox5.Text.Trim(), out id))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                        }
                        else
                        {
                            MessageBox.Show("Invalid Student ID");
                            return;
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView2.DataSource = dt;

                    // ✅ Copy styling from DataGridView1 to DataGridView2
                    dataGridView2.BackgroundColor = dataGridView1.BackgroundColor;
                    dataGridView2.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    dataGridView2.DefaultCellStyle.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                    dataGridView2.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.SelectionBackColor;
                    dataGridView2.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.SelectionForeColor;

                    dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = dataGridView1.AlternatingRowsDefaultCellStyle.BackColor;
                    dataGridView2.AlternatingRowsDefaultCellStyle.ForeColor = dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor;

                    dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
                    dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor;
                    dataGridView2.EnableHeadersVisualStyles = false; // make sure header colors apply
                }

                con.Close();
            }
        }

       

        private void ExportToExcel(DataGridView  dataGridView2, string fileName)
        {
            try
            {
                if (dataGridView2.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export!");
                    return;
                }

                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("ExportedData");

                    // Add column headers
                    for (int i = 0; i < dataGridView2.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dataGridView2.Columns[i].HeaderText;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    }

                    // Add rows
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView2.Columns.Count; j++)
                        {
                            if (dataGridView2.Rows[i].Cells[j].Value != null)
                                worksheet.Cell(i + 2, j + 1).Value = dataGridView2.Rows[i].Cells[j].Value.ToString();
                        }
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save file dialog
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Excel Workbook|*.xlsx";
                    sfd.FileName = fileName + ".xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(sfd.FileName);
                        MessageBox.Show("Exported Successfully to " + sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while exporting: " + ex.Message);
            }
        }

        private void ExportToExcel_2(DataGridView dataGridView1, string fileName)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export!");
                    return;
                }

                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("ExportedData");

                    // Add column headers
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dataGridView1.Columns[i].HeaderText;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    }

                    // Add rows
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                                worksheet.Cell(i + 2, j + 1).Value = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        }
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save file dialog
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Excel Workbook|*.xlsx";
                    sfd.FileName = fileName + ".xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(sfd.FileName);
                        MessageBox.Show("Exported Successfully to " + sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while exporting: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView2, "StudentRecords");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ExportToExcel_2(dataGridView1, "StudentRecords 2");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Use index instead of column name
            int studentId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            string newName = textBox1.Text.Trim();
            string newBatch = textBox2.Text.Trim();
            string newPassword = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newBatch) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please fill all fields before updating.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection con = connection_class.GetConnection();
            {
                try
                {
                    con.Open();
                    string sqlQuery = "UPDATE student_record " +
                                      "SET std_name = @std_name, std_batch_code = @std_batch_code, std_password = @std_password " +
                                      "WHERE std_id = @std_id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@std_name", newName);
                        cmd.Parameters.AddWithValue("@std_batch_code", newBatch);
                        cmd.Parameters.AddWithValue("@std_password", newPassword);
                        cmd.Parameters.AddWithValue("@std_id", studentId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            BindData(); // refresh
                        }
                        else
                        {
                            MessageBox.Show("Update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }

       

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // prevent clicking header row
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Assuming columns are in order: 0=std_id, 1=std_name, 2=std_batch_code, 3=std_password
                textBox1.Text = row.Cells[1].Value.ToString(); // std_name
                textBox2.Text = row.Cells[2].Value.ToString(); // std_batch_code
                textBox3.Text = row.Cells[3].Value.ToString(); // std_password
            }
        }
    }


}
  
    




    
    
      
        
 





   

