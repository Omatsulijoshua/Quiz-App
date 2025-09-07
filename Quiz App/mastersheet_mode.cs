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
    public partial class mastersheet_mode : Form
    {
        public mastersheet_mode()
        {
            InitializeComponent();
        }

        private void btnSaveDuration_Click(object sender, EventArgs e)
        {
            int userType = -1;

            if (radioButtonSec.Checked)
                userType = 1;  // Secondary School
            else if (radioButtonTer.Checked)
                userType = 0;  // Tertiary Institution

            if (userType == -1)
            {
                MessageBox.Show("Please select an option.");
                return;
            }

            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    // Check if a setting already exists
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tbl_exam_settings", conn);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        SqlCommand updateCmd = new SqlCommand("UPDATE tbl_exam_settings SET user_type = @type", conn);
                        updateCmd.Parameters.AddWithValue("@type", userType);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand insertCmd = new SqlCommand("INSERT INTO tbl_exam_settings (user_type) VALUES (@type)", conn);
                        insertCmd.Parameters.AddWithValue("@type", userType);
                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Saved successfully ✅");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            app_settings stt = new app_settings();
            stt.Show();
            this.Hide();
        }
    }
}
