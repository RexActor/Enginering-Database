using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for ConsoleEmulation.xaml
	/// </summary>
	public partial class ConsoleEmulation : Window
	{
		ConsoleContent dc = new ConsoleContent();

		public ConsoleEmulation()
		{
			InitializeComponent();
			DataContext = dc;

			Loaded += ConsoleEmulation_Loaded;

		}

		void ConsoleEmulation_Loaded(object sender, RoutedEventArgs e)
		{
			InputBlock.KeyDown += InputBlock_KeyDown;
			InputBlock.Focus();
		}
		void InputBlock_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				dc.ConsoleInput = InputBlock.Text;
				dc.RunCommand();
				InputBlock.Focus();
				Scroller.ScrollToBottom();
			}
		}

	}


	public class ConsoleContent : INotifyPropertyChanged
	{
		DatabaseClass db = new DatabaseClass();
		string consoleInput = string.Empty;

		ObservableCollection<string> consoleOutput = new ObservableCollection<string>() { " Admin Console for Engineering & Facilities Software \n This console provides easier access to managing application through command-prompt. \n To get list of available commands please type -help\n" };

		public string ConsoleInput
		{
			get
			{
				return consoleInput;
			}
			set
			{
				consoleInput = value;
				OnPropertyChanged("ConsoleInput");
			}
		}


		public ObservableCollection<string> ConsoleOutput
		{
			get
			{
				return consoleOutput;
			}
			set
			{
				consoleOutput = value;
				OnPropertyChanged("ConsoleOutput");
			}
		}

		public void RunCommand()
		{
			string optional = string.Empty;

			//seting up location for application

			string sourceDirectory = Directory.GetCurrentDirectory();
			string destinationDirectory = $"{ sourceDirectory}\\Backup";


			if (consoleInput.Contains("db-open") || consoleInput.Contains("db-backup") || consoleInput.Contains("app-maintenance") || consoleInput.Contains("db-location"))
			{
				if (consoleInput.Contains(" "))
				{
					string phrase = consoleInput;
					string[] words = phrase.Split(' ');
					consoleInput = words[0];
					optional = words[1];
				}
				else
				{
					optional = string.Empty;
				}
			}





			switch (consoleInput)
			{
				#region command - -help
				case "-help":
					consoleOutput.Add("Available comamnds are:" +
						"\n * db-status -- checking status of database " +
						"\n * db-connect -- connecting database" +
						"\n * db-close -- closing database connection" +
						"\n * db-location [database name]-- checking location of database file" +
						"\n * db-backup [database name] -- creates backup for database. Need to provide full database name with extension." +
						"\n * db-open [database name] -- opens datbase file" +
						"\n * app-maintenance [on/off]-- switches maintenance mode for application on/off" +
						"\n * app-admin -- shows administrator of applicaiton" +
						"\n * clear - clears console window");

					break;
				#endregion
				#region command - db-status
				case "db-status":

					consoleOutput.Add("Command not implemented yet");

					break;
				#endregion
				#region command - db-connect
				case "db-connect":
					consoleOutput.Add("Command not implemented yet");
					break;
				#endregion
				#region command - db-close
				case "db-close":
					consoleOutput.Add("Command not implemented yet");
					break;
				#endregion
				#region command - db-location
				case "db-location":
					consoleOutput.Add($"Trying to locate database file --> {optional}");
					break;
				#endregion
				#region command - db-backup
				case "db-backup":

					if (!Directory.Exists(destinationDirectory))
					{
						consoleOutput.Add("\nBackup Directory don't exists.... Creating new ...");
						Thread.Sleep(200);
						try
						{
							Directory.CreateDirectory(destinationDirectory);
							Thread.Sleep(200);
							consoleOutput.Add("\nDirectory created...");
						}
						catch
						{
							consoleOutput.Add("\nSomething went wrong when creating directory... Please check Permissions..");
						}
					}

					string fileName = string.Empty;
					string[] files = Directory.GetFiles(sourceDirectory, "*.accdb");
					bool fileCopied = false;
					if (optional != string.Empty)
					{
						foreach (string f in files)
						{
							fileName = f.Substring(sourceDirectory.Length + 1);
							if (fileName == optional)
							{
								try
								{
									File.Copy(System.IO.Path.Combine(sourceDirectory, fileName), Path.Combine(destinationDirectory, fileName), true);
									consoleOutput.Add($"\nBackup file created for {fileName}");
									fileCopied = true;
									break;
								}
								catch
								{
									consoleOutput.Add("\nSomething went wrong when we were copying files");
									consoleOutput.Add($"{destinationDirectory}");
								}
							}

						}

						if (!fileCopied)
						{
							consoleOutput.Add($"\nCouldn't locate file {optional}. Please check.");
						}

					}
					else
					{

						consoleOutput.Add("\nExisting Databases in folder:");
						foreach (string f in files)
						{

							fileName = f.Substring(sourceDirectory.Length + 1);

							consoleOutput.Add($"{fileName}");

						}
						consoleOutput.Add($"\nTo Create Backup please use command  - db-backup {fileName}");

					}

					break;
				#endregion
				#region command -db-open
				case "db-open":
					consoleOutput.Add($"Trying to open database --> {optional}");
					break;
				#endregion
				#region command - app-maintenance
				case "app-maintenance":
					if (optional == string.Empty)
					{
						consoleOutput.Add($"Command wasn't completed. Please check. Ex: app-maintenance off");
					}
					else
					{
						if (optional.ToLower() != "on" || optional.ToLower() != "off" && optional != string.Empty)
						{
							consoleOutput.Add($"optional command  [ {optional} ]  not recognized. Please check. Available options are - on/off (no case sensitive) Ex: app-maintenance off");
						}
						else
						{

							consoleOutput.Add($"Switching {optional} maintenace mode for application ");
						}
					}

					break;
				#endregion
				#region command - app-admin
				case "app-admin":
					consoleOutput.Add("Command not implemented yet");
					break;
				#endregion
				#region command - clear
				case "clear":
					consoleOutput.Clear();
					break;
				#endregion


				#region default switch function
				default:
					consoleOutput.Add("Command not recognized. Check available commands : -help");

					break;
					#endregion
			}

			ConsoleInput = String.Empty;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}
