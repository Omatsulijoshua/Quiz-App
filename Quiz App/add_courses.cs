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
    public partial class add_courses : BaseForm
    {
        public add_courses()
        {
            InitializeComponent();
        }

        // Base resolution (your dev machine)
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
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = connection_class.GetConnection();
            //SqlConnection con = new SqlConnection(connection);
            SqlCommand command = new SqlCommand();
            con.Open();
            command.Connection = con;
            command.CommandText = "insert into tbl_exams" + "(ex_name)" + "values(@ex_name)";
            // //command.Parameters.AddWithValue("@ex_id", int.Parse(textBox1.Text));
            command.Parameters.AddWithValue("@ex_name", textBox1.Text);
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
                string sqlQuery = "SELECT * FROM tbl_exams";
                SqlCommand command = new SqlCommand(sqlQuery, con);

                // Create a SqlDataAdapter to fetch the data
                SqlDataAdapter sd = new SqlDataAdapter(command);

                // Create a new DataTable to hold the fetched data
                DataTable dt = new DataTable();

                // Fill the DataTable with the fetched data from the SqlDataAdapter
                sd.Fill(dt);

                // Set the DataSource of the DataGridView to the DataTable
                dataGridView1.DataSource = dt;
                ModernUi.StyleDataGridView(dataGridView1);
                dataGridView1.ClearSelection();

                con.Close();
            }
        }

        private void add_courses_Load(object sender, EventArgs e)
        {
            add_courses.ScaleForm(this);
            BindData();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult selectedDeleteResult = MessageBox.Show(
                    $"Are you sure you want to delete {dataGridView1.SelectedRows.Count} selected course(s)?",
                    "Delete Record",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (selectedDeleteResult != DialogResult.Yes)
                {
                    return;
                }

                using (SqlConnection con = connection_class.GetConnection())
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                            {
                                if (row.IsNewRow)
                                {
                                    continue;
                                }

                                object examNameValue = row.Cells["ex_name"].Value;
                                if (examNameValue == null || examNameValue == DBNull.Value)
                                {
                                    continue;
                                }

                                using (SqlCommand command = new SqlCommand("DELETE FROM tbl_exams WHERE ex_name = @ExName", con, transaction))
                                {
                                    command.Parameters.AddWithValue("@ExName", examNameValue.ToString());
                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            MessageBox.Show("Selected course(s) deleted successfully.");
                            BindData();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred while deleting courses: " + ex.Message);
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SqlConnection con = connection_class.GetConnection();
                {
                    if (MessageBox.Show("Are you sure to delete?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        con.Open();

                        string sqlQuery = "DELETE FROM tbl_exams WHERE ex_name = @ExName";

                        SqlCommand command = new SqlCommand(sqlQuery, con);
                        command.Parameters.AddWithValue("@ExName", textBox1.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successfully Deleted");
                            BindData();
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
                MessageBox.Show("Please select one or more courses, or type in the course to be deleted.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}

