using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Repository;

namespace ShopThoiTrang.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext dataContext;
        private readonly ILogger<CartController> _logger;
        public INotyfService _notifyService { get; }
        public ProductController(DataContext dataContext, INotyfService _notifyService, ILogger<CartController> _logger) {
			this.dataContext = dataContext;
			this._notifyService = _notifyService;
			this._logger = _logger;	
		}
		public IActionResult Index()
		{
			return View(); 
		}
		public async Task<IActionResult> Details(int Id )
		{
			if(Id == null)
			{
				return RedirectToAction("Index");
			}
			var productById =  dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();

			if (productById == null)
			{
				return NotFound();  
			}

			return View(productById);
		}
	}
}
