namespace TestR.Desktop.Pattern
{
	public enum ExpandCollapseState
	{
		/// <summary>
		/// No children are visible.
		/// </summary>
		Collapsed,

		/// <summary>
		/// All children are visible.
		/// </summary>
		Expanded,

		/// <summary>
		/// Some, but not all, children are visible.
		/// </summary>
		PartiallyExpanded,

		/// <summary>
		/// The element does not expand or collapse.
		/// </summary>
		LeafNode
	}
}