using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MeterReadings.xaml
	/// </summary>
	public partial class MeterReadings : Window
	{
		private bool canUpload = false;
		private bool dataExists = false;
		public int id = 0;
		public int consumptionValue = 0;
		private DateTime lastDateInserted;
		public int DaysDifference = 0;
		private int result = 0;
		private List<Data> Datalist = new List<Data>();
		public double meterReadingForClosestDate = 0;
		private DatabaseClass db = new DatabaseClass();

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

		public void CreateDataList(DateTime startDate, int days, int totalValue)
		{
			Datalist.Clear();
			int sum = 0;
			int value = totalValue / days;
			for (int i = 1; i <= days; i++)
			{
				if (i == days)
				{
					value = totalValue - sum;
				}

				Datalist.Add(new Data
				{
					InsertDate = startDate.AddDays(i),

					meterReading = value
				});

				sum = sum + value;
			}
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

						db.InsertReadingIntoDatabase("MeterReadings", MeterReadingDatePicker.SelectedDate.Value.Date, result, Convert.ToDouble(consumptionValue));
						saveInfoLabel.Visibility = Visibility.Visible;
						MeterReadingTextBox.Clear();
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

		private void MeterReadingTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			saveInfoLabel.Visibility = Visibility.Hidden;
		}

		private void MeterReadingTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			db.ConnectDB();
			if (!string.IsNullOrEmpty(MeterReadingTextBox.Text))
			{
				lastDateInserted = Convert.ToDateTime(db.GetMeterReadingLastReading("MeterReadings", "InsertDate"));

				DaysDifference = MeterReadingDatePicker.SelectedDate.Value.Date.Subtract(lastDateInserted).Days;

				if (EditableDate.IsChecked != true)
				{
					db.ConnectDB();
					int lastReading = Convert.ToInt32(db.GetMeterReadingLastReading("MeterReadings", "MeterReading"));

					result = Convert.ToInt32(MeterReadingTextBox.Text) - lastReading;
				}
				else
				{
					db.ConnectDB();
					int lastReading = Convert.ToInt32(meterReadingForClosestDate);

					result = Convert.ToInt32(MeterReadingTextBox.Text) - lastReading;
				}
				consumptionValue = result / DaysDifference;
				Consumption.Content = $"{consumptionValue} per {DaysDifference} day/s";
			}
			db.CloseDB();
		}

		private void MeterReadingDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (EditableDate.IsChecked == true)
			{
				db.ConnectDB();
				var list = new List<Data>();

				DateTime selectedDate = MeterReadingDatePicker.SelectedDate.Value.Date;
				DateTime dateFromDatabase;
				TimeSpan differenceIndays;
				int compareValue;

				var reader = db.GetMeterReadingData("MeterReadings");
				while (reader.Read())
				{
					dateFromDatabase = Convert.ToDateTime(reader["InsertDate"]);

					differenceIndays = dateFromDatabase.Subtract(selectedDate);
					compareValue = Convert.ToInt32(differenceIndays.TotalDays);
					DateTime newDate = dateFromDatabase.Add(differenceIndays);
					if (compareValue < 0)
					{
						list.Add(new Data(
							compareValue,
							dateFromDatabase,
							Convert.ToInt32(reader["ID"]),
							Convert.ToDouble(reader["MeterReading"])
							));
					}
				}

				var nearest = list.OrderByDescending(x => x.differenceDays).First();
				meterReadingForClosestDate = nearest.meterReading;
				id = nearest.id;
				lastDateInserted = nearest.InsertDate;
				consumptionValue = Convert.ToInt32(MeterReadingTextBox.Text) - Convert.ToInt32(nearest.meterReading);
				DaysDifference = MeterReadingDatePicker.SelectedDate.Value.Date.Subtract(lastDateInserted).Days;

				CreateDataList(lastDateInserted, DaysDifference, consumptionValue);

				foreach (var item in Datalist)
				{
					Console.WriteLine($"{item.InsertDate} Date iNserted && Value is {item.meterReading}");
				}

				Console.WriteLine($"ID is ==>{id} for { meterReadingForClosestDate}");
			}
		}

		public struct Data
		{
			public Data(double _differenceDays, DateTime _InsertDate, int _id, double _meterReading)
			{
				differenceDays = _differenceDays;
				InsertDate = _InsertDate;
				id = _id;
				meterReading = _meterReading;
			}

			public double differenceDays { get; set; }
			public DateTime InsertDate { get; set; }
			public int id { get; set; }
			public double meterReading { get; set; }
		}
	}
}