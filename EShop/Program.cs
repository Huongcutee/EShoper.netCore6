using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using EShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EShop.Repository;

var builder = WebApplication.CreateBuilder(args);

// connection 
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
});
// Add services to the container.
builder.Services.AddControllersWithViews();


// add toast 
builder.Services.AddNotyf(config =>
{
	config.DurationInSeconds = 10;
	config.IsDismissable = true;
	config.Position = NotyfPosition.BottomRight;
});


// thiết lập thời gian tồn tại session 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(

  options =>
  {
      options.IdleTimeout = TimeSpan.FromMinutes(30);
      options.Cookie.IsEssential = true;
  });



// identity services

builder.Services.AddIdentity<AppUserModel,IdentityRole>()
    .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.User.RequireUniqueEmail = true;
});


var app = builder.Build();


app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

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
// authen
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();
// sử toast 
app.UseNotyf();

app.MapControllerRoute(
	name: "Areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "Category",
    pattern: "category/{Slug?}",
    defaults: new { controller = "Category", action = "Index" });

app.MapControllerRoute(
    name: "Brand",
    pattern: "Brand/{Slug?}",
    defaults: new { controller = "Brand", action = "Index" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");


//Seeding Data
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
