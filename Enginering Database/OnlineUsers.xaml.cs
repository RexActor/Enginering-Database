using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
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
		private DispatcherTimer Timer;
		private List<Users> userList;
		private CollectionView view;
		private DateTime timeToReach;
		private int refreshRate;
		private LoginSystem login = new LoginSystem();
		private List<Users> selectedList;
		private ErrorSystem err = new ErrorSystem();

		public OnlineUsers()
		{
			InitializeComponent();
			refreshRate = 0;
			userList = new List<Users>();

			selectedList = new List<Users>();
			SelectedUsersGrid.ItemsSource = selectedList;
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
				timeToReach = DateTime.Now.AddMinutes(5);

				userList.Clear();
				OnlineUserGrid.ItemsSource = null;
				OnlineUserGrid.Items.Clear();
				string _Logout;
				string _TimeOnline;

				i++;
				refreshRate++;
				RefreshRate.Content = $"Refreshed {refreshRate} times";

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
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		public void Update()
		{
			try
			{
				userList = userList.OrderByDescending(f => f.UserID).ToList();
				OnlineUserGrid.ItemsSource = userList;

				view = CollectionViewSource.GetDefaultView(OnlineUserGrid.ItemsSource) as CollectionView;
				view.Filter = CustomFilter;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private bool CustomFilter(object obj)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
				return true;
			}
		}

		private void SecondsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
		}

		private void FilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			try
			{
				if (OnlineUserGrid.ItemsSource != null)
				{
					CollectionViewSource.GetDefaultView(OnlineUserGrid.ItemsSource).Refresh();
				}
				else { return; }
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void OnlineUserGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void SelectedUsersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			try
			{
				var selectedItem = SelectedUsersGrid.SelectedItem as Users;
				selectedList.Remove(selectedItem);
				CollectionViewSource.GetDefaultView(SelectedUsersGrid.ItemsSource).Refresh();
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
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
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			GetOnline();
		}

		private void AutoRefreshCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				if (AutoRefreshCheckBox.IsChecked == true)
				{
					clock = new DispatcherTimer();

					Timer = new DispatcherTimer();
					timeToReach = DateTime.Now.AddMinutes(5);
					Timer.Tick += new EventHandler(UpdateTimerClock);
					Timer.Interval = new TimeSpan(0, 0, 1);
					Timer.Start();

					clock.Tick += new EventHandler(KeepAnEye);

					clock.Interval = new TimeSpan(0, 5, 0);

					clock.Start();
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void UpdateTimerClock(object sender, EventArgs e)
		{
			try
			{
				if (AutoRefreshCheckBox.IsChecked == false)
				{
					TimerLabel.Content = "Timer switched Off";
				}
				else
				{
					//DateTime diff = timeToReach - DateTime.Now;
					TimeSpan diff = timeToReach - DateTime.Now;

					TimerLabel.Content = $"Time left till refresh: {diff.Minutes}: {diff.Seconds}";
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace, ex.Source);
			}
		}

		private void ExceptioNBtn_Click(object sender, RoutedEventArgs e)
		{
			throw new InvalidOperationException("Logfile cannot be read-only1");
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