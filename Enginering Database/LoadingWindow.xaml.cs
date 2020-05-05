using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Engineering_Database
{
	/// <summary>
	/// Interaction logic for LoadingWindow.xaml
	/// </summary>
	public partial class LoadingWindow : Window
	{




		public string contentLabelText = String.Empty;
		public double loadingProgress = 0;
		public LoadingWindow()
		{
			InitializeComponent();

		}
		public void StartLoadingScreen()
		{
			
		
		}

	
		public void progressChanged(object sender, ProgressChangedEventArgs e)
		{

			ProgressBar.Value = e.ProgressPercentage;
			contentLabel.Content = contentLabelText;
		
		}
		public void JobFinished(object sender, RunWorkerCompletedEventArgs e)
		{


			this.Close();
		}
	}
}
