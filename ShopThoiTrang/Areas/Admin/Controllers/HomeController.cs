using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoper.netCore6.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
		[Authorize]

		public IActionResult Index()
        {

            return View();
        }
    }
}
