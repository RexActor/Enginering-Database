using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Engineering_Database
{
	public partial class ReportWindow : Form
	{
		public ReportWindow()
		{
			InitializeComponent();
		}

		private void ReportWindow_Load(object sender, EventArgs e)
		{
			// TODO: This line of code loads data into the 'DataSetForReport.engineeringDatabaseTable' table. You can move, or remove it, as needed.
			this.engineeringDatabaseTableTableAdapter.ReportFill(this.DataSetForReport.engineeringDatabaseTable);
			
			this.reportViewer1.RefreshReport();
		}
	}
}
