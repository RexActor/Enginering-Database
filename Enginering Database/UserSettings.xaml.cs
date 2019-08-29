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
		public static string Email;
		public static string database;
		public static string jobCount;
		public static string password;
		public static string preview;
		public  string contractors;

		public static Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		public static KeyValueConfigurationCollection configCollection = configManager.AppSettings.Settings;

		public UserSettings()
		{


			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			InitializeComponent();


			openSettings();

		}


		public void openSettings()
		{

			/*
		
			*/
			//string UserName;
			//string SubAdmin1;
			//string SubAdmin2;
			//string Email;
			//string database;
			UserName = ConfigurationManager.AppSettings.Get("UserName");
			SubAdmin1 = ConfigurationManager.AppSettings.Get("SubAdmin1");
			SubAdmin2 = ConfigurationManager.AppSettings.Get("SubAdmin2");
			Email = ConfigurationManager.AppSettings.Get("Email");
			database = ConfigurationManager.AppSettings.Get("database");
			jobCount = ConfigurationManager.AppSettings.Get("jobCount");
			password = ConfigurationManager.AppSettings.Get("password");
			preview = ConfigurationManager.AppSettings.Get("preview");
			contractors = ConfigurationManager.AppSettings.Get("contractors");

			AdminTextBox.Text = UserName;
			SubAdmin1TextBox.Text = SubAdmin1;
			SubAdmin2TextBox.Text = SubAdmin2;
			emailTextBox.Text = Email;
			DatabaseTextBox.Text = database;
			JobCountSettingData.Text = jobCount;
			passwordTextBox.Text = password;
			PreviewSettingComboBox.Text = preview;

			ResetContractors();



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

		private void SaveUserSettingsButton_Click(object sender, RoutedEventArgs e)
		{



			configCollection["UserName"].Value = AdminTextBox.Text;
			configCollection["SubAdmin1"].Value = SubAdmin1TextBox.Text;
			configCollection["SubAdmin2"].Value = SubAdmin2TextBox.Text;
			configCollection["Email"].Value = emailTextBox.Text;
			configCollection["database"].Value = DatabaseTextBox.Text;
			configCollection["jobCount"].Value = JobCountSettingData.Text;
			configCollection["password"].Value = passwordTextBox.Text;
			configCollection["preview"].Value = PreviewSettingComboBox.Text;



			configManager.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);

			this.Close();

		}


		private void AddContractorButton_Click(object sender, RoutedEventArgs e)
		{

			contractors = contractors.Insert(contractors.Length, "/" + ContractorAddTextBox.Text);
			configCollection["contractors"].Value = contractors;
			configManager.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);

			ResetContractors();

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
				//ContractorListBoxSettings.Items.Clear();
				
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
						//ContractorListBoxSettings.Items.Add(word);
					}
				}
				contractors = tempContr;
				configCollection["contractors"].Value = contractors;
				
				configManager.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);



				//ResetContractors();
				//MessageBox.Show(tempContr);
			}
		}

		private void ResetUserSettingsButton_Copy_Click(object sender, RoutedEventArgs e)
		{
			Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			KeyValueConfigurationCollection configCollection = configManager.AppSettings.Settings;

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
	}
}
