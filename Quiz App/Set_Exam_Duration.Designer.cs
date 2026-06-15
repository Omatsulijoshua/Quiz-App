using System;
using System.Drawing;
using System.Windows.Forms;



namespace Quiz_App

{
    partial class Set_Exam_Duration
    {
        private System.Windows.Forms.Label labelSelectExam;
        private System.Windows.Forms.ComboBox comboBoxExams;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.Button btnSaveDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelSelectExam = new System.Windows.Forms.Label();
            this.comboBoxExams = new System.Windows.Forms.ComboBox();
            this.labelDuration = new System.Windows.Forms.Label();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.btnSaveDuration = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.saveTheoryDuration = new Guna.UI2.WinForms.Guna2Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSelectExam
            // 
            this.labelSelectExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectExam.Location = new System.Drawing.Point(55, 67);
            this.labelSelectExam.Name = "labelSelectExam";
            this.labelSelectExam.Size = new System.Drawing.Size(151, 33);
            this.labelSelectExam.TabIndex = 0;
            this.labelSelectExam.Text = "Select Exam:";
            // 
            // comboBoxExams
            // 
            this.comboBoxExams.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxExams.Location = new System.Drawing.Point(235, 68);
            this.comboBoxExams.Name = "comboBoxExams";
            this.comboBoxExams.Size = new System.Drawing.Size(366, 33);
            this.comboBoxExams.TabIndex = 1;
            this.comboBoxExams.Text = "Select Exam to Change Duration";
            // 
            // labelDuration
            // 
            this.labelDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuration.Location = new System.Drawing.Point(55, 123);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(125, 25);
            this.labelDuration.TabIndex = 2;
            this.labelDuration.Text = "Duration (minutes):";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDuration.Location = new System.Drawing.Point(235, 127);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(120, 31);
            this.numericUpDownDuration.TabIndex = 3;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // btnSaveDuration
            // 
            this.btnSaveDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDuration.Location = new System.Drawing.Point(235, 195);
            this.btnSaveDuration.Name = "btnSaveDuration";
            this.btnSaveDuration.Size = new System.Drawing.Size(259, 72);
            this.btnSaveDuration.TabIndex = 4;
            this.btnSaveDuration.Text = "Save Ob bject Duration";
            this.btnSaveDuration.Click += new System.EventHandler(this.btnSaveDuration_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(488, 273);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(386, 243);
            this.dataGridView1.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(667, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Duration (minutes):";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(667, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 33);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select Exam:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(869, 118);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 31);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.Location = new System.Drawing.Point(869, 59);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(366, 33);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "Select Exam to Change Duration";
            // 
            // saveTheoryDuration
            // 
            this.saveTheoryDuration.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.saveTheoryDuration.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.saveTheoryDuration.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.saveTheoryDuration.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.saveTheoryDuration.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveTheoryDuration.ForeColor = System.Drawing.Color.White;
            this.saveTheoryDuration.Location = new System.Drawing.Point(870, 195);
            this.saveTheoryDuration.Name = "saveTheoryDuration";
            this.saveTheoryDuration.Size = new System.Drawing.Size(233, 72);
            this.saveTheoryDuration.TabIndex = 57;
            this.saveTheoryDuration.Text = "Save Theory Duration";
            this.saveTheoryDuration.Click += new System.EventHandler(this.saveTheoryDuration_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(242, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(271, 33);
            this.label3.TabIndex = 0;
            this.label3.Text = "Save Object Exam";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(865, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(318, 33);
            this.label4.TabIndex = 0;
            this.label4.Text = "Save theory exam";
            // 
            // Set_Exam_Duration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1286, 803);
            this.Controls.Add(this.saveTheoryDuration);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelSelectExam);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.comboBoxExams);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.numericUpDownDuration);
            this.Controls.Add(this.btnSaveDuration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Set_Exam_Duration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.Set_Exam_Duration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox7;
        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private ComboBox comboBox1;
        private Guna.UI2.WinForms.Guna2Button saveTheoryDuration;
        private Label label3;
        private Label label4;
    }
}
