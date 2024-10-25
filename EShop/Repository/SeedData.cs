using Microsoft.EntityFrameworkCore;
using EShop.Models;
using System.Xml.Linq;
using EShop.Data;

namespace EShop.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel DT  = new CategoryModel { Name = "Điện thoại", Slug = "dienthoai", Description = "Luôn cập nhật nhưng hãng điện thoại mới ra từ iphone, samsung ... ", Status = 1 };
				BrandModel APPLE = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple là hãng sản xuất điện thoại số 1 thế giớ", Status = 1 };
				CategoryModel MT = new CategoryModel { Name = "Máy tính", Slug = "maytinh", Description = "Là công cụ cấu hình mạnh mẽ  giúp cho chung ta dễ dàng làm việc, chơi game, giải trí.", Status = 1 };
				BrandModel LapTop = new BrandModel { Name = "LapTop", Slug = "laptop", Description = "Laptop giá rẽ thích hợp cho học sinh, sinh viên chơi game học tập", Status = 1 };
                _context.Products.AddRange(

                    new ProductModel { Name = "Iphone 14 Promax", Slug = "iphone14promax ", Description = "Dòng mới nhất của nhà apple cung cấp thêm các tính năng mạng mẽ cho chụp hình và chơi game.", Images = "iphone14promax.png", Quantity = 3, Price = 100000, Category = DT, Brand = APPLE },
                    new ProductModel { Name = "TufGaming f15", Slug = "tuff15", Description = "Hiện đại mạnh mẽ chơi được nhiều game nặng như: pubg, valorent, lol... ", Images = "tuff15.png", Quantity = 3, Price = 100000, Category = MT, Brand = LapTop }
                    
                );
                _context.SaveChanges();
            }
        }
    }
}
