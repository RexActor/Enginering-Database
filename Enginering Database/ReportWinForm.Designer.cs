namespace Engineering_Database
{
	partial class ReportWinForm
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
			this.AssetDataSet = new Engineering_Database.AssetDataSet();
			this.DataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.AssetsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.AssetDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.AssetsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// reportViewer1
			// 
			this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			reportDataSource1.Name = "DataSet1";
			reportDataSource1.Value = this.DataTable1BindingSource;
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
			this.reportViewer1.LocalReport.ReportEmbeddedResource = "Engineering_Database.AssetReport.rdlc";
			this.reportViewer1.Location = new System.Drawing.Point(0, 0);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.Size = new System.Drawing.Size(807, 510);
			this.reportViewer1.TabIndex = 0;
			// 
			// AssetDataSet
			// 
			this.AssetDataSet.DataSetName = "AssetDataSet";
			this.AssetDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// DataTable1BindingSource
			// 
			this.DataTable1BindingSource.DataMember = "DataTable1";
			this.DataTable1BindingSource.DataSource = this.AssetDataSet;
			// 
			// AssetsBindingSource
			// 
			this.AssetsBindingSource.DataSource = typeof(Engineering_Database.Assets);
			// 
			// ReportWinForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(807, 510);
			this.Controls.Add(this.reportViewer1);
			this.Name = "ReportWinForm";
			this.Text = "ReportWinForm";
			this.Load += new System.EventHandler(this.ReportWinForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.AssetDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.AssetsBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
		private System.Windows.Forms.BindingSource AssetsBindingSource;
		private System.Windows.Forms.BindingSource DataTable1BindingSource;
		private AssetDataSet AssetDataSet;
	}
}