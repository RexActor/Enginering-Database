using System;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for DynamicMaintenanceTable.xaml
	/// </summary>
	public partial class DynamicMaintenanceTable : Window
	{
		const string lineMaint = "LineMaintenancePage.xaml";
		const string terraMaint = "TerraMaintenancePage.xaml";
		public DynamicMaintenanceTable()
		{
			InitializeComponent();
			testCombo.Items.Add(lineMaint);
			testCombo.Items.Add(terraMaint);
			testCombo.SelectedIndex = 0;
		}

		public void LoadPage(string value)
		{

			MaintenanceFrame.Source = new Uri(value, UriKind.RelativeOrAbsolute);

		}

		private void testCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			LoadPage(testCombo.SelectedItem.ToString());
		}
	}
}
