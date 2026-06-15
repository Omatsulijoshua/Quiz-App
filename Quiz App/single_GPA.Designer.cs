namespace Quiz_App
{
    partial class single_GPA
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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dataGridViewMaster = new System.Windows.Forms.DataGridView();
            this.labelTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.batchFilterCombo = new System.Windows.Forms.ComboBox();
            this.examFilterCombo = new System.Windows.Forms.ComboBox();
            this.studentFilterCombo = new System.Windows.Forms.ComboBox();
            this.labelBatch = new System.Windows.Forms.Label();
            this.labelExam = new System.Windows.Forms.Label();
            this.labelStudent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(32, 26);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 42);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(974, 26);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 42);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dataGridViewMaster
            // 
            this.dataGridViewMaster.AllowUserToAddRows = false;
            this.dataGridViewMaster.AllowUserToDeleteRows = false;
            this.dataGridViewMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMaster.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMaster.Location = new System.Drawing.Point(32, 176);
            this.dataGridViewMaster.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewMaster.Name = "dataGridViewMaster";
            this.dataGridViewMaster.ReadOnly = true;
            this.dataGridViewMaster.RowHeadersWidth = 51;
            this.dataGridViewMaster.RowTemplate.Height = 29;
            this.dataGridViewMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMaster.Size = new System.Drawing.Size(1092, 476);
            this.dataGridViewMaster.TabIndex = 5;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(479, 28);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(126, 25);
            this.labelTitle.TabIndex = 4;
            this.labelTitle.Text = "Student GPA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(441, 684);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(270, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "GPA = ?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(32, 678);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 52);
            this.button1.TabIndex = 19;
            this.button1.Text = "Load GPA";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(934, 678);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 52);
            this.button2.TabIndex = 20;
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
            this.batchFilterCombo.TabIndex = 21;
            this.batchFilterCombo.SelectedIndexChanged += new System.EventHandler(this.batchFilterCombo_SelectedIndexChanged);
            // 
            // examFilterCombo
            // 
            this.examFilterCombo.FormattingEnabled = true;
            this.examFilterCombo.Location = new System.Drawing.Point(364, 104);
            this.examFilterCombo.Name = "examFilterCombo";
            this.examFilterCombo.Size = new System.Drawing.Size(340, 21);
            this.examFilterCombo.TabIndex = 22;
            this.examFilterCombo.SelectedIndexChanged += new System.EventHandler(this.examFilterCombo_SelectedIndexChanged);
            // 
            // studentFilterCombo
            // 
            this.studentFilterCombo.FormattingEnabled = true;
            this.studentFilterCombo.Location = new System.Drawing.Point(736, 104);
            this.studentFilterCombo.Name = "studentFilterCombo";
            this.studentFilterCombo.Size = new System.Drawing.Size(388, 21);
            this.studentFilterCombo.TabIndex = 23;
            // 
            // labelBatch
            // 
            this.labelBatch.AutoSize = true;
            this.labelBatch.Location = new System.Drawing.Point(32, 82);
            this.labelBatch.Name = "labelBatch";
            this.labelBatch.Size = new System.Drawing.Size(35, 13);
            this.labelBatch.TabIndex = 24;
            this.labelBatch.Text = "Batch";
            // 
            // labelExam
            // 
            this.labelExam.AutoSize = true;
            this.labelExam.Location = new System.Drawing.Point(364, 82);
            this.labelExam.Name = "labelExam";
            this.labelExam.Size = new System.Drawing.Size(67, 13);
            this.labelExam.TabIndex = 25;
            this.labelExam.Text = "Select Exam";
            // 
            // labelStudent
            // 
            this.labelStudent.AutoSize = true;
            this.labelStudent.Location = new System.Drawing.Point(736, 82);
            this.labelStudent.Name = "labelStudent";
            this.labelStudent.Size = new System.Drawing.Size(74, 13);
            this.labelStudent.TabIndex = 26;
            this.labelStudent.Text = "Select Student";
            // 
            // single_GPA
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
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dataGridViewMaster);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "single_GPA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MasterSheetForm2";
            this.Load += new System.EventHandler(this.single_GPA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dataGridViewMaster;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label label2;
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
