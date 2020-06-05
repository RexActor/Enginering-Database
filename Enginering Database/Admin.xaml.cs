
using Enginering_Database;

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for Admin.xaml
	/// </summary>
	public partial class Admin : Window
	{
		DatabaseClass db = new DatabaseClass();
		BackgroundWorker worker = new BackgroundWorker();
		int increase;

		public Admin()
		{
			InitializeComponent();

			startRecalculate();

		}

		private void startRecalculate()
		{

			ProgressBar.Visibility = Visibility.Visible;
			StatusLabel.Visibility = Visibility.Visible;
			StatutoryListViewExpired.Visibility = Visibility.Hidden;
			StatutoryListViewToExpire.Visibility = Visibility.Hidden;
			OutOfDateLabel.Visibility = Visibility.Hidden;
			ExpireLabel.Visibility = Visibility.Hidden;

			worker.ProgressChanged += progressChanged;
			worker.DoWork += UpdateStatutoryDays;
			worker.WorkerReportsProgress = true;
			worker.RunWorkerCompleted += GetStatutoryCompliance;
			worker.RunWorkerAsync();

		}
		public void progressChanged(object sender, ProgressChangedEventArgs e)
		{
			ProgressBar.Value = e.ProgressPercentage;
			StatusLabel.Content = $"Recalculating expiry dates for Statutory items.   Progress: {increase} %";
		}


		private void UpdateStatutoryDays(object sender, DoWorkEventArgs e)
		{
			db.ConnectDB();

			int countofItems = 0;
			int currentLine = 0;


			var reader = db.GetAllPDFIds("StatutoryCompliance");
			countofItems = db.CountLinesInDatabaseTable("StatutoryCompliance");

			while (reader.Read())
			{
				StatutoryClass statutory = new StatutoryClass();

				statutory.ID = Convert.ToInt32(reader["ID"]);
				statutory.RenewDateForCalculation = Convert.ToDateTime(reader["RenewDate"]);
				DateTime dt = DateTime.Now.Date;

				TimeSpan dayDifference = statutory.RenewDateForCalculation - dt;

				db.UpdateInventoryView("StatutoryCompliance", "DaysTillInspection", statutory.ID, dayDifference.Days);

				currentLine++;

				increase = Convert.ToInt32(((double)currentLine / countofItems) * 100);

				Dispatcher.Invoke(new System.Action(() =>
				{ worker.ReportProgress(increase); }));
				Thread.Sleep(100);

			}

			db.CloseDB();
		}

		private void GetStatutoryCompliance(object sender, RunWorkerCompletedEventArgs e)
		{

			db.ConnectDB();

			var reader = db.GetAllPDFIds("StatutoryCompliance");

			while (reader.Read())
			{
				StatutoryClass statutory = new StatutoryClass();

				statutory.ID = Convert.ToInt32(reader["ID"]);
				statutory.EquipmentDescription = reader["EquipmentDescription"].ToString();
				statutory.DaysLeftTillInspection = reader["DaysTillInspection"].ToString();
				statutory.RenewDate = String.Format("{0:d}", reader["RenewDate"]);
				statutory.Manufacturer = reader["Manufacturer/Company"].ToString();

				if (Convert.ToInt32(reader["DaysTillInspection"]) < 0)
				{
					StatutoryListViewExpired.Items.Add(statutory);

				}
				else if (Convert.ToInt32(reader["DaysTillInspection"]) > 0 && Convert.ToInt32(reader["DaysTillInspection"]) < 10)
				{
					StatutoryListViewToExpire.Items.Add(statutory);
				}

			}

			db.CloseDB();

			StatusLabel.Visibility = Visibility.Hidden;
			ProgressBar.Visibility = Visibility.Hidden;
			OutOfDateLabel.Visibility = Visibility.Visible;
			ExpireLabel.Visibility = Visibility.Visible;
			StatutoryListViewExpired.Visibility = Visibility.Visible;
			StatutoryListViewToExpire.Visibility = Visibility.Visible;

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

		private void InventoryViewButton_Click(object sender, RoutedEventArgs e)
		{
			InventoryView inventory = new InventoryView();
			inventory.ShowDialog();
		}

		private void StatutoryComplianceButton_Click(object sender, RoutedEventArgs e)
		{
			StatutoryCompliance statutory = new StatutoryCompliance();
			statutory.ShowDialog();
		}
	}
}
