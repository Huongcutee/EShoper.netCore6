using EShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace EShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly DataContext _datacontext;
        public DashboardController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["title"] = "Thống kê";
            return View();
        }
    }
}
