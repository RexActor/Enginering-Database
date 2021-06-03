using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using System.Security.Principal;
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
		private EmailClass email = new EmailClass();
		private ErrorSystem err = new ErrorSystem();

		public LockOffWindow()
		{
			InitializeComponent();
			updateListbox();
		}

		private void updateListbox()
		{
			try
			{
				if (LockOfflist.ItemsSource != null)
				{
					LockOfflist.ItemsSource = null;
					_issueList.Clear();
				}
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void LockBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (LockOfflist.SelectedItem != null)
				{
					var selectedItem = LockOfflist.SelectedItem as IssueClass;

					if (db.DBStatus() == "DB not Connected")
					{
						db.ConnectDB();
					}

					int convJobNumber = Convert.ToInt32(selectedItem.JobNumber);
					string userName = WindowsIdentity.GetCurrent().Name;
					db.DBQueryInsertData("UserLockedOff", convJobNumber, userName);
					db.DBQueryInsertData("LockedOff", convJobNumber, true);

					email.SendEmail("LockOff", selectedItem, "LockOff");

					updateListbox();

					//MessageBox.Show(selectedItem.JobNumber.ToString());
				}
				else
				{
					return;
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}
	}
}