using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EShop.Models;
using EShop.Models.ViewModels;
using EShop.Repository;
using EShop.Data;


namespace EShop.Controllers
{
    [Authorize]
	public class CartController : Controller
	{
	    private readonly DataContext _dataContext;
		private readonly ILogger<CartController> _logger;
		public INotyfService _notifyService { get; }
		public CartController(DataContext dataContext, INotyfService notifyService,ILogger<CartController> logger) {
			
			_dataContext = dataContext;
			_notifyService = notifyService;
			_logger = logger;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel cartVM = new()
			{
				cartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}
		public async Task<IActionResult> Add(int Id )
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			List<CartItemModel> cart= HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); // kiem tra trong session gio hang  chau ?
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			try
			{
				if (cartItems == null)
				{
					cart.Add(new CartItemModel(product));

				}
				else
				{
					cartItems.Quantity += 1;
				}
			_notifyService.Success("Thêm giỏ hàng thành công");
			HttpContext.Session.SetJson("Cart", cart);
			}catch (Exception ex) 
				{
				_notifyService.Error("Thêm sản phẩm vào giỏ hàng thất bại");
				_logger.LogError(ex, "Lỗi khi thêm sản phẩm vào giỏ hàng");
			}
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); // kiem tra trong session gio hang  chau ?
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			try
			{
				if (cartItems != null)
				{
					if (cartItems.Quantity >= 1)
					{
						cartItems.Quantity -= 1;
						_notifyService.Success("giảm số lượng sản phẩm thành công");
						HttpContext.Session.SetJson("Cart", cart);

						if(cartItems.Quantity == 0)
						{
							cart.RemoveAll(c => c.ProductId == Id);
						}
					}
					else
					{
						cart.RemoveAll(c => c.ProductId == Id);
					}
				}
				if (cart.Count == 0)
				{

					HttpContext.Session.Remove("Cart");
				}
				else
				{
					HttpContext.Session.SetJson("Cart", cart);
				}

			}
			catch (Exception ex)
			{
				_notifyService.Error("Giảm số lượng sản phẩm thất bại");
				_logger.LogError(ex, "Lỗi giảm số lượng sản phẩm");
			}
			// Điều hướng trở về trang trước đó
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public  IActionResult Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); // kiem tra trong session gio hang  chau ?
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			try
			{
				if (cartItems != null)
				{
					cartItems.Quantity += 1;
					_notifyService.Success("Tăng số lượng sản phẩm thành công");
					HttpContext.Session.SetJson("Cart", cart);
				}

			}
			catch (Exception ex)
			{
				_notifyService.Error("Tăng số lượng sản phẩm thất bại");
				_logger.LogError(ex, "Lỗi tăng số lượng sản phẩm");
			}
			return RedirectToAction("Index");
		}
		public  IActionResult Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); // kiem tra trong session gio hang  chau ?
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			try
			{
				_notifyService.Success("Xóa sản phẩm thành công");
				cart.RemoveAll(c => c.ProductId == Id);
				HttpContext.Session.SetJson("Cart", cart);
			}
			catch (Exception ex)
			{
				_notifyService.Error("Xóa sản phẩm thất bại");
				_logger.LogError(ex, "Lỗi xóa sản phẩm");
			}
			return RedirectToAction("Index");
		}
	}
}
