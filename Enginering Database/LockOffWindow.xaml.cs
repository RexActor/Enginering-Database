using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for LockOffWindow.xaml
	/// </summary>
	public partial class LockOffWindow : Window
	{
		private List<IssueClass> _issueList;
		private IssueClass _issueClass;
		private DatabaseClass db = new DatabaseClass();

		public LockOffWindow()
		{
			InitializeComponent();
			updateListbox();
		}

		private void updateListbox()
		{
			_issueList = new List<IssueClass>();

			if (db.DBStatus() == "DB not Connected")
			{
				db.ConnectDB();
			}

			OleDbDataReader reader = db.DBQueryForAllLines().ExecuteReader();

			while (reader.Read())
			{
				if (Convert.ToBoolean(reader["LockedOff"]) == false && Convert.ToBoolean(reader["Completed"]) == false)
				{
					_issueClass = new IssueClass
					{
						JobNumber = Convert.ToInt32(reader["JobNumber"]),
						DetailedDescription = reader["DetailedDescription"].ToString(),
						Code = reader["IssueCode"].ToString(),
						Area = reader["Area"].ToString(),
						Building = reader["Building"].ToString(),
						AssetNumber = reader["AssetNumber"].ToString(),
						FaulyArea = reader["FaultyArea"].ToString()
					};
					_issueList.Add(_issueClass);
				}
			}

			LockOfflist.ItemsSource = _issueList;

			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(LockOfflist.ItemsSource);
			view.SortDescriptions.Add(new SortDescription("JobNumber", ListSortDirection.Descending));
		}

		private void LockBtn_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}