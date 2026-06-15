namespace Quiz_App
{
    partial class multi_GPA
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.batchFilterCombo = new System.Windows.Forms.ComboBox();
            this.examFilterCombo = new System.Windows.Forms.ComboBox();
            this.studentFilterCombo = new System.Windows.Forms.ComboBox();
            this.labelBatch = new System.Windows.Forms.Label();
            this.labelExam = new System.Windows.Forms.Label();
            this.labelStudent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(32, 26);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 42);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(974, 26);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 42);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(32, 176);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1092, 476);
            this.dataGridView1.TabIndex = 2;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(505, 28);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(79, 13);
            this.labelTitle.TabIndex = 3;
            this.labelTitle.Text = "Student CGPA";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(441, 684);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(58, 13);
            this.labelResult.TabIndex = 4;
            this.labelResult.Text = "CGPA = ?";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 678);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 52);
            this.button1.TabIndex = 5;
            this.button1.Text = "Load CGPA";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(934, 678);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 52);
            this.button2.TabIndex = 6;
            this.button2.Text = "Export to excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // batchFilterCombo
            // 
            this.batchFilterCombo.FormattingEnabled = true;
            this.batchFilterCombo.Location = new System.Drawing.Point(32, 104);
            this.batchFilterCombo.Name = "batchFilterCombo";
            this.batchFilterCombo.Size = new System.Drawing.Size(300, 21);
            this.batchFilterCombo.TabIndex = 7;
            this.batchFilterCombo.SelectedIndexChanged += new System.EventHandler(this.batchFilterCombo_SelectedIndexChanged);
            // 
            // examFilterCombo
            // 
            this.examFilterCombo.FormattingEnabled = true;
            this.examFilterCombo.Location = new System.Drawing.Point(364, 104);
            this.examFilterCombo.Name = "examFilterCombo";
            this.examFilterCombo.Size = new System.Drawing.Size(340, 21);
            this.examFilterCombo.TabIndex = 8;
            this.examFilterCombo.SelectedIndexChanged += new System.EventHandler(this.examFilterCombo_SelectedIndexChanged);
            // 
            // studentFilterCombo
            // 
            this.studentFilterCombo.FormattingEnabled = true;
            this.studentFilterCombo.Location = new System.Drawing.Point(736, 104);
            this.studentFilterCombo.Name = "studentFilterCombo";
            this.studentFilterCombo.Size = new System.Drawing.Size(388, 21);
            this.studentFilterCombo.TabIndex = 9;
            // 
            // labelBatch
            // 
            this.labelBatch.AutoSize = true;
            this.labelBatch.Location = new System.Drawing.Point(32, 82);
            this.labelBatch.Name = "labelBatch";
            this.labelBatch.Size = new System.Drawing.Size(35, 13);
            this.labelBatch.TabIndex = 10;
            this.labelBatch.Text = "Batch";
            // 
            // labelExam
            // 
            this.labelExam.AutoSize = true;
            this.labelExam.Location = new System.Drawing.Point(364, 82);
            this.labelExam.Name = "labelExam";
            this.labelExam.Size = new System.Drawing.Size(67, 13);
            this.labelExam.TabIndex = 11;
            this.labelExam.Text = "Select Exam";
            // 
            // labelStudent
            // 
            this.labelStudent.AutoSize = true;
            this.labelStudent.Location = new System.Drawing.Point(736, 82);
            this.labelStudent.Name = "labelStudent";
            this.labelStudent.Size = new System.Drawing.Size(74, 13);
            this.labelStudent.TabIndex = 12;
            this.labelStudent.Text = "Select Student";
            // 
            // multi_GPA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1156, 760);
            this.Controls.Add(this.labelStudent);
            this.Controls.Add(this.labelExam);
            this.Controls.Add(this.labelBatch);
            this.Controls.Add(this.studentFilterCombo);
            this.Controls.Add(this.examFilterCombo);
            this.Controls.Add(this.batchFilterCombo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "multi_GPA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "multi_GPA";
            this.Load += new System.EventHandler(this.multi_GPA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox batchFilterCombo;
        private System.Windows.Forms.ComboBox examFilterCombo;
        private System.Windows.Forms.ComboBox studentFilterCombo;
        private System.Windows.Forms.Label labelBatch;
        private System.Windows.Forms.Label labelExam;
        private System.Windows.Forms.Label labelStudent;
    }
}
