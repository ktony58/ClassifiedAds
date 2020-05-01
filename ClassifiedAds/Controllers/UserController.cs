using ClassifiedAds.Data;
using ClassifiedAds.Data.Dtos;
using ClassifiedAds.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassifiedAds.Controllers
{
    [Authorize(Roles = nameof(RolesEnum.User))]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var userAdvertisements = await _context.UserAdvertisements
                .Include(ua => ua.Advertisement)
                .Include(ua => ua.User)
                .Where(ua => ua.User == currentUser)
                .ToListAsync();

            return View("MyAdvertisements" ,userAdvertisements);
        }

        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userAdvertisement = await _context.UserAdvertisements
                .Include(ua => ua.Advertisement)
                .Include(ua => ua.User)
                .Where(a => a.Id == id && a.User == currentUser)
                .FirstOrDefaultAsync();
            if (userAdvertisement == null)
            {
                return NotFound();
            }

            return View("ViewMyAdvertisement", userAdvertisement);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userAdvertisement = await _context.UserAdvertisements
                .Include(ua => ua.User)
                .Where(a => a.Id == id && a.User == currentUser)
                .FirstOrDefaultAsync();
            if (userAdvertisement == null)
            {
                return NotFound();
            }

            _context.UserAdvertisements.Remove(userAdvertisement);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Browse()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var myAdvertisements = await _context.UserAdvertisements
                .Include(ua => ua.User)
                .Include(ua => ua.Advertisement)
                .Where(ua => ua.User == currentUser)
                .Select(ua => ua.Advertisement)
                .ToListAsync();

            var openAdvertisements = await _context.Advertisements
                .Where(a => a.IsActive == true && !myAdvertisements.Contains(a))
                .Select(a => new Advertisement
                {
                    Id = a.Id,
                    Address = a.Address,
                    CostPerClick = a.CostPerClick,
                    Title = a.Title,
                    Description = a.Description,
                })
                .ToListAsync();

            return View(openAdvertisements);
        }

        public async Task<IActionResult> ViewAdvertisement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        public async Task<IActionResult> Advertise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            if (advertisement == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            _context.UserAdvertisements.Add(new UserAdvertisement
            {
                Advertisement = advertisement,
                Clicks = 0,
                CostPerClick = advertisement.CostPerClick,
                RedirectId = Guid.NewGuid().ToString(),
                User = currentUser
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}