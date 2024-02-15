using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/Errors/Error403");
    // giri� - ��k�� - eri�im engeli durumland�r�nda y�nlendirilecek olan adresler.
});

// TODO : AccessDeniedPath sorunu ��z�lecek, 403 sayfas� i�in.

var contentRootPath = builder.Environment.ContentRootPath;

var keysDirectory = new DirectoryInfo(Path.Combine(contentRootPath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("BilgeShop")
    .SetDefaultKeyLifetime(new TimeSpan(99999, 0, 0, 0))
    .PersistKeysToFileSystem(keysDirectory);

// App_Data -> Keys -> i�erisindeki xml dosyas�na sahip her proje ayn� �ifreleme/�ifre a�ma y�ntemi kullanaca��ndan, birbirlerinin �ifrelerini a�abilirler.

var app = builder.Build();

app.UseStaticFiles(); // wwwroot i�in

app.UseAuthentication();
app.UseAuthorization();
// Auth i�lemleri yap�yorsan, �stteki 2 sat�r yaz�lmal�. Yoksa hata vermez fakat oturum a�amaz, yetkilendirme sorgulayamaz.



app.UseStatusCodePagesWithRedirects("/Errors/Error{0}");

// AREA ���N YAZILAN ROUTE HER ZAMAN DEFAULT'UN �ZER�NDE OLACAK

app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"
    );


app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();





