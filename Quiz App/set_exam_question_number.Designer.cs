
namespace Quiz_App
{
    partial class set_exam_question_number
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
            this.labelSelectExam = new System.Windows.Forms.Label();
            this.comboBoxExams = new System.Windows.Forms.ComboBox();
            this.labelDuration = new System.Windows.Forms.Label();
            this.btnSaveDuration = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.NumericUpDown();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSelectExam
            // 
            this.labelSelectExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectExam.Location = new System.Drawing.Point(400, 136);
            this.labelSelectExam.Name = "labelSelectExam";
            this.labelSelectExam.Size = new System.Drawing.Size(151, 33);
            this.labelSelectExam.TabIndex = 5;
            this.labelSelectExam.Text = "Select Exam:";
            // 
            // comboBoxExams
            // 
            this.comboBoxExams.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxExams.Location = new System.Drawing.Point(580, 137);
            this.comboBoxExams.Name = "comboBoxExams";
            this.comboBoxExams.Size = new System.Drawing.Size(262, 33);
            this.comboBoxExams.TabIndex = 6;
            // 
            // labelDuration
            // 
            this.labelDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuration.Location = new System.Drawing.Point(288, 182);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(286, 29);
            this.labelDuration.TabIndex = 7;
            this.labelDuration.Text = "Total Question Number:";
            // 
            // btnSaveDuration
            // 
            this.btnSaveDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDuration.Location = new System.Drawing.Point(580, 263);
            this.btnSaveDuration.Name = "btnSaveDuration";
            this.btnSaveDuration.Size = new System.Drawing.Size(262, 43);
            this.btnSaveDuration.TabIndex = 9;
            this.btnSaveDuration.Text = "Save";
            this.btnSaveDuration.Click += new System.EventHandler(this.btnSaveDuration_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Silver;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(473, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(369, 36);
            this.label4.TabIndex = 19;
            this.label4.Text = "Set Number of Questions Per Exams";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(848, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 29);
            this.label1.TabIndex = 7;
            this.label1.Text = "< type in here";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(848, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(213, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "< select from here";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(580, 192);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(262, 29);
            this.textBox1.TabIndex = 52;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(580, 341);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(247, 243);
            this.dataGridView1.TabIndex = 56;
            // 
            // set_exam_question_number
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1402, 667);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelSelectExam);
            this.Controls.Add(this.comboBoxExams);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.btnSaveDuration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "set_exam_question_number";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "set_exam_queston_numbercs";
            this.Load += new System.EventHandler(this.set_exam_question_number_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSelectExam;
        private System.Windows.Forms.ComboBox comboBoxExams;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.Button btnSaveDuration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}