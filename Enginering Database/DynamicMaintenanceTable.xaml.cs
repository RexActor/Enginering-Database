using System;
using System.Windows;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for DynamicMaintenanceTable.xaml
	/// </summary>
	public partial class DynamicMaintenanceTable : Window
	{
		public DynamicMaintenanceTable()
		{
			InitializeComponent();
			LoadPage();
		}

		public void LoadPage()
		{
			MaintenanceFrame.Source = new Uri("MaintenancePage.xaml", UriKind.RelativeOrAbsolute);
		}


	}
}
