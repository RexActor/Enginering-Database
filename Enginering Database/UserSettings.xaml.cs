using System.Configuration;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for UserSettings.xaml
	/// </summary>
	public partial class UserSettings : Window
	{

		public static string UserName;
		public static string SubAdmin1;
		public static string SubAdmin2;
		public string Email;
		public static string database;
		public static string jobCount;
		public static string password;
		public static string preview;
		public string contractors;
		public string assignToUser;

		public static Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		public static KeyValueConfigurationCollection configCollection = configManager.AppSettings.Settings;
		DatabaseClass db = new DatabaseClass();

		public UserSettings()
		{


			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			InitializeComponent();


			db.ConnectDB();

			openSettings();

		}


		public void openSettings()
		{
			
		
		

			


			//TODO:move to userSettings profile
			jobCount = Properties.Settings.Default["jobCount"].ToString();
		
			// TODO: move to usersettings profile
			preview = Properties.Settings.Default["preview"].ToString();
		

			UserName = db.DBQueryForGlobalSettings("Admin");
			SubAdmin1 = db.DBQueryForGlobalSettings("SubAdmin1");
			SubAdmin2 = db.DBQueryForGlobalSettings("SubAdmin2");
			Email = db.DBQueryForGlobalSettings("EmailAddress");
			contractors = db.DBQueryForGlobalSettings("Contractors");
			assignToUser = db.DBQueryForGlobalSettings("AssignToUser");
			password = db.DBQueryForGlobalSettings("Password");



			AdminTextBox.Text = UserName;
			SubAdmin1TextBox.Text = SubAdmin1;
			SubAdmin2TextBox.Text = SubAdmin2;
			emailTextBox.Text = Email;
			DatabaseTextBox.Text = database;
			JobCountSettingData.Text = jobCount;
			passwordTextBox.Text = password;
			PreviewSettingComboBox.Text = preview;


			ResetContractors();
			resetAssignToList();


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
			//assignToUser = assignToUser.Replace(" ", string.Empty);
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


	
			Properties.Settings.Default["Database"] = DatabaseTextBox.Text;
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
			//Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			//KeyValueConfigurationCollection configCollection = configManager.AppSettings.Settings;

			AdminTextBox.Text = "default";
			SubAdmin1TextBox.Text = "default";
			SubAdmin2TextBox.Text = "default";
			emailTextBox.Text = "default";
			DatabaseTextBox.Text = "default";
			JobCountSettingData.Text = "0";
			passwordTextBox.Text = "default";
			PreviewSettingComboBox.Text = "Yes";

			/*
			configCollection["UserName"].Value = "default";
			configCollection["SubAdmin1"].Value = "default";
			configCollection["SubAdmin2"].Value = "default";
			configCollection["Email"].Value = "default";
			configCollection["database"].Value = "default";

	*/


			///configManager.Save(ConfigurationSaveMode.Modified);
			///ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);


		}



		private void OpenIssueCodeChanges(object sender, RoutedEventArgs e)
		{
			SettingsForIssueCode settforIssue = new SettingsForIssueCode();
			settforIssue.Show();

		}






	}
}
