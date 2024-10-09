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

		[HttpGet]
		public async Task<IActionResult> Details(int Id)
		{
			if (Id <= 0)
			{
				_notifyService.Warning("Id sản phẩm không hợp lệ");
				return RedirectToAction("Index");
			}

			try
			{
				var productById = await dataContext.Products.FirstOrDefaultAsync(p => p.Id == Id);

				if (productById == null)
				{
					_notifyService.Warning("Không tìm thấy sản phẩm");
					return RedirectToAction("Index");
				}

				return View(productById);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Lỗi khi lấy chi tiết sản phẩm");
				_notifyService.Error("Có lỗi xảy ra khi xử lý yêu cầu");
				return RedirectToAction("Index");
			}
		}

	}
}
