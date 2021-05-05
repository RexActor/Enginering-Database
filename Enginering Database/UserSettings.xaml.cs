using System;
using System.Configuration;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for UserSettings.xaml
	/// </summary>
	public partial class UserSettings : Window
	{
		public string UserName;
		public string SubAdmin1;
		public string SubAdmin2;
		public string Email;
		public string ReportEmail;

		public static string jobCount;
		public static string password;
		public static string preview;
		public string EmailPreview;
		public string contractors;
		public string assignToUser;
		public string Maintenance;
		public string DueDateGap;
		public string productRequests;
		public string StatutoryDays;
		public string MeetingDaysAhead;
		public string UseWeekendsForMeeting;
		public string DebugMode;
		public string MeetingSetupEmail;

		public static Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		public static KeyValueConfigurationCollection configCollection = configManager.AppSettings.Settings;
		private readonly DatabaseClass db = new DatabaseClass();
		private ErrorSystem err = new ErrorSystem();

		public UserSettings()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			InitializeComponent();

			openSettings();
		}

		public void openSettings()
		{
			try
			{
				if (db.DBStatus() == "DB not Connected")
				{
					db.ConnectDB();
				}
				else
				{
					preview = Properties.Settings.Default["preview"].ToString();

					UserName = db.DBQueryForGlobalSettings("Admin");
					SubAdmin1 = db.DBQueryForGlobalSettings("SubAdmin1");
					SubAdmin2 = db.DBQueryForGlobalSettings("SubAdmin2");
					Email = db.DBQueryForGlobalSettings("EmailAddress");
					ReportEmail = db.DBQueryForGlobalSettings("EmailAddress2");
					contractors = db.DBQueryForGlobalSettings("Contractors");
					assignToUser = db.DBQueryForGlobalSettings("AssignToUser");
					password = db.DBQueryForGlobalSettings("Password");
					Maintenance = db.DBQueryForGlobalSettings("Maintenance");
					DueDateGap = db.DBQueryForGlobalSettings("DueDateGap");
					jobCount = db.DBQueryForGlobalSettings("JobCount");
					EmailPreview = db.DBQueryForGlobalSettings("EmailPreview");
					productRequests = db.DBQueryForGlobalSettings("RequestEmail");
					StatutoryDays = db.DBQueryForGlobalSettings("StatutoryDays");
					MeetingDaysAhead = db.DBQueryForGlobalSettings("MeetingDaysAhead");
					UseWeekendsForMeeting = db.DBQueryForGlobalSettings("MeetingsForWeekends");
					DebugMode = db.DBQueryForGlobalSettings("DebugMode");
					MeetingSetupEmail = db.DBQueryForGlobalSettings("MeetingSetupEmail");

					AdminTextBox.Text = UserName;
					SubAdmin1TextBox.Text = SubAdmin1;
					SubAdmin2TextBox.Text = SubAdmin2;
					emailTextBox.Text = Email;
					DueDataTextBox.Text = DueDateGap;
					JobCountSettingData.Text = jobCount;
					passwordTextBox.Text = password;
					PreviewSettingComboBox.Text = preview;
					EmailAddress2TextBox.Text = ReportEmail;
					ProductRequestEmail.Text = productRequests;
					StatutoryDaysTextBox.Text = StatutoryDays;
					MeetingDaysAheadTextBox.Text = MeetingDaysAhead;
					MeetingSetupEmailTextBox.Text = MeetingSetupEmail;

					if (UseWeekendsForMeeting == "No")
					{
						UseWeekendsCheckBox.IsChecked = false;
					}
					else if (UseWeekendsForMeeting == "Yes")
					{
						UseWeekendsCheckBox.IsChecked = true;
					}

					if (DebugMode == "No")
					{
						DebugModeCheckBox.IsChecked = false;
					}
					else if (DebugMode == "Yes")
					{
						DebugModeCheckBox.IsChecked = true;
					}

					if (Maintenance == "Yes")
					{
						MaintenanceCheckBox.IsChecked = true;
					}
					else
					{
						MaintenanceCheckBox.IsChecked = false;
					}

					if (EmailPreview == "Yes")
					{
						EmailPreview_CheckBox.IsChecked = true;
					}
					else
					{
						EmailPreview_CheckBox.IsChecked = false;
					}

					ResetContractors();
					resetAssignToList();
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void ResetContractors()
		{
			try
			{
				contractors = contractors.Replace(" ", string.Empty);
				ContractorListBoxSettings.Items.Clear();
				string[] splitContractors = contractors.Split('/');
				foreach (var word in splitContractors)
				{
					if (!word.Equals(""))
					{
						ContractorListBoxSettings.Items.Add(word);
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void resetAssignToList()
		{
			try
			{
				AssignToUserListSettingsListBox.Items.Clear();
				string[] splitAssignTo = assignToUser.Split('/');
				foreach (var word in splitAssignTo)
				{
					if (!word.Equals(""))
					{
						AssignToUserListSettingsListBox.Items.Add(word);
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void SaveUserSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				db.DBQueryUpdateGlobalSettings("Admin", AdminTextBox.Text);
				db.DBQueryUpdateGlobalSettings("SubAdmin1", SubAdmin1TextBox.Text);
				db.DBQueryUpdateGlobalSettings("SubAdmin2", SubAdmin2TextBox.Text);
				db.DBQueryUpdateGlobalSettings("EmailAddress", emailTextBox.Text);
				db.DBQueryUpdateGlobalSettings("Password", passwordTextBox.Text);
				db.DBQueryUpdateGlobalSettings("EmailAddress2", EmailAddress2TextBox.Text);
				db.DBQueryUpdateGlobalSettings("JobCount", JobCountSettingData.Text);
				db.DBQueryUpdateGlobalSettings("RequestEmail", ProductRequestEmail.Text);
				db.DBQueryUpdateGlobalSettings("StatutoryDays", StatutoryDaysTextBox.Text);
				db.DBQueryUpdateGlobalSettings("MeetingDaysAhead", MeetingDaysAheadTextBox.Text);
				db.DBQueryUpdateGlobalSettings("MeetingSetupEmail", MeetingSetupEmailTextBox.Text);

				if (MaintenanceCheckBox.IsChecked == true)
				{
					db.DBQueryUpdateGlobalSettings("Maintenance", "Yes");
				}
				else
				{
					db.DBQueryUpdateGlobalSettings("Maintenance", "No");
				}

				if (UseWeekendsCheckBox.IsChecked == true)
				{
					db.DBQueryUpdateGlobalSettings("MeetingsForWeekends", "Yes");
				}
				else
				{
					db.DBQueryUpdateGlobalSettings("MeetingsForWeekends", "No");
				}
				if (DebugModeCheckBox.IsChecked == true)
				{
					db.DBQueryUpdateGlobalSettings("DebugMode", "Yes");
				}
				else
				{
					db.DBQueryUpdateGlobalSettings("DebugMode", "No");
				}

				if (EmailPreview_CheckBox.IsChecked == true)
				{
					db.DBQueryUpdateGlobalSettings("EmailPreview", "Yes");
				}
				else
				{
					db.DBQueryUpdateGlobalSettings("EmailPreview", "No");
				}

				db.DBQueryUpdateGlobalSettings("DueDateGap", DueDataTextBox.Text);

				Properties.Settings.Default["jobCount"] = JobCountSettingData.Text;

				Properties.Settings.Default["password"] = passwordTextBox.Text;
				Properties.Settings.Default["preview"] = PreviewSettingComboBox.Text;

				Properties.Settings.Default.Save();

				this.Close();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void AddContractorButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				contractors = contractors.Insert(contractors.Length, "/" + ContractorAddTextBox.Text);

				db.DBQueryUpdateGlobalSettings("Contractors", contractors);

				ResetContractors();
				ContractorAddTextBox.Text = "";
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void AddAssignedTo_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				assignToUser = assignToUser.Insert(assignToUser.Length, "/" + AssignToTextBox.Text);
				db.DBQueryUpdateGlobalSettings("AssignToUser", assignToUser);

				resetAssignToList();
				AssignToTextBox.Text = "";
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void RemoveAssignedToButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string tempAssignTo = "";
				if (AssignToUserListSettingsListBox.SelectedItem == null)
				{
					MessageBox.Show("No item selected");
				}
				else
				{
					string[] splitAssignTo = assignToUser.Split('/');
					assignToUser = "";
					foreach (var word in splitAssignTo)
					{
						if (word.Equals(AssignToUserListSettingsListBox.SelectedItem))
						{
							AssignToUserListSettingsListBox.Items.Remove(word);
						}
						else
						{
							tempAssignTo = tempAssignTo.Insert(tempAssignTo.Length, "/" + word);
						}
					}
					assignToUser = tempAssignTo;

					db.DBQueryUpdateGlobalSettings("AssignToUser", assignToUser);
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void RemoveContractorButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string tempContr = "";

				if (ContractorListBoxSettings.SelectedItem == null)
				{
					MessageBox.Show("No item selected");
				}
				else
				{
					contractors = contractors.Replace(" ", string.Empty);

					string[] splitContractors = contractors.Split('/');
					contractors = "";
					foreach (var word in splitContractors)
					{
						if (word.Equals(ContractorListBoxSettings.SelectedItem))

						{
							ContractorListBoxSettings.Items.Remove(word);
						}
						else
						{
							tempContr = tempContr.Insert(tempContr.Length, "/" + word);
						}
					}
					contractors = tempContr;

					db.DBQueryUpdateGlobalSettings("Contractors", contractors);
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void ResetUserSettingsButton_Copy_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AdminTextBox.Text = "default";
				SubAdmin1TextBox.Text = "default";
				SubAdmin2TextBox.Text = "default";
				emailTextBox.Text = "default";
				DueDataTextBox.Text = "10";
				JobCountSettingData.Text = "0";
				passwordTextBox.Text = "default";
				PreviewSettingComboBox.Text = "Yes";
				ProductRequestEmail.Text = "default";
				StatutoryDaysTextBox.Text = "10";
				MeetingDaysAheadTextBox.Text = "0";
				DebugModeCheckBox.IsChecked = false;
				MeetingSetupEmailTextBox.Text = "default";
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}

		private void OpenIssueCodeChanges(object sender, RoutedEventArgs e)
		{
			try
			{
				SettingsForIssueCode settforIssue = new SettingsForIssueCode();
				settforIssue.Show();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}
	}
}