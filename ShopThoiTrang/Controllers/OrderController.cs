using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using ShopThoiTrang.Models;
using ShopThoiTrang.Models.ViewModels;
using ShopThoiTrang.Repository;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ShopThoiTrang.Controllers
{
    [Authorize]
	public class OrderController : Controller
	{
        public INotyfService _notifyService { get; }
        private readonly DataContext _dataContext;
        private readonly ILogger<OrderController> _logger;
        private UserManager<AppUserModel> _userManager;
        public OrderController (UserManager<AppUserModel> userManager, INotyfService notifyService, DataContext dataContext, ILogger<OrderController> logger)
		{
			_userManager = userManager;
			_notifyService = notifyService;
			_dataContext = dataContext;
			_logger = logger;
		}
        public async Task<IActionResult> Index()
        {
            // Existing code for user information
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    ViewBag.Name = user.UserName;
                    ViewBag.PhoneNumber = user.PhoneNumber;
                }
            }

            // Retrieve cart items from session
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            // Create CartItemViewModel
            CartItemViewModel cartVM = new()
            {
                cartItems = cartItems,
                GrandTotal = cartItems.Any() ? cartItems.Sum(x => x.Quantity * x.Price) : 0
            };
            
            
            return View(cartVM);
        }

        [HttpPost]
        [Route("StoreOrder")]
        public async Task<ActionResult> StoreOrder(OrderModel orderModel, string tinh, string quan, string phuong, string phoneNumber, decimal grandPrice)
        {
            try
            {

                var orderCode = Guid.NewGuid().ToString();
                orderModel.OrderCode = orderCode;
                orderModel.phoneNumber = phoneNumber;
                orderModel.CreateDate = DateTime.Now;
                orderModel.TotalPrice = grandPrice;
                orderModel.Status = 0;
                orderModel.Province = tinh;
                orderModel.District = quan;
                orderModel.Ward = phuong;
                _dataContext.Add(orderModel);

                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var cartItem in cartItems)
                {
                    var orderDetail = new OrderDetailModel();
                    orderDetail.ProductId = cartItem.ProductId;
                    orderDetail.OrderCode = orderCode;
                    orderDetail.Quantity = cartItem.Quantity;
                    orderDetail.Price = cartItem.Price;
                   await _dataContext.AddAsync(orderDetail);
                }
                await _dataContext.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");
                _notifyService.Success("Tạo thành công đơn hàng");
                return Ok(new { success = true });
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi khi tạo đơn hàng");
            }
        }
    }
}
