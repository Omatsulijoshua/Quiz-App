namespace Quiz_App
{
    partial class mastersheet_mode
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
            this.btnSaveDuration = new System.Windows.Forms.Button();
            this.radioButtonTer = new System.Windows.Forms.RadioButton();
            this.radioButtonSec = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(345, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(559, 29);
            this.label1.TabIndex = 36;
            this.label1.Text = "Who is Using this software?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSaveDuration
            // 
            this.btnSaveDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDuration.Location = new System.Drawing.Point(613, 364);
            this.btnSaveDuration.Name = "btnSaveDuration";
            this.btnSaveDuration.Size = new System.Drawing.Size(111, 43);
            this.btnSaveDuration.TabIndex = 35;
            this.btnSaveDuration.Text = "Save";
            this.btnSaveDuration.Click += new System.EventHandler(this.btnSaveDuration_Click);
            // 
            // radioButtonTer
            // 
            this.radioButtonTer.AutoSize = true;
            this.radioButtonTer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonTer.Location = new System.Drawing.Point(400, 266);
            this.radioButtonTer.Name = "radioButtonTer";
            this.radioButtonTer.Size = new System.Drawing.Size(201, 29);
            this.radioButtonTer.TabIndex = 55;
            this.radioButtonTer.TabStop = true;
            this.radioButtonTer.Text = "Tertiary Institution";
            this.radioButtonTer.UseVisualStyleBackColor = true;
            // 
            // radioButtonSec
            // 
            this.radioButtonSec.AutoSize = true;
            this.radioButtonSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSec.Location = new System.Drawing.Point(399, 223);
            this.radioButtonSec.Name = "radioButtonSec";
            this.radioButtonSec.Size = new System.Drawing.Size(205, 29);
            this.radioButtonSec.TabIndex = 54;
            this.radioButtonSec.TabStop = true;
            this.radioButtonSec.Text = "Secondary School";
            this.radioButtonSec.UseVisualStyleBackColor = true;
            // 
            // mastersheet_mode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1311, 653);
            this.Controls.Add(this.radioButtonTer);
            this.Controls.Add(this.radioButtonSec);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSaveDuration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "mastersheet_mode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mastersheet_mode";
            this.Load += new System.EventHandler(this.mastersheet_mode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveDuration;
        private System.Windows.Forms.RadioButton radioButtonTer;
        private System.Windows.Forms.RadioButton radioButtonSec;
    }
}