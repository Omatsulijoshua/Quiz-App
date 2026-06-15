namespace Quiz_App
{
    partial class student_control_panel
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
            this.labelDuration = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnStartTest = new System.Windows.Forms.Button();
            this.numericUpDownDuration1 = new System.Windows.Forms.NumericUpDown();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.numericUpDownQuestionLimit = new System.Windows.Forms.NumericUpDown();
            this.radioButtonShuffle = new System.Windows.Forms.RadioButton();
            this.radioButtonNoShuffle = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuestionLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(248, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 30);
            this.label1.TabIndex = 25;
            this.label1.Text = "SHUFFLE EXAMS ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDuration
            // 
            this.labelDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuration.Location = new System.Drawing.Point(247, 408);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(179, 25);
            this.labelDuration.TabIndex = 30;
            this.labelDuration.Text = "Duration (min):";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Silver;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(252, 330);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(345, 29);
            this.label2.TabIndex = 25;
            this.label2.Text = "EXAM DURATION";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Silver;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(425, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(345, 30);
            this.label3.TabIndex = 25;
            this.label3.Text = "EXAM SETTINGS";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Silver;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(737, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(303, 29);
            this.label4.TabIndex = 47;
            this.label4.Text = "Set Number of Questions For This Exams";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(724, 196);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(286, 29);
            this.label8.TabIndex = 45;
            this.label8.Text = "Total Question Number:";
            // 
            // btnStartTest
            // 
            this.btnStartTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(167)))), ((int)(((byte)(203)))));
            this.btnStartTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartTest.Location = new System.Drawing.Point(741, 408);
            this.btnStartTest.Name = "btnStartTest";
            this.btnStartTest.Size = new System.Drawing.Size(226, 43);
            this.btnStartTest.TabIndex = 46;
            this.btnStartTest.Text = "START EXAM";
            this.btnStartTest.UseVisualStyleBackColor = false;
            this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click_1);
            // 
            // numericUpDownDuration1
            // 
            this.numericUpDownDuration1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDuration1.Location = new System.Drawing.Point(431, 408);
            this.numericUpDownDuration1.Name = "numericUpDownDuration1";
            this.numericUpDownDuration1.Size = new System.Drawing.Size(203, 29);
            this.numericUpDownDuration1.TabIndex = 50;
            // 
            // numericUpDownQuestionLimit
            // 
            this.numericUpDownQuestionLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownQuestionLimit.Location = new System.Drawing.Point(741, 238);
            this.numericUpDownQuestionLimit.Name = "numericUpDownQuestionLimit";
            this.numericUpDownQuestionLimit.Size = new System.Drawing.Size(203, 29);
            this.numericUpDownQuestionLimit.TabIndex = 51;
            // 
            // radioButtonShuffle
            // 
            this.radioButtonShuffle.AutoSize = true;
            this.radioButtonShuffle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonShuffle.Location = new System.Drawing.Point(281, 194);
            this.radioButtonShuffle.Name = "radioButtonShuffle";
            this.radioButtonShuffle.Size = new System.Drawing.Size(68, 29);
            this.radioButtonShuffle.TabIndex = 52;
            this.radioButtonShuffle.TabStop = true;
            this.radioButtonShuffle.Text = "Yes";
            this.radioButtonShuffle.UseVisualStyleBackColor = true;
            // 
            // radioButtonNoShuffle
            // 
            this.radioButtonNoShuffle.AutoSize = true;
            this.radioButtonNoShuffle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNoShuffle.Location = new System.Drawing.Point(282, 237);
            this.radioButtonNoShuffle.Name = "radioButtonNoShuffle";
            this.radioButtonNoShuffle.Size = new System.Drawing.Size(57, 29);
            this.radioButtonNoShuffle.TabIndex = 52;
            this.radioButtonNoShuffle.TabStop = true;
            this.radioButtonNoShuffle.Text = "No";
            this.radioButtonNoShuffle.UseVisualStyleBackColor = true;
            // 
            // student_control_panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1338, 725);
            this.Controls.Add(this.radioButtonNoShuffle);
            this.Controls.Add(this.radioButtonShuffle);
            this.Controls.Add(this.numericUpDownQuestionLimit);
            this.Controls.Add(this.numericUpDownDuration1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnStartTest);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "student_control_panel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "student_control_panel";
            this.Load += new System.EventHandler(this.student_control_panel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuestionLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnStartTest;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.NumericUpDown numericUpDownQuestionLimit;
        private System.Windows.Forms.RadioButton radioButtonShuffle;
        private System.Windows.Forms.RadioButton radioButtonNoShuffle;
    }
}