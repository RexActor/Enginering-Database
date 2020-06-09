
using DocumentFormat.OpenXml.Office.CustomUI;

using Enginering_Database;

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for Admin.xaml
	/// </summary>
	public partial class Admin : Window
	{
		DatabaseClass db = new DatabaseClass();
		BackgroundWorker worker = new BackgroundWorker();
		UserSettings settings = new UserSettings();
		
		int increase;

		public Admin()
		{
			InitializeComponent();

			startRecalculate();

		}

		private void startRecalculate()
		{
			settings.openSettings();
			ExpireLabel.Content = $"Statutory Compliance Items To Expire in {settings.StatutoryDays} days";
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
				statutory.EquipmentDescription = reader["EquipmentDescription"].ToString();
				statutory.RenewDateForCalculation = Convert.ToDateTime(reader["RenewDate"]);
				statutory.meetingSetStatus = (bool)reader["MeetingSet"];
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

			int interval = 0;
			var reader = db.GetAllPDFIds("StatutoryCompliance");

			while (reader.Read())
			{
				StatutoryClass statutory = new StatutoryClass();

				statutory.ID = Convert.ToInt32(reader["ID"]);
				statutory.EquipmentDescription = reader["EquipmentDescription"].ToString();
				statutory.DaysLeftTillInspection = reader["DaysTillInspection"].ToString();
				statutory.RenewDate = String.Format("{0:d}", reader["RenewDate"]);
				statutory.Manufacturer = reader["ManufacturerCompany"].ToString();
				statutory.meetingSetStatus = (bool)reader["MeetingSet"];

				if (Convert.ToInt32(reader["DaysTillInspection"]) < 0)
				{
					StatutoryListViewExpired.Items.Add(statutory);

				}
				else if (Convert.ToInt32(reader["DaysTillInspection"]) > 0 && Convert.ToInt32(reader["DaysTillInspection"]) < Convert.ToInt32(settings.StatutoryDays))
				{

					if (statutory.meetingSetStatus == false)
					{

						DateTime meetingDate = new DateTime();


						DateTime renewDateTime = Convert.ToDateTime(statutory.RenewDate);

						int daysForward = 0 - Convert.ToInt32(settings.DueDateGap);

						meetingDate = renewDateTime.AddDays(0-Convert.ToInt64(settings.MeetingDaysAhead));

						if (meetingDate < DateTime.Now.Date)
						{
							meetingDate = DateTime.Now.Date.AddDays(1);
						}


						setUpMeeting(statutory.ID, statutory.EquipmentDescription, $" {statutory.EquipmentDescription} item due to run out of Compliance. Renew Date is {statutory.RenewDate} and there are {statutory.DaysLeftTillInspection} days left (on day when meeting was set up)", meetingDate.Date, interval);


						interval++;
					}
					


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
			//setUpMeeting(28, "Test Meeting", "Updating Database with meeting request", 2);
		}


		/// <summary>
		/// Meeting set up script. Will run once Admin window is opened and new Statutory compliance days was recalculated
		/// Will mark in database for items which ones have meeting set up
		/// Checks if meeting is already set. if so then ignoring
		/// 
		/// </summary>

		private void setUpMeeting(int id, string meetingSubject, string meetingBody, DateTime meetingDate, int intervalBetweenMeetings)
		{

			EmailClass email = new EmailClass();
			email.SetUpMeetingRequest(meetingSubject, meetingBody, meetingDate, intervalBetweenMeetings);



			db.UpdateStatutoryCompliance("StatutoryCompliance", "MeetingSet", id, true);


		}

		private void StatutoryListViewExpired_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			StatutoryItem itemWindow = new StatutoryItem();

			db.ConnectDB();
			string id = ((StatutoryClass)StatutoryListViewExpired.SelectedItem).ID.ToString();
			string item = ((StatutoryClass)StatutoryListViewExpired.SelectedItem).EquipmentDescription;
			var reader = db.GetStatutoryItem("StatutoryCompliance",((StatutoryClass)StatutoryListViewExpired.SelectedItem).ID);


			while (reader.Read())
			{
				itemWindow.TitleLabel.Content=$"Details for {item} with --> ID [{id}]";
				itemWindow.hiddenID.Content = reader["ID"].ToString();
				itemWindow.ManufacturerTextBox.Text = reader["ManufacturerCompany"].ToString();

				itemWindow.SerialNumberTextBox.Text = reader["SerialNumber"].ToString();
				itemWindow.InsurerTextBox.Text = reader["CompanyInsurer"].ToString();
				itemWindow.MonthlyWeeklyTextBox.Text = reader["MonthlyWeekly"].ToString();

				itemWindow.DateReportIssuedDatePicker.SelectedDate = Convert.ToDateTime(reader["DateReportIssued"]);
				itemWindow.RenewDateDatePicker.SelectedDate = Convert.ToDateTime(reader["RenewDate"]);


				if (Convert.ToInt32(reader["DaysTillInspection"]) > 0)
				{
					itemWindow.NextInspectionLabel.Background = Brushes.LightGreen;
					itemWindow.NextInspectionLabel.Content = $"{Convert.ToInt32(reader["DaysTillInspection"])} days left";
				}
				else
				{
					itemWindow.NextInspectionLabel.Background = Brushes.PaleVioletRed;
					itemWindow.NextInspectionLabel.Content = $"{Math.Abs(Convert.ToInt32(Convert.ToInt32(reader["DaysTillInspection"])))} days overdue";
				}





			}

			db.CloseDB();




			itemWindow.ShowDialog();



		}

		private void StatutoryListViewToExpire_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{

			StatutoryItem itemWindow = new StatutoryItem();

			db.ConnectDB();
			string id = ((StatutoryClass)StatutoryListViewToExpire.SelectedItem).ID.ToString();
			string item = ((StatutoryClass)StatutoryListViewToExpire.SelectedItem).EquipmentDescription;
			var reader = db.GetStatutoryItem("StatutoryCompliance", ((StatutoryClass)StatutoryListViewToExpire.SelectedItem).ID);


			while (reader.Read())
			{
				itemWindow.TitleLabel.Content = $"Details for {item} with --> ID [{id}]";
				itemWindow.hiddenID.Content = reader["ID"].ToString();
				itemWindow.ManufacturerTextBox.Text = reader["ManufacturerCompany"].ToString();

				itemWindow.SerialNumberTextBox.Text = reader["SerialNumber"].ToString();
				itemWindow.InsurerTextBox.Text = reader["CompanyInsurer"].ToString();
				itemWindow.MonthlyWeeklyTextBox.Text = reader["MonthlyWeekly"].ToString();

				itemWindow.DateReportIssuedDatePicker.SelectedDate = Convert.ToDateTime(reader["DateReportIssued"]);
				itemWindow.RenewDateDatePicker.SelectedDate = Convert.ToDateTime(reader["RenewDate"]);


				if (Convert.ToInt32(reader["DaysTillInspection"]) > 0)
				{
					itemWindow.NextInspectionLabel.Background = Brushes.LightGreen;
					itemWindow.NextInspectionLabel.Content = $"{Convert.ToInt32(reader["DaysTillInspection"])} days left";
				}
				else
				{
					itemWindow.NextInspectionLabel.Background = Brushes.PaleVioletRed;
					itemWindow.NextInspectionLabel.Content = $"{Math.Abs(Convert.ToInt32(Convert.ToInt32(reader["DaysTillInspection"])))} days overdue";
				}





			}

			db.CloseDB();




			itemWindow.ShowDialog();



		}
	}
}
