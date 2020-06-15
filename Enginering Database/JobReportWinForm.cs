using Microsoft.Reporting.WinForms;

using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Engineering_Database
{
	public partial class JobReportWinForm : Form
	{
		readonly DatabaseClass db = new DatabaseClass();

		public JobReportWinForm()
		{
			InitializeComponent();
		}


		private void JobReportWinForm_Load(object sender, EventArgs e)
		{

			//ReportRefresh();
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{

			if ( e.KeyData== Keys.Enter)
			{
				ReportParameter rp = new ReportParameter("JobNumberParameter", jobNumberTextBox.Text);

				//MessageBox.Show("Enter in textbox");




				ReportRefresh(rp);

			}
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			ReportParameter rp = new ReportParameter("JobNumberParameter", jobNumberTextBox.Text);

			//MessageBox.Show("Enter in textbox");




			ReportRefresh(rp);
		}




		public void ReportRefresh(ReportParameter rp=null)
		{
			//ReportParameter rp = new ReportParameter("JobNumberParameter", "200");

			try
			{
				AssetDataSet ds = GetData();
				ReportDataSource dataSource = new ReportDataSource("DataSet1", ds.Tables[0]);





				this.reportViewer1.LocalReport.DataSources.Clear();

				if (rp != null)
				{
					reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
				}

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
