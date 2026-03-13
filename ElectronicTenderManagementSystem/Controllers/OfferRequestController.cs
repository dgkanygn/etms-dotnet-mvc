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
    public class OfferRequestController : Controller
    {
        private readonly AppDbContext _context;

        public OfferRequestController(AppDbContext context)
        {
            _context = context;
        }

        // Herkes görebilir
        public async Task<IActionResult> Index()
        {
            var requests = await _context.OfferRequests
                .Include(or => or.CreatedByUser)
                .OrderByDescending(or => or.Deadline)
                .ToListAsync();
            return View(requests);
        }

        // Herkes görebilir
        public async Task<IActionResult> Detail(int id)
        {
            var request = await _context.OfferRequests
                .Include(or => or.CreatedByUser)
                .Include(or => or.Offers)
                    .ThenInclude(o => o.CompanyUser)
                .FirstOrDefaultAsync(or => or.Id == id);

            if (request == null) return NotFound();

            return View(request);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(OfferRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                
                var request = new OfferRequest
                {
                    Title = model.Title,
                    Description = model.Description,
                    Deadline = model.Deadline,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.OfferRequests.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var request = await _context.OfferRequests.FindAsync(id);
            if (request == null) return NotFound();

            var model = new OfferRequestViewModel
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Deadline = request.Deadline
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(OfferRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = await _context.OfferRequests.FindAsync(model.Id);
                if (request == null) return NotFound();

                request.Title = model.Title;
                request.Description = model.Description;
                request.Deadline = model.Deadline;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.OfferRequests.FindAsync(id);
            if (request != null)
            {
                _context.OfferRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
