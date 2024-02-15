using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Error404()
        {
           
            return View();
        }

        public IActionResult Error403()
        {

            return View();
        }
    }
}
