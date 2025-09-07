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
    public partial class studentlogin2 : Form
    {

        public static string exam_id, studentid;



        public static string fk_ad;
        public studentlogin2()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form3 w = new Form3();
            w.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user_name = textBox1.Text;
            string password = textBox2.Text;
            string user_db, passworddb;





            return_class rc = new return_class();
            user_db = rc.scalerReturn("select COUNT (std_id) from student_record where std_id=" + textBox1.Text);

            if (user_db.Equals("0"))
            {
                MessageBox.Show("Invalid Student ID");
            }
            else
            {
                passworddb = rc.scalerReturn("select std_password from student_record where std_id =" + textBox1.Text);


                if (passworddb.Equals(password))
                {
                    string val = rc.scalerReturn("SELECT COUNT(*) FROM set_exam WHERE stud_id_fk = " + textBox1.Text + " AND exam_id_fk = " + comboBox1.SelectedValue.ToString());

                    if (val.Equals("0"))
                    {
                        MessageBox.Show("This exam has not been allocated to this student.");
                    }
                    else
                    {
                        studentid = textBox1.Text;
                        exam_id = comboBox1.SelectedValue.ToString();

                        // Pass exam_id to ExamPreferences
                        Quiz_App.student_control_panel.ExamPreferences.SelectedExamId = Convert.ToInt32(comboBox1.SelectedValue);

                        // Go to student_control_panel to configure and start the test
                        student_control_panel t = new student_control_panel();
                        this.Hide();
                        t.Show();

                    }
                }

                else
                {
                    MessageBox.Show("Invalid Password");
                }
            }
        }

       

        private void studentlogin2_Load(object sender, EventArgs e)
        {
            SqlDataAdapter dadapter2 = new SqlDataAdapter(
        "SELECT * FROM tbl_exams", connection_class.GetConnection());
            DataSet dset2 = new DataSet();
            dadapter2.Fill(dset2);

            // ✅ Create DataView and sort alphabetically by exam name
            DataView dv = new DataView(dset2.Tables[0]);
            dv.Sort = "ex_name ASC";  // <-- make sure the column is spelled correctly

            comboBox1.DataSource = dv;
            comboBox1.DisplayMember = "ex_name"; // text shown to user
            comboBox1.ValueMember = "ex_id";     // underlying exam ID

            // ✅ Optional: no exam pre-selected (forces student to choose)
            comboBox1.SelectedIndex = -1;
        }
    }
}
