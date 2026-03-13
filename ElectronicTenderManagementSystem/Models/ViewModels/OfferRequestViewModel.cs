using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.ViewModels
{
    public class OfferRequestViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Son teklif tarihi zorunludur.")]
        [Display(Name = "Son Teklif Tarihi")]
        public DateTime Deadline { get; set; }
    }
}
