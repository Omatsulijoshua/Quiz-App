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
    public partial class calculator : BaseForm
    {


        Double Firstnumber;
        Double Secondnumber;
        Double Answer;
        string op;
        public calculator()
        {
            InitializeComponent();
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    // Check if button text is a number
                    if ("0123456789".Contains(btn.Text))
                    {
                        btn.Click += button_Click;   // Number handler
                    }
                    // Check if it's one of the operators
                    else if ("+-÷X".Contains(btn.Text))
                    {
                        btn.Click += Arithemetic_Op; // Operator handler
                    }
                    // Special buttons
                    else if (btn.Text == "=")
                    {
                        btn.Click += button2_Click; // Equals handler
                    }
                    else if (btn.Text == "C")
                    {
                        btn.Click += button3_Click; // Clear
                    }
                    else if (btn.Text == "?")  // delete button
                    {
                        btn.Click += button1_Click;
                    }
                }

            }
        }

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

        private void calculator_Load(object sender, EventArgs e)
        {
            calculator.ScaleForm(this);
        }

        private double Compute(double a, double b, string oper)
        {
            switch (oper)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "X":
                case "×": return a * b;
                case "÷":
                case "/": return b == 0 ? double.NaN : a / b;
                default: return b; // fallback
            }
        }

        private void Arithemetic_Op(object sender, EventArgs e)
        {
            var newOp = ((Button)sender).Text.Trim();

            // If there is a pending op, resolve it first (chaining: 2 + 3 + 4)
            if (!string.IsNullOrEmpty(op))
            {
                if (double.TryParse(label1.Text, out double current))
                {
                    Firstnumber = Compute(Firstnumber, current, op);
                    label1.Text = "0"; // ready for next entry
                }
            }
            else
            {
                // No pending op yet: capture first number
                if (!double.TryParse(label1.Text, out Firstnumber)) Firstnumber = 0;
                label1.Text = "0";
            }

            op = newOp;
            label2.Text = $"{Firstnumber} {op}";

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

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(op)) return; // nothing to do

            if (!double.TryParse(label1.Text, out double second)) second = 0;

            double result = Compute(Firstnumber, second, op);
            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                MessageBox.Show("Cannot divide by zero!");
                return;
            }

            label1.Text = result.ToString();
            label2.Text = "";
            Firstnumber = result; // allow continuing with result
            op = "";
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

        private void button18_Click(object sender, EventArgs e)
        {
            if (!label1.Text.Contains("."))
            {
                label1.Text = label1.Text + ".";
            }
        }
    }
}

