using EShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoper.netCore6.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]
    public class HomeController : Controller
    {
        private readonly DataContext _datacontext;



		public IActionResult Index()
        {
            
            return View();
        }
    }
}
