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





namespace Quiz_App
{
    public partial class add_admin : Form
    {
        public add_admin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = connection_class.GetConnection();
           // SqlConnection con = new SqlConnection(connection);
            SqlCommand command = new SqlCommand();
            con.Open();
            command.Connection = con;
            command.CommandText = "insert into tbl_admin" + "(ad_name,ad_password)" + "values(@ad_name, @ad_password)";
            // command.Parameters.AddWithValue("@ad_name", textBox1.Text;
            command.Parameters.AddWithValue("@ad_name", textBox1.Text);
            command.Parameters.AddWithValue("@ad_password", textBox2.Text);
            MessageBox.Show("Successfully Inserted");
            command.ExecuteNonQuery();
            con.Close();
            textBox1.Text = "";
            BindData();
        }

        void BindData()
        {
            SqlConnection con = connection_class.GetConnection();
            {
                con.Open();

                // Create a SQL command to select all data from tbl_exam
                string sqlQuery = "SELECT * FROM tbl_admin";
                SqlCommand command = new SqlCommand(sqlQuery, con);

                // Create a SqlDataAdapter to fetch the data
                SqlDataAdapter sd = new SqlDataAdapter(command);

                // Create a new DataTable to hold the fetched data
                DataTable dt = new DataTable();

                // Fill the DataTable with the fetched data from the SqlDataAdapter
                sd.Fill(dt);

                // Set the DataSource of the DataGridView to the DataTable
                // **Set DataSource here**
                dataGridView1.DataSource = dt;

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

                con.Close();
            }
        }

        private void add_admin_Load(object sender, EventArgs e)

        {
            // Make sure this is set on Form Load or Constructor
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = true;
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            BindData();
        }

        private void button2_Click(object sender, EventArgs e)
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
                            "Are you sure you want to delete the selected admin(s)?",
                            "Confirm Delete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result != DialogResult.Yes) return;

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            if (row.Cells["ad_name"].Value != null) // use your actual column name here
                            {
                                string adminName = row.Cells["ad_name"].Value.ToString();

                                string deleteQuery = "DELETE FROM tbl_admin WHERE ad_name = @AdName";
                                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@AdName", adminName);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Selected admin(s) deleted successfully.");
                    }

                    else
                    {
                        MessageBox.Show("Please select admin(s) or enter an admin name to delete.");
                        transaction.Rollback();
                        return;
                    }

                    // ✅ Refresh DataGridView manually after deletion
                    string selectQuery = "SELECT * FROM tbl_admin";
                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
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
                    MessageBox.Show("An error occurred while deleting admin: " + ex.Message);
                }
            }




        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form2 w = new Form2();
            w.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls;*.xlsx;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        // Automatically detects Excel file format
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });

                            // Assuming first sheet contains the data
                            DataTable dt = result.Tables[0];

                            SqlConnection conn = connection_class.GetConnection();
                            {
                                conn.Open();

                                foreach (DataRow row in dt.Rows)
                                {
                                    string adName = row["ad_name"].ToString().Trim();
                                    string adPassword = row["ad_password"].ToString().Trim();

                                    if (!string.IsNullOrEmpty(adName) && !string.IsNullOrEmpty(adPassword))
                                    {
                                        string insertQuery = "INSERT INTO tbl_admin (ad_name, ad_password) VALUES (@name, @pass)";
                                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                                        {
                                            cmd.Parameters.AddWithValue("@name", adName);
                                            cmd.Parameters.AddWithValue("@pass", adPassword);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }

                                MessageBox.Show("Admins imported successfully!");
                            }

                            BindData(); // Optional: refresh DataGridView
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Please close the Excel file before importing.", "File In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred:\n" + ex.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnDownloadSample_Click(object sender, EventArgs e)
        {
            

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SqlConnection con = connection_class.GetConnection();
                {
                    if (MessageBox.Show("Are you sure to delete?", "DElete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {


                        con.Open();

                        // Create the SQL command with parameters
                        string sqlQuery = "DELETE FROM tbl_admin WHERE ad_name = @AdName";

                        SqlCommand command = new SqlCommand(sqlQuery, con);
                        command.Parameters.AddWithValue("@AdName", textBox1.Text);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successfully Deleted");
                            BindData(); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No rows were deleted. The course name may not exist.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please type in the admin to be deleted.");
            }
         
        }

        private void btnDownloadSample_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "admins_sample.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Create the workbook and worksheet
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Admins");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "ad_name";
                        worksheet.Cell(1, 2).Value = "ad_password";

                        // Optional: Add example rows
                        worksheet.Cell(2, 1).Value = "admin1";
                        worksheet.Cell(2, 2).Value = "pass1";

                        worksheet.Cell(3, 1).Value = "admin2";
                        worksheet.Cell(3, 2).Value = "pass2";

                        // Save to file
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show("Sample Excel file downloaded successfully.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
