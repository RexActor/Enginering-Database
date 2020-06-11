using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for StatutoryItemAdd.xaml
	/// </summary>
	public partial class StatutoryItemAdd : Window
	{

		DatabaseClass db = new DatabaseClass();
		public StatutoryItemAdd()
		{
			InitializeComponent();
			errorLabel.Visibility = Visibility.Hidden;
		}

		private void DateReportIssuedDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{

			if (DateReportIssuedDatePicker.SelectedDate != null && RenewDateDatePicker.SelectedDate !=null)
			{
				TimeSpan days = RenewDateDatePicker.SelectedDate.Value.Date - DateReportIssuedDatePicker.SelectedDate.Value.Date;
				nextInspectionLabelContent.Content = $"{days} days left till next inspection";
			}
		}

		private void RenewDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (RenewDateDatePicker.SelectedDate != null && DateReportIssuedDatePicker.SelectedDate!=null)
			{
				TimeSpan days = RenewDateDatePicker.SelectedDate.Value.Date - DateReportIssuedDatePicker.SelectedDate.Value.Date;
				nextInspectionLabelContent.Content = $"{days.TotalDays} days left till next inspection";
			}
		}

		private void AddItemButton_Click(object sender, RoutedEventArgs e)
		{

			if(ItemDescriptionTextBox.Text!=String.Empty || ManufacturerCompanyTextBox.Text!=String.Empty || CompanyInsurerTextBox.Text!=string.Empty || SerialNumberTextBox.Text!=String.Empty || WeeklyMonthlyTextBox.Text!=string.Empty || DateReportIssuedDatePicker.SelectedDate!=null || RenewDateDatePicker.SelectedDate != null)
			{

				//upload into database
				db.ConnectDB();


				db.AddStatutoryItem("StatutoryCompliance", ItemDescriptionTextBox.Text, ManufacturerCompanyTextBox.Text, DateReportIssuedDatePicker.SelectedDate.Value.Date, RenewDateDatePicker.SelectedDate.Value.Date, SerialNumberTextBox.Text, WeeklyMonthlyTextBox.Text, CompanyInsurerTextBox.Text);


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
