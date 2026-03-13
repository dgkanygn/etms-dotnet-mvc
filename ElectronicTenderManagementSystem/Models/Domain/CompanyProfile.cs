using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.Domain
{
    public class CompanyProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [Required]
        public string CompanyName { get; set; }
        
        public string Phone { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Property
        public User User { get; set; }
    }
}