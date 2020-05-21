﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MaintenanceSummary.xaml
	/// </summary>
	/// 

	public partial class MaintenanceSummary : Window
	{
		public int selectedYear;
		public string selectedMonth = String.Empty;
		public DateTime selectedDate;
		public int selectedIndex = -1;
		public double SlideControlCurrentValue = 0;
		List<string> MaintenanceCollectionForListView = new List<string>();
		DatabaseClass db = new DatabaseClass();
		public MaintenanceSummary()
		{
			InitializeComponent();
			SlideControlCurrentValue = SliderControl.Value;
			selectedYearContent.Content = "No selection";
			selectedMonthContent.Content = "No selection";
			UpdateListBox("Year");
		}


		public void UpdateListBox(string filter)
		{
			MaintenanceCollectionForListView.Clear();
			DataListBox.Items.Clear();

			OleDbDataReader reader;
			db.ConnectDB();

			if (filter == "Month" && selectedYear != 0)
			{
				reader = db.GetAllPDFIds("LineMaintenance", selectedYear);
			}
			else if (filter == "Data")
			{
				reader = db.GetAllPDFIds("LineMaintenance", selectedMonth);
			}
			else
			{
				reader = db.GetAllPDFIds("LineMaintenance");
			}

			while (reader.Read())
			{
				//MeterReadingClass meter = new MeterReadingClass();
				//DateTime pulledDate = (DateTime)reader["InsertDate"];
				//meter.InsertDate = pulledDate.ToString("d/MMM/yy");
				//meter.meterReading = Convert.ToDouble(reader["MeterReading"]);
				//meter.ReadingMonth = reader["ReadingMonth"].ToString();
				//meter.ReadingYear = reader["ReadingYear"].ToString();

				switch (filter)
				{
					case "Month":
						if (checkData(reader["MonthOfMaintenance"].ToString()) == false)
						{
							//MeterReadingColumn.DisplayMemberBinding = new Binding("ReadingMonth");
							DataListBox.Items.Add(reader["MonthOfMaintenance"].ToString());
							MaintenanceCollectionForListView.Add(reader["MonthOfMaintenance"].ToString());
						}
						break;
					case "Year":
						if (checkData(reader["YearOfMaintenance"].ToString()) == false)
						{
							//	MeterReadingColumn.DisplayMemberBinding = new Binding("ReadingYear");
							DataListBox.Items.Add(reader["YearOfMaintenance"].ToString());
							MaintenanceCollectionForListView.Add(reader["YearOfMaintenance"].ToString());
						}
						break;
					case "Data":
						DataListBox.Items.Add(reader["LineOfMaintenance"].ToString());
						MaintenanceCollectionForListView.Add(reader["LineOfMaintenance"].ToString());


						break;

					default:
						if (checkData(reader["MonthOfMaintenance"].ToString()) == false)
						{
							//MeterReadingColumn.DisplayMemberBinding = new Binding("ReadingMonth");
							DataListBox.Items.Add(reader["MonthOfMaintenance"].ToString());
							MaintenanceCollectionForListView.Add(reader["MonthOfMaintenance"].ToString());
						}
						break;
				}
			}

		}
		private bool checkData(string value)
		{
			if (MaintenanceCollectionForListView.Contains(value))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void SliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (selectedIndex < 0)
			{
				SliderControl.Value = 0d;
			}

			if (SliderControl.Value == 0)

			{
				selectedYear = 0;
				selectedMonth = string.Empty;

				//MessageBox.Show("Switching to Years");
				UpdateListBox("Year");
			}
			else if (SliderControl.Value == 1)
			{
				UpdateListBox("Month");
			}
			else
			{
				UpdateListBox("Data");
			}


		}

		private void addNewReportButton_Click(object sender, RoutedEventArgs e)
		{
			AddMaintenanceReport addMaintenance = new AddMaintenanceReport();
			addMaintenance.ShowDialog();
		}

		private void DataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{


			//DataListBox.UnselectAll();

			if (DataListBox.SelectedIndex >= 0)
			{
				selectedIndex = DataListBox.SelectedIndex;
				if (SliderControl.Value == 0)
				{
					selectedYear = Convert.ToInt32(DataListBox.SelectedItem);
					if (selectedYear != 0)
					{
						selectedYearContent.Content = selectedYear.ToString();

						selectedYearContent.Foreground = Brushes.Green;
					}
					else
					{
						selectedYearContent.Foreground = Brushes.Red;
						selectedYearContent.Content = "No selection";
					}
				}

				else if (SliderControl.Value == 1)
				{

					selectedMonth = DataListBox.SelectedItem.ToString();
					if (selectedMonth != string.Empty)
					{
						selectedMonthContent.Foreground = Brushes.Green;
						selectedMonthContent.Content = DataListBox.SelectedItem.ToString();
					}
					else
					{
						selectedMonthContent.Foreground = Brushes.Red;
						selectedMonthContent.Content = "No selection";
					}
				}
				else if (SliderControl.Value == 2)
				{

					int id = 0;

					var reader = db.GetAllPDFIds("LineMaintenance", selectedYear, selectedMonth, DataListBox.SelectedItem.ToString());
					while (reader.Read())
					{
						id = Convert.ToInt32(reader["ID"]);
					}

					GetFile(id);
				}
			}
			else
			{
				selectedIndex = -1;
			}
		}

		private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			SliderControl.Value = 1;
		}

		private void Label_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			SliderControl.Value = 2;
		}

		private void Label_MouseDoubleClick_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			SliderControl.Value = 0;
		}
		public void GetFile(int id)
		{
			EngineerCommentContent.Document.Blocks.Clear();
			UploadedDateContent.Content = "";
			ServiceDateContent.Content = "";


			db.ConnectDB();
			byte[] buffer = null;
			string tempFile = System.IO.Path.GetTempFileName();

			var reader = db.GetPDFFileFromDatabase("LineMaintenance", "ID", id);
			while (reader.Read())
			{
				buffer = (byte[])reader["UploadedFile"];
				UploadedDateContent.Content = reader["UploadDate"];
				ServiceDateContent.Content = reader["DateOfMaintenance"];
				LinkedAssetNumber.Content = reader["LinkedAsset"].ToString();
				EngineerCommentContent.Document.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(reader["EngineerComment"].ToString())));

			}
			using (FileStream fsStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				fsStream.Write(buffer, 0, buffer.Length);
			}


			PdfBrowser.Navigate(tempFile);
		}

		private void templateButton_Click(object sender, RoutedEventArgs e)
		{
			ServiceTemplates _serviceTemplates = new ServiceTemplates();
			_serviceTemplates.ShowDialog();
		}
	}
}
