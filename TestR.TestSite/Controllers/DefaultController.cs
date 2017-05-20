#region References

using System.Web.Mvc;
using System.Web.Security;

#endregion

namespace TestR.TestSite.Controllers
{
	public class DefaultController : Controller
	{
		#region Methods

		public ActionResult Login()
		{
			if (!User.Identity.IsAuthenticated)
			{
				FormsAuthentication.SetAuthCookie("UserName", false);
			}

			return RedirectToAction("Index");
		}

		public ActionResult Index()
		{
			return View();
		}

		#endregion
	}
}