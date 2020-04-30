using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MeterReadingSummary.xaml
	/// </summary>
	public partial class MeterReadingSummary : Window
	{
		DatabaseClass db = new DatabaseClass();


		List<string> CollectionForListView = new List<string>();
		bool chartOnLoad = true;
		public MeterReadingSummary()
		{
			InitializeComponent();

			//getReadings("ReadingYear", "Year");

			
			MeterReadingColumn.DisplayMemberBinding = new Binding($"ReadingYear");
			getReadings("ReadingMonth", "Year");
			loadChart("Year");
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

		public void getReadings(string field = null, string filter = null)
		{
			CollectionForListView.Clear();
			MeterReadingListView.Items.Clear();
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
				meter.ReadingYear = reader["ReadingYear"].ToString();


				switch (filter)
				{
					case "Month":
						if (checkData(meter.ReadingMonth) == false)
						{
							MeterReadingColumn.DisplayMemberBinding = new Binding("ReadingMonth");
							MeterReadingListView.Items.Add(meter);
							CollectionForListView.Add(meter.ReadingMonth);
						}
						break;
					case "Year":
						if (checkData(meter.ReadingYear) == false)
						{
							MeterReadingColumn.DisplayMemberBinding = new Binding("ReadingYear");
							MeterReadingListView.Items.Add(meter);
							CollectionForListView.Add(meter.ReadingYear);
						}
						break;
					default:
						if (checkData(meter.ReadingMonth) == false)
						{
							MeterReadingColumn.DisplayMemberBinding = new Binding("ReadingMonth");
							MeterReadingListView.Items.Add(meter);
							CollectionForListView.Add(meter.ReadingMonth);
						}
						break;
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
			if (MeterReadingListView.SelectedIndex >=0)
			{

				if (FilterSlider.Value == 1)
				{


					MeterReadingClass selectedItem = (MeterReadingClass)MeterReadingListView.SelectedItem;
					loadChart(selectedItem.ReadingMonth, "Month");

				}
				else
				{
					MeterReadingClass selectedItem = (MeterReadingClass)MeterReadingListView.SelectedItem;
					loadChart(selectedItem.ReadingYear, "Year");
				}
				
			}
		
		
		}


		//need to change function to show in chart All month Data if specific month is not selected
		private void loadChart(string month = null, string filter = null)
		{
			db.ConnectDB();
			int t = 0;
			int i = 0;
			int sum = 0;
			int averageValue;
			OleDbDataReader reader;
			OleDbDataReader getAverage;
			KeyValuePair<string, int>[] data;
			KeyValuePair<string, int>[] average;
			((ColumnSeries)TestChart.Series[0]).ItemsSource = null;

			if (filter == "Month")
			{



				t = db.DBMeterReadingCountLines(month);
				reader = db.GetMeterReadingData("MeterReadings", month);
				getAverage = db.GetMeterReadingData("MeterReadings", month);
				data = new KeyValuePair<string, int>[t];
				average = new KeyValuePair<string, int>[t];

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
			}
			else if (filter == "Year")
			{
				var list = new List<Data>();


				//reads through all and create list with Month values

				t = db.DBMeterReadingCountLines(month);
				reader = db.GetMeterReadingData("MeterReadings");
				//getAverage = db.GetMeterReadingData("MeterReadings");
				int total = 0;
				int averageGet = 0;
				while (reader.Read())
				{
					string myString = reader["ReadingMonth"].ToString();

					int addingValue = Convert.ToInt32(reader["MeterReading"]);

					bool containsElement = list.Any(x => x.Month == myString);
					total = list.Where(x => x.Month == myString).Sum(y => y.sum);
					averageGet = averageGet + addingValue;
					int indexOfItem = list.FindIndex(x => x.Month == myString);


					if (!containsElement)
					{


						list.Add(new Data(myString, addingValue));

					}
					else
					{

						if (indexOfItem != -1)
						{
							var oldItem = list[indexOfItem];
							var newCustomItem = new Data();
							newCustomItem.Month = myString;

							int oldSum = oldItem.sum;

							newCustomItem.sum = oldSum + addingValue;


							list[indexOfItem] = newCustomItem;
						}

					}

					//Console.WriteLine(indexOfItem);

				}
				int z = 0;

				data = new KeyValuePair<string, int>[list.Count];
				average = new KeyValuePair<string, int>[list.Count];
				int averageValuePerMonth = averageGet / list.Count;

				foreach (var item in list)
				{


					data[z] = new KeyValuePair<string, int>(list[z].Month, list[z].sum);
					average[z] = new KeyValuePair<string, int>(list[z].Month, averageValuePerMonth);
					z++;


				}

				((LineSeries)TestChart.Series[1]).ItemsSource = average;
				((ColumnSeries)TestChart.Series[0]).ItemsSource = data;
			}


		}

		public struct Data
		{
			public Data(string value, int number)
			{
				Month = value;
				sum = number;
			}
			public string Month { get; set; }
			public int sum { get; set; }
		}


		private void FilterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (FilterSlider.Value == 0)
			{
				MeterReadingColumn.DisplayMemberBinding = new Binding($"ReadingYear");
				getReadings("ReadingMonth", "Year");

			}
			else
			{

				MeterReadingColumn.DisplayMemberBinding = new Binding($"ReadingMonth");
				getReadings("ReadingYear", "Month");
			}

		}


	}
}
