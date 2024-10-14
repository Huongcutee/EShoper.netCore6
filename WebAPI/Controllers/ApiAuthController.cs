using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopThoiTrang.Models;
using ShopThoiTrang.Models.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Threading.Tasks;

namespace ShopThoiTrang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthController : ControllerBase
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly INotyfService _notifyService;

        public ApiAuthController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, INotyfService notifyService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notifyService = notifyService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                return BadRequest(new { error = "Vui lòng nhập tên tài khoản" });
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new { error = "Vui lòng nhập Email" });
            }

            var existingUserByName = await _userManager.FindByNameAsync(user.UserName);
            if (existingUserByName != null)
            {
                return BadRequest(new { error = "Tên đăng nhập đã tồn tại" });
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest(new { error = "Email đã được sử dụng" });
            }

            AppUserModel model = new AppUserModel { UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber };
            IdentityResult result = await _userManager.CreateAsync(model, user.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "Đăng ký tài khoản thành công" });
            }

            return BadRequest(new { errors = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Đăng nhập thành công" });
                }
                return BadRequest(new { error = "Tài khoản hoặc mật khẩu không chính xác" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Đăng xuất thành công" });
        }
    }
}