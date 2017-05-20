#region References

using System.Web.Mvc;

#endregion

namespace TestR.TestSite
{
	public static class FilterConfig
	{
		#region Methods

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		#endregion
	}
}