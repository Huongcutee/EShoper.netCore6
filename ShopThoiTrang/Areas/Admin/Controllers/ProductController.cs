using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Controllers;
using ShopThoiTrang.Models;
using ShopThoiTrang.Repository;

namespace EShoper.netCore6.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly DataContext dataContext;
        private readonly ILogger<CartController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notifyService { get; }
        public ProductController(DataContext dataContext, ILogger<CartController> _logger, INotyfService _notifyService, IWebHostEnvironment _webHostEnvironment)
		{
			this.dataContext = dataContext;
            this._logger = _logger;
            this._notifyService = _notifyService;
            this._webHostEnvironment = _webHostEnvironment;
		}
		public async Task<IActionResult> Index()
		{
			
			return View(await dataContext.Products.OrderByDescending(p => p.Id).Include(p=> p.Category).Include(p => p.Brand).ToListAsync());
		}
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name");
            return View(new ProductModel()); // Pass an empty model to avoid null reference exceptions
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
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
                        string imageName = Guid.NewGuid().ToString() + "_" +product.ImageUpload.FileName;
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
                foreach(var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            ViewBag.Categories = new SelectList(dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        public async Task<IActionResult> Remove(int Id)
        {
			ProductModel product = await dataContext.Products.FirstOrDefaultAsync(c => c.Id == Id);
			if (product == null)
			{
				// If the product does not exist, notify the user and return
				_notifyService.Information("Sản phẩm không tồn tại");
				return RedirectToAction("index");
			}
			try
			{
				_notifyService.Success("Xóa sản phẩm thành công");
                dataContext.Remove(product);
                await dataContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_notifyService.Error("Xóa sản phẩm thất bại");
				_logger.LogError(ex, "Lỗi xóa sản phẩm");
			}
			return RedirectToAction("index");
        }
    }
}
