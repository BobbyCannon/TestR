#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Automation
{
	public static class Automation
	{
		#region Fields

		private static readonly IUIAutomation _factory = new CUIAutomationClass();
		public static readonly Condition ContentViewCondition = Condition.Wrap(Factory.ContentViewCondition);
		public static readonly Condition ControlViewCondition = Condition.Wrap(Factory.ControlViewCondition);
		public static readonly Condition RawViewCondition = Condition.Wrap(Factory.RawViewCondition);

		#endregion

		#region Constructors

		/// <summary>
		/// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
		/// </summary>
		static Automation()
		{
		}

		#endregion

		#region Properties

		internal static IUIAutomation Factory
		{
			get { return _factory; }
		}

		#endregion
	}
}