

using CrystalDecisions.ReportAppServer.DataDefModel;

using DocumentFormat.OpenXml.Drawing.Diagrams;

using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for Hygene.xaml
	/// </summary>
	public partial class Hygene : Window
	{
		readonly string userName = WindowsIdentity.GetCurrent().Name;
		DatabaseClass db = new DatabaseClass();

		public object Controls { get; private set; }

		public Hygene()
		{

			InitializeComponent();


			UploadStatusLabel.Visibility = Visibility.Hidden;
			//HygeneCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.Date));



		}




		private void HygeneCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{

			UploadStatusLabel.Visibility = Visibility.Hidden;
			DateTime selectedDate = HygeneCalendar.SelectedDate.Value.Date;

			CreateLabels(selectedDate.DayOfWeek.ToString());


		}


		public bool checkBlackOutDates(DateTime date)
		{

			db.ConnectDB();

			var rgetBlackoutDates = db.GetBlackOutDate("BlackOutDates", "BlackOutDate", date);
			bool result = false;
			while (rgetBlackoutDates.Read())
			{


				if (Convert.ToDateTime(rgetBlackoutDates["BlackOutDate"]) == date)
				{
					result = true;
				}
				//HygeneCalendar.BlackoutDates.Add(new CalendarDateRange(Convert.ToDateTime(reader["BlackOutDate"])));
				else
				{
					result = false;
				}
			}



			db.CloseDB();

			return result;

		}



		private void AddBlackOut_Click(object sender, RoutedEventArgs e)
		{
			if (HygeneCalendar.SelectedDate.HasValue)
			{
				db.ConnectDB();
				db.InsertBlackOutDate("BlackOutDates", Convert.ToDateTime(HygeneCalendar.SelectedDate.Value.Date));

				db.CloseDB();
				UploadStatusLabel.Visibility = Visibility.Visible;
			}
			else
			{
				UploadStatusLabel.Visibility = Visibility.Visible;
				UploadStatusLabel.Foreground = Brushes.Red;

				UploadStatusLabel.Content = "No date selected";
			}

		}


		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{


			System.Windows.Controls.CheckBox receivingCheckbox = (System.Windows.Controls.CheckBox)sender;

			//var control = Controls.OfType<Label>().FirstOrDefault(c => c.Name == $"User{receivingCheckBox.Name}");
			//MessageBox.Show(receivingCheckbox.Name.ToString());

			if (receivingCheckbox.IsChecked == true)
			{

				UpdateLabel(receivingCheckbox.Name, userName);

			}
			else
			{
				UpdateLabel(receivingCheckbox.Name, "");
			}

		}

		private void UpdateLabel(string labelName, string value)
		{
			foreach (System.Windows.Controls.Control c in UsernameLabelStackPanel.Children)
			{
				if (((System.Windows.Controls.Label)c).Name == $"UserNameLabel{labelName}")
				{
					((System.Windows.Controls.Label)c).Content = $"{value}";
				}

			}
		}

		private void ChangeLabelColour(Brush value)
		{
			foreach (System.Windows.Controls.Control c in HygeneLabelStackPanel.Children)
			{

				((System.Windows.Controls.Label)c).Background = value;
			}
		}


		private void CreateLabels(string value)
		{
			//UsernameLabelStackPanel.Children.Clear();

			HygeneLabelStackPanel.Children.Clear();
			CheckBoxStackPanel.Children.Clear();
			UsernameLabelStackPanel.Children.Clear();

			db.ConnectDB();
			int controlid = 1;

			var reader = db.GetHygeneScheduler("HygieneScheduler", "Weekday", value);

			while (reader.Read())
			{


				//create scheduler labels
				System.Windows.Controls.Label schedulerLabel = new System.Windows.Controls.Label();
				schedulerLabel.Name = $"SchedulerLabel{controlid}";
				schedulerLabel.Content = reader["Task"].ToString();
				schedulerLabel.Width = 300;
				schedulerLabel.Height = 30;
				schedulerLabel.Margin = new Thickness(2);
				schedulerLabel.Padding = new Thickness(1);
				schedulerLabel.BorderBrush = Brushes.Black;
				schedulerLabel.BorderThickness = new Thickness(1, 1, 1, 1);


				

				HygeneLabelStackPanel.Children.Add(schedulerLabel);


				//create Checkboxes
				System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox();
				checkBox.Name = $"CheckBox{controlid}";
				//label.Content = i;
				//checkBox.Width = 165;
				checkBox.Height = 30;
				checkBox.Click += CheckBox_Click;
				checkBox.Margin = new Thickness(2);
				checkBox.Padding = new Thickness(1);
				//checkBox.BorderBrush = Brushes.Black;
				//checkBox.BorderThickness = new Thickness(1, 1, 1, 1);
				//Canvas.SetLeft(label, 14);
				//Canvas.SetTop(TestCanvas, label.Height + 100);

				CheckBoxStackPanel.Children.Add(checkBox);



				//create Username Labels

				System.Windows.Controls.Label label = new System.Windows.Controls.Label();
				label.Name = $"UserNameLabelCheckBox{controlid}";
				//label.Content = label.Name.ToString();
				label.Width = 165;
				label.Height = 30;
				label.Margin = new Thickness(2);
				label.Padding = new Thickness(1);
				label.BorderBrush = Brushes.Black;
				label.BorderThickness = new Thickness(1, 1, 1, 1);
				//Canvas.SetLeft(label, 14);
				//Canvas.SetTop(TestCanvas, label.Height + 100);

				UsernameLabelStackPanel.Children.Add(label);




				controlid++;

			}

			if (checkBlackOutDates(HygeneCalendar.SelectedDate.Value.Date))
			{


				ChangeLabelColour(Brushes.LightGreen);
			}
			else
			{
				ChangeLabelColour(Brushes.LightPink);
				//schedulerLabel.Background = Brushes.LightPink;
			}



			db.CloseDB();
		}

	}
}
