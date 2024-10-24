using EShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions<DataContext> options): base(options) { 
		
		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories{ get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetailModel> OrderDetails { get; set; }
        public DbSet<ShippingModel> Shippings { get; set; }
    }
}
