using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BilgeShop.WebUI.Controllers
{
    // Authentication and Authorization
    // (Kimlik Doğrulama - Yetkilendirme)
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{controller}/{action}")]
        [Route("KayitOl")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("{controller}/{action}")]
        [Route("KayitOl")]
        public IActionResult Register(RegisterViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData); 
            }

            var userAddDto = new UserAddDto()
            {
                FirstName = formData.FirstName.Trim(),
                LastName = formData.LastName.Trim(),
                Email = formData.Email.Trim(),
                Password = formData.Password
            };

            var result = _userService.AddUser(userAddDto);

            if (result.IsSucceed)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View(formData);
            }

            
        }


        public async Task<IActionResult> Login(LoginViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var loginDto = new UserLoginDto()
            {
                Email = formData.Email,
                Password = formData.Password
            };

            var userInfo = _userService.LoginUser(loginDto);

            if(userInfo is null)
            {
                // Uyarı mesajı verilebilir.
                return RedirectToAction("Index", "Home");
            }

            // Buraya kadar geldiyse demek ki oturum açabilirim.

            var claims = new List<Claim>();

            claims.Add(new Claim("id", userInfo.Id.ToString()));
            claims.Add(new Claim("email", userInfo.Email));
            claims.Add(new Claim("firstName", userInfo.FirstName));
            claims.Add(new Claim("lastName", userInfo.LastName));
            claims.Add(new Claim("userType", userInfo.UserType.ToString()));

            // YETKILENDIRME (AUTHORIZATION ICIN) GEREKLI OLAN ALTTAKI KOD
            claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString())); // ClaimTypes.Role -> .net içerisinde authorization mekanizması ile paralel çalışacak.

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var autProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48))
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), autProperties);

            TempData["SuccessMessage"] = "Kullanıcı girişi yapıldı.";


            return RedirectToAction("Index", "Home");

            
        
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            TempData["SuccessMessage"] = "Oturum sonlandırıldı.";
            return RedirectToAction("Index", "Home");
        }
    }


    
}
