
using System.ComponentModel.DataAnnotations;

namespace KyFashionHouse.core.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        [StringLength(20)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Teslimat adresi zorunludur.")]
        [StringLength(500)]
        [Display(Name = "Teslimat Adresi")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
