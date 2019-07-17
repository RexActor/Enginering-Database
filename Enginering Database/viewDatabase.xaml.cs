using Engineering_Database;
using System;
using System.Data;

using System.Windows;

namespace Enginering_Database
{
	/// <summary>
	/// Interaction logic for viewDatabase.xaml
	/// </summary>
	public partial class viewDatabase : Window
	{
		public static int ldata = 0;

		IssueClass issueData = new IssueClass();

		public viewDatabase()


		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			InitializeComponent();
			viewdataGrid();
			this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.9;
			this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
			this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		}


		public void viewdataGrid()
		{

			throw new NotImplementedException();



		}

	}
}
