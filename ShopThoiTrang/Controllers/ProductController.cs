using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Repository;

namespace ShopThoiTrang.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext dataContext;
		public ProductController(DataContext dataContext) {
			this.dataContext = dataContext;
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
