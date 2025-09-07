using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class messageform : Form
    {
        private int score;
        private int totalQuestions;

        public messageform(int score, int totalQuestions)
        {
            InitializeComponent();
            this.score = score;
            this.totalQuestions = totalQuestions;
        }

        private void messageform_Load(object sender, EventArgs e)
        {
            float percentage = (totalQuestions > 0)
       ? ((float)score / totalQuestions) * 100
       : 0;

            label3.Text = $"Your Score is {score}";
            label5.Text = $"Your Percentage = {percentage:F2} %";

            string remark = GetRemark(percentage);
            label2.Text = remark; 
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

        private void button1_Click(object sender, EventArgs e)
        {
            Print_Screen P = new Print_Screen();
            P.Show();
            this.Hide();
        }
        private string GetRemark(float percentage)
        {
            if (percentage <= 40)
                return "Bad. Try better.";
            else if (percentage <= 50)
                return "Pass. You can do better.";
            else if (percentage <= 60)
                return "Fair. Keep improving.";
            else if (percentage <= 70)
                return "Good. Nice work.";
            else if (percentage <= 80)
                return "Very Good. Keep it up!";
            else if (percentage <= 90)
                return "Excellent Performance!";
            else // 91 - 100
                return "Outstanding! You're a star!";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
