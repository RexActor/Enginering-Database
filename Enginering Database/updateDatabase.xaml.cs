//using DocumentFormat.OpenXml.Wordprocessing;
using Engineering_Database;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Enginering_Database
{

	
	/// <summary>
	/// Interaction logic for updateDatabase.xaml
	/// </summary>
	public partial class updateDatabase : Window
	{
		private static readonly log4net.ILog log = LogHelper.GetLogger();
	
		int convJobNumber;
		public string contentForTextTestLabel;
		System.Windows.Controls.Button textTestLabel;
		int jobList;
		IssueClass issueClass = new IssueClass();
		UserSettings userSett = new UserSettings();
		DatabaseClass db = new DatabaseClass();
		private static BindingList<IssueClass> empList = new BindingList<IssueClass>();
		





		public updateDatabase()
		{

			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			db.ConnectDB();
			InitializeComponent();
			empList.AllowRemove = true;

			Frame2JobNumberData.Content = "waiting for data";
			Frame2ReportedDateData.Content = "waiting for data";
			Frame2ReportedUserData.Content = "waiting for data";
			Frame2AreaData.Content = "waiting for data";
			Frame2IssueTypeData.Content = "waiting for data";
			Frame2BuildingData.Content = "waiting for data";
			Frame2ReportedTimeData.Content = "waiting for data";
			Frame2DueDateData.Content = "waiting for data";
			Frame2FaultyAreaData.Content = "waiting for data";
			Frame2IssueCodeData.Content = "waiting for data";
			Frame2AssetData.Content = "waiting for data";
			Frame2PriorityData.Content = "waiting for data";

			Frame2ReportedDescription.Text = "waiting for data";



			createJobList();
			UpdateAllDb();
		}


		
		private void UpdateAllDb()
		{
			emplistDataGrid.ItemsSource = issueClass.updateIssueDataList();
		}

		private void createJobList()
		{
			issueClass.updateIssueDataList();


			
			TestStackPanel.Children.Clear();
			searchCombo.Items.Insert(0, "week");
			searchCombo.Items.Insert(1, "jobnumber");
			searchCombo.SelectedIndex = 0;


			userSett.openSettings();

			

			Frame3DueDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture);


			int p = 0;


			if (Int32.TryParse(UserSettings.jobCount, out p))
			{
				jobList = p;
			}
			else
			{
				jobList = 0;
				System.Windows.Controls.Label ErrorLabel = new System.Windows.Controls.Label();
				ErrorLabel.Content = "Error in settings. Please check job count settings";
				ErrorLabel.Padding = new Thickness(3);
				ErrorLabel.Margin = new Thickness(1);
				TestStackPanel.Children.Add(ErrorLabel);


			}

			if (jobList > 0)
			{
				
				OleDbDataAdapter da = new OleDbDataAdapter(db.DBQuery());
				DataTable dt = new DataTable();
				int i = 0;
				da.Fill(dt);

				



				foreach (DataRow dr in dt.Rows)
				{
					if (i <= dt.Rows.Count && i < jobList)
					{

						
						textTestLabel = new System.Windows.Controls.Button();
						textTestLabel.BorderThickness = new Thickness(0);
						textTestLabel.Background = new SolidColorBrush(Color.FromArgb(195, 195, 195, 0));
						
					
						textTestLabel.Padding = new Thickness(3);
						textTestLabel.Margin = new Thickness(1);
						contentForTextTestLabel = dr["JobNumber"].ToString();
						textTestLabel.Content = contentForTextTestLabel;
					

						

						textTestLabel.Click += (sender, e) => { TextTestLabel_Click(sender, e); };

						//log.Debug($"Content for text Label: {contentForTextTestLabel.ToString()}");

						TestStackPanel.Children.Add(textTestLabel);
					}
					i++;
				}
				
			}

			if (ChangeDueDateCheckBox.IsChecked == true)
			{
				Frame3UpdateDueDateLabel.Visibility = Visibility.Visible;
				Frame3UpdateDueDateDatePicker.Visibility = Visibility.Visible;
				Frame3DueDateTextBox.Foreground = Brushes.Red;
			}

			else
			{
				Frame3UpdateDueDateLabel.Visibility = Visibility.Hidden;
				Frame3UpdateDueDateDatePicker.Visibility = Visibility.Hidden;
			}

			Frame3userNameLabel.Content = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
			Frame3CurrentDateLabel.Content = DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture);

		}

		private void TextTestLabel_Click(object sender, RoutedEventArgs e)
		{

			//log.Debug(p);
			Button btn = sender as Button;
			//log.Debug(btn.Content.ToString());
			
			//btn.Background = btn.Background == Brushes.Red ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Red;
			if (db.DBStatus()== "DB not Connected")
			{
				db.ConnectDB();
			}
		

			Frame2StackPanel.Children.Clear();
			
		
			System.Windows.Controls.TextBlock frame2TextBlock = new System.Windows.Controls.TextBlock();
			// frame2TextBlock.Text = p;
			
			UpdateFrame2(btn.Content.ToString());



			Frame2StackPanel.Children.Add(frame2TextBlock);
		}

		private void searchCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)

		{
			searchCombo.IsReadOnly = false;
		}



		private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			if (!string.IsNullOrEmpty(searchTxt.Text))
			{
				int convertSearch;
				int p;
				if (Int32.TryParse(searchTxt.Text, out p))
				{
					convertSearch = p;
					emplistDataGrid.ItemsSource = issueClass.updateIssueDataListForSpecificJob(convertSearch);
				}
				
			}
		}

		private void StartTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void ChangeDueDateCheckBox_Change(object sender, RoutedEventArgs e)
		{
			if (ChangeDueDateCheckBox.IsChecked == true)
			{
				Frame3UpdateDueDateLabel.Visibility = Visibility.Visible;
				Frame3UpdateDueDateDatePicker.Visibility = Visibility.Visible;
				Frame3DueDateTextBox.Foreground = Brushes.Red;
				Frame3DueDateTextBox.FontWeight = FontWeights.Bold;
			}

			else
			{
				Frame3UpdateDueDateLabel.Visibility = Visibility.Hidden;
				Frame3UpdateDueDateDatePicker.Visibility = Visibility.Hidden;
				Frame3DueDateTextBox.Foreground = Brushes.Black;
				Frame3DueDateTextBox.FontWeight = FontWeights.Normal;
				Frame3DueDateTextBox.Text = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber)).ToShortDateString().ToString();
			}
		}

		private void DueDateChange_change(object sender, SelectionChangedEventArgs e)
		{
			if (ChangeDueDateCheckBox.IsChecked == true)
			{
				Frame3DueDateTextBox.Text = Frame3UpdateDueDateDatePicker.SelectedDate.Value.Date.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture);

				DateTime StartDate = DateTime.Now.Date;
				DateTime EndDate = Frame3UpdateDueDateDatePicker.SelectedDate.Value.Date;
				Double daysBetween = (EndDate - StartDate).TotalDays;

				Frame3DaysTillDueData.Content = daysBetween;
			}
		}

		private void UpdateFrame2(string jobNumber)
		{
			/*
			if (JobItemSelected == true)
			{

				Frame2JobNumberData.Visibility = Visibility.Visible;
				Frame2ReportedDateData.Visibility = Visibility.Visible;
				Frame2ReportedUserData.Visibility = Visibility.Visible;
				Frame2AreaData.Visibility = Visibility.Visible;
				Frame2IssueTypeData.Visibility = Visibility.Visible;
				Frame2BuildingData.Visibility = Visibility.Visible;
				Frame2ReportedTimeData.Visibility = Visibility.Visible;
				Frame2DueDateData.Visibility = Visibility.Visible;
				Frame2FaultyAreaData.Visibility = Visibility.Visible;
				Frame2IssueCodeData.Visibility = Visibility.Visible;
				Frame2AssetData.Visibility = Visibility.Visible;
				Frame2PriorityData.Visibility = Visibility.Visible;

			*/


				int c = 0;


				if (Int32.TryParse(jobNumber, out c))
				{
					convJobNumber = c;

				//log.Debug(c);
				}
				string[] words = jobNumber.Split(null);

				Frame2JobNumberData.Content = words[words.Length - 1];
				Frame2ReportedDateData.Content = Convert.ToDateTime(db.DBQuery("ReportedDate", convJobNumber)).ToShortDateString().ToString();
				Frame2ReportedUserData.Content = db.DBQuery("ReportedUsername", convJobNumber);
				Frame2AreaData.Content = db.DBQuery("Area", convJobNumber);
				Frame2IssueTypeData.Content = db.DBQuery("Type", convJobNumber);
				Frame2BuildingData.Content = db.DBQuery("Building", convJobNumber);
				Frame2ReportedTimeData.Content = Convert.ToDateTime(db.DBQuery("ReportedTime", convJobNumber)).ToShortTimeString();
				Frame2DueDateData.Content = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber)).ToShortDateString().ToString();
				Frame2FaultyAreaData.Content = db.DBQuery("FaultyArea", convJobNumber);
				Frame2IssueCodeData.Content = db.DBQuery("IssueCode", convJobNumber);
				Frame2AssetData.Content = db.DBQuery("AssetNumber", convJobNumber);
				Frame2PriorityData.Content = db.DBQuery("Priority", convJobNumber);

				Frame2ReportedDescription.Text = db.DBQuery("DetailedDescription", convJobNumber);

				Frame3DueDateTextBox.Text = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber)).ToShortDateString().ToString();
				Frame3StartTimeTextBox.Text = Convert.ToDateTime(db.DBQuery("StartTime", convJobNumber)).ToShortTimeString();
				Frame3FinishTimeTextBox.Text = Convert.ToDateTime(db.DBQuery("FinishTime", convJobNumber)).ToShortTimeString();
				Frame3AssignedToData.Content = db.DBQuery("AssignedTo", convJobNumber);

				if (db.DBQuery("Completed", convJobNumber) == "True")
				{
					Frame3CompleteCheckBox.IsChecked = true;
				}
				else
				{
					Frame3CompleteCheckBox.IsChecked = false;

				}
		/*	}
			
		/else
			{

				Frame2JobNumberData.Visibility = Visibility.Hidden;
				Frame2ReportedDateData.Visibility = Visibility.Hidden;
				Frame2ReportedUserData.Visibility = Visibility.Hidden;
				Frame2AreaData.Visibility = Visibility.Hidden;
				Frame2IssueTypeData.Visibility = Visibility.Hidden;
				Frame2BuildingData.Visibility = Visibility.Hidden;
				Frame2ReportedTimeData.Visibility = Visibility.Hidden;
				Frame2DueDateData.Visibility = Visibility.Hidden;
				Frame2FaultyAreaData.Visibility = Visibility.Hidden;
				Frame2IssueCodeData.Visibility = Visibility.Hidden;
				Frame2AssetData.Visibility = Visibility.Hidden;
				Frame2PriorityData.Visibility = Visibility.Hidden;
			}*/
			/*
			DateTime StartDate = DateTime.Now.Date;
			DateTime EndDate = Frame3DueDateTextBox.Text
			Double daysBetween = (EndDate - StartDate).TotalDays;
			*/

			//DateTime newDate = new DateTime(DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture) - DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture));
			//Frame3DaysTillDue.Content = Frame3DueDateTextBox.Text - DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture);




		}

		private void Submit_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(convJobNumber.ToString());

			if (Frame3CompleteCheckBox.IsChecked == true)
			{
				db.DBQueryInsertData("Completed", convJobNumber,  true);
				db.DBQueryInsertData(convJobNumber, "Action", "Actioned");

			}
			else
			{
				db.DBQueryInsertData("Completed", convJobNumber, false);
				db.DBQueryInsertData(convJobNumber, "Action", "Action required");
			}


			createJobList();
			Frame3.Refresh();

			Frame2JobNumberData.Content = "waiting for data";
			Frame2ReportedDateData.Content = "waiting for data";
			Frame2ReportedUserData.Content = "waiting for data";
			Frame2AreaData.Content = "waiting for data";
			Frame2IssueTypeData.Content = "waiting for data";
			Frame2BuildingData.Content = "waiting for data";
			Frame2ReportedTimeData.Content = "waiting for data";
			Frame2DueDateData.Content = "waiting for data";
			Frame2FaultyAreaData.Content = "waiting for data";
			Frame2IssueCodeData.Content = "waiting for data";
			Frame2AssetData.Content = "waiting for data";
			Frame2PriorityData.Content = "waiting for data";

			Frame2ReportedDescription.Text = "waiting for data";
			//MessageBox.Show(db.DBQuery("JobNumber"));


		}
	}


}
