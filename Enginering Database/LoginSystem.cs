using System;
using System.Data.OleDb;
using System.Windows;

namespace Engineering_Database
{
	internal class LoginSystem
	{
		private ErrorSystem err = new ErrorSystem();

		public int CreateUserLogin(string login)
		{
			DateTime dateTimeInsert;
			try
			{
				int id = 0;
				string ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";
				using (var con = new OleDbConnection(ConnectionString))
				{
					con.Open();

					string sql = @"INSERT INTO UsersOnline (UserName,Login) Values
						(@prm_UserName,@prm_Login)";

					using (OleDbCommand query = new OleDbCommand(sql, con))
					{
						dateTimeInsert = DateTime.Now;

						query.Parameters.AddWithValue("@prm_UserName", login);
						query.Parameters.AddWithValue("@prm_Login", dateTimeInsert.ToString());

						query.ExecuteNonQuery();
					}

					string sql2 = @"SELECT ID from UsersOnline WHERE Login = @prm_Login AND UserName= @prm_UserName";

					using (OleDbCommand query = new OleDbCommand(sql2, con))
					{
						query.Parameters.AddWithValue("@prm_Login", dateTimeInsert.ToString());
						query.Parameters.AddWithValue("@prm_UserName", login);
						using (var reader = query.ExecuteReader())
						{
							if (reader.Read())
							{
								id = Convert.ToInt32(reader["ID"]);
							}
							else
							{
								id = -1;
							}
						}
					}
				}
				return id;
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
				return -1;
			}
		}

		public int OnlineUserCount()
		{
			try
			{
				string ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";
				using (var con = new OleDbConnection(ConnectionString))
				{
					con.Open();

					string sql = @"SELECT  COUNT(*) from UsersOnline WHERE Logout IS NULL";

					using (OleDbCommand query = new OleDbCommand(sql, con))
					{
						var data = (int)query.ExecuteScalar();

						return data;
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
				return -1;
			}
		}

		public void UpdateUserLogin(int id)
		{
			DateTime dateTimeInsert;
			try
			{
				string ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";
				using (var con = new OleDbConnection(ConnectionString))
				{
					con.Open();

					string sql = "UPDATE UsersOnline SET Logout = @prm_Logout WHERE ID = @prm_ID";

					using (OleDbCommand query = new OleDbCommand(sql, con))
					{
						dateTimeInsert = DateTime.Now;

						query.Parameters.AddWithValue("@prm_Logout", dateTimeInsert.ToString());
						query.Parameters.AddWithValue("@prm_ID", id);
						query.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				err.RecordError(ex.Message, ex.StackTrace);
			}
		}
	}
}