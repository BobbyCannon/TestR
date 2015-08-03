#region References

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TestR.Extensions;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Interaction logic for ProcessWindow.xaml
	/// </summary>
	public partial class ProcessWindow : Window
	{
		#region Constructors

		public ProcessWindow()
		{
			InitializeComponent();
			Processes = new ObservableCollection<Process>();
			DataContext = this;
		}

		#endregion

		#region Properties

		public ObservableCollection<Process> Processes { get; set; }
		public Process SelectedProcess { get; private set; }

		#endregion

		#region Methods

		private void ProcessList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			SelectedProcess = (Process) ProcessList.SelectedItem;
			DialogResult = true;
		}

		private void Select(object sender, RoutedEventArgs e)
		{
			SelectedProcess = (Process) ProcessList.SelectedItem;
			DialogResult = true;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var process = Process.GetCurrentProcess();
			var processes = Process.GetProcesses()
				.Where(x => x.MainWindowHandle.ToInt32() != 0)
				.Where(x => x.Id != process.Id)
				.OrderBy(x => x.ProcessName)
				.ToList();

			Processes.Clear();
			Processes.AddRange(processes);
		}

		#endregion
	}
}