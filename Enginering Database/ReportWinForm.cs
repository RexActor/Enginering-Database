﻿


using Microsoft.Reporting.WinForms;

using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Engineering_Database
{
	public partial class ReportWinForm : Form
	{

		readonly DatabaseClass db = new DatabaseClass();
		public ReportWinForm()
		{
			InitializeComponent();
		}

		private void ReportWinForm_Load(object sender, EventArgs e)
		{





			ReportRefresh();

		}



		public void ReportRefresh()
		{
		
			
			try
			{
				AssetDataSet ds = GetData();
				ReportDataSource dataSource = new ReportDataSource("DataSet1", ds.Tables[0]);





				this.reportViewer1.LocalReport.DataSources.Clear();
				this.reportViewer1.LocalReport.DataSources.Add(dataSource);

				this.reportViewer1.RefreshReport();
			}
			catch 
			{
				Console.WriteLine("Something went wrong");

			}
		}


		private AssetDataSet GetData()
		{

			//Assets asset = new Assets();
			db.ConnectDB("Assets");

			OleDbDataAdapter da = new OleDbDataAdapter(db.DbAdapter("AssetList"));

			AssetDataSet ds = new AssetDataSet();

			da.Fill(ds, "DataTable1");

			return ds;



		}

		
	}
}
