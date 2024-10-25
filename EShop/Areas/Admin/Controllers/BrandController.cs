using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.Models;
using EShop.Data;

namespace EShoper.netCore6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ILogger<BrandModel> _logger;
        public INotyfService _notifyService { get; }

        public BrandController(DataContext dataContext, ILogger<BrandModel> _logger, INotyfService _notifyService)
        {
            this.dataContext = dataContext;
            this._logger = _logger;
            this._notifyService = _notifyService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["title"] = "Danh sách loại sản phẩm";
            var brand = await dataContext.Brands.OrderByDescending(c => c.Id).ToListAsync();
            return View(brand);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View(new BrandModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            brand.Slug = brand.Name.Replace(" ", "-");
            var slug = await dataContext.Brands.FirstOrDefaultAsync(c => c.Slug == brand.Slug);
            if (slug != null)
            {
                _notifyService.Error("Tên thương hiệu đã tồn tại");
                return View(brand);
            }

            dataContext.Add(brand);
            await dataContext.SaveChangesAsync();
            _notifyService.Success("Thêm loại thương hiệu thành công");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var brand = await dataContext.Brands.FindAsync(id);
            if (brand == null)
            {
                _notifyService.Error("Không tìm thấy thương hiệu");
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BrandModel brand)
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
                return View(brand);
            }

            var existingbrand = await dataContext.Brands.FindAsync( brand.Id);
            if (existingbrand == null)
            {
                _notifyService.Error("Không tìm thấy thương hiệu");
                return NotFound();
            }

            brand.Slug = brand.Name.ToLower().Replace(" ", "-");
            var slugExists = await dataContext.Brands.AnyAsync(c => c.Slug == brand.Slug && c.Id != brand.Id);
            if (slugExists)
            {
                _notifyService.Error("Tên loại sản phẩm đã tồn tại");
                return View(brand);
            }

            try
            {
                existingbrand.Name = brand.Name;
                existingbrand.Description = brand.Description;
                existingbrand.Status = brand.Status;
                existingbrand.Slug = brand.Slug;

                await dataContext.SaveChangesAsync();
                _notifyService.Success("Cập nhật loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _notifyService.Error("Có lỗi xảy ra khi cập nhật loại sản phẩm");
                // Log the exception
                return View(brand);
            }
        }

        public async Task<IActionResult> Remove(int Id)
        {
            BrandModel brandByID = await dataContext.Brands.FirstOrDefaultAsync(c => c.Id == Id);
            if (brandByID == null)
            {
                _notifyService.Warning("Không tìm thấy Id loại sản phẩm");
            }
            else
            {
                try
                {
                    dataContext.Brands.Remove(brandByID);
                    await dataContext.SaveChangesAsync();
                    _notifyService.Success($"Xóa {brandByID.Name} thành công ");
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
