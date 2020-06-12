using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Engineering_Database
{
	public partial class ChartReports : Form
	{
		DatabaseClass db = new DatabaseClass();
		public ChartReports()
		{
			InitializeComponent();
		}

		private void ChartReports_Load(object sender, EventArgs e)
		{

			ReportRefresh();
		}

		public void ReportRefresh()
		{
			//ReportParameter rp = new ReportParameter("JobNumberParameter", "200");

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
			db.ConnectDB();



			OleDbDataAdapter da = new OleDbDataAdapter(db.DbAdapter("engineeringDatabaseTable"));

			AssetDataSet ds = new AssetDataSet();

			da.Fill(ds, "DataTable1");

			return ds;



		}

	}
}
