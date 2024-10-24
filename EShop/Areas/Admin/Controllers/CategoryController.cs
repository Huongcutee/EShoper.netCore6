using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using EShop.Controllers;
using EShop.Models;
using EShop.Repository;
using System.Net.WebSockets;

namespace EShoper.netCore6.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]

	public class CategoryController : Controller
	{
		private readonly DataContext dataContext;
		private readonly ILogger<CategoryController> _logger;
        public INotyfService _notifyService { get; }

        public CategoryController(DataContext dataContext, ILogger<CategoryController> _logger, INotyfService _notifyService)
		{
			this.dataContext = dataContext;
			this._logger = _logger;
			this._notifyService = _notifyService;
		}
		public async Task<IActionResult> Index()
		{
            ViewData["title"] = "Danh sách loại sản phẩm";
            var cateroy = await dataContext.Categories.OrderByDescending(c => c.Id).ToListAsync();
			return View(cateroy);
		}
		[HttpGet]
        public async Task<IActionResult> Create()
		{

			return View(new CategoryModel());
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{
            category.Slug = category.Name.Replace(" ", "-");
			var slug = await dataContext.Categories.FirstOrDefaultAsync(c => c.Slug == category.Slug);
			if(slug != null )
			{
				_notifyService.Error("Tên loại sản phẩm đã tồn tại");
				return View(category);
			}	
			
			dataContext.Add(category);
			await dataContext.SaveChangesAsync();
			_notifyService.Success("Thêm loại sản phẩm thành công");
            return RedirectToAction("Index");
		}

        public async Task<IActionResult> Update(int id)
        {
            var category = await dataContext.Categories.FindAsync(id);
            if (category == null)
            {
                _notifyService.Error("Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryModel category)
        {
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        _notifyService.Error(error.ErrorMessage);
                    }
                }
                return View(category);
            }

            var existingCategory = await dataContext.Categories.FindAsync(category.Id);
            if (existingCategory == null)
            {
                _notifyService.Error("Không tìm thấy loại sản phẩm");
                return NotFound();
            }

            category.Slug = category.Name.ToLower().Replace(" ", "-");
            var slugExists = await dataContext.Categories.AnyAsync(c => c.Slug == category.Slug && c.Id != category.Id);
            if (slugExists)
            {
                _notifyService.Error("Tên loại sản phẩm đã tồn tại");
                return View(category);
            }

            try
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.Status = category.Status;
                existingCategory.Slug = category.Slug;

                await dataContext.SaveChangesAsync();
                _notifyService.Success("Cập nhật loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _notifyService.Error("Có lỗi xảy ra khi cập nhật loại sản phẩm");
                // Log the exception
                return View(category);
            }
        }

        public async Task<IActionResult> Remove(int Id)
		{
			CategoryModel cateroyByID = await dataContext.Categories.FirstOrDefaultAsync(c => c.Id == Id);
			if (cateroyByID == null)
			{
				_notifyService.Warning("Không tìm thấy Id loại sản phẩm");
			}
			else
			{
				try
				{
					dataContext.Categories.Remove(cateroyByID);
					await dataContext.SaveChangesAsync();
					_notifyService.Success($"Xóa {cateroyByID.Name} thành công ");
				}
				catch (Exception ex)
				{
					_notifyService.Error($"Error: {ex.Message}");
				}
			}	
            return RedirectToAction("index");
        }
	}
}
