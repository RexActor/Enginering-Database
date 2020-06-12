
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
		readonly DatabaseClass db = new DatabaseClass();

		public StatutoryCompliance()
		{
			InitializeComponent();
			UpdateList();
		}


		public void UpdateList(string filter = null)
		{
			db.ConnectDB();
			StatutoryComplianceList.Items.Clear();

			var reader = db.GetAllPDFIds("StatutoryCompliance");


			while (reader.Read())
			{
				new StatutoryClass().ID = Convert.ToInt32(reader["ID"]);
				new StatutoryClass().EquipmentDescription = reader["EquipmentDescription"].ToString();
				new StatutoryClass().RenewDate = string.Format("{0:d}", reader["RenewDate"]);
				new StatutoryClass().DateReportIssued = string.Format("{0:d}", reader["DateReportIssued"]);
				new StatutoryClass().CompanyIssuer = reader["CompanyInsurer"].ToString();
				new StatutoryClass().DaysLeftTillInspection = reader["DaysTillInspection"].ToString();
				new StatutoryClass().Manufacturer = reader["ManufacturerCompany"].ToString();
				new StatutoryClass().SerialNumber = reader["SerialNumber"].ToString();
				new StatutoryClass().MonthlyWeekly = reader["MonthlyWeekly"].ToString();


				if (filter == "expired")
				{
					if (Convert.ToInt32(new StatutoryClass().DaysLeftTillInspection) < 0)
					{
						this.StatutoryComplianceList.Items.Add(new StatutoryClass());
					}
				}
				else
				{
					this.StatutoryComplianceList.Items.Add(new StatutoryClass());
				}

			}




			db.CloseDB();
		}

		private void ExpiredCheckBox_Click(object sender, RoutedEventArgs e)
		{

			if (ExpiredCheckBox.IsChecked == true)
			{
				UpdateList("expired");

			}
			else
			{
				UpdateList("all");
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


			UpdateList("all");
		}



		private void UpdateStatutoryDays()
		{
			db.ConnectDB();

			
			var reader = db.GetAllPDFIds("StatutoryCompliance");
		

			while (reader.Read())
			{
				StatutoryClass statutory = new StatutoryClass
				{
					ID = Convert.ToInt32(reader["ID"]),
					EquipmentDescription = reader["EquipmentDescription"].ToString(),
					RenewDateForCalculation = Convert.ToDateTime(reader["RenewDate"]),
					meetingSetStatus = (bool)reader["MeetingSet"]
				};
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
