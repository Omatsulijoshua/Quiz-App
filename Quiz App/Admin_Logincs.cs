using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Admin_Logincs : Form
    {
        public static string fk_ad;
        public Admin_Logincs()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string user_name = textBox1.Text;
            string password = textBox2.Text;
            string user_db, passworddb;


            return_class rc = new return_class();
            user_db = rc.scalerReturn("select COUNT(ad_id) from tbl_admin where ad_name = '" +user_name+"' ");

            if (user_db.Equals("0"))
            {
                MessageBox.Show("Invalid Username");
            }
            else
            {
                passworddb = rc.scalerReturn("select ad_password from tbl_admin where ad_name = '" + user_name + "' ");


                if (passworddb.Equals(password))
                {
                    fk_ad = rc.scalerReturn("select ad_id from tbl_admin where ad_name ='" + user_name + "'");

                    Form2 f = new Form2();
                    f.Show();
                    this.Hide();


                }
                else
                {
                    MessageBox.Show("Invalid Password");
                }
            }

        }

        private void Admin_Logincs_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form1 w = new Form1();
            w.Show();
            this.Hide();
           
        }
    }
}
