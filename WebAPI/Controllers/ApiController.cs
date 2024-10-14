using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using ShopThoiTrang.Models;
using ShopThoiTrang.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace ShopThoiTrang.Controllers
{
	public class ApiController : Controller
	{
		private readonly DataContext _datacontext;

		public ApiController(DataContext datacontext)
		{
			_datacontext = datacontext;
		}

		public async Task<IActionResult> Index(string Slug = "")
		{

			var category = await _datacontext.Categories
				.FirstOrDefaultAsync(c => c.Slug == Slug);

			if (category == null)
			{
				return RedirectToAction("Index", "Home");
			}


			var productsByCategory = await _datacontext.Products
				.Where(p => p.CategoryId == category.Id).OrderByDescending(p => p.Id)
				.ToListAsync(); 
			return View(productsByCategory); 
		}
	}
}
