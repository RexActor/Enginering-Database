using System;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MeterReadingEdit.xaml
	/// </summary>
	public partial class MeterReadingEdit : Window
	{
		private int meterID;
		public string InsertedDate2;
		public double meterReading;
		private bool itemFound = false;
		private ErrorSystem err = new ErrorSystem();

		private DatabaseClass db = new DatabaseClass();
		/*
				public string InsertDate { get; set; }
				public double meterReading { get; set; }

				public string ReadingMonth { get; set; }
				public string ReadingYear { get; set; }

			*/

		public MeterReadingEdit()
		{
			InitializeComponent();
		}

		public void setValues(string date, double reading)
		{
			try
			{
				InsertedDate2 = String.Format("{0:dd-MMM-yyyy}", date);
				meterReading = reading;
				title.Content = $"Edit Meter reading Data for date [{InsertedDate2}]";

				GetReadingID(meterReading, InsertedDate2);

				var getCorrectReading = db.GetMeterReadingDataSelectedYear("MeterReadings", "ID", meterID);
				while (getCorrectReading.Read())
				{
					MeterReadingTextBox.Text = getCorrectReading["MeterReading"].ToString();
				}

				dateInsertedData.Content = InsertedDate2.ToString();

				recordIDdata.Content = meterID.ToString();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void GetReadingID(double value, string dateValue)
		{
			try
			{
				db.ConnectDB();
				var reader = db.GetMeterReadingData("MeterReadings");
				while (reader.Read())
				{
					DateTime dt = Convert.ToDateTime(reader["InsertDate"]);
					string dtString = dt.ToString("dd/MMM/yy");
					//Console.WriteLine($"Comparing {dtString} and comparing against {dateValue}");
					if (dtString == dateValue)
					{
						meterID = Convert.ToInt32(reader["ID"].ToString());
						//Console.WriteLine($"Found in firts if loop {meterID}");
						itemFound = true;
						return;
					}
				}
				if (itemFound == false)
				{
					while (reader.Read())
					{
						DateTime dt = Convert.ToDateTime(reader["InsertDate"]);
						string dtString = dt.ToString("dd-MMM-yy");

						if (dtString == dateValue)
						{
							meterID = Convert.ToInt32(reader["ID"].ToString());
							Console.WriteLine($"Found in second if loop {meterID}");
							itemFound = true;
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				db.MeterReadingsUpdate("MeterReadings", "MeterReading", meterID, Convert.ToDouble(MeterReadingTextBox.Text));
				this.Close();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}