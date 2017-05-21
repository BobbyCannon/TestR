#region References

using System.Web.Mvc;
using System.Web.Security;

#endregion

namespace TestR.TestSite.Controllers
{
	public class DefaultController : Controller
	{
		#region Methods

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Login()
		{
			if (!User.Identity.IsAuthenticated)
			{
				FormsAuthentication.SetAuthCookie("UserName", false);
			}

			return RedirectToAction("Index");
		}

		#endregion
	}
}