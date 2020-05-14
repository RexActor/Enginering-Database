using System.Windows;

namespace Engineering_Database
{


	/// <summary>
	/// Interaction logic for InteractionWindow.xaml
	/// </summary>

	public partial class InteractionWindow : Window
	{

		public string interactionLabelText;
		public string newAreaCode;
		DatabaseClass db = new DatabaseClass();

		public InteractionWindow()
		{
			InitializeComponent();

			InteractionWindowLabel.Content = interactionLabelText;

		}

		private void InteractionWindowOkButton_Click(object sender, RoutedEventArgs e)
		{
			if (InteractionWindowTextBox.Text != "")
			{
				newAreaCode = InteractionWindowTextBox.Text;
				AddAreaCode("AreaComboBox", newAreaCode);



				this.Close();
			}

			else
			{
				InteractionWindowLabel.Content = "Please enter value in textbox. It can't be empty";
			}

		}


		private void AddAreaCode(string table, string areaCode)
		{


			db.ConnectDB();
			db.InsertIssueIntoDatabase(table, areaCode);

		}
		//private void reloadArea()
		//{

		//	db.ConnectDB();
		//	SettingsForIssueCode setIssueWin = new SettingsForIssueCode();
		//	//setIssueWin.SettingsAreaDropDown.Items.Clear();

		//	var getAreaComboBox = db.SetUpComboBox("AreaComboBox");
		//	setIssueWin.SettingsAreaDropDown.Items.Add("Please Select");
		//	while (getAreaComboBox.Read())
		//	{
		//		setIssueWin.SettingsAreaDropDown.Items.Add(getAreaComboBox[1]);
		//	}
		//	setIssueWin.SettingsAreaDropDown.SelectedIndex = 0;
		//}

	}
}
