using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.ViewModels
{
    public class OfferCreateViewModel
    {
        public int OfferRequestId { get; set; }
        
        [Display(Name = "İlgili Talep")]
        public string? OfferRequestTitle { get; set; }

        [Required(ErrorMessage = "Teklif fiyatı zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        [Display(Name = "Teklif Fiyatı (₺)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Teslim süresi zorunludur.")]
        [Range(1, 365, ErrorMessage = "Süre 1 ile 365 gün arasında olmalıdır.")]
        [Display(Name = "Teslim Süresi (Gün)")]
        public int DeliveryDays { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Teklif Belgesi (Opsiyonel)")]
        public IFormFile? Document { get; set; }
    }
}
