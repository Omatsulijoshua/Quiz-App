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
    public partial class studentlogin : Form
    {

        public static string exam_id, studentid;



        public static string fk_ad;

        public studentlogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userId = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Student ID and Password.");
                return;
            }

            return_class rc = new return_class();

            // ✅ Check if student exists
            string userExists = rc.scalerReturn("SELECT COUNT(std_id) FROM student_record WHERE std_id = " + userId);

            if (userExists == "0")
            {
                MessageBox.Show("Invalid Student ID");
                return;
            }

            // ✅ Get stored password
            string passwordDb = rc.scalerReturn("SELECT std_password FROM student_record WHERE std_id = " + userId);

            if (passwordDb != password)
            {
                MessageBox.Show("Invalid Password");
                return;
            }

            // ✅ Check if exam allocated to student
            string examAllocated = rc.scalerReturn(
                "SELECT COUNT(*) FROM set_exam WHERE stud_id_fk = " + userId +
                " AND exam_id_fk = " + comboBox1.SelectedValue.ToString());

            if (examAllocated == "0")
            {
                MessageBox.Show("This exam has not been allocated to this student.");
                return;
            }

            // ✅ Check if student has already written this exam
            string alreadyWritten = rc.scalerReturn(
                "SELECT COUNT(*) FROM score WHERE stud_fk_id = " + userId +
                " AND exam_fk_id = " + comboBox1.SelectedValue.ToString());

            if (alreadyWritten != "0")
            {
                MessageBox.Show("You have already written this exam. Please wait to be allocated a new one.");
                return;
            }

            // ✅ NEW: Check if exam has at least 1 question
            string questionCount = rc.scalerReturn(
                "SELECT COUNT(*) FROM tbl_questions WHERE exam_fk_id = " + comboBox1.SelectedValue.ToString());

            if (questionCount == "0")
            {
                MessageBox.Show("This exam has no questions. Please contact your instructor.");
                return;
            }

            // ✅ Login success → store values and open Test form
            studentid = userId;
            exam_id = comboBox1.SelectedValue.ToString();

            Test t = new Test();
            this.Hide();
            t.FormClosed += (s, args) => this.Close();
            t.Show();
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

        private void studentlogin_Load(object sender, EventArgs e)
        {
            SqlDataAdapter dadapter2 = new SqlDataAdapter("SELECT * FROM tbl_exams", connection_class.GetConnection());
            DataSet dset2 = new DataSet();
            dadapter2.Fill(dset2);

            // ✅ Sort alphabetically by ex_name
            DataView dv = new DataView(dset2.Tables[0]);
            dv.Sort = "ex_name ASC";  // <-- make sure your column name is really ex_name

            comboBox1.DataSource = dv;
            comboBox1.DisplayMember = "ex_name"; // what user sees
            comboBox1.ValueMember = "ex_id";     // the actual ID

        }
    }
}
