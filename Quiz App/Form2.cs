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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            question_type a = new question_type();

            a.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Exam_Settings se = new Exam_Settings();
            se.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            add_student stt = new add_student();
            stt.Show();
            this.Hide();
        }

      

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Admin_Logincs w = new Admin_Logincs();
            w.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            add_courses w = new add_courses();
            w.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            add_admin w = new add_admin();
            w.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            view_scores w = new view_scores();
            w.Show();
            this.Hide();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT TOP 1 user_type FROM tbl_exam_settings";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int userType = Convert.ToInt32(result);

                        Form nextForm = null;

                        if (userType == 1)
                        {
                            nextForm = new MasterSheetsSelect();
                        }
                        else if (userType == 0)
                        {
                            nextForm = new GPA();
                        }
                        else
                        {
                            MessageBox.Show("Invalid user type found in settings.");
                            return;
                        }

                        // Show the new form and hide the current one
                        nextForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("No user type found in settings. Please configure first.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
           Form4 w = new Form4();
            w.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            app_settings w = new app_settings();
            w.Show();
            this.Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
