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

				case "db-status":

					consoleOutput.Add("Command not implemented yet");

					break;

				case "db-connect":
					consoleOutput.Add("Command not implemented yet");
					break;

				case "db-close":
					consoleOutput.Add("Command not implemented yet");
					break;

				case "db-location":
					consoleOutput.Add($"Trying to locate database file --> {optional}");
					break;

				case "db-backup":

					string sourceDirectory = Directory.GetCurrentDirectory();
					string destinationDirectory = $"{ sourceDirectory}\\Backup";

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
							//consoleOutput.Add($"\n {f}");
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
						//	consoleOutput.Add($"Trying to create backup for database file --> {optional}. Current directory {sourceDirectory}// Target DIrectory {destinationDirectory}");

					}
					else
					{

						consoleOutput.Add("\nExisting Databases in folder:");
						foreach (string f in files)
						{
							//consoleOutput.Add($"\n {f}");
							fileName = f.Substring(sourceDirectory.Length + 1);

							consoleOutput.Add($"{fileName}");


						}
						consoleOutput.Add($"\nTo Create Backup please use command  - db-backup {fileName}");

						//consoleOutput.Add($"You didn't provide filename. Ex: db-backup databasename");
					}

					break;

				case "db-open":
					consoleOutput.Add($"Trying to open database --> {optional}");
					break;

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

				case "app-admin":
					consoleOutput.Add("Command not implemented yet");
					break;
				case "clear":
					consoleOutput.Clear();
					break;

				default:
					consoleOutput.Add("Command not recognized. Check available commands : -help");

					break;
			}


			//if (consoleInput == "Status")
			//{
			//	ConsoleOutput.Add(db.DBStatus());
			//}
			//ConsoleOutput.Add(ConsoleInput);


			ConsoleInput = String.Empty;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}
