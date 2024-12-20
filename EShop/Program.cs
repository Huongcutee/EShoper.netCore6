﻿using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using EShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EShop.Repository;
using EShop.Data;
using EShop.Areas.Admin.Hubs;
using EShop.MiddlewawreExtensions;
using EShop.SubscribeTableDepemdemcies;
using EShop.SubcribeTalbeDependecies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

//DI
builder.Services.AddSingleton<DashboardHub>();
builder.Services.AddSingleton<SubscribeProductTableDependency>();
builder.Services.AddSingleton<SubscribeOrderTableDependency>();

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

builder.Services.AddAuthentication()
.AddGoogle(googleOptions =>
{
    // Đọc thông tin Authentication:Google từ appsettings.json
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    // Thiết lập ClientID và ClientSecret để truy cập API google
    googleOptions.ClientId = googleAuthNSection["ClientId"];
    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
    // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
    googleOptions.CallbackPath = "/dang-nhap-tu-google";
});

// Thêm CORS nếu cần
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:7289")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();
var connectionString = app.Configuration.GetConnectionString("ConnectedDb");


app.MapHub<DashboardHub>("/dashboardHub");

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
// Trong phần Configure
app.UseCors("AllowSpecificOrigin");

app.UseSqlTableDependency<SubscribeProductTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeOrderTableDependency>(connectionString);


app.Run();
