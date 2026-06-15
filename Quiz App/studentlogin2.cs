using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class studentlogin2 : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        public static string exam_id;
        public static string studentid;
        public static string fk_ad;

        private Panel heroPanel;
        private Label heroEyebrowLabel;
        private Label heroTitleLabel;
        private Label heroCopyLabel;
        private bool layoutEventsAttached;

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
            Form3 previousForm = new Form3();
            previousForm.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string studentId = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Enter both your student ID and password to continue.");
                return;
            }

            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Choose an exam before continuing.");
                return;
            }

            return_class rc = new return_class();
            string userDb = rc.scalerReturn("select COUNT(std_id) from student_record where std_id=" + studentId);

            if (userDb.Equals("0"))
            {
                MessageBox.Show("That student ID was not found.");
                return;
            }

            string passwordDb = rc.scalerReturn("select std_password from student_record where std_id =" + studentId);
            if (!passwordDb.Equals(password))
            {
                MessageBox.Show("The password you entered is incorrect.");
                return;
            }

            string selectedExamId = comboBox1.SelectedValue.ToString();
            string allocationCount = rc.scalerReturn(
                "SELECT COUNT(*) FROM set_exam WHERE stud_id_fk = " + studentId +
                " AND exam_id_fk = " + selectedExamId);

            if (allocationCount.Equals("0"))
            {
                MessageBox.Show("This past-question exam has not been allocated to this student.");
                return;
            }

            studentid = studentId;
            exam_id = selectedExamId;
            student_control_panel.ExamPreferences.SelectedExamId = Convert.ToInt32(selectedExamId);

            student_control_panel controlPanel = new student_control_panel();
            Hide();
            controlPanel.Show();
        }

        private void studentlogin2_Load(object sender, EventArgs e)
        {
            ModernUi.ScaleForScreen(this);
            ApplyPreferredWindowSize();
            ApplyResponsiveBounds(70, 60);
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(9, 15, 29), Color.FromArgb(20, 32, 52));
            AttachLayoutEvents();
            BuildLoginLayout();
            LoadExams();
            ModernUi.FadeIn(this);
        }

        private void LoadExams()
        {
            SqlDataAdapter examAdapter = new SqlDataAdapter("SELECT * FROM tbl_exams", connection_class.GetConnection());
            DataSet examDataSet = new DataSet();
            examAdapter.Fill(examDataSet);

            DataView sortedView = new DataView(examDataSet.Tables[0]);
            sortedView.Sort = "ex_name ASC";

            comboBox1.DataSource = sortedView;
            comboBox1.DisplayMember = "ex_name";
            comboBox1.ValueMember = "ex_id";
            comboBox1.SelectedIndex = -1;
        }

        private void BuildLoginLayout()
        {
            SuspendLayout();

            BackColor = Color.FromArgb(9, 15, 29);
            FormBorderStyle = FormBorderStyle.None;

            int sidePadding = Math.Max(24, ClientSize.Width / 26);
            int topPadding = Math.Max(56, ClientSize.Height / 9);
            int contentGap = Math.Max(20, ClientSize.Width / 32);
            int availableWidth = ClientSize.Width - (sidePadding * 2) - contentGap;
            int heroWidth = Math.Max(180, Math.Min(250, (int)(availableWidth * 0.38f)));
            int panelHeight = Math.Max(360, ClientSize.Height - topPadding - 44);
            int loginWidth = Math.Max(300, availableWidth - heroWidth);

            if (heroPanel == null)
            {
                heroPanel = ModernUi.CreateCard(new Rectangle(sidePadding, topPadding, heroWidth, panelHeight));
                Controls.Add(heroPanel);
                heroPanel.SendToBack();
            }
            else
            {
                heroPanel.Bounds = new Rectangle(sidePadding, topPadding, heroWidth, panelHeight);
            }

            label4.Text = "Past Questions";
            label4.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label4.ForeColor = ModernUi.Ink;
            label4.Location = new Point(heroPanel.Right + contentGap, 18);
            label4.Size = new Size(loginWidth, 38);

            groupBox1.Text = "Practice Session Sign In";
            groupBox1.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.ForeColor = ModernUi.Ink;
            groupBox1.Location = new Point(heroPanel.Right + contentGap, topPadding);
            groupBox1.Size = new Size(Math.Max(320, loginWidth), panelHeight);

            label1.Text = "Student ID";
            label2.Text = "Password";
            label3.Text = "Select Exam";
            label1.ForeColor = label2.ForeColor = label3.ForeColor = ModernUi.Ink;
            label1.Font = label2.Font = label3.Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(28, 70);
            label2.Location = new Point(28, 162);
            label3.Location = new Point(28, 254);

            ModernUi.StyleTextInput(textBox1);
            ModernUi.StyleTextInput(textBox2);
            ModernUi.StyleComboBox(comboBox1);
            ModernUi.StylePrimaryButton(button1);

            textBox1.Location = new Point(32, 102);
            textBox1.Size = new Size(groupBox1.Width - 68, 34);
            textBox2.Location = new Point(32, 194);
            textBox2.Size = new Size(groupBox1.Width - 68, 34);
            textBox2.UseSystemPasswordChar = true;
            comboBox1.Location = new Point(32, 286);
            comboBox1.Size = new Size(groupBox1.Width - 68, 36);

            button1.Text = "Open Practice Setup";
            button1.Location = new Point(32, 356);
            button1.Size = new Size(groupBox1.Width - 68, 46);

            if (heroEyebrowLabel == null)
            {
                heroEyebrowLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point), ModernUi.Warning, Point.Empty, Size.Empty, ContentAlignment.MiddleLeft);
                heroEyebrowLabel.Parent = heroPanel;
            }

            heroEyebrowLabel.Text = "Practice mode";
            heroEyebrowLabel.Location = new Point(20, 28);
            heroEyebrowLabel.Size = new Size(heroPanel.Width - 40, 22);

            if (heroTitleLabel == null)
            {
                heroTitleLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI Semibold", 21F, FontStyle.Bold, GraphicsUnit.Point), ModernUi.Ink, Point.Empty, Size.Empty, ContentAlignment.TopLeft);
                heroTitleLabel.Parent = heroPanel;
            }

            heroTitleLabel.Text = "Sharpen skills\nbefore exam day.";
            heroTitleLabel.Location = new Point(20, 72);
            heroTitleLabel.Size = new Size(heroPanel.Width - 40, 120);

            if (heroCopyLabel == null)
            {
                heroCopyLabel = ModernUi.CreateLabel(string.Empty, new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point), ModernUi.MutedInk, Point.Empty, Size.Empty, ContentAlignment.TopLeft);
                heroCopyLabel.Parent = heroPanel;
            }

            heroCopyLabel.Text = "Choose an allocated exam, set your practice preferences, and start a cleaner revision session.";
            heroCopyLabel.Location = new Point(20, Math.Min(heroPanel.Height - 150, 208));
            heroCopyLabel.Size = new Size(heroPanel.Width - 40, 120);

            pictureBox4.Cursor = Cursors.Hand;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Location = new Point(ClientSize.Width - pictureBox3.Width - 18, 12);
            pictureBox4.Location = new Point(14, 14);

            ResumeLayout();
        }

        private void AttachLayoutEvents()
        {
            if (layoutEventsAttached)
            {
                return;
            }

            layoutEventsAttached = true;
            Shown += (sender, e) => BuildLoginLayout();
            Resize += (sender, e) =>
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    BuildLoginLayout();
                }
            };
        }

        private void ApplyPreferredWindowSize()
        {
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int width = Math.Min(workingArea.Width - 80, 800);
            int height = Math.Min(workingArea.Height - 80, 560);

            ClientSize = new Size(Math.Max(640, width), Math.Max(460, height));
        }
    }
}

