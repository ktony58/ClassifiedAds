using ClassifiedAds.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassifiedAds.Controllers
{
    [Route("tracking")]
    [ApiController]
    [AllowAnonymous]
    public class TrackingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrackingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string guid)
        {
            var userAdvertisement = _context.UserAdvertisements
                .Include(ua => ua.Advertisement)
                .SingleOrDefault(ua => string.Equals(ua.RedirectId, guid));

            if (userAdvertisement == null)
            {
                return NotFound();
            }

            userAdvertisement.Clicks++;
            await _context.SaveChangesAsync();

            var uri = new UriBuilder(userAdvertisement.Advertisement.Address);

            return RedirectPermanent(uri.ToString());
        }
    }
}
