using Microsoft.EntityFrameworkCore;
using ShopThoiTrang.Repository;

var builder = WebApplication.CreateBuilder(args);

// connection 
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
// thiết lập thời gian tồn tại session 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(

  options =>
  {
      options.IdleTimeout = TimeSpan.FromMinutes(30);
      options.Cookie.IsEssential = true;
  });
    
var app = builder.Build();

// cho sử dụng dịch vụ session 
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Seeding Data
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
