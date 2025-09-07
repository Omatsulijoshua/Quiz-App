
namespace Quiz_App
{
    partial class Print_Screen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.buttonImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxSection = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxLevel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButtonMale = new System.Windows.Forms.RadioButton();
            this.radioButtonFemale = new System.Windows.Forms.RadioButton();
            this.buttonGenerateResult = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.comboBoxExam = new System.Windows.Forms.ComboBox();
            this.tblexamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.quizAppDataSet14 = new Quiz_App.quizAppDataSet14();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbl_examsTableAdapter = new Quiz_App.quizAppDataSet14TableAdapters.tbl_examsTableAdapter();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblexamsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.quizAppDataSet14)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BackColor = System.Drawing.Color.White;
            this.pictureBoxImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxImage.Location = new System.Drawing.Point(12, 62);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(144, 117);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            // 
            // buttonImage
            // 
            this.buttonImage.BackColor = System.Drawing.Color.Blue;
            this.buttonImage.FlatAppearance.BorderSize = 0;
            this.buttonImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImage.ForeColor = System.Drawing.Color.White;
            this.buttonImage.Location = new System.Drawing.Point(26, 206);
            this.buttonImage.Name = "buttonImage";
            this.buttonImage.Size = new System.Drawing.Size(111, 34);
            this.buttonImage.TabIndex = 1;
            this.buttonImage.Text = "Image";
            this.buttonImage.UseVisualStyleBackColor = false;
            this.buttonImage.Click += new System.EventHandler(this.buttonImage_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxName.Location = new System.Drawing.Point(227, 62);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(378, 13);
            this.textBoxName.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Location = new System.Drawing.Point(228, 85);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(380, 2);
            this.panel1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(665, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Section:";
            // 
            // comboBoxSection
            // 
            this.comboBoxSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxSection.FormattingEnabled = true;
            this.comboBoxSection.Items.AddRange(new object[] {
            "2019/2020",
            "2020/2021",
            "2021/2022",
            "2022/2023",
            "2023/2024",
            "2024/2025",
            "2025/2026",
            "2026/2027",
            "2027/2028",
            "2028/2029",
            "2029/2030",
            "2030/2031",
            "2031/2032",
            "2032/2033",
            "2033/2034",
            "2034/2035",
            "2035/2036",
            "2036/2037",
            "2037/2038",
            "2038/2039",
            "2039/2040",
            "2040/2041",
            "2041/2042",
            "2042/2043",
            "2043/2044",
            "2044/2045",
            "2045/2046",
            "2046/2047",
            "2047/2048",
            "2048/2049",
            "2049/2050",
            "2050/2051",
            "2051/2052",
            "2052/2053",
            "2053/2054",
            "2054/2055",
            "2055/2056",
            "2056/2057",
            "2057/2058",
            "2058/2059",
            "2059/2060",
            "2060/2061",
            "2061/2062",
            "2062/2063",
            "2063/2064",
            "2064/2065",
            "2065/2066",
            "2066/2067",
            "2067/2068",
            "2068/2069",
            "2069/2070",
            "2070/2071",
            "2071/2072",
            "2072/2073",
            "2073/2074",
            "2074/2075",
            "2075/2076",
            "2076/2077",
            "2077/2078",
            "2078/2079",
            "2079/2080",
            "2080/2081",
            "2081/2082",
            "2082/2083",
            "2083/2084",
            "2084/2085",
            "2085/2086",
            "2086/2087",
            "2087/2088",
            "2088/2089",
            "2089/2090",
            "2090/2091",
            "2091/2092",
            "2092/2093",
            "2093/2094",
            "2094/2095",
            "2095/2096",
            "2096/2097",
            "2097/2098",
            "2098/2099",
            "2099/2100"});
            this.comboBoxSection.Location = new System.Drawing.Point(668, 63);
            this.comboBoxSection.Name = "comboBoxSection";
            this.comboBoxSection.Size = new System.Drawing.Size(75, 21);
            this.comboBoxSection.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Location = new System.Drawing.Point(668, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(63, 2);
            this.panel2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Level:";
            // 
            // comboBoxLevel
            // 
            this.comboBoxLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxLevel.FormattingEnabled = true;
            this.comboBoxLevel.Items.AddRange(new object[] {
            "100Level First Semester",
            "100Level Second Semester",
            "200Level First Semester",
            "200Level Second Semester",
            ""});
            this.comboBoxLevel.Location = new System.Drawing.Point(227, 182);
            this.comboBoxLevel.Name = "comboBoxLevel";
            this.comboBoxLevel.Size = new System.Drawing.Size(125, 21);
            this.comboBoxLevel.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(665, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Gender:";
            // 
            // radioButtonMale
            // 
            this.radioButtonMale.AutoSize = true;
            this.radioButtonMale.Location = new System.Drawing.Point(716, 164);
            this.radioButtonMale.Name = "radioButtonMale";
            this.radioButtonMale.Size = new System.Drawing.Size(48, 17);
            this.radioButtonMale.TabIndex = 6;
            this.radioButtonMale.TabStop = true;
            this.radioButtonMale.Text = "Male";
            this.radioButtonMale.UseVisualStyleBackColor = true;
            // 
            // radioButtonFemale
            // 
            this.radioButtonFemale.AutoSize = true;
            this.radioButtonFemale.Location = new System.Drawing.Point(716, 187);
            this.radioButtonFemale.Name = "radioButtonFemale";
            this.radioButtonFemale.Size = new System.Drawing.Size(59, 17);
            this.radioButtonFemale.TabIndex = 6;
            this.radioButtonFemale.TabStop = true;
            this.radioButtonFemale.Text = "Female";
            this.radioButtonFemale.UseVisualStyleBackColor = true;
            // 
            // buttonGenerateResult
            // 
            this.buttonGenerateResult.BackColor = System.Drawing.Color.Blue;
            this.buttonGenerateResult.FlatAppearance.BorderSize = 0;
            this.buttonGenerateResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGenerateResult.ForeColor = System.Drawing.Color.White;
            this.buttonGenerateResult.Location = new System.Drawing.Point(227, 273);
            this.buttonGenerateResult.Name = "buttonGenerateResult";
            this.buttonGenerateResult.Size = new System.Drawing.Size(111, 34);
            this.buttonGenerateResult.TabIndex = 1;
            this.buttonGenerateResult.Text = "Generate Result";
            this.buttonGenerateResult.UseVisualStyleBackColor = false;
            this.buttonGenerateResult.Click += new System.EventHandler(this.buttonGenerateResult_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightGray;
            this.panel3.Location = new System.Drawing.Point(227, 209);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(125, 2);
            this.panel3.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(407, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Select The Exam You Just Finish Writing";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightGray;
            this.panel4.Location = new System.Drawing.Point(410, 209);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(195, 2);
            this.panel4.TabIndex = 4;
            // 
            // comboBoxExam
            // 
            this.comboBoxExam.DataSource = this.tblexamsBindingSource;
            this.comboBoxExam.DisplayMember = "ex_name";
            this.comboBoxExam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxExam.FormattingEnabled = true;
            this.comboBoxExam.Location = new System.Drawing.Point(410, 182);
            this.comboBoxExam.Name = "comboBoxExam";
            this.comboBoxExam.Size = new System.Drawing.Size(198, 21);
            this.comboBoxExam.TabIndex = 5;
            // 
            // tblexamsBindingSource
            // 
            this.tblexamsBindingSource.DataMember = "tbl_exams";
            this.tblexamsBindingSource.DataSource = this.quizAppDataSet14;
            // 
            // quizAppDataSet14
            // 
            this.quizAppDataSet14.DataSetName = "quizAppDataSet14";
            this.quizAppDataSet14.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(483, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "label6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(483, 314);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "label7";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(412, 269);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Score:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(412, 314);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Percentage:";
            // 
            // tbl_examsTableAdapter
            // 
            this.tbl_examsTableAdapter.ClearBeforeFill = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(412, 361);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Performance:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(483, 361);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "label7";
            // 
            // Print_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.radioButtonFemale);
            this.Controls.Add(this.radioButtonMale);
            this.Controls.Add(this.comboBoxExam);
            this.Controls.Add(this.comboBoxLevel);
            this.Controls.Add(this.comboBoxSection);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonGenerateResult);
            this.Controls.Add(this.buttonImage);
            this.Controls.Add(this.pictureBoxImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Print_Screen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print_Screen";
            this.Load += new System.EventHandler(this.Print_Screen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblexamsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.quizAppDataSet14)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Button buttonImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSection;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonMale;
        private System.Windows.Forms.RadioButton radioButtonFemale;
        private System.Windows.Forms.Button buttonGenerateResult;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox comboBoxExam;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private quizAppDataSet14 quizAppDataSet14;
        private System.Windows.Forms.BindingSource tblexamsBindingSource;
        private quizAppDataSet14TableAdapters.tbl_examsTableAdapter tbl_examsTableAdapter;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}