using Engineering_Database;

using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Enginering_Database
{
	public partial class updateDatabase : Window
	{
		private string filter = "Outstanding";
		private readonly EmailClass email = new EmailClass();
		private int convJobNumber;
		public string contentForTextTestLabel;
		private Button textTestLabel;
		private int jobList;
		private readonly IssueClass issueClass = new IssueClass();
		private readonly UserSettings userSett = new UserSettings();
		private readonly DatabaseClass db = new DatabaseClass();
		private readonly UserErrorWindow userError = new UserErrorWindow();
		private readonly double ScreenHeight = SystemParameters.WorkArea.Height;
		private readonly double ScreenWidht = SystemParameters.WorkArea.Width;
		readonly private static BindingList<IssueClass> empList = new BindingList<IssueClass>();
		private ErrorSystem err = new ErrorSystem();

		//if data is not selected = can't change due date
		public bool canChangeDueDate = false;

		public bool canSendEmail = false;
		public bool canComplete = false;
		public bool canSubmit = false;
		public bool seperateWindow = false;
		public string jobStatus = null;
		public string isComplete = null;

		//Tooltip object
		private readonly ToolTip myToolTip = new ToolTip();

		public updateDatabase()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			db.ConnectDB();
			InitializeComponent();

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

			UpdateDamageReasonComboBox();

			createJobList(filter);
			GetOldEntries();
		}

		//pulls configuration from app.config file and updates drop down list with latest information
		private void UpdateContractorComboBox()
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateDamageReasonComboBox()

		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}

				var reader = db.DBQueryForAssets("DamageReasons");

				while (reader.Read())
				{
					DamageReasonsComboBox.Items.Add(reader["DamageReason"].ToString());
				}

				DamageReasonsComboBox.SelectedIndex = 0;
				db.CloseDB();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateAssignToComboBox()
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateAllDb()
		{
			try
			{
				if (emplistDataGrid.ItemsSource == null)
				{
					emplistDataGrid.ItemsSource = issueClass.updateIssueDataList();
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void createJobList(string filter)
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}

				issueClass.updateIssueDataList();
				TestStackPanel.Children.Clear();
				searchCombo.Items.Clear();
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
					Label ErrorLabel = new Label
					{
						Content = "Error in settings. Please check job count settings",
						Padding = new Thickness(3),
						Margin = new Thickness(1)
					};
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

				Frame3userNameLabel.Content = WindowsIdentity.GetCurrent().Name;
				Frame3CurrentDateLabel.Content = DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void TextTestLabel_Leave(object sender, MouseEventArgs e)
		{
			previewStackPanel.Visibility = Visibility.Hidden;
		}

		private void TextTestLabel_Hover(object sender, EventArgs e)
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}

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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void TextTestLabel_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				canChangeDueDate = true;
				canSendEmail = true;
				canSubmit = true;
				canComplete = true;

				if (ChangeDueDateCheckBox.IsChecked == true)
				{
					ChangeDueDateCheckBox.IsChecked = false;
				}

				if (ConfirmEmailCheckBox.IsChecked == true)
				{
					ConfirmEmailCheckBox.IsChecked = false;
				}
				Button btn = sender as Button;

				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}

				Frame2StackPanel.Children.Clear();

				TextBlock frame2TextBlock = new System.Windows.Controls.TextBlock();
				UpdateFrame2(btn.Content.ToString());
				Frame2StackPanel.Children.Add(frame2TextBlock);
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void searchCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)

		{
			searchCombo.IsReadOnly = false;
		}

		private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void StartTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
		}

		private void CompleteCheckboxOnCheck(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canComplete == true)
				{
					if (ConfirmEmailCheckBox.IsChecked == false)
					{
						ConfirmEmailCheckBox.IsChecked = true;
					}
				}
				else
				{
					Frame3CompleteCheckBox.IsChecked = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ChangeDueDateCheckBox_Change(object sender, RoutedEventArgs e)
		{
			try
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
						if (db.DBStatus() == "DB not Connected")
						{
							db.ConnectDB();
						}
						Frame3UpdateDueDateLabel.Visibility = Visibility.Hidden;
						Frame3UpdateDueDateDatePicker.Visibility = Visibility.Hidden;
						Frame3DueDateTextBox.Foreground = Brushes.Black;
						Frame3DueDateTextBox.FontWeight = FontWeights.Normal;
						Frame3DueDateTextBox.Text = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber)).ToShortDateString().ToString();
					}
				}
				else
				{
					ChangeDueDateCheckBox.IsChecked = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ConfirmEmailCheckBox_Change(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canSendEmail == false)
				{
					ConfirmEmailCheckBox.IsChecked = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void DueDateChangeCheckBoxMouseOver(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canChangeDueDate == false)
				{
					myToolTip.Content = "Can't change Due date if data is not selected";
					myToolTip.IsEnabled = true;
					myToolTip.IsOpen = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void DueDateChangeCheckBoxMouseLeave(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canChangeDueDate == false)
				{
					myToolTip.IsEnabled = false;
					myToolTip.IsOpen = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void CompleteCheckBoxMouseOver(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canChangeDueDate == false)
				{
					myToolTip.Content = "Can't complete issue if data is not selected";
					myToolTip.IsEnabled = true;
					myToolTip.IsOpen = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void CompleteCheckBoxMouseLeave(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canChangeDueDate == false)
				{
					myToolTip.IsEnabled = false;
					myToolTip.IsOpen = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ConfirmEmailCheckBoxMouseOver(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canSendEmail == false)
				{
					myToolTip.Content = "Can't confirm email if data is not selected";
					myToolTip.IsEnabled = true;
					myToolTip.IsOpen = true;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ConfirmEmailCheckBoxMouseLeave(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canSendEmail == false)
				{
					myToolTip.IsEnabled = false;
					myToolTip.IsOpen = false;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void GetOldEntries()
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}
				var getEntries = db.DBQueryForOldEntries("engineeringDatabaseTable", "false");

				while (getEntries.Read())
				{
					DateTime date = Convert.ToDateTime(getEntries["DueDate"]);
					DateTime now = DateTime.Now;
					TimeSpan diff = now - date;
					if (diff.Days > 0 && Convert.ToInt32(getEntries["JobNumber"]) != 0)
					{
						IssueClass issueOldEntries = new IssueClass();
						OutstandingIssuesLabel.Visibility = Visibility.Hidden;
						issueOldEntries.JobNumber = (int)getEntries["JobNumber"];
						issueOldEntries.DueDate = String.Format("{0:d}", getEntries["DueDate"]);
						OldEntriesListView.Items.Add(issueOldEntries);
					}

					if (OldEntriesListView.Items.Count < 2)
					{
						OldEntriesListView.Visibility = Visibility.Hidden;
						OutstandingIssuesLabel.Visibility = Visibility.Visible;
					}
				}

				if (OldEntriesListView.Items.Count > 0)
				{
					OldEntriesListView.Visibility = Visibility.Visible;
				}
				else
				{
					OldEntriesListView.Visibility = Visibility.Hidden;
					this.Height = 686;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void DueDateChange_change(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (canChangeDueDate == true)
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
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void UpdateFrame2(string jobNumber)
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}
				int c;

				if (Int32.TryParse(jobNumber, out c))
				{
					convJobNumber = c;
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

				if (Convert.ToBoolean(db.DBQuery("LockedOff", convJobNumber)) == true)
				{
					LockedOff.IsChecked = true;
				}
				else
				{
					LockedOff.IsChecked = false;
				}

				if (db.DBQuery("DamageReason", convJobNumber) == "")
				{
					DamageReasonsComboBox.SelectedItem = "Not Specified";
				}
				else
				{
					DamageReasonsComboBox.SelectedItem = db.DBQuery("DamageReason", convJobNumber);
				}

				if (db.DBQuery("AssignedTo", convJobNumber) == "NotAssigned")
				{
					Frame3AssignedToData.Content = "Job is not assigned";

					AssignToDropDownBox.Text = "Please select";
				}
				else
				{
					AssignToDropDownBox.SelectedItem = db.DBQuery("AssignedTo", convJobNumber);
					Frame3AssignedToData.Content = db.DBQuery("AssignedTo", convJobNumber);
				}
				if (db.DBQuery("Completed", convJobNumber) == "True")
				{
					jobStatus = "Closed";
					Frame3CompleteCheckBox.IsChecked = true;
				}
				else
				{
					jobStatus = "Open";
					Frame3CompleteCheckBox.IsChecked = false;
				}

				if (db.DBQuery("Contractor", convJobNumber) != "")
				{
					ContractorComboBoxData.SelectedItem = db.DBQuery("Contractor", convJobNumber);
				}
				else
				{
					ContractorComboBoxData.Text = "Please select";
				}

				if (db.DBQuery("CommentsForActionTaken", convJobNumber) != "")
				{
					Frame2AdminDescriptionTextBox.Text = db.DBQuery("CommentsForActionTaken", convJobNumber);
				}
				else
				{
					Frame2AdminDescriptionTextBox.Text = "";
				}

				DateTime StartDate = DateTime.Now.Date;
				DateTime EndDate = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber));
				Double daysBetween = (EndDate - StartDate).TotalDays;

				if (daysBetween < 0)
				{
					Frame3DaysTillDueData.Foreground = Brushes.Red;
				}
				else
				{
					Frame3DaysTillDueData.Foreground = Brushes.Green;
				}
				Frame3DaysTillDueData.Content = daysBetween;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void CollectSelectedData(int convJobNumber)
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}

				issueClass.Priority = db.DBQuery("Priority", convJobNumber);
				issueClass.JobNumber = convJobNumber;
				issueClass.ReportedDate = Convert.ToDateTime(db.DBQuery("ReportedDate", convJobNumber)).ToShortDateString().ToString();
				issueClass.ReportedUserName = db.DBQuery("ReportedUsername", convJobNumber);
				issueClass.Area = db.DBQuery("Area", convJobNumber);
				issueClass.Type = db.DBQuery("Type", convJobNumber);
				issueClass.Building = db.DBQuery("Building", convJobNumber);
				issueClass.ReportedTime = Convert.ToDateTime(db.DBQuery("ReportedTime", convJobNumber)).ToShortTimeString();
				issueClass.DueDate = Convert.ToDateTime(db.DBQuery("DueDate", convJobNumber)).ToShortDateString().ToString();
				issueClass.FaulyArea = db.DBQuery("FaultyArea", convJobNumber);
				issueClass.Code = db.DBQuery("IssueCode", convJobNumber);
				issueClass.AssetNumber = db.DBQuery("AssetNumber", convJobNumber);
				issueClass.DetailedDescription = db.DBQuery("DetailedDescription", convJobNumber);
				issueClass.AssignedTo = db.DBQuery("AssignedTo", convJobNumber);
				issueClass.CommentsForActionsTaken = db.DBQuery("CommentsForActionTaken", convJobNumber);
				issueClass.ReportedEmail = db.DBQuery("ReporterEmail", convJobNumber);
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void Submit_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (canSubmit == true)
				{
					if (db.DBStatus() == "DB not Connected")
					{
						db.ConnectDB();
					}
					if (Frame3CompleteCheckBox.IsChecked == true)
					{
						db.DBQueryInsertData("Completed", convJobNumber, true);
						isComplete = "Complete";
						DateTime time = DateTime.Now;
						db.DBQueryInsertData(convJobNumber, "CompletedTime", time.ToString("HH:mm"));
						db.DBQueryInsertData(convJobNumber, "CompletedDate", time.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
						db.DBQueryInsertData(convJobNumber, "Action", "Actioned");
						db.DBQueryInsertData(convJobNumber, "CompletedBy", Frame3userNameLabel.Content.ToString());
						Frame3CompleteCheckBox.IsChecked = false;
					}
					else
					{
						if (jobStatus == "Closed")
						{
							isComplete = "ReOpen";
						}

						db.DBQueryInsertData("Completed", convJobNumber, false);
						db.DBQueryInsertData(convJobNumber, "Action", "Action required");
						db.DBQueryInsertData(convJobNumber, "CompletedTime", null);
						db.DBQueryInsertData(convJobNumber, "CompletedDate", null);
						db.DBQueryInsertData(convJobNumber, "CompletedBy", null);
					}
					db.DBQueryInsertData(convJobNumber, "DamageReason", DamageReasonsComboBox.SelectedItem.ToString());
					if (Frame2AdminDescriptionTextBox.Text != null)
					{
						db.DBQueryInsertData("CommentsForActionTaken", convJobNumber, Frame2AdminDescriptionTextBox.Text.ToString());
					}

					if (ChangeDueDateCheckBox.IsChecked == true)
					{
						db.DBQueryInsertData("DueDate", convJobNumber, Frame3UpdateDueDateDatePicker.Text.ToString());
					}

					if (AssignToDropDownBox.SelectedItem != null)
					{
						db.DBQueryInsertData("AssignedTo", convJobNumber, AssignToDropDownBox.SelectedItem.ToString());
					}

					if (ContractorComboBoxData.SelectedIndex > 0)
					{
						db.DBQueryInsertData("Contractor", convJobNumber, ContractorComboBoxData.SelectedItem.ToString());
					}

					CollectSelectedData(convJobNumber);

					if (ConfirmEmailCheckBox.IsChecked == true && canSendEmail == true)
					{
						email.SendEmail(jobStatus, issueClass, isComplete);
						ConfirmEmailCheckBox.IsChecked = false;
						canSendEmail = false;
						isComplete = null;
						jobStatus = null;
					}
					else if (canSendEmail == false)
					{
						userError.errorMessage = "Sorry. You don't have selected any job. You are not allowed to send email";
						userError.Title = "Confirm email error";
						userError.CallWindow();
						userError.ShowDialog();
					}

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

					if (ChangeDueDateCheckBox.IsChecked == true)
					{
						ChangeDueDateCheckBox.IsChecked = false;
					}
					if (seperateWindow == true)
					{
						this.Close();
					}
				}
				else
				{
					userError.errorMessage = "Sorry. You are not allowed to submit anything. Please select job from list to update";
					userError.Title = "Job Selection error";
					userError.CallWindow();
					userError.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				var tc = sender as TabControl;
				if (tc != null)
				{
					TabItem tb = tc.SelectedItem as TabItem;
					if (tb != null)
					{
						String tabItem = tb.Name;

						if (e.Source is TabControl)
						{
							switch (tabItem)
							{
								case "OutstandingIssues":
									WindowState = System.Windows.WindowState.Normal;
									//681.392f;
									this.Height = 683f;
									this.Width = 1113f;
									this.Left = (ScreenWidht / 2) - (this.Width / 2);
									this.Top = (ScreenHeight / 2) - (this.Height / 2);
									Frame1.Width = 772;
									emplistDataGrid.Width = 746;
									break;

								case "ViewDatabase":
									OldEntriesListView.Visibility = Visibility.Hidden;
									WindowState = System.Windows.WindowState.Normal;
									this.Height = 683f;
									this.Width = 1800f;
									emplistDataGrid.Width = 1750;
									Frame1.Width = 1790f;
									this.Left = (ScreenWidht / 2) - (this.Width / 2);
									this.Top = (ScreenHeight / 2) - (this.Height / 2);
									UpdateAllDb();
									break;

								default:

									break;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void FilterLabelClicked(Object sender, EventArgs e)
		{
			try
			{
				Label btn = sender as Label;
				TestStackPanel.Children.Clear();
				filter = btn.Name.ToString();
				filterExpander.IsExpanded = false;
				createJobList(filter);
				Frame3.Refresh();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void OldEntriesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
		}
	}
}