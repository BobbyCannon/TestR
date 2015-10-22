#region References

using System;
using System.ComponentModel;
using System.Diagnostics;
using TestR.Desktop;
using TestR.Web;
using Element = TestR.Web.Element;

#endregion

namespace TestR.Extension
{
	public sealed class Project : IDisposable, INotifyPropertyChanged
	{
		#region Fields

		private Application _application;
		private Browser _browser;
		private string _elementDetails;
		private Element _highlightedElement;
		private readonly Highlighter _highlighter;

		#endregion

		#region Constructors

		public Project()
		{
			_application = null;
			_browser = null;
			_highlighter = new Highlighter();
		}

		#endregion

		#region Properties

		public Application Application
		{
			get { return _browser?.Application ?? _application; }
			private set
			{
				_application = value;
				OnPropertyChanged(nameof(Application));
				OnPropertyChanged(nameof(IsApplicationLoaded));
				OnPropertyChanged(nameof(IsLoaded));
			}
		}

		public Browser Browser
		{
			get { return _browser; }
			private set
			{
				_browser = value;
				OnPropertyChanged(nameof(Browser));
				OnPropertyChanged(nameof(IsBrowserLoaded));
				OnPropertyChanged(nameof(IsLoaded));
			}
		}

		public string ElementDetails
		{
			get { return _elementDetails; }
			set
			{
				_elementDetails = value;
				OnPropertyChanged(nameof(ElementDetails));
			}
		}

		/// <summary>
		/// Returns true if the application or browser is loaded.
		/// </summary>
		public bool IsApplicationLoaded => _application != null;

		/// <summary>
		/// Returns true if the application or browser is loaded.
		/// </summary>
		public bool IsBrowserLoaded => _browser != null;

		/// <summary>
		/// Returns true if an application or browser is loaded.
		/// </summary>
		public bool IsLoaded => IsApplicationLoaded || IsBrowserLoaded;

		#endregion

		#region Methods

		public void Close()
		{
			OnClosed();

			Highlight((Element) null);

			if (Browser != null)
			{
				Browser.Application.Closed -= Close;
				Browser.Dispose();
				Browser = null;
			}

			if (Application != null)
			{
				Application.Closed -= Close;
				Application.Dispose();
				Application = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Highlight(Desktop.Element element)
		{
			_highlighter.SetElement(element);
		}

		public void Highlight(Element element)
		{
			_highlightedElement?.Highlight(false);
			_highlightedElement = element;
			_highlightedElement?.Highlight(true);
		}

		public void Initialize(string applicationPath)
		{
			Close();

			Application = Application.AttachOrCreate(applicationPath);
			Application.Closed += Close;
			Application.Timeout = TimeSpan.FromSeconds(5);
		}

		public void Initialize(Process process)
		{
			Close();

			var browser = Browser.AttachToBrowser(process);
			if (browser != null)
			{
				Browser = browser;
				Browser.Application.Closed += Close;
				Browser.Timeout = TimeSpan.FromSeconds(5);
			}
			else
			{
				Application = Application.Attach(process);
				Application.Closed += Close;
				Application.Timeout = TimeSpan.FromSeconds(5);
			}
		}

		public void Refresh()
		{
			_application?.Refresh();
			_browser?.Refresh();
		}

		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (Application != null || Browser != null)
			{
				Close();
			}
		}

		private void OnClosed()
		{
			Closed?.Invoke();
		}

		private void OnPropertyChanged(string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Events

		public event Action Closed;
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}