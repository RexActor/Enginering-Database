


using Microsoft.Reporting.WinForms;

using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Engineering_Database
{
	public partial class ReportWinForm : Form
	{

		DatabaseClass db = new DatabaseClass();
		public ReportWinForm()
		{
			InitializeComponent();
		}

		private void ReportWinForm_Load(object sender, EventArgs e)
		{

			AssetDataSet ds = GetData("500");

			if (ds != null)
			{
				ReportDataSource dataSource = new ReportDataSource("DataTable1", ds.Tables[0]);


				this.reportViewer1.LocalReport.DataSources.Clear();
				this.reportViewer1.LocalReport.DataSources.Add(dataSource);

				reportViewer1.RefreshReport();
				//this.reportViewer1.RefreshReport();

			}

		}

		
		private AssetDataSet GetData(string assetNumber)
		{

			Assets asset = new Assets();
			db.ConnectDB("Assets");

			OleDbDataAdapter da = new OleDbDataAdapter(db.dbAdapter("AssetList"));

			AssetDataSet ds = new AssetDataSet();

			da.Fill(ds, "DataTable1");

			return ds;
			
			

		}

	}
}
