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
    public partial class MasterSheetsSelect : Form
    {
        public MasterSheetsSelect()
        {
            InitializeComponent();
        }

        private void MasterSheetsSelect_Load(object sender, EventArgs e)
        {
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
            StyleDataGridView1(); // Apply style after data binding
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

        private void StyleDataGridView1()
        {
            dataGridView1.BackgroundColor = Color.LightYellow;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.DefaultCellStyle.ForeColor = Color.DarkBlue;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ClearSelection();
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
