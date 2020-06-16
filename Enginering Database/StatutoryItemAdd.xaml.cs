using System;
using System.Windows;
using System.Windows.Controls;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for StatutoryItemAdd.xaml
	/// </summary>
	public partial class StatutoryItemAdd : Window
	{

		readonly DatabaseClass db = new DatabaseClass();
		public StatutoryItemAdd()
		{
			InitializeComponent();
			errorLabel.Visibility = Visibility.Hidden;
			UpdateGroupComboBox();
		}

		private void DateReportIssuedDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{

			if (DateReportIssuedDatePicker.SelectedDate != null && RenewDateDatePicker.SelectedDate != null)
			{
				TimeSpan days = RenewDateDatePicker.SelectedDate.Value.Date - DateReportIssuedDatePicker.SelectedDate.Value.Date;
				nextInspectionLabelContent.Content = $"{days} days left till next inspection";
			}
		}

		public void UpdateGroupComboBox()
		{
			db.ConnectDB();

			//StatutoryListViewExpiredComboBox
			//StatutoryListViewToExpireComboBox


			var reader = db.GetAllPDFIds("StatutoryComplianceGroups");
			GroupComboBox.Items.Add("Please Select");


			while (reader.Read())
			{
				GroupComboBox.Items.Add(reader["GroupDescription"]);



			}
			GroupComboBox.SelectedIndex = 0;


			db.CloseDB();
		}

		private void RenewDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (RenewDateDatePicker.SelectedDate != null && DateReportIssuedDatePicker.SelectedDate != null)
			{
				TimeSpan days = RenewDateDatePicker.SelectedDate.Value.Date - DateReportIssuedDatePicker.SelectedDate.Value.Date;
				nextInspectionLabelContent.Content = $"{days.TotalDays} days left till next inspection";
			}
		}

		private void AddItemButton_Click(object sender, RoutedEventArgs e)
		{
			string valueToAdd = GroupComboBox.SelectedItem.ToString();

			if (ItemDescriptionTextBox.Text != String.Empty && ManufacturerCompanyTextBox.Text != String.Empty && CompanyInsurerTextBox.Text != string.Empty && SerialNumberTextBox.Text != String.Empty && WeeklyMonthlyTextBox.Text != string.Empty && DateReportIssuedDatePicker.SelectedDate != null && RenewDateDatePicker.SelectedDate != null && GroupComboBox.SelectedIndex != 0)
			{

				//upload into database
				db.ConnectDB();

				
				//MessageBox.Show(valueToAdd);
				db.AddStatutoryItem("StatutoryCompliance", ItemDescriptionTextBox.Text, ManufacturerCompanyTextBox.Text, DateReportIssuedDatePicker.SelectedDate.Value.Date, RenewDateDatePicker.SelectedDate.Value.Date, SerialNumberTextBox.Text, WeeklyMonthlyTextBox.Text, CompanyInsurerTextBox.Text, valueToAdd);


				db.CloseDB();

				errorLabel.Foreground = System.Windows.Media.Brushes.Green;
				errorLabel.FontWeight = FontWeights.Bold;
				errorLabel.Content = "Item added into database";
				errorLabel.Visibility = Visibility.Visible;


				//this.Close();

			}
			else
			{
				errorLabel.Foreground = System.Windows.Media.Brushes.Red;
				errorLabel.FontWeight = FontWeights.Bold;
				errorLabel.Content = "Please fill all required fields";
				errorLabel.Visibility = Visibility.Visible;
			}
		}
	}
}
