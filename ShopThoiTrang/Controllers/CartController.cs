using Microsoft.AspNetCore.Mvc;
using ShopThoiTrang.Models;
using ShopThoiTrang.Models.ViewModels;
using ShopThoiTrang.Repository;

namespace ShopThoiTrang.Controllers
{
	public class CartController : Controller
	{
	    private readonly DataContext _dataContext;
		public CartController(DataContext dataContext) {
			
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")?? new List<CartItemModel>();
			CartItemViewModel cartVM = new()
			{
				cartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quanity * x.Price)
			};
			return View(cartVM);
		}
		public async Task<IActionResult> Add(int Id )
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			List<CartItemModel> cart= HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItems == null)
			{
				cart.Add(new CartItemModel(product));

			}
			else
			{
				cartItems.Quanity += 1;
			}
			HttpContext.Session.SetJson("Cart", cart);
			return Redirect(Request.Headers["Referer"].ToString());
		}
	}
}
