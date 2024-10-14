using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Repository;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<RoleController> _logger;
        public INotyfService _notifyService { get; }
        public RoleController(RoleManager<IdentityRole> roleManager, INotyfService _notifyService, ILogger<RoleController> _logger)
        {
            this.roleManager = roleManager;
            this._notifyService = _notifyService;
            this._logger = _logger;
        }
		public async Task<IActionResult> Index()
        {
            ViewData["title"] = "Danh sách quyền hạn";
            var roles = await roleManager.Roles.OrderByDescending(c => c.Id).ToListAsync();
            return View(roles);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            role.NormalizedName = role.Name.ToUpper();
            try
            {
                await roleManager.CreateAsync(role);
                _notifyService.Success("Tạo quyền hạn thành công");

            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }
            return RedirectToAction("Index","Role");
        }
        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                _notifyService.Error("Không tìm thấy quyền hạn");
                return RedirectToAction("Index","Role");
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Update(IdentityRole role,string Id)
        {
           try
                {
                    var existingRole = await roleManager.FindByIdAsync(Id);
                    if (existingRole == null)
                    {
                        _notifyService.Error("Không tìm thấy quyền hạn");
                        return RedirectToAction("Index","Role");
                    }

                    existingRole.Name = role.Name;
                    existingRole.NormalizedName = role.Name.ToUpper();
                    var result = await roleManager.UpdateAsync(existingRole);

                    if (result.Succeeded)
                    {
                        _notifyService.Success("Cập nhật quyền hạn thành công");
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi cập nhật quyền hạn");
                    _notifyService.Error("Đã xảy ra lỗi khi cập nhật quyền hạn");
                }
            
            return RedirectToAction("Index","Role");
        }
        public async Task<IActionResult> Remove(string Id)
        {

            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                _notifyService.Information("Không tìm thấy quyền hạn");
            }
            try
            {
                 await roleManager.DeleteAsync(role);
                _notifyService.Success("Xóa quyền hạn thành công");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }
           return RedirectToAction("Index", "Role");
        }
    }
}
