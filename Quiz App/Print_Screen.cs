using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Quiz_App
{
    public partial class Print_Screen : Form
    {
        public string ScoreID { get; set; } = "";
        public string score { get; set; } = "";
        public string percentage { get; set; } = "";

        public string Gender, SName, Section, Level;
        public Image img = null;

        private bool isExternalData = false;

        public Print_Screen()
        {
            InitializeComponent();
        }

        // Method to receive external score data (from other form)
        public void UpdateData(string scoreID, string score, string percentage)
        {
            this.ScoreID = scoreID;
            this.score = score;
            this.percentage = percentage;
            isExternalData = true;
        }

        private void Print_Screen_Load(object sender, EventArgs e)
        {
            // If external data was provided
            if (isExternalData)
            {
                label6.Text = score;
                label7.Text = percentage + "%";
            }
            else
            {
                // Use fallback: Test.score
                score = Test.score.ToString();
                label6.Text = score;

                double per = (Test.score / (float)40) * 100;
                percentage = per.ToString("0.00");
                label7.Text = percentage + "%";
            }

            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT ex_id, ex_name FROM tbl_exams", con); // make sure tbl_exam and exam_name exist
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBoxExam.DataSource = dt;
                comboBoxExam.DisplayMember = "ex_name"; // what user sees
                comboBoxExam.ValueMember = "ex_id";       // what app uses
                comboBoxExam.SelectedIndex = -1;
            }
        }

        private void UploadImage()
        {
            try
            {
                openFileDialog1.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    pictureBoxImage.Image = Image.FromFile(openFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Image Upload.\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonImage_Click(object sender, EventArgs e)
        {
            UploadImage();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintPage);

            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument;
            previewDialog.ShowDialog();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 14);
            float y = 100;

            e.Graphics.DrawString("Score Report", new Font("Arial", 18, FontStyle.Bold), Brushes.Black, 100, y);
            y += 40;
            e.Graphics.DrawString("Score ID: " + ScoreID, font, Brushes.Black, 100, y);
            y += 30;
            e.Graphics.DrawString("Score: " + score, font, Brushes.Black, 100, y);
            y += 30;
            e.Graphics.DrawString("Percentage: " + percentage + "%", font, Brushes.Black, 100, y);
        }

        private void buttonGenerateResult_Click(object sender, EventArgs e)
        {
            if (radioButtonMale.Checked)
                Gender = "Male";
            else if (radioButtonFemale.Checked)
                Gender = "Female";

            SName = textBoxName.Text;
            Section = comboBoxSection.SelectedItem != null ? comboBoxSection.SelectedItem.ToString() : "";
            Level = comboBoxLevel.SelectedItem != null ? comboBoxLevel.SelectedItem.ToString() : "";

            GenerateResult frm2 = new GenerateResult();
            frm2.img = pictureBoxImage.Image;
            frm2.SName = SName;
            frm2.Section = Section;
            frm2.Level = Level;
            frm2.Gender = Gender;
            frm2.Show();
        }
    }
}
