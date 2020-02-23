namespace Engineering_Database
{
	partial class ReportWindow
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
			this.engineeringDatabaseTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.DataSetForReport = new Engineering_Database.DataSetForReport();
			this.engineeringDatabaseTableTableAdapter = new Engineering_Database.DataSetForReportTableAdapters.engineeringDatabaseTableTableAdapter();
			((System.ComponentModel.ISupportInitialize)(this.engineeringDatabaseTableBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DataSetForReport)).BeginInit();
			this.SuspendLayout();
			// 
			// reportViewer1
			// 
			this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			reportDataSource1.Name = "Report";
			reportDataSource1.Value = this.engineeringDatabaseTableBindingSource;
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
			this.reportViewer1.LocalReport.ReportEmbeddedResource = "Engineering_Database.EngineeringReport.rdlc";
			this.reportViewer1.Location = new System.Drawing.Point(0, 0);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.ServerReport.BearerToken = null;
			this.reportViewer1.Size = new System.Drawing.Size(800, 450);
			this.reportViewer1.TabIndex = 0;
			// 
			// engineeringDatabaseTableBindingSource
			// 
			this.engineeringDatabaseTableBindingSource.DataMember = "engineeringDatabaseTable";
			this.engineeringDatabaseTableBindingSource.DataSource = this.DataSetForReport;
			// 
			// DataSetForReport
			// 
			this.DataSetForReport.DataSetName = "DataSetForReport";
			this.DataSetForReport.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// engineeringDatabaseTableTableAdapter
			// 
			this.engineeringDatabaseTableTableAdapter.ClearBeforeFill = true;
			// 
			// ReportWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.reportViewer1);
			this.Name = "ReportWindow";
			this.Text = "ReportWindow";
			this.Load += new System.EventHandler(this.ReportWindow_Load);
			((System.ComponentModel.ISupportInitialize)(this.engineeringDatabaseTableBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DataSetForReport)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
		private System.Windows.Forms.BindingSource engineeringDatabaseTableBindingSource;
		private DataSetForReport DataSetForReport;
		private DataSetForReportTableAdapters.engineeringDatabaseTableTableAdapter engineeringDatabaseTableTableAdapter;
	}
}