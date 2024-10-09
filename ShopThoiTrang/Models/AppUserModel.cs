using Microsoft.AspNetCore.Identity;

namespace ShopThoiTrang.Models
{
    public class AppUserModel : IdentityUser
    {
        public  string Occupation {  get; set; }
    }
}
