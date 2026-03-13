using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.ViewModels
{
    public class CompanyProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        [Display(Name = "Şirket Adı")]
        public string CompanyName { get; set; }

        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; }
    }
}
