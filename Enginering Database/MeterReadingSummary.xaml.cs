using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for MeterReadingSummary.xaml
	/// </summary>
	public partial class MeterReadingSummary : Window
	{
		public MeterReadingSummary()
		{
			InitializeComponent();
			getReadings();
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
				meter.InsertDate =pulledDate.ToString("d/MMM/yyyy");
				meter.meterReading = Convert.ToDouble(reader["MeterReading"]);
				Console.WriteLine(meter.InsertDate.ToString());
				MeterReadingListView.Items.Add(meter);
			}
			
		}
		/// <summary>
		/// pulling specific field from desired database and returning value
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public string getReadingsForDate(string field,DateTime date)
		{
			string data = string.Empty;
			DatabaseClass db = new DatabaseClass();
			db.ConnectDB();

			data = db.GetMeterReadingData("MeterReadings","MeterReading",date);
		
			return data;
		}

		private void MeterReadingListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MeterReadingClass selectedItem = (MeterReadingClass)MeterReadingListView.SelectedItem;


			testLabel.Content = selectedItem.meterReading;
		}


	}
}
