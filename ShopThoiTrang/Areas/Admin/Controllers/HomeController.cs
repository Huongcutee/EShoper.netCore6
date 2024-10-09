using Microsoft.AspNetCore.Mvc;

namespace EShoper.netCore6.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {

            return View();
        }
    }
}
