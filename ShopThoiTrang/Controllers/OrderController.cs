using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using ShopThoiTrang.Models;
using ShopThoiTrang.Models.ViewModels;
using ShopThoiTrang.Repository;

namespace ShopThoiTrang.Controllers
{
	public class OrderController : Controller
	{
        public INotyfService _notifyService { get; }
        private readonly DataContext _dataContext;
        private readonly ILogger<OrderController> _logger;
		public OrderController (INotyfService notifyService, DataContext dataContext, ILogger<OrderController> logger)
		{
			_notifyService = notifyService;
			_dataContext = dataContext;
			_logger = logger;
		}
        public IActionResult Index()
		{
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			
			if (cartItems == null)
			{
				_notifyService.Error("Giỏ hàng rỗng");
				return RedirectToAction("Index","Cart");
			}

            CartItemViewModel cartVM = new()
            {
                cartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
            };

            return View(cartVM);
		}
		[HttpPost]
		public async Task<IActionResult> Create(OrderDetailModel order)
		{

			return View();
		}
	}
}
