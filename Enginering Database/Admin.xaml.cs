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
		private ErrorSystem err = new ErrorSystem();
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
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				ProgressBar.Value = e.ProgressPercentage;
				StatusLabel.Content = $"Recalculating expiry dates for Statutory items.   Progress: {increase} %";
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateStatutoryDays(object sender, DoWorkEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void GetStatutoryCompliance(object sender, RunWorkerCompletedEventArgs e)
		{
			try
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

					if ((Convert.ToInt32(reader["DaysTillInspection"]) < 0 || Convert.ToInt32(reader["InspectionCount"]) < 1) && (Convert.ToBoolean(reader["Decomission"]) != true))
					{
						StatutoryListViewExpired.Items.Add(statutory);
					}
					else if ((Convert.ToInt32(reader["DaysTillInspection"]) >= 0 && Convert.ToInt32(reader["DaysTillInspection"]) < Convert.ToInt32(settings.StatutoryDays) || Convert.ToInt32(reader["InspectionCount"]) < 1) && (Convert.ToBoolean(reader["Decomission"]) != true))
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateDatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				updateDatabase updateDB = new updateDatabase();

				updateDB.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ShowReportButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ReportWindow reportWindow = new ReportWindow();
				reportWindow.ShowDialog();
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		private void LineMaintenanceButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				MaintenanceSummary addMaintenance = new MaintenanceSummary();
				addMaintenance.Show();
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		private void MeterReadingButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				MeterReadings meterReadings = new MeterReadings();
				meterReadings.Show();
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		private void AssetListButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AssetList assetList = new AssetList();
				assetList.Show();
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		private void InventoryViewButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				InventoryView inventory = new InventoryView();
				inventory.ShowDialog();
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		private void StatutoryComplianceButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				StatutoryCompliance statutory = new StatutoryCompliance();
				statutory.ShowDialog();
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		/// <summary>
		/// Meeting set up script. Will run once Admin window is opened and new Statutory compliance days was recalculated
		/// Will mark in database for items which ones have meeting set up
		/// Checks if meeting is already set. if so then ignoring
		///
		/// </summary>

		private void SetUpMeeting(int id, string meetingSubject, string meetingBody, DateTime meetingDate, int intervalBetweenMeetings)
		{
			try
			{
				EmailClass email = new EmailClass();
				email.SetUpMeetingRequest(meetingSubject, meetingBody, meetingDate, intervalBetweenMeetings);
				db.UpdateStatutoryCompliance("StatutoryCompliance", "MeetingSet", id, true);
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		private void StatutoryListViewExpired_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
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
					if ((bool)reader["Decomission"] == true)
					{
						itemWindow.DecomissionCheckBox.IsChecked = true;
					}
					else
					{
						itemWindow.DecomissionCheckBox.IsChecked = false;
					}
				}
				db.CloseDB();
				itemWindow.ShowDialog();
			}
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		public void UpdateComboBoxes()
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void StatutoryListViewToExpire_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
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

					if ((bool)reader["Decomission"] == true)
					{
						itemWindow.DecomissionCheckBox.IsChecked = true;
					}
					else
					{
						itemWindow.DecomissionCheckBox.IsChecked = false;
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
			catch (Exception ex) { err.RecordError(ex.Message, ex.StackTrace, ex.Source); }
		}

		private void StatutoryListViewToExpireComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			try
			{
				UpdateGroups("ToExpire");
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void StatutoryListViewExpiredComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			try
			{
				UpdateGroups("Expired");
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void UpdateGroups(string filter = null)
		{
			try
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
							if (Convert.ToInt32(reader["DaysTillInspection"]) < 0 && reader["GroupName"].ToString() == StatutoryListViewExpiredComboBox.SelectedItem.ToString() && StatutoryListViewExpiredComboBox.SelectedIndex != 0 || Convert.ToInt32(reader["InspectionCount"]) < 1 && (bool)reader["Decomission"] != true)
							{
								StatutoryListViewExpired.Items.Add(statutory);
							}
							else if (Convert.ToInt32(reader["DaysTillInspection"]) < 0 && StatutoryListViewExpiredComboBox.SelectedIndex == 0 || Convert.ToInt32(reader["InspectionCount"]) < 1 && (bool)reader["Decomission"] != true)
							{
								StatutoryListViewExpired.Items.Add(statutory);
							}

							break;

						case "ToExpire":
							if (Convert.ToInt32(reader["DaysTillInspection"]) > 0 && Convert.ToInt32(reader["DaysTillInspection"]) < Convert.ToInt32(settings.StatutoryDays) && reader["GroupName"].ToString() == StatutoryListViewToExpireComboBox.SelectedItem.ToString() && StatutoryListViewToExpireComboBox.SelectedIndex != 0 && (bool)reader["Decomission"] != true)
							{
								StatutoryListViewToExpire.Items.Add(statutory);
							}
							else if (Convert.ToInt32(reader["DaysTillInspection"]) > 0 && Convert.ToInt32(reader["DaysTillInspection"]) < Convert.ToInt32(settings.StatutoryDays) && StatutoryListViewToExpireComboBox.SelectedIndex == 0 && (bool)reader["Decomission"] != true)
							{
								StatutoryListViewToExpire.Items.Add(statutory);
							}
							break;
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void HyegeneButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Hygene hygene = new Hygene();
				hygene.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void WasteManagementButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				WasteManagement wasteManagement = new WasteManagement();
				wasteManagement.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void AdminConsoleButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ConsoleEmulation console = new ConsoleEmulation();
				console.Show();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}