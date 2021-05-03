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
		private readonly DatabaseClass db = new DatabaseClass();
		private readonly BackgroundWorker worker = new BackgroundWorker();
		private readonly UserSettings settings = new UserSettings();

		private int increase;

		public Admin()
		{
			InitializeComponent();
			UpdateComboBoxes();
			StartRecalculate();
		}

		private void StartRecalculate()
		{
			settings.openSettings();
			ExpireLabel.Content = $"Statutory Compliance Items To Expire in {settings.StatutoryDays} days";
			ProgressBar.Visibility = Visibility.Visible;

			StatusLabel.Visibility = Visibility.Visible;
			StatutoryListViewExpired.Visibility = Visibility.Hidden;
			StatutoryListViewToExpire.Visibility = Visibility.Hidden;

			StatutoryListViewExpiredComboBox.Visibility = Visibility.Hidden;
			StatutoryListViewToExpireComboBox.Visibility = Visibility.Hidden;

			GroupLabel1.Visibility = Visibility.Hidden;
			GroupLabel2.Visibility = Visibility.Hidden;

			OutOfDateLabel.Visibility = Visibility.Hidden;
			ExpireLabel.Visibility = Visibility.Hidden;

			worker.ProgressChanged += ProgressChanged;
			worker.DoWork += UpdateStatutoryDays;
			worker.WorkerReportsProgress = true;
			worker.RunWorkerCompleted += GetStatutoryCompliance;
			worker.RunWorkerAsync();
		}

		public void ProgressChanged(object sender, ProgressChangedEventArgs e)
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
				StatutoryClass statutory = new StatutoryClass
				{
					ID = Convert.ToInt32(reader["ID"]),
					EquipmentDescription = reader["EquipmentDescription"].ToString(),
					RenewDateForCalculation = Convert.ToDateTime(reader["RenewDate"]),
					Booked = reader["Booked"].ToString(),
					meetingSetStatus = (bool)reader["MeetingSet"]
				};
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
			StatutoryListViewExpired.Items.Clear();
			StatutoryListViewToExpire.Items.Clear();

			db.ConnectDB();

			int interval = 0;
			var reader = db.GetAllPDFIds("StatutoryCompliance");

			while (reader.Read())
			{
				StatutoryClass statutory = new StatutoryClass
				{
					ID = Convert.ToInt32(reader["ID"]),
					EquipmentDescription = reader["EquipmentDescription"].ToString(),
					DaysLeftTillInspection = reader["DaysTillInspection"].ToString(),
					RenewDate = String.Format("{0:d}", reader["RenewDate"]),
					Manufacturer = reader["ManufacturerCompany"].ToString(),
					CompanyIssuer = reader["CompanyInsurer"].ToString(),
					Booked = reader["Booked"].ToString(),
					MonthlyWeeklyRange = reader["MonthlyWeeklyRange"].ToString(),
					meetingSetStatus = (bool)reader["MeetingSet"]
				};

				if (Convert.ToInt32(reader["DaysTillInspection"]) < 0 || Convert.ToInt32(reader["InspectionCount"]) < 1)
				{
					StatutoryListViewExpired.Items.Add(statutory);
				}
				else if (Convert.ToInt32(reader["DaysTillInspection"]) >= 0 && Convert.ToInt32(reader["DaysTillInspection"]) < Convert.ToInt32(settings.StatutoryDays) || Convert.ToInt32(reader["InspectionCount"]) < 1)
				{
					if (statutory.meetingSetStatus == false)
					{
						DateTime meetingDate;

						DateTime renewDateTime = Convert.ToDateTime(statutory.RenewDate);

						meetingDate = renewDateTime.AddDays(0 - Convert.ToInt64(settings.MeetingDaysAhead));

						if (meetingDate < DateTime.Now.Date)
						{
							meetingDate = DateTime.Now.Date.AddDays(1);
						}

						SetUpMeeting(statutory.ID, statutory.EquipmentDescription, $"Comapny/Insurer :{statutory.CompanyIssuer} ##  Item: {statutory.EquipmentDescription}  ## due to run out of Compliance. Renew Date is {statutory.RenewDate} and there are {statutory.DaysLeftTillInspection} days left (on day when meeting was set up)", meetingDate.Date, interval);

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
			StatutoryListViewExpiredComboBox.Visibility = Visibility.Visible;
			StatutoryListViewToExpireComboBox.Visibility = Visibility.Visible;

			GroupLabel1.Visibility = Visibility.Visible;
			GroupLabel2.Visibility = Visibility.Visible;
		}

		private void UpdateDatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			updateDatabase updateDB = new updateDatabase();

			updateDB.ShowDialog();
		}

		private void ShowReportButton_Click(object sender, RoutedEventArgs e)
		{
			ReportWindow reportWindow = new ReportWindow();
			reportWindow.ShowDialog();
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

		/// <summary>
		/// Meeting set up script. Will run once Admin window is opened and new Statutory compliance days was recalculated
		/// Will mark in database for items which ones have meeting set up
		/// Checks if meeting is already set. if so then ignoring
		///
		/// </summary>

		private void SetUpMeeting(int id, string meetingSubject, string meetingBody, DateTime meetingDate, int intervalBetweenMeetings)
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
			var reader = db.GetStatutoryItem("StatutoryCompliance", ((StatutoryClass)StatutoryListViewExpired.SelectedItem).ID);

			while (reader.Read())
			{
				itemWindow.TitleLabel.Content = $"Details for {item} with --> ID [{id}]";
				itemWindow.hiddenID.Content = reader["ID"].ToString();
				itemWindow.hiddenInspectionCount.Content = reader["InspectionCount"].ToString();
				itemWindow.ManufacturerTextBox.Text = reader["ManufacturerCompany"].ToString();

				itemWindow.SerialNumberTextBox.Text = reader["SerialNumber"].ToString();
				itemWindow.InsurerTextBox.Text = reader["CompanyInsurer"].ToString();
				itemWindow.MonthlyWeeklyTextBox.Text = reader["MonthlyWeekly"].ToString();
				itemWindow.GroupLabelContent.Content = reader["GroupName"].ToString();
				itemWindow.DateReportIssuedDatePicker.SelectedDate = Convert.ToDateTime(reader["DateReportIssued"]);
				itemWindow.MonthlyWeeklyRangeLabelContent.Content = reader["MonthlyWeeklyRange"].ToString();
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
				if (reader["Booked"].ToString() == "Yes")
				{
					itemWindow.BookedCheckBox.IsChecked = true;
				}
				else
				{
					itemWindow.BookedCheckBox.IsChecked = false;
				}
			}

			db.CloseDB();

			itemWindow.ShowDialog();
		}

		public void UpdateComboBoxes()
		{
			db.ConnectDB();

			//StatutoryListViewExpiredComboBox
			//StatutoryListViewToExpireComboBox

			var reader = db.GetAllPDFIds("StatutoryComplianceGroups");
			StatutoryListViewExpiredComboBox.Items.Add("Please Select");
			StatutoryListViewToExpireComboBox.Items.Add("Please Select");

			while (reader.Read())
			{
				StatutoryListViewExpiredComboBox.Items.Add(reader["GroupDescription"]);
				StatutoryListViewToExpireComboBox.Items.Add(reader["GroupDescription"]);
			}
			StatutoryListViewExpiredComboBox.SelectedIndex = 0;
			StatutoryListViewToExpireComboBox.SelectedIndex = 0;

			db.CloseDB();
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
				itemWindow.hiddenInspectionCount.Content = reader["InspectionCount"].ToString();
				itemWindow.SerialNumberTextBox.Text = reader["SerialNumber"].ToString();
				itemWindow.InsurerTextBox.Text = reader["CompanyInsurer"].ToString();
				itemWindow.MonthlyWeeklyTextBox.Text = reader["MonthlyWeekly"].ToString();
				itemWindow.GroupLabelContent.Content = reader["GroupName"].ToString();
				itemWindow.DateReportIssuedDatePicker.SelectedDate = Convert.ToDateTime(reader["DateReportIssued"]);
				itemWindow.MonthlyWeeklyRangeLabelContent.Content = reader["MonthlyWeeklyRange"].ToString();
				itemWindow.RenewDateDatePicker.SelectedDate = Convert.ToDateTime(reader["RenewDate"]);

				if (reader["Booked"].ToString() == "Yes")
				{
					itemWindow.BookedCheckBox.IsChecked = true;
				}
				else
				{
					itemWindow.BookedCheckBox.IsChecked = false;
				}

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

		private void StatutoryListViewToExpireComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			UpdateGroups("ToExpire");
		}

		private void StatutoryListViewExpiredComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			UpdateGroups("Expired");
		}

		public void UpdateGroups(string filter = null)
		{
			if (filter == "Expired")
			{
				StatutoryListViewExpired.Items.Clear();
			}

			if (filter == "ToExpire")
			{
				StatutoryListViewToExpire.Items.Clear();
			}

			db.ConnectDB();

			var reader = db.GetAllPDFIds("StatutoryCompliance");

			while (reader.Read())
			{
				StatutoryClass statutory = new StatutoryClass
				{
					ID = Convert.ToInt32(reader["ID"]),
					EquipmentDescription = reader["EquipmentDescription"].ToString(),
					DaysLeftTillInspection = reader["DaysTillInspection"].ToString(),
					RenewDate = String.Format("{0:d}", reader["RenewDate"]),
					Manufacturer = reader["ManufacturerCompany"].ToString(),
					Booked = reader["Booked"].ToString(),
					InspectionCount = reader["InspectionCount"].ToString(),
					meetingSetStatus = (bool)reader["MeetingSet"]
				};
				switch (filter)
				{
					case "Expired":
						if (Convert.ToInt32(reader["DaysTillInspection"]) < 0 && reader["GroupName"].ToString() == StatutoryListViewExpiredComboBox.SelectedItem.ToString() && StatutoryListViewExpiredComboBox.SelectedIndex != 0 || Convert.ToInt32(reader["InspectionCount"]) < 1)
						{
							StatutoryListViewExpired.Items.Add(statutory);
						}
						else if (Convert.ToInt32(reader["DaysTillInspection"]) < 0 && StatutoryListViewExpiredComboBox.SelectedIndex == 0 || Convert.ToInt32(reader["InspectionCount"]) < 1)
						{
							StatutoryListViewExpired.Items.Add(statutory);
						}

						break;

					case "ToExpire":
						if (Convert.ToInt32(reader["DaysTillInspection"]) > 0 && Convert.ToInt32(reader["DaysTillInspection"]) < Convert.ToInt32(settings.StatutoryDays) && reader["GroupName"].ToString() == StatutoryListViewToExpireComboBox.SelectedItem.ToString() && StatutoryListViewToExpireComboBox.SelectedIndex != 0)
						{
							StatutoryListViewToExpire.Items.Add(statutory);
						}
						else if (Convert.ToInt32(reader["DaysTillInspection"]) > 0 && Convert.ToInt32(reader["DaysTillInspection"]) < Convert.ToInt32(settings.StatutoryDays) && StatutoryListViewToExpireComboBox.SelectedIndex == 0)
						{
							StatutoryListViewToExpire.Items.Add(statutory);
						}
						break;
				}
			}
		}

		private void HyegeneButton_Click(object sender, RoutedEventArgs e)
		{
			Hygene hygene = new Hygene();
			hygene.ShowDialog();
		}

		private void WasteManagementButton_Click(object sender, RoutedEventArgs e)
		{
			WasteManagement wasteManagement = new WasteManagement();
			wasteManagement.ShowDialog();
		}

		private void AdminConsoleButton_Click(object sender, RoutedEventArgs e)
		{
			ConsoleEmulation console = new ConsoleEmulation();
			console.Show();
		}
	}
}