using ClassifiedAds.Data;
using ClassifiedAds.Data.Dtos;
using ClassifiedAds.Data.Enums;
using ClassifiedAds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClassifiedAds.Controllers
{
    [Authorize(Roles = nameof(RolesEnum.Advertiser))]
    public class AdvertiserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public AdvertiserController(ApplicationDbContext context, UserManager<ApplicationIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var masterAdvertisements = await _context.Advertisements
                .Select(ad => new MasterAdvertisementModel
                {
                    Advertisement = ad,
                    TotalClicks = ad.UserAdvertisements.Sum(ua => ua.Clicks),
                    TotalCost = ad.UserAdvertisements.Sum(ua => ua.Clicks * ua.CostPerClick),
                })
                .ToListAsync();

            return View(masterAdvertisements);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Address,CostPerClick")] Advertisement advertisement)
        {
            advertisement.Owner = await _userManager.GetUserAsync(User);
            advertisement.IsActive = true;

            if (ModelState.IsValid)
            {
                _context.Add(advertisement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advertisement);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            return View(advertisement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Address,CostPerClick")] Advertisement advertisement)
        {
            if (id != advertisement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertisement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementExists(advertisement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(advertisement);
        }

        public async Task<IActionResult> Enable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            advertisement.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            advertisement.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementExists(int id)
        {
            return _context.Advertisements.Any(e => e.Id == id);
        }

        private MasterAdvertisementModel GetMasterAdvertisementFromAdvertisement(Advertisement advertisement)
        {
            return new MasterAdvertisementModel
            {
                Advertisement = advertisement,
                TotalClicks = advertisement.UserAdvertisements?.Sum(ua => ua.Clicks) ?? 0,
                TotalCost = advertisement.UserAdvertisements?.Sum(ua => ua.Clicks * ua.CostPerClick) ?? 0,
            };
        }
    }
}
