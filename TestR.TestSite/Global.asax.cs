#region References

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

#endregion

namespace TestR.TestSite
{
	public class Global : HttpApplication
	{
		#region Methods

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			var uri = Request.Url.AbsoluteUri.ToLower();
			var newUri = uri;

			if (!Request.IsSecureConnection)
			{
				newUri = newUri.Replace("http://", "https://");
			}
			
			if (newUri != uri)
			{
				Response.Redirect(newUri);
			}
		}


		protected void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		#endregion
	}
}