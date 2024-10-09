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

		public AuthController(UserManager<AppUserModel> _userManager, SignInManager<AppUserModel> _signInManager, INotyfService _notifyService)
		{

			this._userManager = _userManager;
			this._signInManager = _signInManager;
			this._notifyService = _notifyService;
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

			AppUserModel model = new AppUserModel { UserName = user.UserName, Email = user.Email };

			IdentityResult result = await _userManager.CreateAsync(model, user.Password);
			if (result.Succeeded)
			{
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
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel login)
		{
			if(ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(login.UserName,login.Password,false,false);
				if(result.Succeeded)
				{
					_notifyService.Success("Đăng nhập thành công");
					return Redirect("/");
				}
				ModelState.AddModelError("UserName", "Tài khoản hoặc mật khẩu không chính xác");
				ModelState.AddModelError("Password", "Tài khoản hoặc mật khẩu không chính xác");
			}
			
			return View(login);
		}
		public async Task<IActionResult> Logout (string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}
	}
}
