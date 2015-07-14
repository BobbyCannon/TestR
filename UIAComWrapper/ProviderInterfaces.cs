// (c) Copyright Microsoft Corporation, 2010.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Runtime.InteropServices;
using System.Windows;
using UIAutomationClient;
using IAccessible = Accessibility.IAccessible;

#endregion

// Provider interfaces.
// IRawElementProviderSimple is defined in the interop DLL,
// since the Client code refers to it.  Everything else is here.

namespace UIAComWrapper
{
	[Guid("670c3006-bf4c-428b-8534-e1848f645122")]
	[ComVisible(true)]
	public enum NavigateDirection
	{
		Parent,
		NextSibling,
		PreviousSibling,
		FirstChild,
		LastChild
	}

	[Flags]
	public enum ProviderOptions
	{
		ClientSideProvider = 1,
		ServerSideProvider = 2,
		NonClientAreaProvider = 4,
		OverrideProvider = 8,
		ProviderOwnsSetFocus = 16,
		UseComThreading = 32
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("159bc72c-4ad3-485e-9637-d7052edf0146")]
	[ComVisible(true)]
	public interface IDockProvider
	{
		#region Properties

		DockPosition DockPosition { get; }

		#endregion

		#region Methods

		void SetDockPosition(DockPosition dockPosition);

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d847d3a5-cab0-4a98-8c32-ecb45c59ad24")]
	[ComVisible(true)]
	public interface IExpandCollapseProvider
	{
		#region Properties

		ExpandCollapseState ExpandCollapseState { get; }

		#endregion

		#region Methods

		void Collapse();
		void Expand();

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d02541f1-fb81-4d64-ae32-f520f8a6dbd1")]
	[ComVisible(true)]
	public interface IGridItemProvider
	{
		#region Properties

		int Column { get; }
		int ColumnSpan { get; }
		IRawElementProviderSimple ContainingGrid { get; }
		int Row { get; }
		int RowSpan { get; }

		#endregion
	}

	[ComVisible(true)]
	[Guid("b17d6187-0907-464b-a168-0ef17a1572b1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGridProvider
	{
		#region Properties

		int ColumnCount { get; }
		int RowCount { get; }

		#endregion

		#region Methods

		IRawElementProviderSimple GetItem(int row, int column);

		#endregion
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("54fcb24b-e18e-47a2-b4d3-eccbe77599a2")]
	public interface IInvokeProvider
	{
		#region Methods

		void Invoke();

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("6278cab1-b556-4a1a-b4e0-418acc523201")]
	public interface IMultipleViewProvider
	{
		#region Properties

		int CurrentView { get; }

		#endregion

		#region Methods

		int[] GetSupportedViews();
		string GetViewName(int viewId);
		void SetCurrentView(int viewId);

		#endregion
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("36dc7aef-33e6-4691-afe1-2be7274b3d33")]
	public interface IRangeValueProvider
	{
		#region Properties

		bool IsReadOnly
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		double LargeChange { get; }
		double Maximum { get; }
		double Minimum { get; }
		double SmallChange { get; }
		double Value { get; }

		#endregion

		#region Methods

		void SetValue(double value);

		#endregion
	}

	[Guid("a407b27b-0f6d-4427-9292-473c7bf93258")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface IRawElementProviderAdviseEvents : IRawElementProviderSimple
	{
		#region Methods

		void AdviseEventAdded(int eventId, int[] properties);
		void AdviseEventRemoved(int eventId, int[] properties);

		#endregion
	}

	[Guid("f7063da8-8359-439c-9297-bbc5299a7d87")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface IRawElementProviderFragment : IRawElementProviderSimple
	{
		#region Properties

		Rect BoundingRectangle { get; }
		IRawElementProviderFragmentRoot FragmentRoot { get; }

		#endregion

		#region Methods

		IRawElementProviderSimple[] GetEmbeddedFragmentRoots();
		int[] GetRuntimeId();
		IRawElementProviderFragment Navigate(NavigateDirection direction);
		void SetFocus();

		#endregion
	}

	[ComVisible(true)]
	[Guid("620ce2a5-ab8f-40a9-86cb-de3c75599b58")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IRawElementProviderFragmentRoot : IRawElementProviderFragment, IRawElementProviderSimple
	{
		#region Methods

		IRawElementProviderFragment ElementProviderFromPoint(double x, double y);
		IRawElementProviderFragment GetFocus();

		#endregion
	}

	[ComVisible(true)]
	[Guid("1d5df27c-8947-4425-b8d9-79787bb460b8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IRawElementProviderHwndOverride : IRawElementProviderSimple
	{
		#region Methods

		IRawElementProviderSimple GetOverrideProviderForHwnd(IntPtr hwnd);

		#endregion
	}

	[Guid("2360c714-4bf1-4b26-ba65-9b21316127eb")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IScrollItemProvider
	{
		#region Methods

		void ScrollIntoView();

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("b38b8077-1fc3-42a5-8cae-d40c2215055a")]
	public interface IScrollProvider
	{
		#region Properties

		bool HorizontallyScrollable
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		double HorizontalScrollPercent { get; }
		double HorizontalViewSize { get; }

		bool VerticallyScrollable
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		double VerticalScrollPercent { get; }
		double VerticalViewSize { get; }

		#endregion

		#region Methods

		void Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount);
		void SetScrollPercent(double horizontalPercent, double verticalPercent);

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("2acad808-b2d4-452d-a407-91ff1ad167b2")]
	public interface ISelectionItemProvider
	{
		#region Properties

		bool IsSelected
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		IRawElementProviderSimple SelectionContainer { get; }

		#endregion

		#region Methods

		void AddToSelection();
		void RemoveFromSelection();
		void Select();

		#endregion
	}

	[ComVisible(true)]
	[Guid("fb8b03af-3bdf-48d4-bd36-1a65793be168")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISelectionProvider
	{
		#region Properties

		bool CanSelectMultiple
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		bool IsSelectionRequired
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		#endregion

		#region Methods

		IRawElementProviderSimple[] GetSelection();

		#endregion
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b9734fa6-771f-4d78-9c90-2517999349cd")]
	public interface ITableItemProvider : IGridItemProvider
	{
		#region Methods

		IRawElementProviderSimple[] GetColumnHeaderItems();
		IRawElementProviderSimple[] GetRowHeaderItems();

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("9c860395-97b3-490a-b52a-858cc22af166")]
	public interface ITableProvider : IGridProvider
	{
		#region Properties

		RowOrColumnMajor RowOrColumnMajor { get; }

		#endregion

		#region Methods

		IRawElementProviderSimple[] GetColumnHeaders();
		IRawElementProviderSimple[] GetRowHeaders();

		#endregion
	}

	[Guid("3589c92c-63f3-4367-99bb-ada653b77cf2")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ITextProvider
	{
		#region Properties

		ITextRangeProvider DocumentRange { get; }
		SupportedTextSelection SupportedTextSelection { get; }

		#endregion

		#region Methods

		ITextRangeProvider[] GetSelection();
		ITextRangeProvider[] GetVisibleRanges();
		ITextRangeProvider RangeFromChild(IRawElementProviderSimple childElement);
		ITextRangeProvider RangeFromPoint(Point screenLocation);

		#endregion
	}

	[Guid("5347ad7b-c355-46f8-aff5-909033582f63")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface ITextRangeProvider
	{
		#region Methods

		void AddToSelection();
		ITextRangeProvider Clone();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool Compare(ITextRangeProvider range);

		int CompareEndpoints(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint);
		void ExpandToEnclosingUnit(TextUnit unit);
		ITextRangeProvider FindAttribute(int attribute, object value, [MarshalAs(UnmanagedType.Bool)] bool backward);
		ITextRangeProvider FindText(string text, [MarshalAs(UnmanagedType.Bool)] bool backward, [MarshalAs(UnmanagedType.Bool)] bool ignoreCase);
		object GetAttributeValue(int attribute);
		double[] GetBoundingRectangles();
		IRawElementProviderSimple[] GetChildren();
		IRawElementProviderSimple GetEnclosingElement();
		string GetText(int maxLength);
		int Move(TextUnit unit, int count);
		void MoveEndpointByRange(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint);
		int MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count);
		void RemoveFromSelection();
		void ScrollIntoView([MarshalAs(UnmanagedType.Bool)] bool alignToTop);
		void Select();

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("56d00bd0-c4f4-433c-a836-1a52a57e0892")]
	[ComVisible(true)]
	public interface IToggleProvider
	{
		#region Properties

		ToggleState ToggleState { get; }

		#endregion

		#region Methods

		void Toggle();

		#endregion
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6829ddc4-4f91-4ffa-b86f-bd3e2987cb4c")]
	public interface ITransformProvider
	{
		#region Properties

		bool CanMove
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		bool CanResize
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		bool CanRotate
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		#endregion

		#region Methods

		void Move(double x, double y);
		void Resize(double width, double height);
		void Rotate(double degrees);

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c7935180-6fb3-4201-b174-7df73adbf64a")]
	[ComVisible(true)]
	public interface IValueProvider
	{
		#region Properties

		bool IsReadOnly
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		string Value { get; }

		#endregion

		#region Methods

		void SetValue([MarshalAs(UnmanagedType.LPWStr)] string value);

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("987df77b-db06-4d77-8f8a-86a9c3bb90b9")]
	[ComVisible(true)]
	public interface IWindowProvider
	{
		#region Properties

		WindowInteractionState InteractionState { get; }

		bool IsModal
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		bool IsTopmost
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		bool Maximizable
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		bool Minimizable
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			get;
		}

		WindowVisualState VisualState { get; }

		#endregion

		#region Methods

		void Close();
		void SetVisualState(WindowVisualState state);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool WaitForInputIdle(int milliseconds);

		#endregion
	}

	// New for Windows 7
	//
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("E747770B-39CE-4382-AB30-D8FB3F336F24")]
	[ComVisible(true)]
	public interface IItemContainerProvider
	{
		#region Methods

		IRawElementProviderSimple FindItemByProperty(IRawElementProviderSimple pStartAfter, int propertyId, object Value);

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("E44C3566-915D-4070-99C6-047BFF5A08F5")]
	[ComVisible(true)]
	public interface ILegacyIAccessibleProvider
	{
		#region Properties

		int ChildId { get; }
		string DefaultAction { get; }
		string Description { get; }
		string Help { get; }
		string KeyboardShortcut { get; }
		string Name { get; }
		uint Role { get; }
		uint state { get; }
		string Value { get; }

		#endregion

		#region Methods

		void DoDefaultAction();

		[return: MarshalAs(UnmanagedType.Interface)]
		IAccessible GetIAccessible();

		IRawElementProviderSimple[] GetSelection();
		void Select(int flagsSelect);
		void SetValue([MarshalAs(UnmanagedType.LPWStr)] string szValue);

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("29DB1A06-02CE-4CF7-9B42-565D4FAB20EE")]
	[ComVisible(true)]
	public interface ISynchronizedInputProvider
	{
		#region Methods

		void Cancel();
		void StartListening([In] SynchronizedInputType inputType);

		#endregion
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CB98B665-2D35-4FAC-AD35-F3C60D0C0B8B")]
	[ComVisible(true)]
	public interface IVirtualizedItemProvider
	{
		#region Methods

		void Realize();

		#endregion
	}
}