using System.ComponentModel.DataAnnotations;
namespace ShopThoiTrang.Models.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string UserName { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}