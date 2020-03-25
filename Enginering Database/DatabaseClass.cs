﻿using System;
using System.Data;
using System.Data.OleDb;

namespace Engineering_Database
{
	class DatabaseClass
	{
		/*
		 * 
		 * 
		 * Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\engineeringDatabase.accdb;Persist Security Info=True
		 * 
		 */

		readonly UserErrorWindow userErr = new UserErrorWindow();


		readonly OleDbConnection con = new OleDbConnection();


		#region connect DB close DB and DB status DB count lines functions

		public void ConnectDB()
		{


			if (con.State == ConnectionState.Closed)
			{

				try
				{
					if (GetCPU().IndexOf("64") > 0)
					{
						con.ConnectionString = "Provider = Microsoft.ACE.OLEDB.16.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";
					}
					else
					{
						con.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";
					}
					con.Open();

				}
				catch
				{
					userErr.errorMessage = "Ace.OLEDB error";
					userErr.CallWindow();
					userErr.Show();
				}





			}






		}
		public void CloseDB()
		{

			con.Dispose();
			con.Close();


		}

		public string DBStatus()
		{

			if (con.State == System.Data.ConnectionState.Open)
			{

				return "DB connected";
			}
			else if (con.State == System.Data.ConnectionState.Closed)
			{
				return "DB not Connected";
			}

			else

				return "Some kind of error";
		}

		public int DBCountLines()
		{

			string queryString = "SELECT COUNT(*) FROM engineeringDatabaseTable";


			OleDbCommand cmd = new OleDbCommand(queryString, con);

			var data = (int)cmd.ExecuteScalar();
			cmd.Dispose();
			return data;

		}
		#endregion
		#region db query functions --> retreive data from database 3 overloaded functions string (table)/string (table) int(jobNumber)/ simple without any parameters
		public string DBQuery(string table)
		{


			string queryString = $"SELECT * FROM engineeringDatabaseTable";

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			reader.Read();
			var data = reader[table].ToString();
			cmd.Dispose();
			return data;


		}
		public string DBQuery(string table, int jobnumber)
		{

			string queryString = $"SELECT * FROM engineeringDatabaseTable WHERE JobNumber in (@param1)";


			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.Add(new OleDbParameter("@param1", jobnumber));

			OleDbDataReader reader = cmd.ExecuteReader();
			if (reader != null)
			{
				reader.Read();
			}

			var data = reader[table].ToString();
			cmd.Dispose();


			reader.Close();
			return data;

		}
		public OleDbCommand DBQuery()
		{


			string queryString = "SELECT * FROM engineeringDatabaseTable WHERE Completed =false";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			return cmd;

		}

		public OleDbCommand DBQueryforJobList(string filter)
		{

			string queryString;
			if (filter == "Outstanding")
			{
				queryString = "SELECT * FROM engineeringDatabaseTable WHERE Completed=false ORDER BY JobNumber DESC";
			}
			else if (filter == "Resolved")
			{
				queryString = "SELECT * FROM engineeringDatabaseTable WHERE Completed=true ORDER BY JobNumber DESC";
			}

			else
			{
				queryString = "SELECT * FROM engineeringDatabaseTable ORDER BY JobNumber DESC";

			}
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			return cmd;

		}


		public OleDbCommand DBQueryforUsers(string filter, string reporter)
		{

			string queryString;
			if (filter == "Outstanding")
			{
				queryString = "SELECT * FROM engineeringDatabaseTable WHERE Action='Action required' AND ReportedUsername='" + reporter + "'";
			}
			else if (filter == "Resolved")
			{
				queryString = "SELECT * FROM engineeringDatabaseTable WHERE Completed=true AND ReportedUsername='" + reporter + "'";
			}
			else
			{
				queryString = "SELECT * FROM engineeringDatabaseTable";

			}

			OleDbCommand cmd = new OleDbCommand(queryString, con);

			return cmd;

		}



		public OleDbCommand DBQueryForAllLines()
		{

			string queryString = "SELECT * FROM engineeringDatabaseTable";

			OleDbCommand cmd = new OleDbCommand(queryString, con);

			return cmd;

		}


		public OleDbCommand DBQueryForViewDatabase(int jobNumber)
		{
			string queryString = $"SELECT * FROM engineeringDatabaseTable WHERE JobNumber ={jobNumber}";

			OleDbCommand cmd = new OleDbCommand(queryString, con);

			return cmd;

		}




		public int DBQueryLastJobNumber(string table)
		{

			bool LastJobNumberIsNumber;
			int number;
			int LastJobNumber;
			string queryString = "SELECT TOP 1  *  FROM engineeringDatabaseTable ORDER BY JobNumber DESC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();

			if (reader != null)
			{
				if (reader.HasRows)
				{
					reader.Read();
					cmd.Dispose();

				}
				else
				{
					cmd.Dispose();
					return 0;
				}

			}

			var data = reader[table].ToString();
			//converts data into integer. If successfull returns value as int, else returns 0 (there will not be job number as 0, so can perform check. 
			//if job number was returned as 0, then there wasn't number in database recorded. 

			LastJobNumberIsNumber = Int32.TryParse(data, out number);

			if (LastJobNumberIsNumber)
			{

				LastJobNumber = number;
				return LastJobNumber;
			}
			else
			{
				cmd.Dispose();

				return 0;
			}


		}
		#endregion

		#region updateDatabase with values 3 overloaded functions - bool/string/int
		public void DBQueryInsertData(string table, int jobnumber, bool value)
		{

			string queryString = "UPDATE engineeringDatabaseTable SET " + table + " = @value WHERE JobNumber = @jobnumber";


			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);



			cmd.ExecuteNonQuery();
			cmd.Dispose();

		}
		public void DBQueryInsertData(string table, int jobnumber, string value)
		{

			string queryString = "UPDATE engineeringDatabaseTable SET " + table + " = @value WHERE JobNumber = @jobnumber";


			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);



			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void DBQueryInsertData(string table, int jobnumber, int value)
		{


			string queryString = "UPDATE engineeringDatabaseTable SET " + table + " = @value WHERE JobNumber = @jobnumber";


			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);



			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void DBQueryInsertData(int jobnumber, string table, string value)
		{



			string queryString = "UPDATE engineeringDatabaseTable SET [" + table + "] = @value WHERE [JobNumber] = @jobnumber";

			OleDbCommand cmd = new OleDbCommand(queryString, con);




			cmd.Parameters.AddWithValue("@value", value == null ? DBNull.Value : (object)value);
			cmd.Parameters.AddWithValue("@jobnumber", jobnumber);




			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}




		public void DBQueryUpdateGlobalSettings(string table, string value)
		{


			string queryString = "Update GlobalSettings SET [" + table + "] = @value  ";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}


		public string DBQueryForGlobalSettings(string table)
		{


			string queryString = $"SELECT * FROM GlobalSettings";


			OleDbCommand cmd = new OleDbCommand(queryString, con);


			OleDbDataReader reader = cmd.ExecuteReader();
			reader.Read();
			var data = reader[table].ToString();
			reader.Close();

			cmd.Dispose();
			return data;

		}





		#endregion

		#region insert values into database
		public void InsertDataIntoDatabase(int jobNumber, string repdate, string repTime, string ReportedUsername, string assetNumber, string faultyArea, string Building, string IssueCode, string Priority, string type, string detailedDesc, string DueDate, string Area, string ReporterEmail)
		{

			string queryString = "INSERT INTO engineeringDatabaseTable (JobNumber,ReportedDate,ReportedTime,ReportedUsername,AssetNumber,FaultyArea,Building,IssueCode,Priority,Type,DetailedDescription,DueDate,Area,ReporterEmail) Values(@JobNumber,@ReportedDate,@ReportedTime,@ReportedUsername,@AssetNumber,@FaultyArea,@Building,@IssueCode,@Priority,@Type,@DetailedDescription,@DueDate,@Area,@ReporterEmail)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@JobNumber", jobNumber);
			cmd.Parameters.AddWithValue("@ReportedDate", repdate);
			cmd.Parameters.AddWithValue("@ReportedTime", repTime);
			cmd.Parameters.AddWithValue("@ReportedUsername", ReportedUsername);
			cmd.Parameters.AddWithValue("@AssetNumber", assetNumber);
			cmd.Parameters.AddWithValue("@FaultyArea", faultyArea);
			cmd.Parameters.AddWithValue("@Building", Building);
			cmd.Parameters.AddWithValue("@IssueCode", IssueCode);
			cmd.Parameters.AddWithValue("@Priority", Priority);
			cmd.Parameters.AddWithValue("@Type", type);
			cmd.Parameters.AddWithValue("@DetailedDescription", detailedDesc);

			cmd.Parameters.AddWithValue("@DueDate", DueDate);
			cmd.Parameters.AddWithValue("@Area", Area);
			cmd.Parameters.AddWithValue("@ReporterEmail", ReporterEmail);



			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}



		#endregion

		public string GetCPU()
		{


			string cpu = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");

			return cpu;

		}



		#region getting information for combobox setups

		public OleDbDataReader SetUpComboBox(string table)
		{


			string queryString = $"SELECT * FROM {table}";


			OleDbCommand cmd = new OleDbCommand(queryString, con);


			OleDbDataReader reader = cmd.ExecuteReader();




			cmd.Dispose();

			return reader;

		}

		public OleDbDataReader SetUpComboBox(string table, string text)
		{


			string queryString = $"SELECT ID FROM {table} WHERE parrent = {text}";


			OleDbCommand cmd = new OleDbCommand(queryString, con);


			OleDbDataReader reader = cmd.ExecuteReader();




			cmd.Dispose();

			return reader;

		}

		public OleDbDataReader SetUpComboBoxBasedonParrent(string table, string UID)
		{
			
				//string queryString = $"SELECT * FROM {table} WHERE parrent = '" + UID + "'";
				string queryString = $"SELECT * FROM {table} WHERE parrent = '"+ UID+"'";

				OleDbCommand cmd = new OleDbCommand(queryString, con);


				OleDbDataReader reader = cmd.ExecuteReader();




				cmd.Dispose();


		
			return reader;
		}
		public OleDbDataReader SetUpComboBoxBasedonUID(string table, int UID, int GUID)
		{


			string queryString = $"SELECT * FROM {table} WHERE UID = {UID} AND GUID = {GUID}";


			OleDbCommand cmd = new OleDbCommand(queryString, con);


			OleDbDataReader reader = cmd.ExecuteReader();




			cmd.Dispose();

			return reader;

		}


		#endregion



		public void InsertIssueIntoDatabase(string table,string description)
		{

			string queryString = $"INSERT INTO {table} (description) Values(@description)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@JobNumber", description);
			
			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void InsertIssueIntoDatabase(string table, string description,string parrent )
		{

			string queryString = $"INSERT INTO {table} (description, parrent) Values(@description, @parrent)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@description", description);
			cmd.Parameters.AddWithValue("@parrent", parrent);

			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void InsertIssueIntoDatabase(string table, string description, string parrent,string area)
		{

			string queryString = $"INSERT INTO {table} (description, parrent, area) Values(@description, @parrent, @area)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.AddWithValue("@description", description);
			cmd.Parameters.AddWithValue("@parrent", parrent);
			cmd.Parameters.AddWithValue("@area", area);

			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void DeleteIssueFromDatabase(string table, string description, string parrent)
		{

			string queryString = $"DELETE FROM  {table} Where description='{description}' and parrent='{parrent}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@description", description);
			//cmd.Parameters.AddWithValue("@parrent", parrent);

			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void DeleteIssueFromDatabase(string table, string description)
		{

			string queryString = $"DELETE FROM  {table} Where description='{description}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@description", description);
			//cmd.Parameters.AddWithValue("@parrent", parrent);

			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void DeleteParrentFromDatabase(string table, string parrent)
		{

			string queryString = $"DELETE FROM  {table} Where parrent='{parrent}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@description", description);
			//cmd.Parameters.AddWithValue("@parrent", parrent);

			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}
		public void DeleteAreaFromDatabase(string table, string area)
		{

			string queryString = $"DELETE FROM  {table} Where area='{area}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@description", description);
			//cmd.Parameters.AddWithValue("@parrent", parrent);

			cmd.ExecuteNonQuery();
			cmd.Dispose();


		}


	}
}
