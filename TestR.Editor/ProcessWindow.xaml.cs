#region References

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
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
			var processes = Process.GetProcesses().Where(x => x.MainWindowHandle.ToInt32() != 0);
			Processes.AddRange(processes);
			DataContext = this;
		}

		#endregion

		#region Properties

		public ObservableCollection<Process> Processes { get; set; }
		public Process SelectedProcess { get; private set; }

		#endregion

		#region Methods

		private void Select(object sender, RoutedEventArgs e)
		{
			SelectedProcess = (Process) ProcessList.SelectedItem;
			DialogResult = true;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var processes = Process.GetProcesses().Where(x => x.MainWindowHandle.ToInt32() != 0);
			Processes.Clear();
			Processes.AddRange(processes);
		}

		#endregion
	}
}