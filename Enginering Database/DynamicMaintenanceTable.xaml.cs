using System;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for DynamicMaintenanceTable.xaml
	/// </summary>
	public partial class DynamicMaintenanceTable : Window
	{
		private ErrorSystem err = new ErrorSystem();
		private const string lineMaint = "LineMaintenancePage.xaml";

		private const string terraMaint = "TerraMaintenancePage.xaml";

		public DynamicMaintenanceTable()
		{
			InitializeComponent();
			testCombo.Items.Add(lineMaint);
			testCombo.Items.Add(terraMaint);
			testCombo.SelectedIndex = 0;
		}

		public void LoadPage(string value)
		{
			try
			{
				MaintenanceFrame.Source = new Uri(value, UriKind.RelativeOrAbsolute);
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void testCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			try
			{
				LoadPage(testCombo.SelectedItem.ToString());
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}