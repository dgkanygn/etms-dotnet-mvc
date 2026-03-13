using System;
using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.Domain
{
    public class Offer
    {
        public int Id { get; set; }
        
        public int OfferRequestId { get; set; }
        public int CompanyUserId { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int DeliveryDays { get; set; }
        
        public string? Description { get; set; }
        public string? DocumentPath { get; set; }
        
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public OfferRequest OfferRequest { get; set; }
        public User CompanyUser { get; set; }
    }
}