namespace Quiz_App
{
    partial class ReportCardForm
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

        private void InitializeComponent()
        {
            this.lblStudent = new System.Windows.Forms.Label();
            this.dataGridViewReport = new System.Windows.Forms.DataGridView();
            this.labelAverage = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.labelRemark = new System.Windows.Forms.Label();
            this.labelGrade = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStudent
            // 
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblStudent.Location = new System.Drawing.Point(22, 13);
            this.lblStudent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(125, 21);
            this.lblStudent.TabIndex = 0;
            this.lblStudent.Text = "Student Report";
            // 
            // dataGridViewReport
            // 
            this.dataGridViewReport.AllowUserToAddRows = false;
            this.dataGridViewReport.AllowUserToDeleteRows = false;
            this.dataGridViewReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReport.Location = new System.Drawing.Point(22, 39);
            this.dataGridViewReport.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewReport.Name = "dataGridViewReport";
            this.dataGridViewReport.ReadOnly = true;
            this.dataGridViewReport.RowHeadersWidth = 51;
            this.dataGridViewReport.RowTemplate.Height = 29;
            this.dataGridViewReport.Size = new System.Drawing.Size(1021, 408);
            this.dataGridViewReport.TabIndex = 1;
            // 
            // labelAverage
            // 
            this.labelAverage.AutoSize = true;
            this.labelAverage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelAverage.Location = new System.Drawing.Point(18, 473);
            this.labelAverage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAverage.Name = "labelAverage";
            this.labelAverage.Size = new System.Drawing.Size(114, 19);
            this.labelAverage.TabIndex = 2;
            this.labelAverage.Text = "Average: 0.00%";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(945, 14);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(98, 20);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export Report";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // labelRemark
            // 
            this.labelRemark.AutoSize = true;
            this.labelRemark.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelRemark.Location = new System.Drawing.Point(288, 473);
            this.labelRemark.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(61, 19);
            this.labelRemark.TabIndex = 2;
            this.labelRemark.Text = "Remark";
            // 
            // labelGrade
            // 
            this.labelGrade.AutoSize = true;
            this.labelGrade.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelGrade.Location = new System.Drawing.Point(610, 473);
            this.labelGrade.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelGrade.Name = "labelGrade";
            this.labelGrade.Size = new System.Drawing.Size(50, 19);
            this.labelGrade.TabIndex = 2;
            this.labelGrade.Text = "Grade";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelPosition.Location = new System.Drawing.Point(776, 473);
            this.labelPosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(63, 19);
            this.labelPosition.TabIndex = 2;
            this.labelPosition.Text = "Position";
            // 
            // ReportCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 520);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.labelGrade);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.labelAverage);
            this.Controls.Add(this.dataGridViewReport);
            this.Controls.Add(this.lblStudent);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ReportCardForm";
            this.Text = "Report Card";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.DataGridView dataGridViewReport;
        private System.Windows.Forms.Label labelAverage;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label labelRemark;
        private System.Windows.Forms.Label labelGrade;
        private System.Windows.Forms.Label labelPosition;
    }
}