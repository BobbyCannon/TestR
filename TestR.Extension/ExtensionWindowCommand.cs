#region References

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace TestR.Extension
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class ExtensionWindowCommand
	{
		#region Constants

		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0100;

		#endregion

		#region Fields

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("364ab674-ad69-4a25-91ed-61d8e04a1154");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionWindowCommand" /> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package"> Owner package, not null. </param>
		private ExtensionWindowCommand(Package package)
		{
			if (package == null)
			{
				throw new ArgumentNullException(nameof(package));
			}

			_package = package;

			var commandService = ServiceProvider.GetService(typeof (IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null)
			{
				var menuCommandId = new CommandID(CommandSet, CommandId);
				var menuItem = new MenuCommand(ShowToolWindow, menuCommandId);
				commandService.AddCommand(menuItem);
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static ExtensionWindowCommand Instance { get; private set; }

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider => _package;

		#endregion

		#region Methods

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package"> Owner package, not null. </param>
		public static void Initialize(Package package)
		{
			Instance = new ExtensionWindowCommand(package);
		}

		/// <summary>
		/// Shows the tool window when the menu item is clicked.
		/// </summary>
		/// <param name="sender"> The event sender. </param>
		/// <param name="e"> The event args. </param>
		private void ShowToolWindow(object sender, EventArgs e)
		{
			// Get the instance number 0 of this tool window. This window is single instance so this instance
			// is actually the only one.
			// The last flag is set to true so that if the tool window does not exists it will be created.
			var window = _package.FindToolWindow(typeof (ExtensionWindow), 0, true);
			if (window?.Frame == null)
			{
				throw new NotSupportedException("Cannot create tool window");
			}

			var windowFrame = (IVsWindowFrame) window.Frame;
			ErrorHandler.ThrowOnFailure(windowFrame.Show());
		}

		#endregion
	}
}