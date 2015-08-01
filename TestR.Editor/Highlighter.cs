#region References

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Class for drawing screen rectangle.
	/// </summary>
	internal class ScreenRectangle : IDisposable
	{
		#region Fields

		//form used as a rectangle
		private readonly Form _window;

		#endregion

		#region Constructors

		public ScreenRectangle()
		{
			//initialize the form
			//initialize the form
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
		/// get/set location of the rectangle
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
		/// get/set opacity for the rectangle
		/// </summary>
		public double Opacity
		{
			get { return _window.Opacity; }
			set { _window.Opacity = value; }
		}

		/// <summary>
		/// get/set visibility for the rectangle
		/// </summary>
		public bool Visible
		{
			get { return _window.Visible; }
			set { _window.Visible = value; }
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			_window.Close();
		}

		#endregion
	}

	/// <summary>
	/// This class is responsible to draw bounding rectangle around some location on the screen.
	/// </summary>
	internal class ScreenBoundingRectangle : IDisposable
	{
		#region Fields

		private readonly ScreenRectangle _bottomRectangle;
		private readonly ScreenRectangle _leftRectangle;
		private Rectangle _location;
		private readonly ScreenRectangle[] _rectangles;
		private readonly ScreenRectangle _rightRectangle;
		private readonly ScreenRectangle _topRectangle;
		private bool _visible;

		#endregion

		#region Constructors

		public ScreenBoundingRectangle()
		{
			//initialize instance
			LineWidth = 3;

			_leftRectangle = new ScreenRectangle();
			_topRectangle = new ScreenRectangle();
			_rightRectangle = new ScreenRectangle();
			_bottomRectangle = new ScreenRectangle();

			_rectangles = new[] { _leftRectangle, _topRectangle, _rightRectangle, _bottomRectangle };
		}

		#endregion

		#region Properties

		public int LineWidth { get; }

		public Rectangle Location
		{
			get { return _location; }
			set
			{
				_location = value;
				Layout();
			}
		}

		public double Opacity
		{
			get { return _leftRectangle.Opacity; }
			set
			{
				_leftRectangle.Opacity = _rightRectangle.Opacity =
					_topRectangle.Opacity = _bottomRectangle.Opacity = value;
			}
		}

		public string ToolTipText { get; set; }

		public bool Visible
		{
			get { return _visible; }
			set
			{
				_visible = _leftRectangle.Visible = _rightRectangle.Visible =
					_topRectangle.Visible = _bottomRectangle.Visible = value;
			}
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			foreach (var rectangle in _rectangles)
			{
				rectangle.Dispose();
			}
		}

		private void Layout()
		{
			_leftRectangle.Location = new Rectangle(_location.Left - LineWidth, _location.Top, LineWidth, _location.Height);
			_topRectangle.Location = new Rectangle(_location.Left - LineWidth, _location.Top - LineWidth, _location.Width + (2 * LineWidth), LineWidth);
			_rightRectangle.Location = new Rectangle(_location.Left + _location.Width, _location.Top, LineWidth, _location.Height);
			_bottomRectangle.Location = new Rectangle(_location.Left - LineWidth, _location.Top + _location.Height, _location.Width + (2 * LineWidth), LineWidth);
		}

		#endregion
	}
}