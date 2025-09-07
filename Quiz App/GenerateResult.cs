using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

using System.Windows.Forms;

namespace Quiz_App
{
    public partial class GenerateResult : Form
    {
        public string ScoreID { get; set; }
        public string score { get; set; }

        public string percentage { get; set; }




        public void UpdateData(string scoreID, string score, string percentage)
        {
            ScoreID = scoreID;
            this.score = score;
            this.percentage = percentage;
        }




        public string Date, SName, Section, Level, Gender, Exam;
        public Image img = null;
        public GenerateResult()
        {
            InitializeComponent();
            Date = DateTime.Now.ToString("M/d/yyyy"); 
        }

        private void Print(Panel pnl)
        {
            PrinterSettings ps = new PrinterSettings();
            panelPrint = pnl;
            getprintarea(pnl);
            printPreviewDialog1.Document = printDocument1;
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printPreviewDialog1.Show();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            e.Graphics.DrawImage(memoryimg, (pagearea.Width / 2) - (this.panelPrint.Width / 2), this.panelPrint.Location.Y);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form1 w = new Form1();
            w.Show();
            this.Hide();
        }

        private void pictureBoxPrint_Click(object sender, EventArgs e)
        {
            Print(this.panelPrint);
        }

        private Bitmap memoryimg;
        private void getprintarea(Panel pnl)
        {
            memoryimg = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(memoryimg, new Rectangle(0, 0,  pnl.Width, pnl.Height));
        }

        private void pictureBoxPrint_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBoxPrint, "Print");
        }

        private void GenerateResult_Load(object sender, EventArgs e)
        {


            labelScore.Text = score;

            labelPercentage.Text = percentage;

            labelScore.Text = Test.score.ToString();
            double per = (Test.score / (float)40) * 100;

            labelPercentage.Text = per.ToString("0.00");

            labelDate.Text = Date;
            pictureBoxPic.Image = img;
            labelName.Text = SName;
            labelSection.Text = Section;
            labelLevel.Text = Level;
            labelGender.Text = Gender;
            labelExam.Text = Exam;



           

        }
    }
}
