using System;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MeterReadings.xaml
	/// </summary>
	public partial class MeterReadings : Window
	{

		bool canUpload = false;
		bool dataExists = false;
		public MeterReadings()
		{
			InitializeComponent();
			updateFields();
		}


		private void updateFields()
		{
			MeterReadingDatePicker.SelectedDate = DateTime.Now.Date;
			MeterReadingDatePicker.IsEnabled = false;
		}

		private void SaveReadingButton_Click(object sender, RoutedEventArgs e)
		{
			CheckOldEntries();
			double result = convertToDouble(MeterReadingTextBox.Text);


			if (dataExists == false)
			{


				if (canUpload)
				{

					if (!string.IsNullOrEmpty(MeterReadingTextBox.Text))
					{
						TryParseErrorLabel.Visibility = Visibility.Hidden;
						DatabaseClass db = new DatabaseClass();
						db.ConnectDB();


						db.InsertReadingIntoDatabase("MeterReadings", MeterReadingDatePicker.SelectedDate.Value.Date, result);

						MeterReadingSummary meterReadingSummary = new MeterReadingSummary();
						meterReadingSummary.Show();
						this.Close();
					}
					else
					{
						TryParseErrorLabel.Visibility = Visibility.Visible;
					}

				}
				
				else
				{

					TryParseErrorLabel.Visibility = Visibility.Visible;
				}
			}
			

		}

		private Double convertToDouble(string value)
		{
			double result = 0.0;
			bool success = double.TryParse(value, out double tryParseresult);
			if (success)
			{
				result = Convert.ToDouble(tryParseresult);
				canUpload = true;
			}
			else
			{
				bool Intsuccess = int.TryParse(value, out int intResult);

				if (Intsuccess)
				{
					result = Convert.ToDouble(intResult);
					canUpload = true;
				}
				else
				{
					canUpload = false;

				}
			}
			return result;
		}

		private void ShowSummary_Click(object sender, RoutedEventArgs e)
		{
			MeterReadingSummary meterReadingSummary = new MeterReadingSummary();
			meterReadingSummary.Show();
		}

		private void CheckOldEntries()
		{
			dataExists = false;
			DatabaseClass db = new DatabaseClass();
			db.ConnectDB();

			var reader = db.GetMeterReadingData("MeterReadings");
			while (reader.Read())
			{
				if ((DateTime)reader["InsertDate"] == MeterReadingDatePicker.SelectedDate.Value.Date)
				{
					DataAlreadyUploadedErrors.Visibility = Visibility.Visible;
					dataExists = true;
					return;
				}
				
			}

		}

		private void EditableDate_Click(object sender, RoutedEventArgs e)
		{
			if (EditableDate.IsChecked == true)
			{
				MeterReadingDatePicker.IsEnabled = true;
			}
			else
			{
				MeterReadingDatePicker.IsEnabled = false;
			}
		}
	}
}
