namespace Quiz_App
{
    partial class Test3
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
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnSaveDraft = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSubmitAll = new System.Windows.Forms.Button();
            this.lblExamTitle = new System.Windows.Forms.Label();
            this.lblQuestionIndex = new System.Windows.Forms.Label();
            this.lblQuestionMark = new System.Windows.Forms.Label();
            this.rtbQuestion = new System.Windows.Forms.RichTextBox();
            this.rtbAnswer = new System.Windows.Forms.RichTextBox();
            this.progressBarQuestions = new System.Windows.Forms.ProgressBar();
            this.lblTimer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.ForeColor = System.Drawing.Color.Black;
            this.btnPrev.Location = new System.Drawing.Point(37, 959);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(267, 48);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "Previous";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.BtnPrev_Click);
            // 
            // btnSaveDraft
            // 
            this.btnSaveDraft.ForeColor = System.Drawing.Color.Black;
            this.btnSaveDraft.Location = new System.Drawing.Point(595, 959);
            this.btnSaveDraft.Name = "btnSaveDraft";
            this.btnSaveDraft.Size = new System.Drawing.Size(267, 48);
            this.btnSaveDraft.TabIndex = 1;
            this.btnSaveDraft.Text = "Save";
            this.btnSaveDraft.UseVisualStyleBackColor = true;
            this.btnSaveDraft.Click += new System.EventHandler(this.BtnSaveDraft_Click);
            // 
            // btnNext
            // 
            this.btnNext.ForeColor = System.Drawing.Color.Black;
            this.btnNext.Location = new System.Drawing.Point(1073, 959);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(267, 48);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSubmitAll
            // 
            this.btnSubmitAll.ForeColor = System.Drawing.Color.Black;
            this.btnSubmitAll.Location = new System.Drawing.Point(1596, 959);
            this.btnSubmitAll.Name = "btnSubmitAll";
            this.btnSubmitAll.Size = new System.Drawing.Size(267, 48);
            this.btnSubmitAll.TabIndex = 2;
            this.btnSubmitAll.Text = "Submit All";
            this.btnSubmitAll.UseVisualStyleBackColor = true;
            this.btnSubmitAll.Click += new System.EventHandler(this.BtnSubmitAll_Click);
            // 
            // lblExamTitle
            // 
            this.lblExamTitle.AutoSize = true;
            this.lblExamTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExamTitle.ForeColor = System.Drawing.Color.White;
            this.lblExamTitle.Location = new System.Drawing.Point(680, 108);
            this.lblExamTitle.Name = "lblExamTitle";
            this.lblExamTitle.Size = new System.Drawing.Size(66, 31);
            this.lblExamTitle.TabIndex = 3;
            this.lblExamTitle.Text = "Title";
            // 
            // lblQuestionIndex
            // 
            this.lblQuestionIndex.AutoSize = true;
            this.lblQuestionIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestionIndex.ForeColor = System.Drawing.Color.White;
            this.lblQuestionIndex.Location = new System.Drawing.Point(32, 168);
            this.lblQuestionIndex.Name = "lblQuestionIndex";
            this.lblQuestionIndex.Size = new System.Drawing.Size(196, 31);
            this.lblQuestionIndex.TabIndex = 4;
            this.lblQuestionIndex.Text = "Question Index";
            // 
            // lblQuestionMark
            // 
            this.lblQuestionMark.AutoSize = true;
            this.lblQuestionMark.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestionMark.ForeColor = System.Drawing.Color.White;
            this.lblQuestionMark.Location = new System.Drawing.Point(32, 225);
            this.lblQuestionMark.Name = "lblQuestionMark";
            this.lblQuestionMark.Size = new System.Drawing.Size(190, 31);
            this.lblQuestionMark.TabIndex = 5;
            this.lblQuestionMark.Text = "Question Mark";
            // 
            // rtbQuestion
            // 
            this.rtbQuestion.ForeColor = System.Drawing.Color.Black;
            this.rtbQuestion.Location = new System.Drawing.Point(37, 445);
            this.rtbQuestion.Name = "rtbQuestion";
            this.rtbQuestion.Size = new System.Drawing.Size(858, 385);
            this.rtbQuestion.TabIndex = 7;
            this.rtbQuestion.Text = "";
            // 
            // rtbAnswer
            // 
            this.rtbAnswer.ForeColor = System.Drawing.Color.Black;
            this.rtbAnswer.Location = new System.Drawing.Point(976, 445);
            this.rtbAnswer.Name = "rtbAnswer";
            this.rtbAnswer.Size = new System.Drawing.Size(887, 385);
            this.rtbAnswer.TabIndex = 8;
            this.rtbAnswer.Text = "";
            // 
            // progressBarQuestions
            // 
            this.progressBarQuestions.Location = new System.Drawing.Point(37, 865);
            this.progressBarQuestions.Name = "progressBarQuestions";
            this.progressBarQuestions.Size = new System.Drawing.Size(1826, 50);
            this.progressBarQuestions.TabIndex = 9;
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimer.ForeColor = System.Drawing.Color.White;
            this.lblTimer.Location = new System.Drawing.Point(1737, 108);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(71, 25);
            this.lblTimer.TabIndex = 10;
            this.lblTimer.Text = "Timer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(32, 386);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 31);
            this.label1.TabIndex = 11;
            this.label1.Text = "Question";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(970, 386);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 31);
            this.label2.TabIndex = 11;
            this.label2.Text = "Answer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2110, -68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 25);
            this.label3.TabIndex = 10;
            this.label3.Text = "Timer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(1550, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Time Left:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(769, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 31);
            this.label5.TabIndex = 17;
            this.label5.Text = "Theory Exam";
            // 
            // Test3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1924, 1061);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.progressBarQuestions);
            this.Controls.Add(this.rtbAnswer);
            this.Controls.Add(this.rtbQuestion);
            this.Controls.Add(this.lblQuestionMark);
            this.Controls.Add(this.lblQuestionIndex);
            this.Controls.Add(this.lblExamTitle);
            this.Controls.Add(this.btnSubmitAll);
            this.Controls.Add(this.btnSaveDraft);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Test3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test3";
            this.Load += new System.EventHandler(this.Test3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnSaveDraft;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSubmitAll;
        private System.Windows.Forms.Label lblExamTitle;
        private System.Windows.Forms.Label lblQuestionIndex;
        private System.Windows.Forms.Label lblQuestionMark;
        private System.Windows.Forms.RichTextBox rtbQuestion;
        private System.Windows.Forms.RichTextBox rtbAnswer;
        private System.Windows.Forms.ProgressBar progressBarQuestions;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label5;
    }
}