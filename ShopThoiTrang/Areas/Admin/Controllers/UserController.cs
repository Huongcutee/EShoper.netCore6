using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Areas.Admin.Model;
using ShopThoiTrang.Models;
using ShopThoiTrang.Repository;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUserModel> userManager;
        private readonly DataContext dataContext;   
        public INotyfService _notifyService { get; }

        public UserController(DataContext dataContext, INotyfService _notifyService, RoleManager<IdentityRole> roleManager, UserManager<AppUserModel> userManager)
        {
            this.dataContext = dataContext;
            this._notifyService = _notifyService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            ViewData["title"] = "Danh sách tài khoản";
            // Lấy danh sách tất cả người dùng
            var users = await dataContext.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.role = new SelectList(roleManager.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {

            if (!ModelState.IsValid)
            {
                var errorMessages = new List<string>();
                foreach (var err in ModelState.Values)
                {
                    foreach (var error in err.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }
                TempData["ValidationErrors"] = errorMessages;
                // Trả về lại View với model để người dùng có thể sửa lại thông tin
                ViewBag.Role = new SelectList(await roleManager.Roles.ToListAsync(), "Name", "Name");
                return View(user);
            }

            // Tạo một đối tượng người dùng mới
            var newUser = new AppUserModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    RoleName = user.RoleName,

                };

            IdentityResult result = await userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser,user.RoleName);
                _notifyService.Success("Đăng ký tài khoản thành công");
                return Redirect("Index");
            }
            foreach (IdentityError error in result.Errors)
            {
                _notifyService.Error(error.Description);
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction("Index", "User");
               
        }
        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {

            var userByUserName = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (userByUserName == null)
            {
                _notifyService.Information("Không tìm thấy tài khoản");
                return RedirectToAction("Index");
            }

            ViewBag.Role = new SelectList(await roleManager.Roles.ToListAsync(), "Name", "Name");
            return View(userByUserName);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppUserModel model, string Id)
        {

            if (!ModelState.IsValid)
            {
                var errorMessages = new List<string>();
                foreach (var err in ModelState.Values)
                {
                    foreach (var error in err.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }
                TempData["ValidationErrors"] = errorMessages;
                // Trả về lại View với model để người dùng có thể sửa lại thông tin
                ViewBag.Role = new SelectList(await roleManager.Roles.ToListAsync(), "Name", "Name");
                return View(model);
            }

            try
            {
                AppUserModel existingUser = await userManager.FindByIdAsync(Id);
                if (existingUser == null)
                {
                    _notifyService.Warning("Không tìm thấy tài khoản");
                    return RedirectToAction("Index");
                }

                // Cập nhật thông tin người dùng
                existingUser.UserName = model.UserName;
                existingUser.PhoneNumber = model.PhoneNumber;
                existingUser.Email = model.Email;
                existingUser.RoleName = model.RoleName;
                // Cập nhật mật khẩu nếu được cung cấp
                if (!string.IsNullOrWhiteSpace(model.PasswordHash))
                {
                    var newPasswordHash = userManager.PasswordHasher.HashPassword(existingUser, model.PasswordHash);
                    existingUser.PasswordHash = newPasswordHash;
                }
                var updateUserResult = await userManager.UpdateAsync(existingUser);
                if (updateUserResult.Succeeded) 
                {
                    TempData["SuccessMessage"] = "Cập nhật vai trò thành công!";
                    _notifyService.Success("Cập nhật tài khoản thành công");
                    return RedirectToAction("Index");   
                }
                else
                {
                    foreach (var error in updateUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }    
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Có lỗi xảy ra: {ex.Message}");

                // Để cho phép người dùng sửa thông tin lại
                ViewBag.Role = new SelectList(await roleManager.Roles.ToListAsync(), "Name", "Name");
                return View(model);
            }
        }
        public async Task<IActionResult> Remove(string UserName)
        {
            var userByName = await userManager.FindByNameAsync(UserName); 
            await userManager.DeleteAsync(userByName);
            await dataContext.SaveChangesAsync();
            _notifyService.Success("Xóa tài khoản thành công");
            return RedirectToAction("Index", "User");
        }
    }
}
