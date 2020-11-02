using System.Windows;
using System.Windows.Controls;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for WasteManagement.xaml
	/// </summary>
	public partial class WasteManagement : Window
	{
		readonly DatabaseClass db = new DatabaseClass();
		public string test;
		public bool SetUpSchedule=false;

		public WasteManagement()
		{
			InitializeComponent();
			
			SetUpWasteStream();

		}

		private void SetUpWasteStream()
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
			SetUpSchedule = true;
			SetUpSchedulerWindow setUp = new SetUpSchedulerWindow();
			setUp.ShowDialog();


		}




		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
