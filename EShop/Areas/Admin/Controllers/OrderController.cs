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
	public class OrderController : Controller
	{
		private DataContext _dataContext;
        public INotyfService _notifyService { get; }
        public OrderController(DataContext dataContext,   INotyfService notifyService )
		{
			_dataContext = dataContext;
			_notifyService = notifyService;
		}
		public async Task<IActionResult> Index()
		{
			ViewData["title"] = "Danh sách đặt hàng";

			return View(await _dataContext.Orders.OrderByDescending(o => o.CreateDate).ToListAsync());
		}
		public async Task<IActionResult> ViewDetail(string orderCode)
		{
            var orderDetails = await _dataContext.OrderDetails.Where(o => o.OrderCode == orderCode).ToListAsync();
            if (!orderDetails.Any())
            {
                _notifyService.Information("Không tìm thấy đơn hàng");
                return RedirectToAction("Index");
            }
            return View(orderDetails);
		}
       

        public async Task<IActionResult> Remove(int id)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                _notifyService.Information("Không tìm thấy đơn hàng");
                return RedirectToAction("Index");
            }

            // Lấy danh sách chi tiết đơn hàng theo OrderCode
            var orderDetails = await _dataContext.OrderDetails
                .Where(od => od.OrderCode == order.OrderCode)
                .ToListAsync();

            // Xóa tất cả các chi tiết đơn hàng trước
            _dataContext.OrderDetails.RemoveRange(orderDetails);

            // Xóa đơn hàng
            _dataContext.Orders.Remove(order);

            // Lưu thay đổi
            await _dataContext.SaveChangesAsync();

            _notifyService.Success("Đã xóa đơn hàng và chi tiết đơn hàng thành công");

            return RedirectToAction("Index");
        }

        

    }
}
