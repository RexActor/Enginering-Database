using Microsoft.Reporting.WinForms;

using System;
using System.Windows.Forms;

namespace Engineering_Database
{
	public partial class ReportWindow : Form
	{

		//ReportParameter[] param = new ReportParameter[1];
		//int repPar;

		public ReportWindow()
		{
			InitializeComponent();
		}

		private void ReportWindow_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)

		{




			
			this.engineeringDatabaseTableTableAdapter.ReportFill(DataSetForReport.engineeringDatabaseTable,Convert.ToDateTime(comboBox1.Text));
			
			this.reportViewer1.RefreshReport();
		}

		
	}
}
