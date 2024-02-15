using System.ComponentModel.DataAnnotations;

namespace BilgeShop.WebUI.Areas.Admin.Models
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Kategori adı girmek zorunludur.")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        // ? ile validation sırasında required/zorunlu olmadığını belirtiyorum.
        // ? yazılmayan property default olarak requireddır.
        // Name için Required'ı özel olarak yazmamdaki neden hata mesajını özelleştirmek.
    }
}
