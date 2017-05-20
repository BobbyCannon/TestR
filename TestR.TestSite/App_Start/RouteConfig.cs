#region References

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace TestR.TestSite
{
	public static class RouteConfig
	{
		#region Methods

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute("Default", "{controller}/{action}/{id}", new { action = "Index", id = UrlParameter.Optional });
		}

		#endregion
	}
}