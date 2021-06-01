//File MeterReadingSummary.xaml.cs Created by Rexactor
// @12:55 PM  28/4/2020
// Copyright (c) 2020  Rexactor

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;

using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MeterReadingSummary.xaml
	/// </summary>
	public partial class MeterReadingSummary : Window
	{
		private DatabaseClass db = new DatabaseClass();
		private ToolTip tp = new ToolTip();
		private int selectedYear = 0;
		private bool canEditData = false;
		private string contentLabelText = string.Empty;
		private List<string> CollectionForListView = new List<string>();
		private BackgroundWorker worker = new BackgroundWorker();
		private LoadingWindow loadingWind = new LoadingWindow();
		private ErrorSystem err = new ErrorSystem();

		public MeterReadingSummary()
		{
			InitializeComponent();

			MeterReadingColumn.DisplayMemberBinding = new Binding($"ReadingYear");
			getReadings("ReadingMonth", "Year");
			loadChart("Year");
			MeterReadingListView.SelectedIndex = 0;
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
			try
			{
				CollectionForListView.Clear();
				MeterReadingListView.Items.Clear();
				DatabaseClass db = new DatabaseClass();
				OleDbDataReader reader;
				db.ConnectDB();

				if (filter == "Month")
				{
					reader = db.GetMeterReadingData("MeterReadings", selectedYear);
				}
				else
				{
					reader = db.GetMeterReadingData("MeterReadings");
				}

				while (reader.Read())
				{
					MeterReadingClass meter = new MeterReadingClass();
					DateTime pulledDate = (DateTime)reader["InsertDate"];
					meter.InsertDate = pulledDate.ToString("d/MMM/yy");
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		/// <summary>
		/// pulling specific field from desired database and returning value
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public string getReadingsForDate(string field, DateTime date)
		{
			db.ConnectDB();

			string data = db.GetMeterReadingData("MeterReadings", "MeterReading", date);
			return data;
		}

		private void MeterReadingListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (MeterReadingListView.SelectedIndex >= 0)
				{
					if (FilterSlider.Value == 1)
					{
						MeterReadingClass selectedItem = (MeterReadingClass)MeterReadingListView.SelectedItem;
						loadChart(selectedItem.ReadingMonth, "Month");
					}
					else
					{
						if (selectedYear != 0)
						{
							MeterReadingListView.SelectedItem = selectedYear;
						}
						MeterReadingClass selectedItem = (MeterReadingClass)MeterReadingListView.SelectedItem;
						selectedYear = Convert.ToInt32(selectedItem.ReadingYear);
						loadChart(selectedItem.ReadingYear, "Year");
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void loadChart(string month = null, string filter = null)
		{
			try
			{
				ColumnSeriesData.IsSelectionEnabled = false;
				db.ConnectDB();
				int t = 0;
				int i = 0;
				int sum = 0;
				int averageValue = 20;
				OleDbDataReader reader;
				OleDbDataReader getAverage;
				KeyValuePair<string, int>[] data;
				KeyValuePair<string, int>[] average;
				((ColumnSeries)TestChart.Series[0]).ItemsSource = null;
				if (selectedYear != 0)
				{
					if (filter == "Month")
					{
						canEditData = true;
						t = db.DBMeterReadingCountLines(month, selectedYear);
						reader = db.GetMeterReadingData("MeterReadings", month, selectedYear);
						getAverage = db.GetMeterReadingData("MeterReadings", month, selectedYear);
						data = new KeyValuePair<string, int>[t];
						average = new KeyValuePair<string, int>[t];

						while (getAverage.Read())
						{
							sum += Convert.ToInt32(getAverage["MeterCalculation"]);
						}

						while (reader.Read())
						{
							DateTime dt = (DateTime)reader["InsertDate"];
							average[i] = new KeyValuePair<string, int>(dt.ToString("dd/MMM/yy"), averageValue);
							data[i] = new KeyValuePair<string, int>(dt.ToString("dd/MMM/yy"), Convert.ToInt32(reader["MeterCalculation"]));
							i++;
						}
						((ColumnSeries)TestChart.Series[0]).ItemsSource = data;
						((LineSeries)TestChart.Series[1]).ItemsSource = average;
					}
					else if (filter == "Year")
					{
						canEditData = false;
						var list = new List<Data>();

						//reads through all and create list with Month values
						reader = db.GetMeterReadingData("MeterReadings", selectedYear);
						int total = 0;
						int averageGet = 0;
						while (reader.Read())
						{
							string myString = reader["ReadingMonth"].ToString();
							int addingValue = Convert.ToInt32(reader["MeterCalculation"]);
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
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		#region temporary script

		//script for recalculating consumption based on meter readings for already inserted data
		//required due that data was inserted without populating data for consumption
		//script needs to be runned once.
		//This script is not meant for regular re-calculation. But once for filling missing data

		private void runScript(object sender, DoWorkEventArgs e)
		{
			try
			{
				int increase = 0;
				int newId = 0;

				List<ExistingData> existingData = new List<ExistingData>();
				var reader = db.GetMeterReadingData("MeterReadings");
				while (reader.Read())
				{
					existingData.Add(
						new ExistingData(
						Convert.ToInt32(reader["ID"]),
						Convert.ToDateTime(reader["InsertDate"]),
						Convert.ToDouble(reader["MeterReading"]),
						Convert.ToInt32(reader["MeterCalculation"])
						));
				}
				existingData.OrderBy(x => x.insertDate);
				int newConsumption = 0;
				for (int i = 0; i < existingData.Count; i++)
				{
					if (i > 0)
					{
						newId = i - 1;
						if (existingData[i].meterReading == 0)
						{
							newConsumption = 0;
						}
						else
						{
							newConsumption = Convert.ToInt32(existingData[i].meterReading - existingData[i - 1].meterReading);
						}
						db.MeterReadingsUpdate("MeterReadings", "MeterCalculation", existingData[i].ID, newConsumption);
					}
					else
					{
						newId = 0;

						db.MeterReadingsUpdate("MeterReadings", "MeterCalculation", existingData[i].ID, 0);
					}

					increase = Convert.ToInt32(((double)i / existingData.Count) * 100);
					contentLabelText = $"Updating Database Id [{existingData[i].ID}] with date [{string.Format("{0:d}", existingData[i].insertDate)}] consupmtion is [{existingData[i].consumption}] ==> calculated new consumption  {newConsumption} ";
					Dispatcher.Invoke(new System.Action(() => { worker.ReportProgress(increase); }));
					Thread.Sleep(100);
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public struct ExistingData
		{
			public ExistingData(int _id, DateTime _insertDate, double _meterReading, int _consumption)
			{
				ID = _id;
				insertDate = _insertDate;
				meterReading = _meterReading;
				consumption = _consumption;
			}

			public int ID { get; set; }
			public DateTime insertDate { get; set; }
			public double meterReading { get; set; }
			public int consumption { get; set; }
		}

		#endregion temporary script

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
			try
			{
				int result;
				if (MeterReadingListView.SelectedItem != null)
				{
					bool success = int.TryParse(MeterReadingListView.SelectedItem.ToString(), out result);
					if (success)
					{
						selectedYear = Convert.ToInt32(MeterReadingListView.SelectedItem.ToString());
					}
				}

				canEditData = false;
				ChangeDataCheckBox.IsChecked = false;
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ColumnSeriesData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				if (ChangeDataCheckBox.IsChecked == true)
				{
					ColumnSeries cs = (ColumnSeries)sender;
					KeyValuePair<string, int> kv = (KeyValuePair<string, int>)cs.SelectedItem;

					MeterReadingEdit editMeterReading = new MeterReadingEdit();
					editMeterReading.setValues(kv.Key, kv.Value);
					//MessageBox.Show(kv.Key);
					editMeterReading.Show();
					ChangeDataCheckBox.IsChecked = false;
				}
				else
				{
					return;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ChangeDataCheckBox_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canEditData == false)
				{
					ColumnSeriesData.IsSelectionEnabled = false;
					tp.IsOpen = false;
					ChangeDataCheckBox.IsChecked = false;
					tp.ToolTip = "Testing";
					tp.Content = "Please choose listing in Month's rather than Year";
					tp.IsOpen = true;
					tp.StaysOpen = false;
					tp.IsEnabled = true;
				}
				else
				{
					tp.IsOpen = false;
					if (ChangeDataCheckBox.IsChecked == true)
					{
						ColumnSeriesData.IsSelectionEnabled = true;
					}
					else
					{
						ColumnSeriesData.IsSelectionEnabled = false;
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ReCalculateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				TestChart.Visibility = Visibility.Hidden;
				MeterReadingListView.Visibility = Visibility.Hidden;
				testProgressBar.Visibility = Visibility.Visible;
				testLabelContent.Visibility = Visibility.Visible;
				worker.ProgressChanged += progressChanged;
				worker.DoWork += runScript;
				worker.WorkerReportsProgress = true;
				worker.RunWorkerCompleted += JobFinished;
				worker.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void progressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				testProgressBar.Value = e.ProgressPercentage;
				testLabelContent.Content = contentLabelText;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void JobFinished(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				testProgressBar.Visibility = Visibility.Hidden;
				testLabelContent.Visibility = Visibility.Hidden;
				TestChart.Visibility = Visibility.Visible;
				MeterReadingListView.Visibility = Visibility.Visible;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}