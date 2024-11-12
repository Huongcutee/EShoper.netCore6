using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EShop.Models;
using EShop.Models.ViewModels;
using EShop.Repository;
using EShop.Data;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace EShop.Controllers
{
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

            var shippingPriceCookie = Request.Cookies["ShippingPrices"];
            decimal shippingPrice = 0;
            if (shippingPriceCookie != null)
            {
                shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceCookie);
            }
            decimal grandTotal = cartItems.Any() ? cartItems.Sum(x => x.Quantity * x.Price) : 0;
            decimal totalPrice = grandTotal + shippingPrice;
            // Create CartItemViewModel
            CartItemViewModel cartVM = new()
            {
                ShippingPrice = shippingPrice,
                cartItems = cartItems,
                GrandTotal = grandTotal,
                TotalPrice = totalPrice
            };
            
            
            return View(cartVM);
        }

        [HttpPost]
        [Route("StoreOrder")]
        public async Task<ActionResult> StoreOrder(OrderModel orderModel, string tinh, string quan, string phuong,string diachi, string phoneNumber, decimal totalPrice)
        {
            try
            {

                var orderCode = Guid.NewGuid().ToString();
                orderModel.OrderCode = orderCode;
                orderModel.phoneNumber = phoneNumber;
                orderModel.CreateDate = DateTime.Now;
                orderModel.TotalPrice = totalPrice;
                orderModel.Status = 0;
                orderModel.Province = tinh;
                orderModel.District = quan;
                orderModel.Ward = phuong;
                orderModel.Address = diachi;
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

        [HttpPost]
        public async Task<IActionResult> ShippingCount(string tinh, string quan, string phuong)
        {
             var existedshipping = await _dataContext.Shippings.FirstOrDefaultAsync(s => s.Province == tinh && s.District == quan && s.Ward == phuong);
            decimal priceShipping = 0;
            if(existedshipping != null)
            {
                priceShipping = existedshipping.Price;
            }
            else
            {
                priceShipping = 50000;
            } 
            var shippingPriceJSon = JsonConvert.SerializeObject(priceShipping);
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(30),
                    Secure = true,
                };
                Response.Cookies.Append("ShippingPrices", shippingPriceJSon, cookieOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error adding shipping price {ex.Message}");
                return StatusCode(500, new { error = "Error processing shipping price" });
            }

            return Json(new { priceShipping });
        }
    }
}
