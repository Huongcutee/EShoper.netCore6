using AspNetCoreHero.ToastNotification.Abstractions;
using EShop.Data;
using EShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notifyService { get; }
        public ProductController(DataContext dataContext, ILogger<ProductController> _logger, INotyfService _notifyService, IWebHostEnvironment _webHostEnvironment)
        {
            this.dataContext = dataContext;
            this._logger = _logger;
            this._notifyService = _notifyService;
            this._webHostEnvironment = _webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["title"] = "Danh sách sản phẩm";
            return View(await dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name");
            return View(new ProductModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    _notifyService.Error("Sản phẩm đã có trong database");
                    return View(product);
                }
                else
                {
                    if (product.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                        string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await product.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                        product.Images = imageName;
                    }
                }
                _notifyService.Success("Thêm sản phẩm thành công");
                dataContext.Add(product);
                await dataContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model cos mootj vaif loi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                _notifyService.Error(errorMessage);
            }
            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        public async Task<IActionResult> Remove(int Id)
        {
            ProductModel product = await dataContext.Products.FirstOrDefaultAsync(c => c.Id == Id);
            var existingProduct = await dataContext.Products.FindAsync(product.Id);
            if (product == null)
            {
                // If the product does not exist, notify the user and return
                _notifyService.Information("Sản phẩm không tồn tại");
                return RedirectToAction("index");
            }
            try
            {
                if (!string.Equals(product.Images, "noname.jpg"))
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string oldfileImage = Path.Combine(uploadDir, product.Images);


                    try
                    {
                        if (System.IO.File.Exists(oldfileImage))
                        {
                            System.IO.File.Delete(oldfileImage);
                        }
                    }
                    catch (Exception ex)
                    {
                        _notifyService.Error($"Lỗi Khi xóa sản phẩm: {ex}");

                    }
                }
                dataContext.Products.Remove(product);
                await dataContext.SaveChangesAsync();
                _notifyService.Success("Xóa sản phẩm thành công");
                return RedirectToAction("index");

            }
            catch (Exception ex)
            {
                _notifyService.Error($"Xóa sản phẩm thất bại{ex}");
                _logger.LogError(ex, "Lỗi xóa sản phẩm");
            }
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Update(int Id)
        {
            ProductModel product = await dataContext.Products.FirstOrDefaultAsync(c => c.Id == Id);
            if (Id != product.Id)
            {
                _notifyService.Information("Không tìm thấy sản phẩm");
                return RedirectToAction("Index");

            }
            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }


        [Route("Admin/Product/Update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductModel product)
        {
            var existingProduct = await dataContext.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug && p.Id != product.Id);
                if (slug != null)
                {
                    _notifyService.Error("Sản phẩm đã có trong database");
                }
                else
                {
                    if (product.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                        string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageUpload.CopyToAsync(fs);
                        }

                        existingProduct.Images = imageName;
                    }
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.BrandId = product.BrandId;
                    existingProduct.Slug = product.Slug;

                    await dataContext.SaveChangesAsync();
                    _notifyService.Success("Cập nhật sản phẩm thành công");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                _notifyService.Error(errorMessage);
            }

            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }



    }
}