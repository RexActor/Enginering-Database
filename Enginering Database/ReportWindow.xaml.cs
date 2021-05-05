using DocumentFormat.OpenXml.Drawing.Charts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for ReportWindow.xaml
	/// </summary>
	public partial class ReportWindow : Window
	{
		private ErrorSystem err = new ErrorSystem();

		//readonly DatabaseClass db = new DatabaseClass();
		public ReportWindow()
		{
			InitializeComponent();
		}

		private void ReportsForJobs_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				JobReportWinForm jobReports = new JobReportWinForm();
				jobReports.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void AssetReportButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ReportWinForm WinFormReport = new ReportWinForm();

				WinFormReport.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void ChartButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ChartReports chartReports = new ChartReports();

				chartReports.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}
	}
}