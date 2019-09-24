﻿
using Engineering_Database;
using System;

using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Enginering_Database
{



	public partial class updateDatabase : Window
	{
		//private static readonly log4net.ILog log = LogHelper.GetLogger();

		private string filter = "Outstanding";
		//readonly EmailClass email = new EmailClass();
		int convJobNumber;
		public string contentForTextTestLabel;
		System.Windows.Controls.Button textTestLabel;
		int jobList;
		readonly IssueClass issueClass = new IssueClass();
		readonly UserSettings userSett = new UserSettings();
		readonly DatabaseClass db = new DatabaseClass();

		readonly double ScreenHeight = SystemParameters.WorkArea.Height;
		readonly double ScreenWidht = SystemParameters.WorkArea.Width;
		readonly private static BindingList<IssueClass> empList = new BindingList<IssueClass>();

		//if data is not selected = can't change due date
		private bool canChangeDueDate = false;


		//Tooltip opbjet
		readonly ToolTip myToolTip = new ToolTip();






		public updateDatabase()
		{

			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			db.ConnectDB();
			InitializeComponent();

			//TODO:check what was this for?!
			empList.AllowRemove = true;

			///disable preview window at start
			previewStackPanel.Visibility = Visibility.Hidden;


			//set all values in frame2 for default

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

			//update contractor ComboBox with latest data
			UpdateContractorComboBox();


			//update assigntocombo box with latest data 
			UpdateAssignToComboBox();


			//TODO:requires fix for this function. Show exception error on work PC
			createJobList(filter);

		}


		//pulls configuration from app.config file and updates drop down list with latest information
		private void UpdateContractorComboBox()
		{

			//open usersettings configuration file
			userSett.openSettings();

			userSett.contractors = userSett.contractors.Replace(" ", string.Empty);

			ContractorComboBoxData.Items.Clear();

			//contractors are splited by "/" due that all contractors are saved under one configuration name
			string[] splitContractors = userSett.contractors.Split('/');

			foreach (var word in splitContractors)
			{

				//checks if first value is not empty string. If so, then ignore it
				if (!word.Equals(""))
				{
					ContractorComboBoxData.Items.Add(word);
				}
			}
		}


		private void UpdateAssignToComboBox()
		{


			//open usersettings configuration file
			userSett.openSettings();
			//userSett.assignToUser = userSett.assignToUser.Replace(" ", string.Empty);
			AssignToDropDownBox.Items.Clear();

			//Assign to users are splited by "/" due that all contractors are saved under one configuration name
			string[] splitAssignTo = userSett.assignToUser.Split('/');
			foreach (var word in splitAssignTo)
			{
				//checks if first value is not empty string. If so, then ignore it
				if (!word.Equals(""))
				{
					AssignToDropDownBox.Items.Add(word);
				}
			}



		}

		private void UpdateAllDb()
		{


			if (emplistDataGrid.ItemsSource == null)
			{
				emplistDataGrid.ItemsSource = issueClass.updateIssueDataList();
			}


		}

		private void createJobList(string filter)
		{
			issueClass.updateIssueDataList();



			TestStackPanel.Children.Clear();
			searchCombo.Items.Insert(0, "Job Number");
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

				OleDbDataAdapter da = new OleDbDataAdapter(db.DBQueryforJobList(filter));
				DataTable dt = new DataTable();
				int i = 0;
				da.Fill(dt);
				da.Dispose();




				foreach (DataRow dr in dt.Rows)
				{
					if (i <= dt.Rows.Count && i < jobList)
					{


						textTestLabel = new System.Windows.Controls.Button();
						textTestLabel.BorderThickness = new Thickness(0);
						textTestLabel.Background = new SolidColorBrush(Color.FromArgb(195, 195, 195, 0));

						textTestLabel.Height = 30;
						textTestLabel.Padding = new Thickness(3);
						textTestLabel.Margin = new Thickness(2);
						contentForTextTestLabel = dr["JobNumber"].ToString();
						textTestLabel.Content = contentForTextTestLabel;




						textTestLabel.Click += (sender, e) => { TextTestLabel_Click(sender, e); };
						if (userSett.PreviewSettingComboBox.Text == "Yes")
						{
							textTestLabel.MouseEnter += (sender, e) => { TextTestLabel_Hover(sender, e); };
							textTestLabel.MouseLeave += (sender, e) => { TextTestLabel_Leave(sender, e); };
						}
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


			//TODO: error fix required for displaying name
			//Frame3userNameLabel.Content = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
			//Frame3userNameLabel.Content = "Gatis - Fix required";

			Frame3userNameLabel.Content = Environment.UserName;

			Frame3CurrentDateLabel.Content = DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture);

		}

		private void TextTestLabel_Leave(object sender, MouseEventArgs e)
		{

			previewStackPanel.Visibility = Visibility.Hidden;
		}

		private void TextTestLabel_Hover(object sender, EventArgs e)
		{
			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}

			//TODO:testing email class

			//email.SendEmail();



			//MouseEventArgs e
			Button lbl = (Button)sender;
			previewStackPanel.Visibility = Visibility.Visible;
			if (lbl == null)
			{
				previewJobNumber.Content = "looking for data";

			}
			else
			{

				int convertSearch;
				int p;
				if (Int32.TryParse(lbl.Content.ToString(), out p))
				{
					convertSearch = p;

				}
				else
				{
					convertSearch = 1;
				}





				previewReportedName.Content = db.DBQuery("ReportedUsername", convertSearch);
				prevAssetNumber.Content = db.DBQuery("AssetNumber", convertSearch);
				prevFaultyArea.Content = db.DBQuery("FaultyArea", convertSearch);
				prevPriority.Content = db.DBQuery("Priority", convertSearch);
				prevDetailedDescription.Content = db.DBQuery("DetailedDescription", convertSearch);
				prevAssignedTo.Content = db.DBQuery("AssignedTo", convertSearch);
				prevDueDate.Content = db.DBQuery("DueDate", convertSearch);

				previewJobNumber.Content = lbl.Content.ToString();



			}



		}

		private void TextTestLabel_Click(object sender, RoutedEventArgs e)
		{
			canChangeDueDate = true;

			//log.Debug(p);
			Button btn = sender as Button;



			//log.Debug(btn.Content.ToString());


			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}


			Frame2StackPanel.Children.Clear();


			System.Windows.Controls.TextBlock frame2TextBlock = new System.Windows.Controls.TextBlock();


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

					if (emplistDataGrid != null)
					{
						emplistDataGrid.ItemsSource = null;

						emplistDataGrid.ItemsSource = issueClass.updateIssueDataListForSpecificJob(convertSearch);
					}

				}

			}
			else
			{
				if (emplistDataGrid != null)
				{
					emplistDataGrid.ItemsSource = null;
					emplistDataGrid.ItemsSource = issueClass.updateIssueDataList();
				}
				else
				{
					emplistDataGrid.ItemsSource = issueClass.updateIssueDataList();
				}
			}
		}

		private void StartTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void ChangeDueDateCheckBox_Change(object sender, RoutedEventArgs e)
		{
			if (canChangeDueDate)
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
		}

		private void DueDateChangeCheckBoxMouseOver(object sender, RoutedEventArgs e)
		{
			if (canChangeDueDate == false)
			{
				myToolTip.Content = "Can't change Due date if data is not selected";
				myToolTip.IsEnabled = true;
				myToolTip.IsOpen = true;
			}
		}

		private void DueDateChangeCheckBoxMouseLeave(object sender, RoutedEventArgs e)
		{
			if (canChangeDueDate == false)
			{
				myToolTip.IsEnabled = false;
				myToolTip.IsOpen = false;
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
			int c;


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

			if (db.DBQuery("AssignedTo", convJobNumber) == "")
			{
				Frame3AssignedToData.Content = "Job is not assigned";
			}
			else
			{
				AssignToDropDownBox.SelectedItem = db.DBQuery("AssignedTo", convJobNumber);
				Frame3AssignedToData.Content = db.DBQuery("AssignedTo", convJobNumber);
			}
			if (db.DBQuery("Completed", convJobNumber) == "True")
			{
				Frame3CompleteCheckBox.IsChecked = true;
			}
			else
			{
				Frame3CompleteCheckBox.IsChecked = false;

			}




		}

		private void Submit_Click(object sender, RoutedEventArgs e)
		{
			//MessageBox.Show(convJobNumber.ToString());

			if (Frame3CompleteCheckBox.IsChecked == true)
			{
				db.DBQueryInsertData("Completed", convJobNumber, true);
				db.DBQueryInsertData(convJobNumber, "Action", "Actioned");

			}
			else
			{
				db.DBQueryInsertData("Completed", convJobNumber, false);
				db.DBQueryInsertData(convJobNumber, "Action", "Action required");
			}

			db.DBQueryInsertData("AssignedTo", convJobNumber, AssignToDropDownBox.SelectedItem.ToString());



			createJobList(filter);
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


		public void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var tc = sender as TabControl;


			if (tc != null)
			{

				TabItem tb = tc.SelectedItem as TabItem;
				if (tb != null)
				{
					String tabItem = tb.Name;
					//log.Debug(tabItem);
					if (e.Source is TabControl)
					{


						switch (tabItem)
						{

							case "OutstandingIssues":
								WindowState = System.Windows.WindowState.Normal;
								this.Height = 681.392f;
								this.Width = 800f;
								this.Left = (ScreenWidht / 2) - (this.Width / 2);
								this.Top = (ScreenHeight / 2) - (this.Height / 2);


								Frame1.Width = 772;
								emplistDataGrid.Width = 746;

								break;
							case "ViewDatabase":

								WindowState = System.Windows.WindowState.Normal;
								this.Height = 681.392f;
								this.Width = 1800f;
								emplistDataGrid.Width = 1750;
								Frame1.Width = 1790f;
								this.Left = (ScreenWidht / 2) - (this.Width / 2);
								this.Top = (ScreenHeight / 2) - (this.Height / 2);

								UpdateAllDb();




								break;


							default:
								//MessageBox.Show("Default");
								//log.Info("default message box Called");
								break;
						}
					}
				}
			}

		}

		private void FilterLabelClicked(Object sender, EventArgs e)
		{
			Label btn = sender as Label;
			//MessageBox.Show(btn.Name.ToString());

			filter = btn.Name.ToString();
			filterExpander.IsExpanded = false;
			createJobList(filter);

		}
	}


}
