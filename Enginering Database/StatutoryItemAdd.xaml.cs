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
		private readonly DatabaseClass db = new DatabaseClass();

		public StatutoryItemAdd()
		{
			InitializeComponent();
			errorLabel.Visibility = Visibility.Hidden;
			UpdateGroupComboBox();
			UpdateWeeklyMonthlyComboBox();
		}

		private void DateReportIssuedDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			//if (DateReportIssuedDatePicker.SelectedDate != null && RenewDateDatePicker.SelectedDate != null)
			//{
			//	TimeSpan days = RenewDateDatePicker.SelectedDate.Value.Date - DateReportIssuedDatePicker.SelectedDate.Value.Date;
			//	nextInspectionLabelContent.Content = $"{days} days left till next inspection";
			//}
			try
			{
				if (WeeklyMonthlyGroupComboBox.SelectedIndex != 0 && WeeklyMonthlyTextBox.Text != String.Empty)
				{
					switch (WeeklyMonthlyGroupComboBox.SelectedItem.ToString())
					{
						case "Yearly":

							//Do calculation by adding specific amount of Years
							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddYears(Convert.ToInt32(WeeklyMonthlyTextBox.Text));

							break;

						case "Monthly":

							//Do calculation by adding specific amount of Months

							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddMonths(Convert.ToInt32(WeeklyMonthlyTextBox.Text));

							break;

						case "Weekly":
							//Do calculation by adding specific amount of Weeks

							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(WeeklyMonthlyTextBox.Text) * 7);

							break;

						case "Daily":

							//Do calculation by adding specific amount of Days
							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(WeeklyMonthlyTextBox.Text));

							break;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message} {ex.StackTrace}");
			}
		}

		public void UpdateWeeklyMonthlyComboBox()
		{
			db.ConnectDB();

			//StatutoryListViewExpiredComboBox
			//StatutoryListViewToExpireComboBox

			var reader = db.GetAllPDFIds("WeeklyMonthlyRange");
			WeeklyMonthlyGroupComboBox.Items.Add("Please Select");

			while (reader.Read())
			{
				WeeklyMonthlyGroupComboBox.Items.Add(reader["WeeklyMonthlyRange"]);
			}
			WeeklyMonthlyGroupComboBox.SelectedIndex = 0;

			db.CloseDB();
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
				TimeSpan days;
				if (DateReportIssuedDatePicker.SelectedDate < DateTime.Now.Date)
				{
					days = RenewDateDatePicker.SelectedDate.Value.Date - DateTime.Now.Date;
				}
				else
				{
					days = RenewDateDatePicker.SelectedDate.Value.Date - DateReportIssuedDatePicker.SelectedDate.Value.Date;
				}

				nextInspectionLabelContent.Content = $"{days.TotalDays} days left till next inspection";
			}
		}

		private void AddItemButton_Click(object sender, RoutedEventArgs e)
		{
			string valueToAdd = GroupComboBox.SelectedItem.ToString();

			if (ItemDescriptionTextBox.Text != String.Empty && ManufacturerCompanyTextBox.Text != String.Empty && CompanyInsurerTextBox.Text != string.Empty && SerialNumberTextBox.Text != String.Empty && WeeklyMonthlyTextBox.Text != string.Empty && DateReportIssuedDatePicker.SelectedDate != null && RenewDateDatePicker.SelectedDate != null && GroupComboBox.SelectedIndex != 0 && WeeklyMonthlyGroupComboBox.SelectedIndex != 0)
			{
				//upload into database
				db.ConnectDB();

				//TimeSpan daysTillInspection = RenewDateDatePicker.SelectedDate.Value.Date - DateReportIssuedDatePicker.SelectedDate.Value.Date;

				//TimeSpan diff = RenewDateDatePicker.SelectedDate.Value.Date.Subtract(DateTime.Now.Date);

				TimeSpan daysTillInspection = RenewDateDatePicker.SelectedDate.Value.Date - DateTime.Now.Date;

				//string totalDays = (RenewDateDatePicker.SelectedDate.Value.Date - DateTime.Now.Date).TotalDays.ToString();

				//int correctTimeSpan = Convert.ToInt32(daysTillInspection);
				//MessageBox.Show(valueToAdd);
				db.AddStatutoryItem("StatutoryCompliance", ItemDescriptionTextBox.Text, ManufacturerCompanyTextBox.Text, DateReportIssuedDatePicker.SelectedDate.Value.Date, RenewDateDatePicker.SelectedDate.Value.Date, SerialNumberTextBox.Text, WeeklyMonthlyTextBox.Text, daysTillInspection.TotalDays.ToString(), CompanyInsurerTextBox.Text, valueToAdd, WeeklyMonthlyGroupComboBox.SelectedItem.ToString(), "No");

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

		private void WeeklyMonthlyGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (WeeklyMonthlyTextBox.Text != String.Empty && DateReportIssuedDatePicker.SelectedDate != null)
			{
				switch (WeeklyMonthlyGroupComboBox.SelectedItem.ToString())
				{
					case "Yearly":

						//Do calculation by adding specific amount of Years
						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddYears(Convert.ToInt32(WeeklyMonthlyTextBox.Text));

						break;

					case "Monthly":

						//Do calculation by adding specific amount of Months

						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddMonths(Convert.ToInt32(WeeklyMonthlyTextBox.Text));

						break;

					case "Weekly":
						//Do calculation by adding specific amount of Weeks

						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(WeeklyMonthlyTextBox.Text) * 7);

						break;

					case "Daily":

						//Do calculation by adding specific amount of Days
						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(WeeklyMonthlyTextBox.Text));

						break;
				}
			}
		}
	}
}