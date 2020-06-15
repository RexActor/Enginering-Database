namespace Engineering_Database
{
	partial class JobReportWinForm
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
			Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
			this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
			this.jobNumberTextBox = new System.Windows.Forms.TextBox();
			this.RefreshButton = new System.Windows.Forms.Button();
			this.DataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.JobReportsDataSet = new Engineering_Database.JobReportsDataSet();
			((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.JobReportsDataSet)).BeginInit();
			this.SuspendLayout();
			// 
			// reportViewer1
			// 
			reportDataSource1.Name = "DataSet1";
			reportDataSource1.Value = this.DataTable1BindingSource;
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
			this.reportViewer1.LocalReport.ReportEmbeddedResource = "Engineering_Database.JobReportsV2.rdlc";
			this.reportViewer1.Location = new System.Drawing.Point(1, 45);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.ServerReport.BearerToken = null;
			this.reportViewer1.Size = new System.Drawing.Size(1296, 536);
			this.reportViewer1.TabIndex = 0;
			// 
			// jobNumberTextBox
			// 
			this.jobNumberTextBox.Location = new System.Drawing.Point(12, 12);
			this.jobNumberTextBox.Name = "jobNumberTextBox";
			this.jobNumberTextBox.Size = new System.Drawing.Size(100, 20);
			this.jobNumberTextBox.TabIndex = 1;
			this.jobNumberTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
			// 
			// RefreshButton
			// 
			this.RefreshButton.Location = new System.Drawing.Point(127, 12);
			this.RefreshButton.Name = "RefreshButton";
			this.RefreshButton.Size = new System.Drawing.Size(75, 23);
			this.RefreshButton.TabIndex = 2;
			this.RefreshButton.Text = "Refresh";
			this.RefreshButton.UseVisualStyleBackColor = true;
			this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
			// 
			// DataTable1BindingSource
			// 
			this.DataTable1BindingSource.DataMember = "DataTable1";
			this.DataTable1BindingSource.DataSource = this.JobReportsDataSet;
			// 
			// JobReportsDataSet
			// 
			this.JobReportsDataSet.DataSetName = "JobReportsDataSet";
			this.JobReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// JobReportWinForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1300, 579);
			this.Controls.Add(this.RefreshButton);
			this.Controls.Add(this.jobNumberTextBox);
			this.Controls.Add(this.reportViewer1);
			this.Name = "JobReportWinForm";
			this.Text = "Job Report ";
			this.Load += new System.EventHandler(this.JobReportWinForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.JobReportsDataSet)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
		private System.Windows.Forms.TextBox jobNumberTextBox;
		private System.Windows.Forms.BindingSource DataTable1BindingSource;
		private JobReportsDataSet JobReportsDataSet;
		private System.Windows.Forms.Button RefreshButton;
	}
}