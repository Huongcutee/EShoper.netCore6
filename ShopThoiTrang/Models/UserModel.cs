using System.ComponentModel.DataAnnotations;
using ShopThoiTrang.Models;

public class UserModel
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên")]
    public string UserName { get; set; }
    [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập email"), EmailAddress]
    public string Email { get; set; }
}
