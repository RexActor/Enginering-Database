using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for OnlineUsers.xaml
	/// </summary>
	public partial class OnlineUsers : Window
	{
		private int i;
		private DispatcherTimer clock;
		private List<Users> userList;
		private CollectionView view;
		private int refreshRate;
		private LoginSystem login = new LoginSystem();
		private List<Users> selectedList;

		public OnlineUsers()
		{
			InitializeComponent();
			refreshRate = 0;
			userList = new List<Users>();

			selectedList = new List<Users>();
			SelectedUsersGrid.ItemsSource = selectedList;
			clock = new DispatcherTimer();
			clock.Tick += new EventHandler(KeepAnEye);
			clock.Interval = new TimeSpan(0, 5, 0);
			clock.Start();
			GetOnline();
		}

		private void GetOnline()
		{
			try
			{
				RefreshRate.Content = $"Refreshed {refreshRate} times";
				userList.Clear();
				OnlineUserGrid.ItemsSource = null;
				OnlineUserGrid.Items.Clear();
				string _Logout;
				string _TimeOnline;

				i++;

				string ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";
				using (var con = new OleDbConnection(ConnectionString))
				{
					con.Open();

					string sql = "SELECT * FROM UsersOnline";

					using (OleDbCommand query = new OleDbCommand(sql, con))
					{
						using (var reader = query.ExecuteReader())
						{
							while (reader.Read())
							{
								int _UserID = Convert.ToInt32(reader["ID"]);
								string _UserName = reader["UserName"].ToString();
								string _Login = Convert.ToDateTime(reader["Login"]).ToString();
								if (reader["Logout"] == DBNull.Value)
								{
									_Logout = "Online";
									_TimeOnline = "Still Online";
								}
								else
								{
									_Logout = Convert.ToDateTime(reader["Logout"]).ToString();
									int minutesOnline = Convert.ToDateTime(reader["Logout"]).Subtract(Convert.ToDateTime(reader["Login"])).Minutes;
									int secondsOnline = Convert.ToDateTime(reader["Logout"]).Subtract(Convert.ToDateTime(reader["Login"])).Seconds;
									int HoursOnline = Convert.ToDateTime(reader["Logout"]).Subtract(Convert.ToDateTime(reader["Login"])).Hours;

									_TimeOnline = $"{HoursOnline} Hours {minutesOnline} minutes  {secondsOnline} seconds";
								}

								userList.Add(new Users(

							_UserID,
							_UserName,
							_Login,
							_Logout,
							_TimeOnline
						));
							}
						}
					}
					Update();
					refreshRate++;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.StackTrace);
			}
		}

		private void KeepAnEye(object sender, EventArgs e)
		{
			try
			{
				RefreshRate.Content = $"Refreshed {refreshRate} times";
				userList.Clear();
				OnlineUserGrid.ItemsSource = null;
				OnlineUserGrid.Items.Clear();
				string _Logout;
				string _TimeOnline;

				i++;

				string ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";
				using (var con = new OleDbConnection(ConnectionString))
				{
					con.Open();

					string sql = "SELECT * FROM UsersOnline";

					using (OleDbCommand query = new OleDbCommand(sql, con))
					{
						using (var reader = query.ExecuteReader())
						{
							while (reader.Read())
							{
								int _UserID = Convert.ToInt32(reader["ID"]);
								string _UserName = reader["UserName"].ToString();
								string _Login = Convert.ToDateTime(reader["Login"]).ToString();
								if (reader["Logout"] == DBNull.Value)
								{
									_Logout = "Online";
									_TimeOnline = "Still Online";
								}
								else
								{
									_Logout = Convert.ToDateTime(reader["Logout"]).ToString();
									int minutesOnline = Convert.ToDateTime(reader["Logout"]).Subtract(Convert.ToDateTime(reader["Login"])).Minutes;
									int secondsOnline = Convert.ToDateTime(reader["Logout"]).Subtract(Convert.ToDateTime(reader["Login"])).Seconds;
									int HoursOnline = Convert.ToDateTime(reader["Logout"]).Subtract(Convert.ToDateTime(reader["Login"])).Hours;

									_TimeOnline = $"{HoursOnline} Hours {minutesOnline} minutes  {secondsOnline} seconds";
								}

								userList.Add(new Users(

							_UserID,
							_UserName,
							_Login,
							_Logout,
							_TimeOnline
						));
							}
						}
					}
					Update();
					refreshRate++;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.StackTrace);
			}
		}

		public void Update()
		{
			userList = userList.OrderByDescending(f => f.UserID).ToList();
			OnlineUserGrid.ItemsSource = userList;

			view = CollectionViewSource.GetDefaultView(OnlineUserGrid.ItemsSource) as CollectionView;
			view.Filter = CustomFilter;
		}

		private bool CustomFilter(object obj)
		{
			Users item = obj as Users;
			if (string.IsNullOrEmpty(FilterTextBox.Text))
			{
				return true;
			}
			else
			{
				return (item.Logout.ToString().IndexOf(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
			}
		}

		private void SecondsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
		}

		private void FilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(OnlineUserGrid.ItemsSource).Refresh();
		}

		private void OnlineUserGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var selectedItem = OnlineUserGrid.SelectedItem as Users;

			selectedList.Add(new Users(
				selectedItem.UserID,
				selectedItem.UserName,
				selectedItem.Login,
				selectedItem.Logout,
				selectedItem.TimeOnline)
				);

			CollectionViewSource.GetDefaultView(SelectedUsersGrid.ItemsSource).Refresh();
		}

		private void SelectedUsersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var selectedItem = SelectedUsersGrid.SelectedItem as Users;
			selectedList.Remove(selectedItem);
			CollectionViewSource.GetDefaultView(SelectedUsersGrid.ItemsSource).Refresh();
		}

		private void MarkOfflineButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (selectedList.Count > 0)
				{
					foreach (var item in selectedList)
					{
						login.UpdateUserLogin(item.UserID);
					}
				}
				selectedList.Clear();
				CollectionViewSource.GetDefaultView(SelectedUsersGrid.ItemsSource).Refresh();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message} || {ex.StackTrace}");
			}
		}
	}

	public class Users
	{
		public Users(int _UserID, string _UserName, string _Login, string _Logout, string _TimeOnline)
		{
			UserID = _UserID;
			UserName = _UserName;
			Login = _Login;
			Logout = _Logout;
			TimeOnline = _TimeOnline;
		}

		public int UserID { get; set; }
		public string UserName { get; set; }

		public string Login { get; set; }

		public string Logout { get; set; }

		public string TimeOnline { get; set; }
	}
}