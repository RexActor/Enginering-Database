﻿
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
			UpdateList("all");
			UpdateComboBox();
		}


		public void UpdateList(string filter = null)
		{
			db.ConnectDB();
			StatutoryComplianceList.Items.Clear();

			var reader = db.GetAllPDFIds("StatutoryCompliance");


			while (reader.Read())
			{
				StatutoryClass stat = new StatutoryClass()
				{
					ID = Convert.ToInt32(reader["ID"]),
					EquipmentDescription = reader["EquipmentDescription"].ToString(),
					RenewDate = string.Format("{0:d}", reader["RenewDate"]),
					DateReportIssued = string.Format("{0:d}", reader["DateReportIssued"]),
					CompanyIssuer = reader["CompanyInsurer"].ToString(),
					DaysLeftTillInspection = reader["DaysTillInspection"].ToString(),
					Manufacturer = reader["ManufacturerCompany"].ToString(),
					SerialNumber = reader["SerialNumber"].ToString(),
					Booked=reader["Booked"].ToString(),
					MonthlyWeeklyRange=reader["MonthlyWeeklyRange"].ToString(),
					MonthlyWeekly = reader["MonthlyWeekly"].ToString(),
					Group=reader["GroupName"].ToString(),
				};

				if (filter == "expired")
				{
					if (Convert.ToInt32(stat.DaysLeftTillInspection) < 0)
					{
						this.StatutoryComplianceList.Items.Add(stat);
					}
				}

				else if(filter == "Group")
				{
					if (stat.Group == StatutoryComplianceGroupComboBox.SelectedItem.ToString())
					{
						this.StatutoryComplianceList.Items.Add(stat);
					}
				}
				else
				{
					this.StatutoryComplianceList.Items.Add(stat);
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
			ItemWindow.MonthlyWeeklyRangeLabelContent.Content = selectedItem.MonthlyWeeklyRange;
			ItemWindow.GroupLabelContent.Content = selectedItem.Group;

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

			if (selectedItem.Booked.ToString() == "Yes")
			{
				ItemWindow.BookedCheckBox.IsChecked = true;
			}
			else
			{
				ItemWindow.BookedCheckBox.IsChecked = false;
			}


			ItemWindow.DateReportIssuedDatePicker.SelectedDate = Convert.ToDateTime(selectedItem.DateReportIssued);
			ItemWindow.RenewDateDatePicker.SelectedDate = Convert.ToDateTime(selectedItem.RenewDate);

			ItemWindow.ShowDialog();


		}

		private void RefreshList_Click(object sender, RoutedEventArgs e)
		{

			UpdateStatutoryDays();
			StatutoryComplianceGroupComboBox.SelectedIndex = 0;

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

		public void UpdateComboBox()
		{

			db.ConnectDB();
				


			var reader = db.GetAllPDFIds("StatutoryComplianceGroups");
			StatutoryComplianceGroupComboBox.Items.Add("Please Select");
		

			while (reader.Read())
			{
				StatutoryComplianceGroupComboBox.Items.Add(reader["GroupDescription"]);
				


			}
			StatutoryComplianceGroupComboBox.SelectedIndex = 0;
			

			db.CloseDB();
		}


		private void StatutoryComplianceGroupComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (StatutoryComplianceGroupComboBox.SelectedIndex > 0)
			{
				UpdateList("Group");
			}

		}
	}
}
