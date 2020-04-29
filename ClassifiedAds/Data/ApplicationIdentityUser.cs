using Microsoft.AspNetCore.Identity;

namespace ClassifiedAds.Data
{
    public class ApplicationIdentityUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
    }
}
