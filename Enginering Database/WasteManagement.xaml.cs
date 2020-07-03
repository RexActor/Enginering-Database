
using System;
using System.Windows;
using System.Windows.Controls;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for WasteManagement.xaml
	/// </summary>
	public partial class WasteManagement : Window
	{
		DatabaseClass db = new DatabaseClass();
		public string test;

		private bool setUpSchedule = false;
		public WasteManagement()
		{
			InitializeComponent();
			setUpWasteStream();
			
		}

		private void setUpWasteStream()
		{

			db.ConnectDB();

			WasteStreamComboBox.Items.Add("All");
			var reader = db.GetAllPDFIds("WasteStreams");


			while (reader.Read())
			{
				WasteStreamComboBox.Items.Add(reader["WasteStreamDescription"]);
			}
			WasteStreamComboBox.SelectedIndex = 0;

			db.CloseDB();

		}

		private void SetUpSchedulerButton_Click(object sender, RoutedEventArgs e)
		{
			setUpSchedule = true;
			SetUpSchedulerWindow setUp = new SetUpSchedulerWindow();
			setUp.ShowDialog();
			
			
		}
		

		public void updateTestLabel(string text)
		{
			
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}
	}
}
