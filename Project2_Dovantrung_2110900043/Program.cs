using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project2_Dovantrung_2110900043.DB;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromDays(10);// thời gian phiên
    option.LoginPath = "/Home/Login";// Đường dẫn đến trang đăng nhập 
    option.AccessDeniedPath = "/";// Đường đẫn đến trang từ trối đăng nhập
});
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDb>().AddDefaultTokenProviders();

//builder.Services.ConfigureApplicationCookie(option =>
//{
//    //option.Cookie.HttpOnly = true;
//    option.ExpireTimeSpan = TimeSpan.FromDays(10);// thời gian phiên
//    //option.LoginPath = "/Home/Login";// Đường dẫn đến trang đăng nhập 
//    //option.AccessDeniedPath = "/";// Đường đẫn đến trang từ trối đăng nhập
//});

builder.Services.AddSingleton<IHttpContextAccessor , HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(0.3); // set thời gian có hiệu lực của cookies theo phut
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "test";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


app.UseSession();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.Run();
