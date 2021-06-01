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
		private readonly DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();
		public string test;
		public bool SetUpSchedule = false;

		public WasteManagement()
		{
			InitializeComponent();

			SetUpWasteStream();
		}

		private void SetUpWasteStream()
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void SetUpSchedulerButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SetUpSchedule = true;
				SetUpSchedulerWindow setUp = new SetUpSchedulerWindow();
				setUp.ShowDialog();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
		}
	}
}