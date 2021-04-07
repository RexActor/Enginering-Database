using System;
using System.Data;
using System.Data.OleDb;

namespace Engineering_Database
{
	internal class DatabaseClass
	{
		/*
		 *
		 *
		 * Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\engineeringDatabase.accdb;Persist Security Info=True
		 *
		 */

		//readonly UserErrorWindow userErr = new UserErrorWindow();

		private readonly string ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = engineeringDatabase.accdb; Jet OLEDB:Database Password = test";

		private readonly OleDbConnection con = new OleDbConnection();

		#region connect DB close DB and DB status DB count lines functions

		public void ConnectDB()
		{
			if (con.State == ConnectionState.Closed)
			{
				OleDbConnection.ReleaseObjectPool();
				con.ConnectionString = ConnectionString;
				con.Open();
			}
			else
			{
				OleDbConnection.ReleaseObjectPool();
			}
		}

		public void ConnectDB(string databaseName)
		{
			string ConnectionString = $"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {databaseName}.accdb; Jet OLEDB:Database Password = test";

			if (con.State == ConnectionState.Closed)
			{
				OleDbConnection.ReleaseObjectPool();
				con.ConnectionString = ConnectionString;
				con.Open();
			}
			else
			{
				OleDbConnection.ReleaseObjectPool();
			}
		}

		public void CloseDB()
		{
			con.Dispose();
			con.Close();
		}

		public string DBStatus()
		{
			if (con != null)
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
				{
					return "Some kind of error";
				}
			}
			else
			{
				return "DB not Connected";
			}
		}

		public int DBCountLines()
		{
			string queryString = "SELECT COUNT(*) FROM engineeringDatabaseTable";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			var data = (int)cmd.ExecuteScalar();
			cmd.Dispose();
			return data;
		}

		#endregion connect DB close DB and DB status DB count lines functions

		#region db query functions --> retreive data from database 3 overloaded functions string (table)/string (table) int(jobNumber)/ simple without any parameters

		public string DBQuery(string table)
		{
			string queryString = $"SELECT * FROM engineeringDatabaseTable ORDER BY JobNumber ASC";
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

		public OleDbDataReader DBQueryForExistingAssets(string table)
		{
			string queryString = $"SELECT * FROM {table} ORDER BY JobNumber ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbCommand DBQueryForAllLines()
		{
			string queryString = "SELECT * FROM engineeringDatabaseTable";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			return cmd;
		}

		public OleDbDataReader DBQueryForOldEntries(string table, string completed)
		{
			string queryString = $"SELECT * FROM {table} WHERE Completed ={completed} ORDER BY DueDate ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader DBQueryForAssets(string table)
		{
			string queryString = $"SELECT * FROM {table} ORDER BY ID ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader DBQueryForAssetsWithFilter(string table, string fieldfilter, string fieldfiltervalue)
		{
			string queryString = $"SELECT * FROM {table} where {fieldfilter}='{fieldfiltervalue}' ORDER BY JobNumber ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader DBQueryForAssetsWithFilterOrderByID(string table, string fieldfilter, string fieldfiltervalue)
		{
			string queryString = "SELECT * FROM " + table + "  where " + fieldfilter + " ALike  '" + fieldfiltervalue + "%'";
			//string queryString = "SELECT * FROM " + table + "  where " + fieldfilter + " Like  '* 5 *'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
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
			LastJobNumberIsNumber = Int32.TryParse(data, out int number);
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

		#endregion db query functions --> retreive data from database 3 overloaded functions string (table)/string (table) int(jobNumber)/ simple without any parameters

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

		#endregion updateDatabase with values 3 overloaded functions - bool/string/int

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

		#endregion insert values into database

		//public string GetCPU()
		//{
		//	string cpu = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");

		//	return cpu;

		//}

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
			string queryString = $"SELECT * FROM {table} WHERE parrent = '" + UID + "'";
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

		#endregion getting information for combobox setups

		#region issuecode add/remove from database

		public void InsertIssueIntoDatabase(string table, string description)
		{
			string queryString = $"INSERT INTO {table} (description) Values(@description)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@JobNumber", description);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void InsertIssueIntoDatabase(string table, string description, string parrent)
		{
			string queryString = $"INSERT INTO {table} (description, parrent) Values(@description, @parrent)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@description", description);
			cmd.Parameters.AddWithValue("@parrent", parrent);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void InsertIssueIntoDatabase(string table, string description, string parrent, string area)
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
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void DeleteIssueFromDatabase(string table, string description)
		{
			string queryString = $"DELETE FROM  {table} Where description='{description}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void DeleteParrentFromDatabase(string table, string parrent)
		{
			string queryString = $"DELETE FROM  {table} Where parrent='{parrent}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void DeleteAreaFromDatabase(string table, string area)
		{
			string queryString = $"DELETE FROM  {table} Where area='{area}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		#endregion issuecode add/remove from database

		#region inserting/updating asset list into database

		//insert Asset into Database
		public void InsertAssetIntoDatabase(string table, string description, string make, string model, string assetNumber, string serialNumber, string dateOfManufacture, DateTime DateOfInstallation, string IssueLevel, string installedOn, bool decomissioned, bool onSite)
		{
			string queryString = $"INSERT INTO {table} (Description,Make,Model,AssetNumber,SerialNumber,DateOfManufacture,DateOfInstallation,IssueLevel,InstalledOn,Decomissioned,OnSite) Values(@Description,@Make,@Model,@AssetNumber,@SerialNumber,@DateOfManufacture,@DateOfInstallation,@IssueLevel,@InstalledOn,@Decomissioned,@OnSite)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@Description", description);
			cmd.Parameters.AddWithValue("@Make", make);
			cmd.Parameters.AddWithValue("@Model", model);
			cmd.Parameters.AddWithValue("@AssetNumber", assetNumber);
			cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
			cmd.Parameters.AddWithValue("@DateOfManufacture", dateOfManufacture);
			cmd.Parameters.AddWithValue("@DateOfInstallation", DateOfInstallation);
			cmd.Parameters.AddWithValue("@IssueLevel", IssueLevel);
			cmd.Parameters.AddWithValue("@InstalledOn", installedOn);
			cmd.Parameters.AddWithValue("@Decomissioned", decomissioned);
			cmd.Parameters.AddWithValue("@OnSite", onSite);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void UpdateAsset(string table, string field, int assetID, string value)
		{
			string queryString = "UPDATE " + table + " SET " + field + " = @value WHERE ID = @assetID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@assetID", assetID);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void UpdateAsset(string table, string field, int assetID, DateTime value)
		{
			string queryString = "UPDATE " + table + " SET " + field + " = @value WHERE ID = @assetID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@assetID", assetID);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		#endregion inserting/updating asset list into database

		/// <summary>
		/// Function to insert into database meter Readings. Values - Date Time and float number
		/// </summary>
		/// <param name="table"></param>
		/// <param name="insertDate"></param>
		/// <param name="meterReading"></param>
		public void InsertReadingIntoDatabase(string table, DateTime insertDate, double meterReading, double consumption)
		{
			string queryString = $"INSERT INTO {table} (InsertDate,MeterReading,MeterCalculation) Values(@InsertDate,@MeterReading,@MeterCalculation)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@InsertDate", insertDate);
			cmd.Parameters.AddWithValue("@MeterReading", meterReading);
			cmd.Parameters.AddWithValue("@MeterCalculation", consumption);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public OleDbDataReader GetMeterReadingData(string table)
		{
			string queryString = $"SELECT * FROM {table} ORDER BY InsertDate ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetMeterReadingData(string table, int year)
		{
			string queryString = $"SELECT * FROM {table} WHERE ReadingYear = {year} ORDER BY InsertDate ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetMeterReadingData(string table, string month, int year)
		{
			string queryString = $"SELECT * FROM {table} WHERE ReadingMonth='{month}' AND ReadingYear ={year} ORDER BY InsertDate ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetMeterReadingData(string table, string field, string month, string field2, string value2)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}='{month}' AND {field2}='{value2}'  ORDER BY InsertDate ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetMeterReadingData(string table, string field, double value)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}='" + value + "'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetMeterReadingDataSelectedYear(string table, string field, int value)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}={value}";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetMeterReadingDataForField(string table, string field, DateTime date)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}='{date}'  ORDER BY InsertDate ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		/// <summary>
		/// gets data  for specific date in specific field in database table
		/// </summary>
		/// <param name="table"></param>
		/// <param name="field"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public string GetMeterReadingData(string table, string field, DateTime date)
		{
			string queryString = $"SELECT * FROM {table} WHERE InsertDate = '{date.ToOADate()}'";

			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			reader.Read();
			var data = reader[field].ToString();
			reader.Close();
			cmd.Dispose();
			return data;
		}

		public string GetMeterReadingLastReading(string table, string field)
		{
			string queryString = $"SELECT TOP 1 * FROM {table} ORDER BY InsertDate DESC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			reader.Read();
			var data = reader[field].ToString();
			reader.Close();
			cmd.Dispose();
			return data;
		}

		/// <summary>
		/// Counting lines in MeterReading table
		/// </summary>
		/// <returns></returns>
		public int DBMeterReadingCountLines()
		{
			string queryString = "SELECT COUNT(*) FROM MeterReadings";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			var data = (int)cmd.ExecuteScalar();
			cmd.Dispose();
			return data;
		}

		public void MeterReadingsUpdate(string table, string fieldToUpdate, int ID, double value)
		{
			string queryString = $"UPDATE {table} SET " + fieldToUpdate + " = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@ID", ID);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		/// <summary>
		/// Counts lines in database MeterReadings table. Months are recorded as Jan, Feb, Apr etc.
		/// </summary>
		/// <param name="month"></param>
		/// <returns></returns>
		public int DBMeterReadingCountLines(string month, int year)
		{
			string queryString = $"SELECT COUNT(*) FROM MeterReadings Where ReadingMonth='{month}' AND ReadingYear ={year}";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			var data = (int)cmd.ExecuteScalar();
			cmd.Dispose();
			return data;
		}

		public int CountLinesInDatabaseTable(string table)
		{
			string queryString = $"SELECT COUNT(*) FROM {table}";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			var data = (int)cmd.ExecuteScalar();
			cmd.Dispose();
			return data;
		}

		public void UploadFile(string table, byte[] file, string lineOfMaintenance, DateTime dateOfMaintenance, DateTime uploadDate, string EngineerComment, string linkedAsset)
		{
			string queryString = "INSERT INTO " + table + " (UploadedFile,LineOfMaintenance,DateOfMaintenance,UploadDate,EngineerComment,LinkedAsset) " + " Values(@UploadedFile,@LineOfMaintenance,@DateOfMaintenance,@UploadDate,@EngineerComment,@linkedAsset)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@UploadDate", date);
			//cmd.Parameters.AddWithValue("@UploadedFile", file);

			cmd.Parameters.Add("@UploadedFile", OleDbType.Binary).Value = (object)file;
			cmd.Parameters.Add("LineOfMaintenance", OleDbType.VarWChar).Value = lineOfMaintenance;
			cmd.Parameters.Add("DateOfMaintenance", OleDbType.Date).Value = dateOfMaintenance;
			cmd.Parameters.Add("UploadDate", OleDbType.Date).Value = uploadDate;
			cmd.Parameters.Add("EngineerComment", OleDbType.VarWChar).Value = EngineerComment;
			cmd.Parameters.Add("LinkedAsset", OleDbType.VarWChar).Value = linkedAsset;
			cmd.ExecuteNonQuery();
			//cmd.Dispose();
		}

		public void UploadTemplateFile(string table, byte[] file, string templateName)
		{
			string queryString = "INSERT INTO " + table + " (TemplateFile,TemplateName) " + " Values(@TemplateFile,@TemplateName)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@UploadDate", date);
			//cmd.Parameters.AddWithValue("@UploadedFile", file);

			cmd.Parameters.Add("@TemplateFile", OleDbType.Binary).Value = (object)file;
			cmd.Parameters.Add("@TemplateName", OleDbType.VarWChar).Value = templateName;

			cmd.ExecuteNonQuery();
			//cmd.Dispose();
		}

		public OleDbDataReader GetPDFFileFromDatabase(string table, string field, int id)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}={id}";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetPDFFileFromDatabase(string table, string field, string value)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}='{value}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetAllPDFIds(string table, int lookingYear, string lookingMonth, string lineOfMaintenance)
		{
			string queryString = $"SELECT * FROM {table} WHERE YearOfMaintenance ={lookingYear} AND MonthOfMaintenance = '{lookingMonth}' AND LineOfMaintenance='{lineOfMaintenance}' ORDER BY DateOfMaintenance ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetAllPDFIds(string table)
		{
			string queryString = $"SELECT * FROM {table}";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetAllPDFIds(string table, int year)
		{
			string queryString = $"SELECT * FROM {table} WHERE YearOfMaintenance={year} ORDER BY DateOfMaintenance ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetAllPDFIds(string table, string month)
		{
			string queryString = $"SELECT * FROM {table} WHERE MonthOfMaintenance='{month}' ORDER BY DateOfMaintenance ASC";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public void DeleteTemplate(string table, string templateName)
		{
			string queryString = $"DELETE FROM  {table} Where TemplateName='{templateName}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		#region Inventory View Database functions

		public void AddMeasure(string table, string measureType)
		{
			string queryString = "INSERT INTO " + table + " (MeasureType) " + " Values(@MeasureType)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.Add("@MeasureType", OleDbType.VarWChar).Value = measureType;

			cmd.ExecuteNonQuery();
		}

		public void AddCategory(string table, string value)
		{
			string queryString = "INSERT INTO " + table + " (Category) " + " Values(@Category)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.Add("@Category", OleDbType.VarWChar).Value = value;

			cmd.ExecuteNonQuery();
		}

		public void UploadInventoryProduct(string table, byte[] file, string productName, string measureType, string category)
		{
			string queryString = "INSERT INTO " + table + " (ProductImage,ProductName,MeasureType,ProductCategory) " + " Values(@ProductImage,@ProductName,@MeasureType,@ProductCategory)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.Add("@ProductImage", OleDbType.Binary).Value = (object)file;
			cmd.Parameters.Add("@ProductName", OleDbType.VarWChar).Value = productName;
			cmd.Parameters.Add("@MeasureType", OleDbType.VarWChar).Value = measureType;
			cmd.Parameters.Add("@ProductCategory", OleDbType.VarWChar).Value = category;

			cmd.ExecuteNonQuery();
			//cmd.Dispose();
		}

		public void UploadInventoryProductWithoutPic(string table, string productName, string measureType, string category)
		{
			string queryString = "INSERT INTO " + table + " (ProductName,MeasureType,ProductCategory) " + " Values(@ProductName,@MeasureType,@ProductCategory)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.Add("@ProductName", OleDbType.VarWChar).Value = productName;
			cmd.Parameters.Add("@MeasureType", OleDbType.VarWChar).Value = measureType;
			cmd.Parameters.Add("@ProductCategory", OleDbType.VarWChar).Value = category;

			cmd.ExecuteNonQuery();
			//cmd.Dispose();
		}

		public void AddProduct(string table, string product, int qty, string measureType, string productCategory)
		{
			string queryString = "INSERT INTO " + table + " (Product,Qty,MeasureType,ProductCategory) " + " Values(@Product,@Qty,MeasureType,ProductCategory)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			cmd.Parameters.Add("@Product", OleDbType.VarWChar).Value = product;
			cmd.Parameters.Add("@Qty", OleDbType.Integer).Value = qty;
			cmd.Parameters.Add("@MeasureType", OleDbType.VarWChar).Value = measureType;
			cmd.Parameters.Add("@ProductCategory", OleDbType.VarWChar).Value = productCategory;
			cmd.ExecuteNonQuery();
			//cmd.Dispose();
		}

		public OleDbDataReader GetInventoryProduct(string table, string field1, string field1Value)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field1}='{field1Value}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetInventoryProduct2Fields(string table, string field1, string field1Value, string field2, string field2Value)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field1}='{field1Value}' And {field2}='{field2Value}' ";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public void UpdateInventoryView(string table, string fieldToUpdate, int ID, int value)
		{
			string queryString = $"UPDATE {table} SET " + fieldToUpdate + " = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@ID", ID);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void UpdateProductPicture(string table, string fieldToUpdate, byte[] file, int ID)
		{
			string queryString = $"UPDATE {table} SET " + fieldToUpdate + " = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.Add("@ProductImage", OleDbType.Binary).Value = (object)file;
			cmd.Parameters.AddWithValue("@ID", ID);
			cmd.ExecuteNonQuery();

			cmd.ExecuteNonQuery();
			//cmd.Dispose();
		}

		public void UpdateInventoryView(string table, string fieldToUpdate, int ID, string value)
		{
			string queryString = $"UPDATE {table} SET " + fieldToUpdate + " = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@ID", ID);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		#endregion Inventory View Database functions

		public void UpdateStatutoryCompliance(string table, string field, int id, bool value)
		{
			string queryString = $"UPDATE {table} SET " + field + " = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@ID", id);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void UpdateStatutoryCompliance(string table, string field, int id, DateTime value)
		{
			string queryString = $"UPDATE {table} SET " + field + " = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@ID", id);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void UpdateStatutoryCompliance(string table, string field, int id, string value)
		{
			string queryString = $"UPDATE {table} SET {field} = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@ID", id);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public void UpdateStatutoryCompliance(string table, string field, int id, int value)
		{
			string queryString = $"UPDATE {table} SET {field} = @value WHERE ID = @ID";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			cmd.Parameters.AddWithValue("@value", value);
			cmd.Parameters.AddWithValue("@ID", id);
			cmd.ExecuteNonQuery();
			cmd.Dispose();
		}

		public OleDbDataReader GetStatutoryItem(string table, int id)
		{
			string queryString = $"SELECT * FROM {table} WHERE ID={id} ";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public void AddStatutoryItem(string table, string itemDescription, string manufacturerCompany, DateTime dateReportIssued, DateTime renewDate, string serialNumber, string weeklyMonthly, string DaysTillInspection, string companyInsurer, string groupvalue, string monthlyWeeklyRange, string booked)
		{
			string queryString = "INSERT INTO " + table + " (ManufacturerCompany,EquipmentDescription,CompanyInsurer,SerialNumber,MonthlyWeekly,DaysTillInspection,DateReportIssued,RenewDate,GroupName,MonthlyWeeklyRange,Booked) " + " Values(@ManufacturerCompany,@EquipmentDescription,@CompanyInsurer,@SerialNumber,@MonthlyWeekly,@DaysTillInspection,@DateReportIssued,@RenewDate,@GroupName,@MonthlyWeeklyRange,@Booked)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@UploadDate", date);
			//cmd.Parameters.AddWithValue("@UploadedFile", file);

			cmd.Parameters.Add("@ManufacturerCompany", OleDbType.VarWChar).Value = manufacturerCompany;
			cmd.Parameters.Add("@EquipmentDescription", OleDbType.VarWChar).Value = itemDescription;
			cmd.Parameters.Add("@CompanyInsurer", OleDbType.VarWChar).Value = companyInsurer;
			cmd.Parameters.Add("@SerialNumber", OleDbType.VarWChar).Value = serialNumber;
			cmd.Parameters.Add("@MonthlyWeekly", OleDbType.VarWChar).Value = weeklyMonthly;
			cmd.Parameters.Add("@DaysTillInspection", OleDbType.VarChar).Value = DaysTillInspection;
			cmd.Parameters.Add("@DateReportIssued", OleDbType.Date).Value = dateReportIssued;
			cmd.Parameters.Add("@RenewDate", OleDbType.Date).Value = renewDate;
			cmd.Parameters.Add("@GroupName", OleDbType.VarWChar).Value = groupvalue;
			cmd.Parameters.Add("@MonthlyWeeklyRange", OleDbType.VarWChar).Value = monthlyWeeklyRange;
			cmd.Parameters.Add("@Booked", OleDbType.VarChar).Value = booked;
			//cmd.Parameters.Add("@Booked", OleDbType.VarChar).Value = Booked;

			//cmd.Parameters.AddWithValue("@GroupName",groupvalue);

			//cmd.Dispose();l
			cmd.ExecuteNonQuery();
		}

		public OleDbCommand DbAdapter(string db)
		{
			string queryString = $"SELECT * FROM {db}";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			return cmd;
		}

		public void InsertBlackOutDate(string table, DateTime date)
		{
			string queryString = "INSERT INTO " + table + " (BlackOutDate) " + " Values(@BlackOutDate)";
			OleDbCommand cmd = new OleDbCommand(queryString, con);

			//cmd.Parameters.AddWithValue("@UploadDate", date);
			//cmd.Parameters.AddWithValue("@UploadedFile", file);

			cmd.Parameters.Add("@BlackOutDate", OleDbType.Date).Value = date;

			//cmd.Parameters.AddWithValue("@GroupName",groupvalue);

			//cmd.Dispose();l
			cmd.ExecuteNonQuery();
		}

		public OleDbDataReader GetBlackOutDate(string table, string field, DateTime date)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}={date.ToOADate()}";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}

		public OleDbDataReader GetHygeneScheduler(string table, string field, string value)
		{
			string queryString = $"SELECT * FROM {table} WHERE {field}='{value}'";
			OleDbCommand cmd = new OleDbCommand(queryString, con);
			OleDbDataReader reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}
	}
}