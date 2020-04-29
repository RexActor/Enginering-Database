using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MeterReadingSummary.xaml
	/// </summary>
	public partial class MeterReadingSummary : Window
	{
		DatabaseClass db = new DatabaseClass();
		bool dataExists = false;
		List<string> CollectionForListView = new List<string>();
		public MeterReadingSummary()
		{
			InitializeComponent();
			getReadings();

			loadChart("All");
		

		}


		private bool checkData(string value)
		{

			if (CollectionForListView.Contains(value))
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}

		public void getReadings()
		{
			DatabaseClass db = new DatabaseClass();
			db.ConnectDB();
			

			OleDbDataReader reader = db.GetMeterReadingData("MeterReadings");
			while (reader.Read())
			{

				MeterReadingClass meter = new MeterReadingClass();
				DateTime pulledDate = (DateTime)reader["InsertDate"];
				meter.InsertDate = pulledDate.ToString("d/MMM/yyyy");
				meter.meterReading = Convert.ToDouble(reader["MeterReading"]);
				meter.ReadingMonth = reader["ReadingMonth"].ToString();

				if (checkData(meter.ReadingMonth)==false)
				{

					MeterReadingListView.Items.Add(meter);
					CollectionForListView.Add(meter.ReadingMonth);
				}
				


			}
				

		}
		/// <summary>
		/// pulling specific field from desired database and returning value
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public string getReadingsForDate(string field, DateTime date)
		{
			string data = string.Empty;

			db.ConnectDB();

			data = db.GetMeterReadingData("MeterReadings", "MeterReading", date);

			return data;
		}

		private void MeterReadingListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			

			if (MeterReadingListView.SelectedIndex > 0)
			{
				MeterReadingClass selectedItem = (MeterReadingClass)MeterReadingListView.SelectedItem;
				loadChart(selectedItem.ReadingMonth);
			}
			else
			{
				loadChart("All");
			}
		}


		//need to change function to show in chart All month Data if specific month is not selected
		private void loadChart(string month)
		{
			db.ConnectDB();
			int t=0;
			int i = 0;
			int sum = 0;
			int averageValue;
			OleDbDataReader reader;
			OleDbDataReader getAverage;
			KeyValuePair<string, int>[] data; 
			KeyValuePair<string, int>[] average;

			if (month == "All")
			{
				t = db.DBMeterReadingCountLines();
				reader = db.GetMeterReadingData("MeterReadings");
				 getAverage = db.GetMeterReadingData("MeterReadings");
				data = new KeyValuePair<string, int>[t];
				average = new KeyValuePair<string, int>[t];

			}
			else
			{
				t = db.DBMeterReadingCountLines(month);
				reader = db.GetMeterReadingData("MeterReadings", month);
				 getAverage = db.GetMeterReadingData("MeterReadings", month);
				data = new KeyValuePair<string, int>[t];
				average = new KeyValuePair<string, int>[t];
			}
			while (getAverage.Read())
			{
				sum += Convert.ToInt32(getAverage["MeterReading"]);
			}
			averageValue = sum / t;

			while (reader.Read())
			{
				DateTime dt = (DateTime)reader["InsertDate"];
				
				average[i] = new KeyValuePair<string, int>(dt.ToString("dd/MM"), averageValue);
				data[i] = new KeyValuePair<string, int>(dt.ToString("dd/MM"), Convert.ToInt32(reader["MeterReading"]));

				i++;
			}
			//db.CloseDB();


			((ColumnSeries)TestChart.Series[0]).ItemsSource = data;


			((LineSeries)TestChart.Series[1]).ItemsSource = average;

			

		//	((ColumnSeries)TestChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[]
		//	{
		//		new KeyValuePair<string, int>("Jan",value1),
		//		new KeyValuePair<string, int>("Feb",value2),
		//		new KeyValuePair<string, int>("Mar",value3),
		//		new KeyValuePair<string, int>("Apr",value4)
		//	};
		//	((LineSeries)TestChart.Series[1]).ItemsSource = new KeyValuePair<string, int>[]
		//{
		//		new KeyValuePair<string, int>("Jan",value1*2),
		//		new KeyValuePair<string, int>("Feb",value2*2),
		//		new KeyValuePair<string, int>("Mar",value3*2),
		//		new KeyValuePair<string, int>("Apr",value4*2)
		//};






	}

		
	}
}
