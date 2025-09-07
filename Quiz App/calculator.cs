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
    public partial class calculator : Form
        
    {
        Double Firstnumber;
        Double Secondnumber;
        Double Answer;
        string op;
        public calculator()
        {
            InitializeComponent();
        }

        

        private void button_Click(object sender, EventArgs e)
        {
            Button Number = (Button)sender;
            if (label1.Text == "0")
            {
                label1.Text = Number.Text;
            }
            else
            { 
                label1.Text = label1.Text + Number.Text;
            }
        }

        private void Arithemetic_Op(object sender, EventArgs e)
        {
            Button operation = (Button)sender;
            Firstnumber = Double.Parse(label1.Text);   
            label1.Text = "";
            op = operation.Text;
            label2.Text = System.Convert.ToString(Firstnumber) + " " + op;

        }

        private void button16_Click(object sender, EventArgs e)
        {
            label2.Text = "";

            Secondnumber = Double.Parse(label1.Text);
            switch (op)
            {
                case "+":
                    Answer = (Firstnumber + Secondnumber);
                    label1.Text = System.Convert.ToString(Answer);
                    break;


                case "-":
                    Answer = (Firstnumber - Secondnumber);
                    label1.Text = System.Convert.ToString(Answer);
                    break;

                case "X":
                    Answer = (Firstnumber * Secondnumber);
                    label1.Text = System.Convert.ToString(Answer);
                    break;

                case "/":
                    Answer = (Firstnumber / Secondnumber);
                    label1.Text = System.Convert.ToString(Answer);
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label1.Text.Length > 0)
            {
                label1.Text = label1.Text.Remove(label1.Text.Length - 1, 1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "0";
            label2.Text = "";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (!label1.Text.Contains("."))
            {
                label1.Text = label1.Text + ".";
            }
        }

        private void calculator_Load(object sender, EventArgs e)
        {

        }
    }
}
