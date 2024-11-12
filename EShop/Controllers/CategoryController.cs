using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.Models;
using System.Linq;
using System.Threading.Tasks;
using EShop.Data;

namespace EShop.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _datacontext;

		public CategoryController(DataContext datacontext)
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
