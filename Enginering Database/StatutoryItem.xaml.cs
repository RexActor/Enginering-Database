
using DocumentFormat.OpenXml.Bibliography;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for StatutoryItem.xaml
	/// </summary>
	public partial class StatutoryItem : Window
	{

		private bool correctReportDate = false;
		private bool correctRenewDate = false;
		private bool ReportDateWasChanged = false;
		private bool RenewDateWasChanged = false;
		private bool bookedStatusChanged = false;
		private bool editMode = false;
		// readonly private bool uploaded = false;

		private bool manufacturedChanged = false;
		private bool companyInsurerChanged = false;
		private bool serialNumberChanged = false;
		private bool monthlyWeeklyChanged = false;
		private DateTime currentReportDate;

		readonly DatabaseClass db = new DatabaseClass();
		public StatutoryItem()
		{
			InitializeComponent();





			RenewDateInfoLabel.Visibility = Visibility.Hidden;
			ReportIssuedInfoLabel.Visibility = Visibility.Hidden;
			SerialNumberUploadStatus.Visibility = Visibility.Hidden;
			ManufacturerCompanyUploadStatus.Visibility = Visibility.Hidden;
			CompanyInsurerUploadStatus.Visibility = Visibility.Hidden;
			MonthlyWeeklyUploadStatus.Visibility = Visibility.Hidden;
			MonthlyWeeklyComboBoxUploadStatus.Visibility = Visibility.Hidden;
			BookedInfoLabel.Visibility = Visibility.Hidden;
			MonthlyWeeklyChangeComboBox.Visibility = Visibility.Hidden;

			//not needed to be shown. required just for pulling data to update database
			hiddenID.Visibility = Visibility.Hidden;

		}

		private void StatutoryItemUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			currentReportDate = DateReportIssuedDatePicker.SelectedDate.Value.Date;
			if (!editMode)
			{

				editMode = true;
				StatutoryItemUpdateButton.Content = "Save";


				//activating textboxes for edit mode
				DateReportIssuedDatePicker.IsEnabled = true;
				RenewDateDatePicker.IsEnabled = true;
				ManufacturerTextBox.IsEnabled = true;
				InsurerTextBox.IsEnabled = true;
				SerialNumberTextBox.IsEnabled = true;
				MonthlyWeeklyTextBox.IsEnabled = true;
				BookedCheckBox.IsEnabled = true;

				ManufacturerCompanyUploadStatus.Visibility = Visibility.Hidden;
				CompanyInsurerUploadStatus.Visibility = Visibility.Hidden;
				SerialNumberUploadStatus.Visibility = Visibility.Hidden;
				MonthlyWeeklyUploadStatus.Visibility = Visibility.Hidden;
				ReportIssuedInfoLabel.Visibility = Visibility.Hidden;
				RenewDateInfoLabel.Visibility = Visibility.Hidden;

				MonthlyWeeklyChangeComboBox.Visibility = Visibility.Visible;
				MonthlyWeeklyRangeLabelContent.Visibility = Visibility.Hidden;


				UpdateMonthlyWeeklyComboBox();
				MonthlyWeeklyChangeComboBox.SelectedItem = MonthlyWeeklyRangeLabelContent.Content;



				//marking which text boxes are active for change
				DateReportIssuedDatePicker.Background = Brushes.PeachPuff;
				RenewDateDatePicker.Background = Brushes.PeachPuff;
				ManufacturerTextBox.Background = Brushes.PeachPuff;
				InsurerTextBox.Background = Brushes.PeachPuff;
				SerialNumberTextBox.Background = Brushes.PeachPuff;
				MonthlyWeeklyTextBox.Background = Brushes.PeachPuff;

			}

			else
			{


				//update database with changes

				db.ConnectDB();

				if (manufacturedChanged)
				{
					string value = ManufacturerTextBox.Text.ToString();
					db.UpdateStatutoryCompliance("StatutoryCompliance", "ManufacturerCompany", Convert.ToInt32(hiddenID.Content), value);

					ManufacturerCompanyUploadStatus.Foreground = Brushes.Green;
					ManufacturerCompanyUploadStatus.FontWeight = FontWeights.Bold;
					ManufacturerCompanyUploadStatus.Content = "Uploaded";
					ManufacturerCompanyUploadStatus.Visibility = Visibility.Visible;
				}
				else
				{
					ManufacturerCompanyUploadStatus.Foreground = Brushes.Red;
					ManufacturerCompanyUploadStatus.FontWeight = FontWeights.Bold;
					ManufacturerCompanyUploadStatus.Content = "Not Uploaded";
					ManufacturerCompanyUploadStatus.Visibility = Visibility.Visible;

				}

				if (companyInsurerChanged)
				{
					string value = InsurerTextBox.Text.ToString();
					db.UpdateStatutoryCompliance("StatutoryCompliance", "CompanyInsurer", Convert.ToInt32(hiddenID.Content), value);



					CompanyInsurerUploadStatus.Foreground = Brushes.Green;
					CompanyInsurerUploadStatus.FontWeight = FontWeights.Bold;
					CompanyInsurerUploadStatus.Content = "Uploaded";
					CompanyInsurerUploadStatus.Visibility = Visibility.Visible;
				}
				else
				{
					CompanyInsurerUploadStatus.Foreground = Brushes.Red;
					CompanyInsurerUploadStatus.FontWeight = FontWeights.Bold;
					CompanyInsurerUploadStatus.Content = "Not Uploaded";
					CompanyInsurerUploadStatus.Visibility = Visibility.Visible;
				}

				if (serialNumberChanged)
				{
					string value = SerialNumberTextBox.Text.ToString();
					db.UpdateStatutoryCompliance("StatutoryCompliance", "SerialNumber", Convert.ToInt32(hiddenID.Content), value);

					SerialNumberUploadStatus.Foreground = Brushes.Green;
					SerialNumberUploadStatus.FontWeight = FontWeights.Bold;
					SerialNumberUploadStatus.Content = "Uploaded";
					SerialNumberUploadStatus.Visibility = Visibility.Visible;

				}
				else
				{

					SerialNumberUploadStatus.Foreground = Brushes.Red;
					SerialNumberUploadStatus.FontWeight = FontWeights.Bold;
					SerialNumberUploadStatus.Content = "Not Uploaded";
					SerialNumberUploadStatus.Visibility = Visibility.Visible;


				}

				if (MonthlyWeeklyChangeComboBox.SelectedIndex > 0 && MonthlyWeeklyChangeComboBox.SelectedItem.ToString() != MonthlyWeeklyRangeLabelContent.Content.ToString())
				{
					db.UpdateStatutoryCompliance("StatutoryCompliance", "MonthlyWeeklyRange", Convert.ToInt32(hiddenID.Content), MonthlyWeeklyChangeComboBox.SelectedItem.ToString());

					MonthlyWeeklyComboBoxUploadStatus.Foreground = Brushes.Green;
					MonthlyWeeklyComboBoxUploadStatus.FontWeight = FontWeights.Bold;
					MonthlyWeeklyComboBoxUploadStatus.Content = "Uploaded";
					MonthlyWeeklyComboBoxUploadStatus.Visibility = Visibility.Visible;

				}
				else
				{
					MonthlyWeeklyComboBoxUploadStatus.Foreground = Brushes.Red;
					MonthlyWeeklyComboBoxUploadStatus.FontWeight = FontWeights.Bold;
					MonthlyWeeklyComboBoxUploadStatus.Content = "Not Uploaded";
					MonthlyWeeklyComboBoxUploadStatus.Visibility = Visibility.Visible;
				}


				if (monthlyWeeklyChanged)
				{

					string value = MonthlyWeeklyTextBox.Text.ToString();
					db.UpdateStatutoryCompliance("StatutoryCompliance", "MonthlyWeekly", Convert.ToInt32(hiddenID.Content), value);


					MonthlyWeeklyUploadStatus.Foreground = Brushes.Green;
					MonthlyWeeklyUploadStatus.FontWeight = FontWeights.Bold;
					MonthlyWeeklyUploadStatus.Content = "Uploaded";
					MonthlyWeeklyUploadStatus.Visibility = Visibility.Visible;

				}
				else
				{
					MonthlyWeeklyUploadStatus.Foreground = Brushes.Red;
					MonthlyWeeklyUploadStatus.FontWeight = FontWeights.Bold;
					MonthlyWeeklyUploadStatus.Content = "Not Uploaded";
					MonthlyWeeklyUploadStatus.Visibility = Visibility.Visible;
				}

				if (RenewDateWasChanged)
				{
					if (correctRenewDate)
					{

						db.UpdateStatutoryCompliance("StatutoryCompliance", "RenewDate", Convert.ToInt32(hiddenID.Content), Convert.ToDateTime(RenewDateDatePicker.SelectedDate.Value.Date));

						RenewDateInfoLabel.Foreground = Brushes.Green;
						RenewDateInfoLabel.FontWeight = FontWeights.Bold;
						RenewDateInfoLabel.Content = "Uploaded";
						RenewDateInfoLabel.Visibility = Visibility.Visible;
						db.UpdateStatutoryCompliance("StatutoryCompliance", "MeetingSet", Convert.ToInt32(hiddenID.Content), false);

					}
					else
					{
						RenewDateInfoLabel.Foreground = Brushes.Red;
						RenewDateInfoLabel.FontWeight = FontWeights.Bold;
						RenewDateInfoLabel.Content = "Not Uploaded";
						RenewDateInfoLabel.Visibility = Visibility.Visible;
					}
				}
				else
				{
					RenewDateInfoLabel.Foreground = Brushes.Red;
					RenewDateInfoLabel.Content = "Not Uploaded";
					RenewDateInfoLabel.Visibility = Visibility.Visible;
				}

				if (ReportDateWasChanged)
				{
					if (correctReportDate)
					{
						db.UpdateStatutoryCompliance("StatutoryCompliance", "DateReportIssued", Convert.ToInt32(hiddenID.Content), Convert.ToDateTime(DateReportIssuedDatePicker.SelectedDate.Value.Date));

						ReportIssuedInfoLabel.Foreground = Brushes.Green;
						ReportIssuedInfoLabel.Content = "Uploaded";
						ReportIssuedInfoLabel.Visibility = Visibility.Visible;
					}
					else
					{
						ReportIssuedInfoLabel.Foreground = Brushes.Red;
						ReportIssuedInfoLabel.Content = "Not Uploaded";
						ReportIssuedInfoLabel.Visibility = Visibility.Visible;
					}
				}
				else
				{
					ReportIssuedInfoLabel.Foreground = Brushes.Red;
					ReportIssuedInfoLabel.Content = "Not Uploaded";
					ReportIssuedInfoLabel.Visibility = Visibility.Visible;
				}




				if (bookedStatusChanged)
				{
					if (BookedCheckBox.IsChecked==true)
					{
						db.UpdateStatutoryCompliance("StatutoryCompliance", "Booked", Convert.ToInt32(hiddenID.Content), "Yes");
												
					}
					else
					{
						db.UpdateStatutoryCompliance("StatutoryCompliance", "Booked", Convert.ToInt32(hiddenID.Content), "No");
					}
					BookedInfoLabel.Foreground = Brushes.Green;
					BookedInfoLabel.Content = "Uploaded";
					BookedInfoLabel.Visibility = Visibility.Visible;
				}
				else
				{
					BookedInfoLabel.Foreground = Brushes.Red;
					BookedInfoLabel.Content = "Not Uploaded";
					BookedInfoLabel.Visibility = Visibility.Visible;
				}






				db.CloseDB();

				//deactivating textBoxes

				DateReportIssuedDatePicker.IsEnabled = false;
				RenewDateDatePicker.IsEnabled = false;
				ManufacturerTextBox.IsEnabled = false;
				InsurerTextBox.IsEnabled = false;
				SerialNumberTextBox.IsEnabled = false;
				MonthlyWeeklyTextBox.IsEnabled = false;
				BookedCheckBox.IsEnabled = false;


				MonthlyWeeklyChangeComboBox.Visibility = Visibility.Hidden;
				MonthlyWeeklyRangeLabelContent.Content = MonthlyWeeklyChangeComboBox.SelectedItem.ToString();

				MonthlyWeeklyRangeLabelContent.Visibility = Visibility.Visible;


				//changing back background colour to default
				DateReportIssuedDatePicker.Background = Brushes.Transparent;
				RenewDateDatePicker.Background = Brushes.Transparent;
				ManufacturerTextBox.Background = Brushes.Transparent;
				InsurerTextBox.Background = Brushes.Transparent;
				SerialNumberTextBox.Background = Brushes.Transparent;
				MonthlyWeeklyTextBox.Background = Brushes.Transparent;

				editMode = false;

				StatutoryItemUpdateButton.Content = "Edit Mode";

			}


		}

		private void UpdateMonthlyWeeklyComboBox()
		{
			db.ConnectDB();
			var reader = db.GetAllPDFIds("WeeklyMonthlyRange");
			MonthlyWeeklyChangeComboBox.Items.Add("Please Select");
			while (reader.Read())
			{

				MonthlyWeeklyChangeComboBox.Items.Add(reader["WeeklyMonthlyRange"].ToString());

			}

			MonthlyWeeklyChangeComboBox.SelectedIndex = 0;
			db.CloseDB();

		}

		private void RenewDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{

			RenewDateInfoLabel.Visibility = Visibility.Hidden;
			if (editMode)
			{
				RenewDateWasChanged = true;

				if (RenewDateDatePicker.SelectedDate.Value.Date < DateTime.Now.Date)
				{
					RenewDateInfoLabel.Foreground = Brushes.Red;
					RenewDateInfoLabel.FontWeight = FontWeights.Bold;
					RenewDateInfoLabel.Content = "Can`t select renew date backdated";
					RenewDateInfoLabel.Visibility = Visibility.Visible;
					correctRenewDate = false;
				}
				else
				{
					TimeSpan daysTillNewRenew = RenewDateDatePicker.SelectedDate.Value.Date - DateTime.Now.Date;
					RenewDateInfoLabel.Foreground = Brushes.Green;
					RenewDateInfoLabel.FontWeight = FontWeights.Bold;
					RenewDateInfoLabel.Content = $"{daysTillNewRenew.Days} day/s left till next inspection";
					RenewDateInfoLabel.Visibility = Visibility.Visible;
					correctRenewDate = true;
				}
			}
		}

		private void DateReportIssuedDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{

			ReportIssuedInfoLabel.Visibility = Visibility.Hidden;

			if (editMode)
			{

				ReportDateWasChanged = true;


				if (DateReportIssuedDatePicker.SelectedDate.Value.Date == currentReportDate)
				{

					correctReportDate = false;

				}
				else
				{

					switch (MonthlyWeeklyChangeComboBox.SelectedItem.ToString())
					{

						case "Yearly":

							//Do calculation by adding specific amount of Years
							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddYears(Convert.ToInt32(MonthlyWeeklyTextBox.Text));


							break;

						case "Monthly":

							//Do calculation by adding specific amount of Months

							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddMonths(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

							break;

						case "Weekly":
							//Do calculation by adding specific amount of Weeks

							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text) * 7);

							break;

						case "Daily":

							//Do calculation by adding specific amount of Days
							RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

							break;
					}

					correctReportDate = true;
				}
			}

		}

		private void ManufacturerTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (editMode)
			{
				manufacturedChanged = true;
			}
		}

		private void InsurerTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (editMode)
			{
				companyInsurerChanged = true;
			}
		}

		private void SerialNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (editMode)
			{
				serialNumberChanged = true;
			}
		}

		private void MonthlyWeeklyTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (editMode)
			{
				monthlyWeeklyChanged = true;
			}
		}

		private void MonthlyWeeklyChangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (editMode)
			{
				switch (MonthlyWeeklyChangeComboBox.SelectedItem.ToString())
				{

					case "Yearly":

						//Do calculation by adding specific amount of Years
						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddYears(Convert.ToInt32(MonthlyWeeklyTextBox.Text));


						break;

					case "Monthly":

						//Do calculation by adding specific amount of Months

						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddMonths(Convert.ToInt32(MonthlyWeeklyTextBox.Text));

						break;

					case "Weekly":
						//Do calculation by adding specific amount of Weeks

						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text) * 7);

						break;

					case "Daily":

						//Do calculation by adding specific amount of Days
						RenewDateDatePicker.SelectedDate = DateReportIssuedDatePicker.SelectedDate.Value.Date.AddDays(Convert.ToInt32(MonthlyWeeklyTextBox.Text));


						break;




				}
			}
		}

		private void BookedCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			bookedStatusChanged = true;
		}
	}
}
