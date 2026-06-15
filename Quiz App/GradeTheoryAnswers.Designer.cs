namespace Quiz_App
{
    partial class GradeTheoryAnswers
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
            this.btnSaveScores = new System.Windows.Forms.Button();
            this.dgvGrading = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBatch = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbStudent = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoadStudentAnswers = new Guna.UI2.WinForms.Guna2Button();
            this.cmbExam = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFinalizeGrades = new Guna.UI2.WinForms.Guna2Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrading)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSaveScores
            // 
            this.btnSaveScores.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveScores.Location = new System.Drawing.Point(349, 875);
            this.btnSaveScores.Name = "btnSaveScores";
            this.btnSaveScores.Size = new System.Drawing.Size(308, 50);
            this.btnSaveScores.TabIndex = 0;
            this.btnSaveScores.Text = "Save All Scores";
            this.btnSaveScores.UseVisualStyleBackColor = true;
            this.btnSaveScores.Click += new System.EventHandler(this.btnSaveScores_Click);
            // 
            // dgvGrading
            // 
            this.dgvGrading.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrading.Location = new System.Drawing.Point(349, 327);
            this.dgvGrading.Name = "dgvGrading";
            this.dgvGrading.Size = new System.Drawing.Size(916, 483);
            this.dgvGrading.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(415, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(437, 41);
            this.label1.TabIndex = 2;
            this.label1.Text = "Theory Grading Panel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbBatch
            // 
            this.cmbBatch.BackColor = System.Drawing.Color.Transparent;
            this.cmbBatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatch.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBatch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbBatch.ItemHeight = 30;
            this.cmbBatch.Location = new System.Drawing.Point(481, 134);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(784, 36);
            this.cmbBatch.TabIndex = 3;
            // 
            // cmbStudent
            // 
            this.cmbStudent.BackColor = System.Drawing.Color.Transparent;
            this.cmbStudent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudent.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStudent.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbStudent.ItemHeight = 30;
            this.cmbStudent.Location = new System.Drawing.Point(481, 190);
            this.cmbStudent.Name = "cmbStudent";
            this.cmbStudent.Size = new System.Drawing.Size(784, 36);
            this.cmbStudent.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(345, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select Batch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(345, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select Studen";
            // 
            // btnLoadStudentAnswers
            // 
            this.btnLoadStudentAnswers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLoadStudentAnswers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLoadStudentAnswers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLoadStudentAnswers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLoadStudentAnswers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadStudentAnswers.ForeColor = System.Drawing.Color.White;
            this.btnLoadStudentAnswers.Location = new System.Drawing.Point(481, 249);
            this.btnLoadStudentAnswers.Name = "btnLoadStudentAnswers";
            this.btnLoadStudentAnswers.Size = new System.Drawing.Size(180, 45);
            this.btnLoadStudentAnswers.TabIndex = 5;
            this.btnLoadStudentAnswers.Text = "Load Answers";
            this.btnLoadStudentAnswers.Click += new System.EventHandler(this.btnLoadStudentAnswers_Click);
            // 
            // cmbExam
            // 
            this.cmbExam.BackColor = System.Drawing.Color.Transparent;
            this.cmbExam.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbExam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExam.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbExam.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbExam.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbExam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbExam.ItemHeight = 30;
            this.cmbExam.Location = new System.Drawing.Point(481, 82);
            this.cmbExam.Name = "cmbExam";
            this.cmbExam.Size = new System.Drawing.Size(784, 36);
            this.cmbExam.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(345, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Select Exam";
            // 
            // btnFinalizeGrades
            // 
            this.btnFinalizeGrades.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFinalizeGrades.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFinalizeGrades.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFinalizeGrades.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFinalizeGrades.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinalizeGrades.ForeColor = System.Drawing.Color.White;
            this.btnFinalizeGrades.Location = new System.Drawing.Point(993, 875);
            this.btnFinalizeGrades.Name = "btnFinalizeGrades";
            this.btnFinalizeGrades.Size = new System.Drawing.Size(272, 45);
            this.btnFinalizeGrades.TabIndex = 7;
            this.btnFinalizeGrades.Text = "Finalize Grades";
            this.btnFinalizeGrades.Click += new System.EventHandler(this.btnFinalizeGrades_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(906, 810);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(221, 52);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(735, 823);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "TOTAL SCORE:";
            // 
            // GradeTheoryAnswers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1531, 999);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnFinalizeGrades);
            this.Controls.Add(this.cmbExam);
            this.Controls.Add(this.btnLoadStudentAnswers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbStudent);
            this.Controls.Add(this.cmbBatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvGrading);
            this.Controls.Add(this.btnSaveScores);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GradeTheoryAnswers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GradeTheoryAnswers";
            this.Load += new System.EventHandler(this.GradeTheoryAnswers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSaveScores;
        private System.Windows.Forms.DataGridView dgvGrading;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ComboBox cmbBatch;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStudent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2Button btnLoadStudentAnswers;
        private Guna.UI2.WinForms.Guna2ComboBox cmbExam;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2Button btnFinalizeGrades;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label5;
    }
}
