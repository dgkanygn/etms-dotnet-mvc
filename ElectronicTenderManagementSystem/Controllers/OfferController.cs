using ETTS.Data;
using ETTS.Models.Domain;
using ETTS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ETTS.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public OfferController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<IActionResult> Create(int requestId)
        {
            var request = await _context.OfferRequests.FindAsync(requestId);
            if (request == null || request.Deadline < DateTime.UtcNow)
            {
                return BadRequest("Geçersiz talep veya süresi dolmuş.");
            }

            var model = new OfferCreateViewModel
            {
                OfferRequestId = requestId,
                OfferRequestTitle = request.Title
            };

            return View(model);
        }

        [Authorize(Roles = "Company")]
        [HttpPost]
        public async Task<IActionResult> Create(OfferCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                
                // Aynı talebe mükerrer teklif kontrolü (Opsiyonel)
                var existingOffer = await _context.Offers.FirstOrDefaultAsync(o => o.OfferRequestId == model.OfferRequestId && o.CompanyUserId == userId);
                if (existingOffer != null)
                {
                    ModelState.AddModelError("", "Bu talebe zaten teklif vermişsiniz.");
                    return View(model);
                }

                var offer = new Offer
                {
                    OfferRequestId = model.OfferRequestId,
                    CompanyUserId = userId,
                    Price = model.Price,
                    DeliveryDays = model.DeliveryDays,
                    Description = model.Description,
                    Status = OfferStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                // Dosya Yükleme
                if (model.Document != null && model.Document.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Document.CopyToAsync(fileStream);
                    }

                    offer.DocumentPath = "/uploads/" + uniqueFileName;
                }

                _context.Offers.Add(offer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyOffers));
            }
            return View(model);
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> MyOffers()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var offers = await _context.Offers
                .Include(o => o.OfferRequest)
                .Where(o => o.CompanyUserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            return View(offers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Evaluate(OfferEvaluateViewModel model)
        {
            var offer = await _context.Offers.FindAsync(model.OfferId);
            if (offer != null)
            {
                offer.Status = model.NewStatus;
                // Değerlendirme notu gerekirse Domain modeline eklenmeli. 
                // Şimdilik sadece statü güncelliyoruz.
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Detail", "OfferRequest", new { id = offer?.OfferRequestId });
        }
    }
}
