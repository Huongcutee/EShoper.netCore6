using EShop.Areas.Admin.Models;
using EShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Data
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        public DbSet<ShippingModel> Shippings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductForGraph>().HasNoKey();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderProductForGraph>().HasNoKey();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderModel>()
            .HasMany(o => o.OrderDetailModel)    
            .WithOne()                          
            .HasForeignKey(od => od.OrderCode)   
            .HasPrincipalKey(o => o.OrderCode)  
            .OnDelete(DeleteBehavior.Cascade);
        }
       

    }
}
