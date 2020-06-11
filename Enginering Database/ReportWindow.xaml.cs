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

		DatabaseClass db = new DatabaseClass();
		public ReportWindow()
		{
			InitializeComponent();
		}

		private void ReportsForJobs_Click(object sender, RoutedEventArgs e)
		{
			ReportWinForm WinFormReport = new ReportWinForm();
			WinFormReport.ShowDialog();

		}

		private void AssetReportButton_Click(object sender, RoutedEventArgs e)
		{
			

			
			
			ReportWinForm WinFormReport = new ReportWinForm();

			WinFormReport.ShowDialog();

		}
	}
}
