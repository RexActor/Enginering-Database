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
		private ErrorSystem err = new ErrorSystem();

		public MeterReadings()
		{
			InitializeComponent();
			updateFields();
			GetLastReadings();
		}

		private void updateFields()
		{
			try
			{
				MeterReadingDatePicker.SelectedDate = DateTime.Now.Date;
				MeterReadingDatePicker.IsEnabled = false;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void CreateDataList(DateTime startDate, int days, int NewMeterReading, double LastmeterReading)
		{
			try
			{
				//Console.WriteLine();
				//Console.WriteLine("########## CREATING LIST [START] ###########");
				//Console.WriteLine($"Start Date => {startDate} || Days => {days} || New Meter Reading =>{NewMeterReading} || Last Meter Reading => {LastmeterReading}");

				//Console.WriteLine("########## CREATING LIST [END] ###########");
				//Console.WriteLine();

				Datalist.Clear();
				int sum = 0;
				int meterValue = Convert.ToInt32(LastmeterReading);
				int value;
				int readingDifference = (NewMeterReading - Convert.ToInt32(LastmeterReading));
				string inputMode = string.Empty;
				for (int i = 1; i <= days; i++)
				{
					if (i == days)
					{
						value = readingDifference - sum;
						meterValue = NewMeterReading;
						inputMode = "User";
					}
					else
					{
						value = readingDifference / days;
						meterValue = meterValue + value;
						inputMode = "System";
					}

					Datalist.Add(new Data
					{
						InsertDate = startDate.AddDays(i),
						consumption = value,
						meterReading = meterValue,
						InputMode = inputMode
					});
					sum = sum + value;
					//Console.WriteLine($"SUM ==> {sum}");
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void SaveReadingButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool SaveToDatabase = true;

				if (SaveToDatabase == false)
				{
					//Console.WriteLine($"Creating List with {consumptionValue} <== Total consumption value!. And {DaysDifference} difference of days");
					//CreateDataList(lastDateInserted, DaysDifference, consumptionValue, meterReadingForClosestDate);

					foreach (var item in Datalist)
					{
						Console.WriteLine($"Date to be inserted ==> {item.InsertDate} && \\n Consumption Value is {item.consumption} \\n and new Meter Reading is {item.meterReading}");
					}
				}
				else
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

								foreach (var item in Datalist)
								{
									//Console.WriteLine($"Date to be inserted ==> {item.InsertDate} && \\n Consumption Value is {item.consumption} \\n and new Meter Reading is {item.meterReading}");

									db.InsertReadingIntoDatabase("MeterReadings", item.InsertDate, item.meterReading, Convert.ToDouble(item.consumption), item.InputMode);
								}

								//db.InsertReadingIntoDatabase("MeterReadings", MeterReadingDatePicker.SelectedDate.Value.Date, result, Convert.ToDouble(consumptionValue));
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
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
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
			try
			{
				MeterReadingSummary meterReadingSummary = new MeterReadingSummary();
				meterReadingSummary.Show();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void CheckOldEntries()
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void EditableDate_Click(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void MeterReadingTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			saveInfoLabel.Visibility = Visibility.Hidden;
		}

		private void MeterReadingTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(MeterReadingTextBox.Text))
				{
					int lastReading;

					db.ConnectDB();
					lastDateInserted = Convert.ToDateTime(ClosestDateTextBox.Text);

					DaysDifference = MeterReadingDatePicker.SelectedDate.Value.Date.Subtract(lastDateInserted).Days;

					if (EditableDate.IsChecked != true)
					{
						lastReading = Convert.ToInt32(db.GetMeterReadingLastReading("MeterReadings", "MeterReading"));

						result = Convert.ToInt32(MeterReadingTextBox.Text) - lastReading;
					}
					else
					{
						lastReading = Convert.ToInt32(meterReadingForClosestDate);

						result = Convert.ToInt32(MeterReadingTextBox.Text) - lastReading;
					}
					if (DaysDifference != 0)
					{
						consumptionValue = result / DaysDifference;
					}
					Consumption.Content = $"{consumptionValue} per {DaysDifference} day/s";

					Console.WriteLine($"Creating List from Value Change with {consumptionValue} <== Total consumption value!. And {DaysDifference} difference of days");
					CreateDataList(lastDateInserted, DaysDifference, Convert.ToInt32(MeterReadingTextBox.Text), lastReading);
				}
				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void MeterReadingDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			try
			{
				if (EditableDate.IsChecked == true)
				{
					db.ConnectDB();
					var list = new List<Data>();

					DateTime selectedDate = MeterReadingDatePicker.SelectedDate.Value.Date;
					DateTime dateFromDatabase;
					TimeSpan differenceIndays;
					int compareValue;
					int meterReading;

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
								Convert.ToDouble(reader["MeterReading"]),
								Convert.ToInt32(reader["MeterCalculation"]),
								reader["InputMode"].ToString()
								));
						}
					}

					var nearest = list.OrderByDescending(x => x.differenceDays).First();
					meterReadingForClosestDate = nearest.meterReading;
					id = nearest.id;
					lastDateInserted = nearest.InsertDate;

					ClosestMeterReadingTextBox.Text = $"{nearest.meterReading}";
					ClosestDateTextBox.Text = $"{nearest.InsertDate.ToShortDateString()}";

					if (MeterReadingTextBox.Text == "")
					{
						consumptionValue = 0 - Convert.ToInt32(nearest.meterReading);
						meterReading = 0;
					}
					else
					{
						consumptionValue = Convert.ToInt32(MeterReadingTextBox.Text) - Convert.ToInt32(nearest.meterReading);
						meterReading = Convert.ToInt32(MeterReadingTextBox.Text);
					}

					DaysDifference = MeterReadingDatePicker.SelectedDate.Value.Date.Subtract(nearest.InsertDate).Days;
					Console.WriteLine($"Creating List from DateChange with {consumptionValue} <== Total consumption value!. And {DaysDifference} difference of days");

					CreateDataList(lastDateInserted, DaysDifference, meterReading, nearest.meterReading);
					//Console.WriteLine($"ID is ==>{id} for { meterReadingForClosestDate}");
					Consumption.Content = $"{consumptionValue} per {DaysDifference} day/s";
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void GetLastReadings()
		{
			try
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
							Convert.ToDouble(reader["MeterReading"]),
							Convert.ToInt32(reader["MeterCalculation"]),
							reader["InputMode"].ToString()
							));
					}
				}

				var nearest = list.OrderByDescending(x => x.differenceDays).First();

				ClosestMeterReadingTextBox.Text = $"{nearest.meterReading}";
				ClosestDateTextBox.Text = $"{nearest.InsertDate.ToShortDateString()}";
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public struct Data
		{
			public Data(double _differenceDays, DateTime _InsertDate, int _id, double _meterReading, int _consumption, string _inputMode)
			{
				differenceDays = _differenceDays;
				InsertDate = _InsertDate;
				id = _id;
				meterReading = _meterReading;
				consumption = _consumption;
				InputMode = _inputMode;
			}

			public double differenceDays { get; set; }
			public DateTime InsertDate { get; set; }
			public int id { get; set; }
			public double meterReading { get; set; }
			public int consumption { get; set; }
			public string InputMode { get; set; }
		}
	}
}