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
    public partial class MasterSheetsSelect : BaseForm
    {
        public MasterSheetsSelect()
        {
            InitializeComponent();
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
        private void MasterSheetsSelect_Load(object sender, EventArgs e)
        {
            MasterSheetsSelect.ScaleForm(this);
            
            using (SqlConnection con = connection_class.GetConnection())
            {
                string query = "SELECT DISTINCT std_batch_code FROM student_record";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "std_batch_code";
                comboBox1.ValueMember = "std_batch_code"; // Enables use of SelectedValue
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid batch from the dropdown first.");
                return;
            }

            string batchCode = comboBox1.SelectedValue.ToString();
            string query = $"SELECT * FROM student_record WHERE std_id IN (SELECT std_id FROM set_exam) AND std_batch_code = '{batchCode}'";

            viewclass vc = new viewclass(query);
            dataGridView1.DataSource = vc.showrecord();
            ModernUi.StyleDataGridView(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                string selectedBatchCode = comboBox1.SelectedValue.ToString();
                MasterSheetForm form = new MasterSheetForm(selectedBatchCode);
                form.Show();
            }
            else
            {
                MessageBox.Show("Please select a valid batch code.");
            }
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
    }
}

