using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.Models;
using EShop.Data;

namespace EShop.Controllers
{
    [Authorize]
	public class BrandController : Controller
	{
		private readonly DataContext dataContext;
		public BrandController(DataContext dataContext)
		{
			this.dataContext = dataContext; 
		}
		public async  Task<IActionResult> Index(string Slug = "")
		{
			var brand = await dataContext.Brands.FirstOrDefaultAsync(c => c.Slug == Slug);
			if (brand == null) return RedirectToAction("Index");
			var productsByBrand = await dataContext.Products.Where(p => p.BrandId == brand.Id).OrderByDescending(p=> p.Id).ToListAsync();
			return View(productsByBrand);
		}
	}
}
