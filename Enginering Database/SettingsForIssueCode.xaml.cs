using System.Windows;
using System.Windows.Controls;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for SettingsForIssueCode.xaml
	/// </summary>
	public partial class SettingsForIssueCode : Window
	{
		readonly DatabaseClass db = new DatabaseClass();
		public SettingsForIssueCode()
		{
			InitializeComponent();

			setUpArea();
		}

		private void setUpArea()
		{
			db.ConnectDB();

			var getAreaComboBox = db.SetUpComboBox("AreaComboBox");
			SettingsAreaDropDown.Items.Add("Please Select");
			while (getAreaComboBox.Read())
			{
				SettingsAreaDropDown.Items.Add(getAreaComboBox[1]);
			}
			SettingsAreaDropDown.SelectedIndex = 0;

			var getPriorityComboBox = db.SetUpComboBox("PriorityComboBox");

			while (getPriorityComboBox.Read())
			{
				SettingsPriorityListBox.Items.Add(getPriorityComboBox[1]);
			}

			var getBuildingComboBox = db.SetUpComboBox("BuildingComboBox");

			while (getBuildingComboBox.Read())
			{
				SettingsBuildingListBox.Items.Add(getBuildingComboBox[1]);
			}

		}

		//once SettingsAreaDropDown selection is changed --> will change values in SettingsIssueTypeListBox
		private void SettingsAreaDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			db.ConnectDB();
			SettingsIssueTypeListBox.Items.Clear();
			SettingsFaultyAreaListBox.Items.Clear();
			SettingsIssueCodeListBox.Items.Clear();
			if (SettingsFaultyAreaListBox.SelectedIndex > 0 || SettingsIssueCodeListBox.SelectedIndex > 0 || SettingsIssueTypeListBox.SelectedIndex > 0)
			{
				SettingsFaultyAreaListBox.SelectedIndex = -1;
				SettingsIssueTypeListBox.SelectedIndex = -1;
				SettingsIssueCodeListBox.SelectedIndex = -1;
			}
			if (SettingsAreaDropDown.SelectedIndex > 0)
			{

				var setIssueComboBox = db.SetUpComboBoxBasedonParrent("IssueTypeComboBox", SettingsAreaDropDown.SelectedItem.ToString());


				while (setIssueComboBox.Read())
				{
					SettingsIssueTypeListBox.Items.Add(setIssueComboBox[1]);
				}

			}
			else
			{
				SettingsIssueTypeListBox.Items.Clear();
				SettingsFaultyAreaListBox.Items.Clear();
				SettingsIssueCodeListBox.Items.Clear();
			}

		}

		private void SettingsAddNewArea_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SettingsRemoveArea_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SettingsAddIssueTypeTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				MessageBox.Show("Pressed Enter in Add Issue TextBox");
			}
		}

		private void SettingsAddFaultyAreaTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				MessageBox.Show("Pressed Enter in Add Faulty Area TextBox");
			}
		}

		private void SettingsAddIssueCodeTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				MessageBox.Show("Pressed Enter in Add Issue Code TextBox");
			}
		}


		//once SettingsIssueTypeListBox have item selected it will update Faulty Area List box with required information
		private void SettingsIssueTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			db.ConnectDB();
			SettingsFaultyAreaListBox.SelectedIndex = -1;
			SettingsFaultyAreaListBox.Items.Clear();
			SettingsIssueCodeListBox.Items.Clear();


			if (SettingsIssueTypeListBox.SelectedIndex >= 0 && SettingsAreaDropDown.SelectedIndex > 0)
			{

				var setIssueComboBox = db.SetUpComboBoxBasedonParrent("FaultyAreaComboBox", SettingsIssueTypeListBox.SelectedItem.ToString());


				while (setIssueComboBox.Read())
				{
					SettingsFaultyAreaListBox.Items.Add(setIssueComboBox[1]);
				}

				var setIssueComboBox2 = db.SetUpComboBoxBasedonParrent("IssueComboBox", SettingsIssueTypeListBox.SelectedItem.ToString());

				while (setIssueComboBox2.Read())
				{
					SettingsIssueCodeListBox.Items.Add(setIssueComboBox2[1]);
				}

			}

		}

	}
}
