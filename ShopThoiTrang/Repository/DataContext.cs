using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Repository
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options): base(options) { 
		
		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories{ get; set; }

	}
}
