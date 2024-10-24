using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.Models;
using EShop.Repository;
using System.Diagnostics;
using System.Drawing.Printing;

namespace EShop.Controllers
{

    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;

        private readonly ILogger<HomeController> _logger;
		public HomeController(ILogger<HomeController> logger, DataContext dataContext, INotyfService notifyService)
        {
            _logger = logger;
            _dataContext = dataContext;
	

		}

		public IActionResult Index()
        {
		
			var product = _dataContext.Products.Include("Category").Include("Brand").ToList(); 
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode == 404)
            {
                return View("NotFound");
            }    
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
