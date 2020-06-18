using DocumentFormat.OpenXml.Drawing.Charts;

using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for Hygene.xaml
	/// </summary>
	public partial class Hygene : Window
	{
		private bool blackedOut = false;
		DatabaseClass db = new DatabaseClass();
		public Hygene()
		{
			
			InitializeComponent();

			updatedBlackOutDates();
			//HygeneCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.Date));



		}






		private void CalendarDayButton_Click(object sender, RoutedEventArgs e)
		{
			CalendarDayButton button = sender as CalendarDayButton;

			DateTime selectedDate = (DateTime)button.DataContext;

			GetHygeneScheduler(selectedDate.Date.DayOfWeek.ToString());
			if (HygeneCalendar.BlackoutDates.Contains(selectedDate.Date))
			{
				//HygeneCalendar.SelectedDate = selectedDate;
				//GetHygeneScheduler(selectedDate.Date.DayOfWeek.ToString());
				blackedOut = true;
			}
		

		}


		private void HygeneCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			DateTime selectedDate = HygeneCalendar.SelectedDate.Value.Date;

			GetHygeneScheduler(selectedDate.Date.DayOfWeek.ToString());

			if (!HygeneCalendar.BlackoutDates.Contains(selectedDate))
			{
				
				blackedOut = false;
			}
			else
			{
				blackedOut = true;
			}

		}

		public void updatedBlackOutDates()
		{

			Label1.Visibility = Visibility.Hidden;
			Label2.Visibility = Visibility.Hidden;
			Label3.Visibility = Visibility.Hidden;
			Label4.Visibility = Visibility.Hidden;
			Label5.Visibility = Visibility.Hidden;
			Label6.Visibility = Visibility.Hidden;
			Label7.Visibility = Visibility.Hidden;
			Label8.Visibility = Visibility.Hidden;
			Label9.Visibility = Visibility.Hidden;



			HygeneCalendar.BlackoutDates.Clear();
			db.ConnectDB();

			var reader = db.GetAllPDFIds("BlackOutDates");

			while (reader.Read())
			{

				HygeneCalendar.BlackoutDates.Add(new CalendarDateRange(Convert.ToDateTime(reader["BlackOutDate"])));
				
			}



			db.CloseDB();

		}


		private void AddBlackOut_Click(object sender, RoutedEventArgs e)
		{
			db.ConnectDB();
			db.InsertBlackOutDate("BlackOutDates", Convert.ToDateTime(HygeneCalendar.SelectedDate.Value.Date));

			db.CloseDB();



			updatedBlackOutDates();

		}

		private void GetHygeneScheduler(string weekday)
		{
			if (!blackedOut)
			{

				Label1.Background = Brushes.LightGreen;
				Label2.Background = Brushes.LightGreen;
				Label3.Background = Brushes.LightGreen;
				Label4.Background = Brushes.LightGreen;
				Label5.Background = Brushes.LightGreen;
				Label6.Background = Brushes.LightGreen;
				Label7.Background = Brushes.LightGreen;
				Label8.Background = Brushes.LightGreen;
				Label9.Background = Brushes.LightGreen;

			}
			else
			{
				Label1.Background = Brushes.PaleVioletRed;
				Label2.Background = Brushes.PaleVioletRed;
				Label3.Background = Brushes.PaleVioletRed;
				Label4.Background = Brushes.PaleVioletRed;
				Label5.Background = Brushes.PaleVioletRed;
				Label6.Background = Brushes.PaleVioletRed;
				Label7.Background = Brushes.PaleVioletRed;
				Label8.Background = Brushes.PaleVioletRed;
				Label9.Background = Brushes.PaleVioletRed;

			}

			switch (weekday)
			{
				case "Monday":

					Label1.Visibility = Visibility.Visible;
					Label2.Visibility = Visibility.Visible;
					Label3.Visibility = Visibility.Visible;
					Label4.Visibility = Visibility.Visible;
					Label5.Visibility = Visibility.Visible;
					Label6.Visibility = Visibility.Visible;
					Label7.Visibility = Visibility.Visible;
					Label8.Visibility = Visibility.Visible;
					Label9.Visibility = Visibility.Visible;


					Label1.Content = "Clean manholes near Glasshouse";
					Label2.Content = "Rubbish outside & Smoking areas B1 & B2 (Litter Pick)";
					Label3.Content = "To Clean Rollers & Elevators Production";
					Label4.Content = "Suck water under Carousel";
					Label5.Content = "Clean Bucket Room";
					Label6.Content = "Wash Line Belts Line 2 & 3";
					Label7.Content = "Wash Line Belts Line 4 & 5";
					Label8.Content = "Wash barriers B1 & B2";
					Label9.Content = "Clean Waste Conveyors Rollers & Conveyors";

					break;


				case "Tuesday":

					Label1.Visibility = Visibility.Visible;
					Label2.Visibility = Visibility.Visible;
					Label3.Visibility = Visibility.Visible;
					Label4.Visibility = Visibility.Visible;

					Label1.Content = "Sanitise Production tables";
					Label2.Content = "Scrubb floors and Clean Walls in Corridors";
					Label3.Content = "Line Machines blades cleaning B1";
					Label4.Content = "Suck water under Carousel";
					Label5.Visibility = Visibility.Hidden;
					Label6.Visibility = Visibility.Hidden;
					Label7.Visibility = Visibility.Hidden;
					Label8.Visibility = Visibility.Hidden;
					Label9.Visibility = Visibility.Hidden;


					break;

				case "Wednesday":

					Label1.Visibility = Visibility.Visible;
					Label2.Visibility = Visibility.Visible;
					Label3.Visibility = Visibility.Visible;
					Label4.Visibility = Visibility.Visible;
					Label5.Visibility = Visibility.Visible;

					Label1.Content = "Full carousel wash, under belts (power wash)";
					Label2.Content = "Wash roller doors in all areas B1";
					Label3.Content = "Scrubb floors and Clean Walkways B1";
					Label4.Content = "Suck water under Carousel";
					Label5.Content = "Wash Line belts Line 6 & 7";
					Label6.Visibility = Visibility.Hidden;
					Label7.Visibility = Visibility.Hidden;
					Label8.Visibility = Visibility.Hidden;
					Label9.Visibility = Visibility.Hidden;

					break;

				case "Thursday":

					Label1.Visibility = Visibility.Visible;
					Label2.Visibility = Visibility.Visible;
					Label3.Visibility = Visibility.Visible;
					Label4.Visibility = Visibility.Visible;
					Label5.Visibility = Visibility.Visible;
					Label6.Visibility = Visibility.Visible;
					Label7.Visibility = Visibility.Visible;

					Label1.Content = "Clean Waste Conveyors Rollers & Conveyors";
					Label2.Content = "Full Drain wash";
					Label3.Content = "Extra cleaning jobs required";
					Label4.Content = "Cleaning Loading Bay drains";
					Label5.Content = "Wash Line belts Line 8";
					Label6.Content = "Wash Line belts Line 1";
					Label7.Content = "Clean manholes near Glasshouse";
					Label8.Visibility = Visibility.Hidden;
					Label9.Visibility = Visibility.Hidden;

					break;

				case "Friday":

					Label1.Visibility = Visibility.Visible;
					Label2.Visibility = Visibility.Visible;
					Label3.Visibility = Visibility.Visible;
					Label4.Visibility = Visibility.Visible;
					Label5.Visibility = Visibility.Visible;
					Label6.Visibility = Visibility.Visible;

					Label1.Content = "Full Boxing wash (power wash)";
					Label2.Content = "Wash Mecaflora Building 2";
					Label3.Content = "Wash Line Belt Line 2 & 3 Building 2";
					Label4.Content = "Wash Line Belt Line 4 & 5 Building 2";
					Label5.Content = "Clean Glasshouse VOR area and Engineering area";
					Label6.Visibility = Visibility.Hidden;
					Label7.Visibility = Visibility.Hidden;
					Label8.Visibility = Visibility.Hidden;
					Label9.Visibility = Visibility.Hidden;

					break;
				case "Saturday":


					Label1.Visibility = Visibility.Visible;
					Label2.Visibility = Visibility.Visible;
					Label3.Visibility = Visibility.Visible;
					Label4.Visibility = Visibility.Visible;

					Label1.Content = "Wash Line Belt Line 6 & 7 Building 2";
					Label2.Content = "Wash Line Belt Line 8 Building 2";
					Label3.Content = "Clean Main cave 1";
					Label4.Content = "Clean Main cave 2";
					Label5.Visibility = Visibility.Hidden;
					Label6.Visibility = Visibility.Hidden;
					Label7.Visibility = Visibility.Hidden;
					Label8.Visibility = Visibility.Hidden;
					Label9.Visibility = Visibility.Hidden;


					break;
				case "Sunday":

					Label1.Visibility = Visibility.Visible;
					Label2.Visibility = Visibility.Visible;

					Label1.Content = "Line Machines blades cleaning B2";
					Label2.Content = "Sanitise production tables";
					Label3.Visibility = Visibility.Hidden;
					Label4.Visibility = Visibility.Hidden;
					Label5.Visibility = Visibility.Hidden;
					Label6.Visibility = Visibility.Hidden;
					Label7.Visibility = Visibility.Hidden;
					Label8.Visibility = Visibility.Hidden;
					Label9.Visibility = Visibility.Hidden;


					break;

			}

		}

	}
}
