using System.Configuration;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for UserSettings.xaml
	/// </summary>
	public partial class UserSettings : Window
	{

		public  string UserName;
		public  string SubAdmin1;
		public  string SubAdmin2;
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

		public static Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		public static KeyValueConfigurationCollection configCollection = configManager.AppSettings.Settings;
		readonly DatabaseClass db = new DatabaseClass();

		public UserSettings()
		{



			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			InitializeComponent();




			openSettings();

		}


		public void openSettings()
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



				AdminTextBox.Text = UserName;
				SubAdmin1TextBox.Text = SubAdmin1;
				SubAdmin2TextBox.Text = SubAdmin2;
				emailTextBox.Text = Email;
				DueDataTextBox.Text = DueDateGap;
				JobCountSettingData.Text = jobCount;
				passwordTextBox.Text = password;
				PreviewSettingComboBox.Text = preview;
				EmailAddress2TextBox.Text = ReportEmail;


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

		private void ResetContractors()
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

		private void resetAssignToList()
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

		private void SaveUserSettingsButton_Click(object sender, RoutedEventArgs e)
		{




			db.DBQueryUpdateGlobalSettings("Admin", AdminTextBox.Text);
			db.DBQueryUpdateGlobalSettings("SubAdmin1", SubAdmin1TextBox.Text);
			db.DBQueryUpdateGlobalSettings("SubAdmin2", SubAdmin2TextBox.Text);
			db.DBQueryUpdateGlobalSettings("EmailAddress", emailTextBox.Text);
			db.DBQueryUpdateGlobalSettings("Password", passwordTextBox.Text);
			db.DBQueryUpdateGlobalSettings("EmailAddress2", EmailAddress2TextBox.Text);
			db.DBQueryUpdateGlobalSettings("JobCount", JobCountSettingData.Text);

			if (MaintenanceCheckBox.IsChecked == true)
			{


				db.DBQueryUpdateGlobalSettings("Maintenance", "Yes");
			}
			else
			{
				db.DBQueryUpdateGlobalSettings("Maintenance", "No");
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


		private void AddContractorButton_Click(object sender, RoutedEventArgs e)
		{

			contractors = contractors.Insert(contractors.Length, "/" + ContractorAddTextBox.Text);

			db.DBQueryUpdateGlobalSettings("Contractors", contractors);


			ResetContractors();
			ContractorAddTextBox.Text = "";

		}

		private void AddAssignedTo_Click(object sender, RoutedEventArgs e)
		{
			assignToUser = assignToUser.Insert(assignToUser.Length, "/" + AssignToTextBox.Text);
			db.DBQueryUpdateGlobalSettings("AssignToUser", assignToUser);




			resetAssignToList();
			AssignToTextBox.Text = "";
		}
		private void RemoveAssignedToButton_Click(object sender, RoutedEventArgs e)
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

		private void RemoveContractorButton_Click(object sender, RoutedEventArgs e)
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

		private void ResetUserSettingsButton_Copy_Click(object sender, RoutedEventArgs e)
		{


			AdminTextBox.Text = "default";
			SubAdmin1TextBox.Text = "default";
			SubAdmin2TextBox.Text = "default";
			emailTextBox.Text = "default";
			DueDataTextBox.Text = "10";
			JobCountSettingData.Text = "0";
			passwordTextBox.Text = "default";
			PreviewSettingComboBox.Text = "Yes";




		}



		private void OpenIssueCodeChanges(object sender, RoutedEventArgs e)
		{
			SettingsForIssueCode settforIssue = new SettingsForIssueCode();
			settforIssue.Show();

		}






	}
}
