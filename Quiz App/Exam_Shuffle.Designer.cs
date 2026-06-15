
namespace Quiz_App
{
    partial class Exam_Shuffle
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
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonYes = new System.Windows.Forms.RadioButton();
            this.radioButtonNo = new System.Windows.Forms.RadioButton();
            this.btnSaveDuration = new System.Windows.Forms.Button();
            this.comboBoxExams = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(439, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 29);
            this.label1.TabIndex = 15;
            this.label1.Text = "SET EXAMS SHUFFLE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonYes
            // 
            this.radioButtonYes.AutoSize = true;
            this.radioButtonYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonYes.Location = new System.Drawing.Point(441, 208);
            this.radioButtonYes.Name = "radioButtonYes";
            this.radioButtonYes.Size = new System.Drawing.Size(89, 35);
            this.radioButtonYes.TabIndex = 16;
            this.radioButtonYes.TabStop = true;
            this.radioButtonYes.Text = "YES";
            this.radioButtonYes.UseVisualStyleBackColor = true;
            // 
            // radioButtonNo
            // 
            this.radioButtonNo.AutoSize = true;
            this.radioButtonNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNo.Location = new System.Drawing.Point(441, 249);
            this.radioButtonNo.Name = "radioButtonNo";
            this.radioButtonNo.Size = new System.Drawing.Size(75, 35);
            this.radioButtonNo.TabIndex = 16;
            this.radioButtonNo.TabStop = true;
            this.radioButtonNo.Text = "NO";
            this.radioButtonNo.UseVisualStyleBackColor = true;
            this.radioButtonNo.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // btnSaveDuration
            // 
            this.btnSaveDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDuration.Location = new System.Drawing.Point(609, 248);
            this.btnSaveDuration.Name = "btnSaveDuration";
            this.btnSaveDuration.Size = new System.Drawing.Size(111, 43);
            this.btnSaveDuration.TabIndex = 19;
            this.btnSaveDuration.Text = "Save";
            this.btnSaveDuration.Click += new System.EventHandler(this.btnSaveDuration_Click);
            // 
            // comboBoxExams
            // 
            this.comboBoxExams.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxExams.FormattingEnabled = true;
            this.comboBoxExams.Location = new System.Drawing.Point(441, 138);
            this.comboBoxExams.Name = "comboBoxExams";
            this.comboBoxExams.Size = new System.Drawing.Size(279, 37);
            this.comboBoxExams.TabIndex = 20;
            this.comboBoxExams.Text = "Select Exam To Shuffle";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(445, 351);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(245, 179);
            this.dataGridView1.TabIndex = 21;
            // 
            // Exam_Shuffle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1432, 742);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBoxExams);
            this.Controls.Add(this.btnSaveDuration);
            this.Controls.Add(this.radioButtonNo);
            this.Controls.Add(this.radioButtonYes);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Exam_Shuffle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exam_Shuffle";
            this.Load += new System.EventHandler(this.Exam_Shuffle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonYes;
        private System.Windows.Forms.RadioButton radioButtonNo;
        private System.Windows.Forms.Button btnSaveDuration;
        private System.Windows.Forms.ComboBox comboBoxExams;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}