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
		int selectedIndex;
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

			UpdateListBox();
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

				//check if area is selected. Otherwise return. As to be able to insert into database issue type, we need to have area code selected
				if (SettingsAreaDropDown.SelectedIndex > 0)
				{
					//MessageBox.Show($"Area selected -> {SettingsAreaDropDown.SelectedItem}");
					db.InsertIssueIntoDatabase("IssueTypeComboBox", SettingsAddIssueTypeTextBox.Text, SettingsAreaDropDown.SelectedItem.ToString());
					UpdateListBox();

					SettingsAddIssueTypeTextBox.Clear();
				}
				

			}
		}

		private void SettingsAddFaultyAreaTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{

				//check if Issue Type  is selected. Otherwise return. As to be able to insert into database issue type, we need to have Issue Type selected
				//in database Issue type is being linked with Faulty area and issue code
				if (SettingsIssueTypeListBox.SelectedIndex >= 0)
				{
					if (SettingsAreaDropDown.SelectedIndex > 0)
					{
						//MessageBox.Show($"Area selected -> {SettingsAreaDropDown.SelectedItem}");
						db.InsertIssueIntoDatabase("FaultyAreaComboBox", SettingsAddFaultyAreaTextBox.Text, SettingsIssueTypeListBox.SelectedItem.ToString());
						UpdateListBox();

						SettingsAddFaultyAreaTextBox.Clear();
					}
				}
				//MessageBox.Show(selectedIndex.ToString());
				//selecting back default issuetype when you were inserting data
				SettingsIssueTypeListBox.SelectedIndex = selectedIndex;

			}
		}

		private void SettingsAddIssueCodeTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
			{
				//check if Issue Type  is selected. Otherwise return. As to be able to insert into database issue type, we need to have Issue Type selected
				//in database Issue type is being linked with Faulty area and issue code
				if (SettingsIssueTypeListBox.SelectedIndex >= 0)
				{
					if (SettingsAreaDropDown.SelectedIndex > 0)
					{
						//MessageBox.Show($"Area selected -> {SettingsAreaDropDown.SelectedItem}");
						db.InsertIssueIntoDatabase("IssueComboBox", SettingsAddIssueCodeTextBox.Text, SettingsIssueTypeListBox.SelectedItem.ToString());
						UpdateListBox();
						
						SettingsAddIssueCodeTextBox.Clear();
					}
				}
				//MessageBox.Show(selectedIndex.ToString());
				//selecting back default issuetype when you were inserting data
				SettingsIssueTypeListBox.SelectedIndex = selectedIndex;

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

				//selecting default issue type index to be able to set it back
				selectedIndex = SettingsIssueTypeListBox.SelectedIndex;
			}

		}

		private void SettingsIssueTypeListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{

			if (SettingsIssueTypeListBox.SelectedIndex >= 0)
			{
				//MessageBox.Show($"Double click detected for line {SettingsIssueTypeListBox.SelectedItem}");
				 var result = MessageBox.Show($"Are you sure that you want to remove  [{SettingsIssueTypeListBox.SelectedItem}] from list?", "Removing issue", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.Yes)
				{

					db.DeleteParrentFromDatabase("FaultyAreaComboBox", SettingsIssueTypeListBox.SelectedItem.ToString());
					db.DeleteParrentFromDatabase("IssueComboBox", SettingsIssueTypeListBox.SelectedItem.ToString());
					db.DeleteIssueFromDatabase("IssueTypeComboBox", SettingsIssueTypeListBox.SelectedItem.ToString(), SettingsAreaDropDown.SelectedItem.ToString());
					UpdateListBox();
				}
				else
				{
					return;
				}

			}
		}


		private void UpdateListBox()
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

		private void SettingsIssueCodeListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (SettingsIssueCodeListBox.SelectedIndex >= 0)
			{
				//MessageBox.Show($"Double click detected for line {SettingsIssueTypeListBox.SelectedItem}");
				var result = MessageBox.Show($"Are you sure that you want to remove  [{SettingsIssueCodeListBox.SelectedItem}] from list?", "Removing issue", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.Yes)
				{
					db.DeleteIssueFromDatabase("IssueComboBox", SettingsIssueCodeListBox.SelectedItem.ToString(), SettingsIssueTypeListBox.SelectedItem.ToString());
					UpdateListBox();
					SettingsIssueTypeListBox.SelectedIndex = selectedIndex;
				}
				else
				{
					return;
				}

			}
		}

		private void SettingsFaultyAreaListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (SettingsFaultyAreaListBox.SelectedIndex >= 0)
			{
				//MessageBox.Show($"Double click detected for line {SettingsIssueTypeListBox.SelectedItem}");
				var result = MessageBox.Show($"Are you sure that you want to remove  [{SettingsFaultyAreaListBox.SelectedItem}] from list?", "Removing issue", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.Yes)
				{
					db.DeleteIssueFromDatabase("FaultyAreaComboBox", SettingsFaultyAreaListBox.SelectedItem.ToString(), SettingsIssueTypeListBox.SelectedItem.ToString());
					UpdateListBox();
					SettingsIssueTypeListBox.SelectedIndex = selectedIndex;
				}
				else
				{
					return;
				}

			}
		}

	
	}
}
