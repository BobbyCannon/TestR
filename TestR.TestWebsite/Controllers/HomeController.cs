#region References

using Microsoft.AspNetCore.Mvc;

#endregion

namespace TestR.TestWebsite.Controllers
{
	public class HomeController : Controller
	{
		#region Methods

		public IActionResult Index()
		{
			return View();
		}

		#endregion
	}
}