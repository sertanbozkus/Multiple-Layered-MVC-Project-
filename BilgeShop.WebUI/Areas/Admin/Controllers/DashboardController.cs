using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")] // program.cs'teki area:exists kısmı ile eşleşir.
    [Authorize(Roles = "Admin")] // Claimlerdeki claimTypes.Role ile paralel çalışır. Admin olmayanlar bu controllera istek atamaz.
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }


    }
}
