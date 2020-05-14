using Enginering_Database;
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
	/// Interaction logic for Admin.xaml
	/// </summary>
	public partial class Admin : Window
	{
		public Admin()
		{
			InitializeComponent();
		}

		private void UpdateDatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			updateDatabase updateDB = new updateDatabase();

			updateDB.ShowDialog();

		}

		private void ShowReportButton_Click(object sender, RoutedEventArgs e)
		{
			ReportWindow repWin = new ReportWindow();
			repWin.Show();
		}

		private void LineMaintenanceButton_Click(object sender, RoutedEventArgs e)
		{
			MaintenanceSummary addMaintenance = new MaintenanceSummary();

			addMaintenance.Show();
		}

		private void MeterReadingButton_Click(object sender, RoutedEventArgs e)
		{

			MeterReadings meterReadings = new MeterReadings();
			meterReadings.Show();
		}

		private void AssetListButton_Click(object sender, RoutedEventArgs e)
		{
			AssetList assetList = new AssetList();
			assetList.Show();
		}
	}
}
