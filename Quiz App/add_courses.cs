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
    public partial class add_courses : Form
    {
        public add_courses()
        {
            InitializeComponent();
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

                con.Close();
            }
        }

        private void add_courses_Load(object sender, EventArgs e)
        {
            BindData();
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
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SqlConnection con = connection_class.GetConnection();
                {
                    if (MessageBox.Show("Are you sure to delete?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {


                        con.Open();

                        // Create the SQL command with parameters
                        string sqlQuery = "DELETE FROM tbl_exams WHERE ex_name = @ExName";

                        SqlCommand command = new SqlCommand(sqlQuery, con);
                        command.Parameters.AddWithValue("@ExName", textBox1.Text);

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
                MessageBox.Show("Please type in the course to be deleted.");
            }
        
    }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
