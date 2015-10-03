#region References

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TestR.Desktop;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Class for drawing screen rectangle.
	/// </summary>
	internal class ScreenRectangle : IDisposable
	{
		#region Fields

		private Form _window;

		#endregion

		#region Constructors

		public ScreenRectangle()
		{
			_window = new Form();
			_window.FormBorderStyle = FormBorderStyle.None;
			_window.ShowInTaskbar = false;
			_window.TopMost = true;
			_window.Visible = false;
			_window.Left = 0;
			_window.Top = 0;
			_window.Width = 1;
			_window.Height = 1;
			_window.Show();
			_window.Hide();
			_window.Opacity = 1;
			_window.BackColor = Color.Red;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the location of the rectangle.
		/// </summary>
		public Rectangle Location
		{
			get { return new Rectangle(_window.Left, _window.Top, _window.Width, _window.Height); }
			set
			{
				_window.Left = value.X;
				_window.Top = value.Y;
				_window.Height = value.Height;
				_window.Width = value.Width;
			}
		}

		/// <summary>
		/// Gets or sets if the rectangle is visible.
		/// </summary>
		public bool Visible
		{
			get { return _window.Visible; }
			set { _window.Visible = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing || _window == null)
			{
				return;
			}

			_window.Close();
			_window = null;
		}

		#endregion
	}

	/// <summary>
	/// This class is responsible to draw bounding rectangle around some location on the screen.
	/// </summary>
	internal class Highlighter : IDisposable
	{
		#region Fields

		private readonly ScreenRectangle _bottomRectangle;
		private Rectangle _currentLocation;
		private Element _element;
		private readonly ScreenRectangle _leftRectangle;
		private ScreenRectangle[] _rectangles;
		private readonly ScreenRectangle _rightRectangle;
		private readonly ScreenRectangle _topRectangle;
		private bool _visible;
		private BackgroundWorker _worker;

		#endregion

		#region Constructors

		public Highlighter()
		{
			LineWidth = 2;

			_leftRectangle = new ScreenRectangle();
			_topRectangle = new ScreenRectangle();
			_rightRectangle = new ScreenRectangle();
			_bottomRectangle = new ScreenRectangle();

			_rectangles = new[] { _leftRectangle, _topRectangle, _rightRectangle, _bottomRectangle };
		}

		#endregion

		#region Properties

		public int LineWidth { get; }
		public string ToolTipText { get; set; }

		public bool Visible
		{
			get { return _visible; }
			set { _visible = _leftRectangle.Visible = _rightRectangle.Visible = _topRectangle.Visible = _bottomRectangle.Visible = value; }
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Layout the form using the provided location.
		/// </summary>
		public void Layout()
		{
			try
			{
				if (Visible && !_element.Visible)
				{
					Visible = false;
					return;
				}

				if (!Visible && _element.Visible)
				{
					Visible = true;
				}

				if (_currentLocation == _element.BoundingRectangle)
				{
					return;
				}

				_currentLocation = _element.BoundingRectangle;
			}
			catch (Exception ex)
			{
				_currentLocation = new Rectangle(0,0,0,0);
				Debug.WriteLine("Error trying to layout the element highlighter. " + ex.Message);
			}

			_leftRectangle.Location = new Rectangle(_currentLocation.Left - LineWidth, _currentLocation.Top, LineWidth, _currentLocation.Height);
			_topRectangle.Location = new Rectangle(_currentLocation.Left - LineWidth, _currentLocation.Top - LineWidth, _currentLocation.Width + (2 * LineWidth), LineWidth);
			_rightRectangle.Location = new Rectangle(_currentLocation.Left + _currentLocation.Width, _currentLocation.Top, LineWidth, _currentLocation.Height);
			_bottomRectangle.Location = new Rectangle(_currentLocation.Left - LineWidth, _currentLocation.Top + _currentLocation.Height, _currentLocation.Width + (2 * LineWidth), LineWidth);
		}

		public void SetElement(Element element)
		{
			Stop();
			_element = element;
			Layout();
			Start();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing || _rectangles == null)
			{
				return;
			}

			Stop();

			lock (_rectangles)
			{
				foreach (var rectangle in _rectangles)
				{
					rectangle.Dispose();
				}

				_rectangles = null;
			}
		}

		internal void Start()
		{
			Stop();
			_worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.WorkerSupportsCancellation = true;
			_worker.DoWork += WorkerOnDoWork;
			_worker.ProgressChanged += WorkerOnProgressChanged;
			_worker.RunWorkerAsync(_element);
		}

		private void Stop()
		{
			_worker?.CancelAsync();
			_worker = null;
		}

		private static void WorkerOnDoWork(object sender, DoWorkEventArgs args)
		{
			var worker = (BackgroundWorker) sender;
			var element = (Element) args.Argument;
			var lastLocation = element.BoundingRectangle;
			var lastVisible = element.Visible;

			while (!worker.CancellationPending)
			{
				try
				{
					if (element.BoundingRectangle != lastLocation)
					{
						worker.ReportProgress(1);
						lastLocation = element.BoundingRectangle;
						continue;
					}

					if (element.Visible != lastVisible)
					{
						worker.ReportProgress(1);
						lastVisible = element.Visible;
						continue;
					}

					Thread.Sleep(25);
				}
				catch (Exception)
				{
					worker.ReportProgress(100);
					worker.CancelAsync();
					break;
				}
			}
		}

		private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs args)
		{
			switch (args.ProgressPercentage)
			{
				case 1:
					Layout();
					break;

				default:
					Visible = false;
					_element = null;
					break;
			}
		}

		#endregion
	}
}