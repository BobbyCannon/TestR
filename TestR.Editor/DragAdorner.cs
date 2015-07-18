#region References

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Renders a visual which can follow the mouse cursor,
	/// such as during a drag-and-drop operation.
	/// </summary>
	public class DragAdorner : Adorner
	{
		#region Fields

		private readonly Rectangle _child;
		private double _offsetLeft;
		private double _offsetTop;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of DragVisualAdorner.
		/// </summary>
		/// <param name="adornedElement"> The element being adorned. </param>
		/// <param name="size"> The size of the adorner. </param>
		/// <param name="brush"> A brush to with which to paint the adorner. </param>
		public DragAdorner(UIElement adornedElement, Size size, Brush brush)
			: base(adornedElement)
		{
			var rect = new Rectangle();
			rect.Fill = brush;
			rect.Width = size.Width;
			rect.Height = size.Height;
			rect.IsHitTestVisible = false;
			_child = rect;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets/sets the horizontal offset of the adorner.
		/// </summary>
		public double OffsetLeft
		{
			get { return _offsetLeft; }
			set
			{
				_offsetLeft = value;
				UpdateLocation();
			}
		}

		/// <summary>
		/// Gets/sets the vertical offset of the adorner.
		/// </summary>
		public double OffsetTop
		{
			get { return _offsetTop; }
			set
			{
				_offsetTop = value;
				UpdateLocation();
			}
		}

		/// <summary>
		/// Override.  Always returns 1.
		/// </summary>
		protected override int VisualChildrenCount
		{
			get { return 1; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Override.
		/// </summary>
		/// <param name="transform"> </param>
		/// <returns> </returns>
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			var result = new GeneralTransformGroup();
			result.Children.Add(base.GetDesiredTransform(transform));
			result.Children.Add(new TranslateTransform(_offsetLeft, _offsetTop));
			return result;
		}

		/// <summary>
		/// Updates the location of the adorner in one atomic operation.
		/// </summary>
		/// <param name="left"> </param>
		/// <param name="top"> </param>
		public void SetOffsets(double left, double top)
		{
			_offsetLeft = left;
			_offsetTop = top;
			UpdateLocation();
		}

		/// <summary>
		/// Override.
		/// </summary>
		/// <param name="finalSize"> </param>
		/// <returns> </returns>
		protected override Size ArrangeOverride(Size finalSize)
		{
			_child.Arrange(new Rect(finalSize));
			return finalSize;
		}

		/// <summary>
		/// Override.
		/// </summary>
		/// <param name="index"> </param>
		/// <returns> </returns>
		protected override Visual GetVisualChild(int index)
		{
			return _child;
		}

		/// <summary>
		/// Override.
		/// </summary>
		/// <param name="constraint"> </param>
		/// <returns> </returns>
		protected override Size MeasureOverride(Size constraint)
		{
			_child.Measure(constraint);
			return _child.DesiredSize;
		}

		private void UpdateLocation()
		{
			var adornerLayer = Parent as AdornerLayer;
			if (adornerLayer != null)
			{
				adornerLayer.Update(AdornedElement);
			}
		}

		#endregion
	}
}