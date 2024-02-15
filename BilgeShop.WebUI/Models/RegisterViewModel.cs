using System.ComponentModel.DataAnnotations;

namespace BilgeShop.WebUI.Models
{
    public class RegisterViewModel
    {

        [Display(Name = "Ad")]
        [MaxLength(25, ErrorMessage = "Ad en fazla 25 karakter uzunluğunda olabilir.")]
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz.")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        [MaxLength(25, ErrorMessage = "Soyad en fazla 25 karakter uzunluğunda olabilir.")]
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
        public string LastName { get; set; }


        [Display(Name = "Eposta")]
        [Required(ErrorMessage = "Eposta alanı boş bırakılamaz.")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string Password { get; set; }

        [Display(Name = "Şifre tekrar")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz.")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")]
        public string PasswordConfirm { get; set; }



    }
}
