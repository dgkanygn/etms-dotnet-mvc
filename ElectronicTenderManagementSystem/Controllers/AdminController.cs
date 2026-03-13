using ETTS.Data;
using ETTS.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETTS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalCompanies = await _context.CompanyProfiles.CountAsync();
            var openTenders = await _context.OfferRequests.CountAsync(o => o.Deadline > DateTime.UtcNow);
            var monthlyOffers = await _context.Offers.CountAsync(o => o.CreatedAt.Month == DateTime.UtcNow.Month);
            var recentOffers = await _context.Offers
                .Include(o => o.CompanyUser)
                .Include(o => o.OfferRequest)
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalCompanies = totalCompanies;
            ViewBag.OpenTenders = openTenders;
            ViewBag.MonthlyOffers = monthlyOffers;

            return View(recentOffers);
        }

        public async Task<IActionResult> Companies()
        {
            var companies = await _context.CompanyProfiles
                .Include(cp => cp.User)
                .ToListAsync();
            return View(companies);
        }

        public async Task<IActionResult> CompanyDetail(int id)
        {
            var company = await _context.CompanyProfiles
                .Include(cp => cp.User)
                .FirstOrDefaultAsync(cp => cp.Id == id);

            if (company == null) return NotFound();

            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCompany(int id)
        {
            var company = await _context.CompanyProfiles.FindAsync(id);
            if (company != null)
            {
                company.IsActive = !company.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Companies));
        }
    }
}
