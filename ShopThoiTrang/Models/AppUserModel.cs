using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShopThoiTrang.Models
{
    public class AppUserModel : IdentityUser
    {
        public  string Occupation {  get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
