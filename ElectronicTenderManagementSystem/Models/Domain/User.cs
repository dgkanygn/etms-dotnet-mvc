using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        public UserRole Role { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public CompanyProfile CompanyProfile { get; set; }
        public ICollection<OfferRequest> CreatedOfferRequests { get; set; } = new List<OfferRequest>();
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}