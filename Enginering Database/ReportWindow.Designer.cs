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
			Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
			this.engineeringDatabaseTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.DataSetForReport = new Engineering_Database.DataSetForReport();
			this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
			this.dateParameterReport = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.engineeringDatabaseTableTableAdapter = new Engineering_Database.DataSetForReportTableAdapters.engineeringDatabaseTableTableAdapter();
			this.dataSetForReport1 = new Engineering_Database.DataSetForReport();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.engineeringDatabaseTableBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DataSetForReport)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSetForReport1)).BeginInit();
			this.SuspendLayout();
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
			// reportViewer1
			// 
			reportDataSource2.Name = "Report";
			reportDataSource2.Value = this.engineeringDatabaseTableBindingSource;
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
			this.reportViewer1.LocalReport.ReportEmbeddedResource = "Engineering_Database.EngineeringReport.rdlc";
			this.reportViewer1.Location = new System.Drawing.Point(0, 89);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.ServerReport.BearerToken = null;
			this.reportViewer1.Size = new System.Drawing.Size(800, 361);
			this.reportViewer1.TabIndex = 0;
			// 
			// dateParameterReport
			// 
			this.dateParameterReport.Location = new System.Drawing.Point(20, 37);
			this.dateParameterReport.Name = "dateParameterReport";
			this.dateParameterReport.Size = new System.Drawing.Size(175, 20);
			this.dateParameterReport.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(432, 37);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(136, 20);
			this.button1.TabIndex = 2;
			this.button1.Text = "Run Report";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// engineeringDatabaseTableTableAdapter
			// 
			this.engineeringDatabaseTableTableAdapter.ClearBeforeFill = true;
			// 
			// dataSetForReport1
			// 
			this.dataSetForReport1.DataSetName = "DataSetForReport";
			this.dataSetForReport1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// comboBox1
			// 
			this.comboBox1.DataSource = this.engineeringDatabaseTableBindingSource;
			this.comboBox1.DisplayMember = "ReportedDate";
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(213, 37);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 3;
			this.comboBox1.ValueMember = "ReportedDate";
			// 
			// ReportWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(820, 470);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dateParameterReport);
			this.Controls.Add(this.reportViewer1);
			this.Name = "ReportWindow";
			this.Text = "ReportWindow";
			this.Load += new System.EventHandler(this.ReportWindow_Load);
			((System.ComponentModel.ISupportInitialize)(this.engineeringDatabaseTableBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DataSetForReport)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSetForReport1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
		private System.Windows.Forms.BindingSource engineeringDatabaseTableBindingSource;
		private DataSetForReport DataSetForReport;
		private DataSetForReportTableAdapters.engineeringDatabaseTableTableAdapter engineeringDatabaseTableTableAdapter;
		private System.Windows.Forms.TextBox dateParameterReport;
		private System.Windows.Forms.Button button1;
		private DataSetForReport dataSetForReport1;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}