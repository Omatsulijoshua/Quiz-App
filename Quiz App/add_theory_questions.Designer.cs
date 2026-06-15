namespace Quiz_App
{
    partial class add_theory_questions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(add_theory_questions));
            this.cmbExam = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvQuestions = new System.Windows.Forms.DataGridView();
            this.numQuestionNo = new System.Windows.Forms.NumericUpDown();
            this.numMark = new System.Windows.Forms.NumericUpDown();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.lblQuestionNo = new System.Windows.Forms.Label();
            this.lblExam = new System.Windows.Forms.Label();
            this.txtQuestion = new System.Windows.Forms.RichTextBox();
            this.lblMark = new System.Windows.Forms.Label();
            this.lblModelAnswer = new System.Windows.Forms.Label();
            this.txtModelAnswer = new System.Windows.Forms.RichTextBox();
            this.exam_Id = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuestions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuestionNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbExam
            // 
            this.cmbExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbExam.FormattingEnabled = true;
            this.cmbExam.Location = new System.Drawing.Point(1218, 79);
            this.cmbExam.Name = "cmbExam";
            this.cmbExam.Size = new System.Drawing.Size(293, 33);
            this.cmbExam.TabIndex = 0;
            this.cmbExam.SelectedIndexChanged += new System.EventHandler(this.cmbExam_SelectedIndexChanged_1);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(473, 556);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(183, 41);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(225, 554);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(185, 44);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvQuestions
            // 
            this.dgvQuestions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuestions.Location = new System.Drawing.Point(227, 636);
            this.dgvQuestions.Name = "dgvQuestions";
            this.dgvQuestions.Size = new System.Drawing.Size(943, 351);
            this.dgvQuestions.TabIndex = 3;
            this.dgvQuestions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQuestions_CellContentClick);
            // 
            // numQuestionNo
            // 
            this.numQuestionNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQuestionNo.Location = new System.Drawing.Point(435, 45);
            this.numQuestionNo.Name = "numQuestionNo";
            this.numQuestionNo.Size = new System.Drawing.Size(198, 31);
            this.numQuestionNo.TabIndex = 5;
            // 
            // numMark
            // 
            this.numMark.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMark.Location = new System.Drawing.Point(435, 280);
            this.numMark.Name = "numMark";
            this.numMark.Size = new System.Drawing.Size(198, 31);
            this.numMark.TabIndex = 5;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(1862, 12);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(56, 55);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoSize = true;
            this.lblQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestion.ForeColor = System.Drawing.Color.White;
            this.lblQuestion.Location = new System.Drawing.Point(224, 122);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(76, 18);
            this.lblQuestion.TabIndex = 9;
            this.lblQuestion.Text = "Question";
            // 
            // lblQuestionNo
            // 
            this.lblQuestionNo.AutoSize = true;
            this.lblQuestionNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestionNo.ForeColor = System.Drawing.Color.White;
            this.lblQuestionNo.Location = new System.Drawing.Point(224, 53);
            this.lblQuestionNo.Name = "lblQuestionNo";
            this.lblQuestionNo.Size = new System.Drawing.Size(140, 18);
            this.lblQuestionNo.TabIndex = 9;
            this.lblQuestionNo.Text = "Question Number";
            // 
            // lblExam
            // 
            this.lblExam.AutoSize = true;
            this.lblExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExam.ForeColor = System.Drawing.Color.White;
            this.lblExam.Location = new System.Drawing.Point(1046, 79);
            this.lblExam.Name = "lblExam";
            this.lblExam.Size = new System.Drawing.Size(102, 18);
            this.lblExam.TabIndex = 9;
            this.lblExam.Text = "Select Exam";
            // 
            // txtQuestion
            // 
            this.txtQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuestion.Location = new System.Drawing.Point(435, 122);
            this.txtQuestion.Name = "txtQuestion";
            this.txtQuestion.Size = new System.Drawing.Size(556, 141);
            this.txtQuestion.TabIndex = 10;
            this.txtQuestion.Text = "";
            // 
            // lblMark
            // 
            this.lblMark.AutoSize = true;
            this.lblMark.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMark.ForeColor = System.Drawing.Color.White;
            this.lblMark.Location = new System.Drawing.Point(224, 277);
            this.lblMark.Name = "lblMark";
            this.lblMark.Size = new System.Drawing.Size(46, 18);
            this.lblMark.TabIndex = 9;
            this.lblMark.Text = "Mark";
            // 
            // lblModelAnswer
            // 
            this.lblModelAnswer.AutoSize = true;
            this.lblModelAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelAnswer.ForeColor = System.Drawing.Color.White;
            this.lblModelAnswer.Location = new System.Drawing.Point(224, 347);
            this.lblModelAnswer.Name = "lblModelAnswer";
            this.lblModelAnswer.Size = new System.Drawing.Size(196, 18);
            this.lblModelAnswer.TabIndex = 9;
            this.lblModelAnswer.Text = "Model Answer (optional):";
            // 
            // txtModelAnswer
            // 
            this.txtModelAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModelAnswer.Location = new System.Drawing.Point(435, 347);
            this.txtModelAnswer.Name = "txtModelAnswer";
            this.txtModelAnswer.Size = new System.Drawing.Size(556, 164);
            this.txtModelAnswer.TabIndex = 10;
            this.txtModelAnswer.Text = "";
            // 
            // exam_Id
            // 
            this.exam_Id.AutoSize = true;
            this.exam_Id.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exam_Id.ForeColor = System.Drawing.Color.White;
            this.exam_Id.Location = new System.Drawing.Point(1226, 143);
            this.exam_Id.Name = "exam_Id";
            this.exam_Id.Size = new System.Drawing.Size(17, 18);
            this.exam_Id.TabIndex = 9;
            this.exam_Id.Text = "1";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(719, 554);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(203, 44);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // add_theory_questions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1531, 999);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtModelAnswer);
            this.Controls.Add(this.txtQuestion);
            this.Controls.Add(this.lblQuestionNo);
            this.Controls.Add(this.exam_Id);
            this.Controls.Add(this.lblExam);
            this.Controls.Add(this.lblModelAnswer);
            this.Controls.Add(this.lblMark);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.numMark);
            this.Controls.Add(this.numQuestionNo);
            this.Controls.Add(this.dgvQuestions);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.cmbExam);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "add_theory_questions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add_theory_questions";
            this.Load += new System.EventHandler(this.add_theory_questions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuestions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuestionNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbExam;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvQuestions;
        private System.Windows.Forms.NumericUpDown numQuestionNo;
        private System.Windows.Forms.NumericUpDown numMark;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.Label lblQuestionNo;
        private System.Windows.Forms.Label lblExam;
        private System.Windows.Forms.RichTextBox txtQuestion;
        private System.Windows.Forms.Label lblMark;
        private System.Windows.Forms.Label lblModelAnswer;
        private System.Windows.Forms.RichTextBox txtModelAnswer;
        private System.Windows.Forms.Label exam_Id;
        private System.Windows.Forms.Button btnUpdate;
    }
}