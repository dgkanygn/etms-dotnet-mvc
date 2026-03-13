using ETTS.Models.Domain;

namespace ETTS.Models.ViewModels
{
    public class OfferEvaluateViewModel
    {
        public int OfferId { get; set; }
        public OfferStatus NewStatus { get; set; }
        public string? ReviewNote { get; set; }
    }
}
