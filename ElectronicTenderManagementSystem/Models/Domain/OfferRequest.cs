using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.Domain
{
    public class OfferRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public DateTime Deadline { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int CreatedByUserId { get; set; }

        // Navigation Properties
        public User CreatedByUser { get; set; }
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}