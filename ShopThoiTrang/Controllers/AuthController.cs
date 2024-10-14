using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopThoiTrang.Models.ViewModels;
using ShopThoiTrang.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ShopThoiTrang.Controllers
{
	public class AuthController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		public INotyfService _notifyService { get; }
		private RoleManager<IdentityRole> roleManager;

		public AuthController(UserManager<AppUserModel> _userManager, SignInManager<AppUserModel> _signInManager, INotyfService _notifyService, RoleManager<IdentityRole> roleManager)
		{

			this._userManager = _userManager;
			this._signInManager = _signInManager;
			this._notifyService = _notifyService;
			this.roleManager = roleManager;
		}
		public  IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(UserModel user)
		{
			if(user.UserName == null)
			{
				_notifyService.Error("Vui lòng nhập tên tài khoản");
				ModelState.AddModelError("UserName", "Vui lòng nhập tên đăng nhập");
				return View(user);
			}
			if (user.Email == null)
			{
				_notifyService.Error("Vui lòng nhập Email");
				ModelState.AddModelError("UserName", "Vui lòng nhập Email");
				return View(user);
			}
          
            // Kiểm tra tên người dùng đã tồn tại
            var existingUserByName = await _userManager.FindByNameAsync(user.UserName);
			if (existingUserByName != null)
			{
				_notifyService.Error("Tên đăng nhập đã tồn tại");
				ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
				return View(user);
			}

			// Kiểm tra email đã tồn tại
			var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email);
			if (existingUserByEmail != null)
			{
				_notifyService.Error("Email đã được sử dụng");
				ModelState.AddModelError("Email", "Email đã được sử dụng");
				return View(user);
			}

			AppUserModel model = new AppUserModel { UserName = user.UserName, Email = user.Email,PhoneNumber = user.PhoneNumber, RoleName = "Client" };


			IdentityResult result = await _userManager.CreateAsync(model, user.Password);

            if (result.Succeeded)
			{
                await _userManager.AddToRoleAsync(model, "Client");
                _notifyService.Success("Đăng ký tài khoản thành công");
				return Redirect("/auth/login");
			}
			foreach (IdentityError error in result.Errors)
			{
				_notifyService.Error(error.Description);
				ModelState.AddModelError("", error.Description);
			}
			return View(user);

		}
		public IActionResult Login()
		{
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
			   

                var user = await _userManager.FindByNameAsync(login.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("UserName", "Tài khoản không tồn tại");
                    return View(login);
                }
              


                // Thử đăng nhập
                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);
                if (result.Succeeded)
                {
                    _notifyService.Success("Đăng nhập thành công");
                    return Redirect("/");
                }
                else
                {
                    // Kiểm tra các lỗi cụ thể
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa. Vui lòng thử lại sau.");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "Tài khoản chưa được kích hoạt.");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Mật khẩu không chính xác");
                    }
                }
            }

            // Nếu có lỗi, trả về view với model
            return View(login);
        }
        public async Task<IActionResult> Logout ()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Create","Auth");
		}
	}
}
