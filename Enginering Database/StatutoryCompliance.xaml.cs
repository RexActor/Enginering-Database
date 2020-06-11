
using DocumentFormat.OpenXml.Office.CustomUI;

using System;

using System.Windows;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for StatutoryCompliance.xaml
	/// </summary>
	public partial class StatutoryCompliance : Window
	{
		DatabaseClass db = new DatabaseClass();

		public StatutoryCompliance()
		{
			InitializeComponent();
			updateList();
		}


		public void updateList(string filter = null)
		{
			db.ConnectDB();
			StatutoryComplianceList.Items.Clear();

			var reader = db.GetAllPDFIds("StatutoryCompliance");


			while (reader.Read())
			{
				StatutoryClass StatutoryItem = new StatutoryClass();

				StatutoryItem.ID = Convert.ToInt32(reader["ID"]);
				StatutoryItem.EquipmentDescription = reader["EquipmentDescription"].ToString();
				StatutoryItem.RenewDate = String.Format("{0:d}", reader["RenewDate"]);
				StatutoryItem.DateReportIssued = String.Format("{0:d}", reader["DateReportIssued"]);
				StatutoryItem.CompanyIssuer = reader["CompanyInsurer"].ToString();
				StatutoryItem.DaysLeftTillInspection = reader["DaysTillInspection"].ToString();
				StatutoryItem.Manufacturer = reader["ManufacturerCompany"].ToString();
				StatutoryItem.SerialNumber = reader["SerialNumber"].ToString();
				StatutoryItem.MonthlyWeekly = reader["MonthlyWeekly"].ToString();


				if (filter == "expired")
				{
					if (Convert.ToInt32(StatutoryItem.DaysLeftTillInspection) < 0)
					{
						StatutoryComplianceList.Items.Add(StatutoryItem);
					}
				}
				else
				{
					StatutoryComplianceList.Items.Add(StatutoryItem);
				}

			}




			db.CloseDB();
		}

		private void ExpiredCheckBox_Click(object sender, RoutedEventArgs e)
		{

			if (ExpiredCheckBox.IsChecked == true)
			{
				updateList("expired");

			}
			else
			{
				updateList("all");
			}

		}

		private void StatutoryComplianceList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{


			StatutoryClass selectedItem = (StatutoryClass)StatutoryComplianceList.SelectedItem;

			StatutoryItem ItemWindow = new StatutoryItem();


			ItemWindow.TitleLabel.Content = $"Details for {selectedItem.EquipmentDescription} with --> ID [{selectedItem.ID}]";

			ItemWindow.ManufacturerTextBox.Text = selectedItem.Manufacturer;
			ItemWindow.InsurerTextBox.Text = selectedItem.CompanyIssuer;
			ItemWindow.SerialNumberTextBox.Text = selectedItem.SerialNumber;
			ItemWindow.MonthlyWeeklyTextBox.Text = selectedItem.MonthlyWeekly;
			ItemWindow.hiddenID.Content = selectedItem.ID;
			if (Convert.ToInt32(selectedItem.DaysLeftTillInspection) > 0)
			{
				ItemWindow.NextInspectionLabel.Background = Brushes.LightGreen;
				ItemWindow.NextInspectionLabel.Content = $"{selectedItem.DaysLeftTillInspection} days left";
			}
			else
			{
				ItemWindow.NextInspectionLabel.Background = Brushes.PaleVioletRed;
				ItemWindow.NextInspectionLabel.Content = $"{Math.Abs(Convert.ToInt32(selectedItem.DaysLeftTillInspection))} days overdue";
			}

			ItemWindow.DateReportIssuedDatePicker.SelectedDate = Convert.ToDateTime(selectedItem.DateReportIssued);
			ItemWindow.RenewDateDatePicker.SelectedDate = Convert.ToDateTime(selectedItem.RenewDate);

			ItemWindow.ShowDialog();


		}

		private void RefreshList_Click(object sender, RoutedEventArgs e)
		{

			UpdateStatutoryDays();


			updateList("all");
		}



		private void UpdateStatutoryDays()
		{
			db.ConnectDB();

			
			var reader = db.GetAllPDFIds("StatutoryCompliance");
		

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
				

			}

			db.CloseDB();
		}

		private void AddStatutoryItemButton_Click(object sender, RoutedEventArgs e)
		{
			StatutoryItemAdd addItem = new StatutoryItemAdd();

			addItem.ShowDialog();
		}
	}
}
