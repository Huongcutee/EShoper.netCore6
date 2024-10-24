using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using EShop.Models;
using EShop.Repository;

namespace EShop.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles ="Admin")]
	public class ShippingController : Controller
    {
        private readonly DataContext dataContext;
        private readonly ILogger<ShippingController> _logger;
        public INotyfService _notifyService { get; }

        public ShippingController(DataContext dataContext, ILogger<ShippingController> _logger, INotyfService _notifyService)
        {
            this.dataContext = dataContext;
            this._logger = _logger;
            this._notifyService = _notifyService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["title"] = "Danh sách shipping";
            var shipping = await dataContext.Shippings.OrderByDescending(c => c.Id).ToListAsync();
            return View(shipping);
        }
        public   IActionResult Create()
        {
            return View(new ShippingModel());
        }
        [HttpPost]
        [Route("StoreShipping")]
        public async Task<ActionResult> StoreShipping(ShippingModel storeShipping, string tinh, string quan, string phuong, decimal price)
        {
            storeShipping.Province = tinh;
            storeShipping.District = quan;
            storeShipping.Ward = phuong;
            storeShipping.Price = price;
            try
            {
                var existingShipping = await dataContext.Shippings.AnyAsync(x=> x.Province == tinh && x.District == quan && x.Ward == phuong);
                if(existingShipping)
                {
                    _notifyService.Information("Đã có giá cho nơi vận chuyển này kiểm tra lại");
                    return Ok(new { duplicate = true, message = "Dữ liệu trùng lặp" });
                }
                dataContext.Shippings.Add(storeShipping);
                await dataContext.SaveChangesAsync();
				_notifyService.Success("Tạo giá vận chuyển thành công");
                return Ok(new { success = true });
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi khi thêm giá vận chuyển vào database");
            }
        }

        public async  Task<ActionResult> Update(int Id)
        {
            ShippingModel shipping = await dataContext.Shippings.FirstOrDefaultAsync(x => x.Id == Id);
            return View(shipping);
        }
        [HttpPost]
        public async Task<ActionResult> Update(ShippingModel shipping)
        {
            if(!ModelState.IsValid)
            {
                foreach(var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        _notifyService.Error(error.ErrorMessage);
                    }    
                }    
            }    
            var existingShipping =  await dataContext.Shippings.AnyAsync(s => s.Province == shipping.Province && s.District == shipping.District && s.Ward == shipping.Ward && s.Id != shipping.Id);
            if(existingShipping == true )
            {
                _notifyService.Error("Giá vận chuyển ở tỉnh thành này đã tồn tại");
            }  

            if (shipping == null)
            {
                _notifyService.Error("Giá vận chuyển không tìm thấy");
            }
            ShippingModel exeistingShipping = await dataContext.Shippings.FindAsync(shipping.Id);
            if (exeistingShipping == null) {
                _notifyService.Error("Không tìm thấy giá vận chuyển");
            }
            else
            {
                try
                {

                    exeistingShipping.Id = shipping.Id;
                    exeistingShipping.Province = shipping.Province;
                    exeistingShipping.District = shipping.District;
                    exeistingShipping.Ward = shipping.Ward;
                    exeistingShipping.Price = shipping.Price;
                    await dataContext.SaveChangesAsync();
                    _notifyService.Success("Cập nhật giá vận chuyển thành công");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                _notifyService.Error($"{ex.Message}");
                }

            }
            return View();
        }

        public async Task<IActionResult> Remove(int Id)
        {
            ShippingModel shipping = await dataContext.Shippings.FindAsync(Id);
            if (shipping == null)
            {
                _notifyService.Error("Không tìm thấy giá vận chuyển");
            }
            dataContext.Shippings.Remove(shipping);
             await dataContext.SaveChangesAsync();
            _notifyService.Success("Xóa giá vận chuyển thành công");
            return RedirectToAction("Index","Shipping");
        }
    }
}
