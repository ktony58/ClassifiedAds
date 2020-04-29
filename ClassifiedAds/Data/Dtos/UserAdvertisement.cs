using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassifiedAds.Data.Dtos
{
    public class UserAdvertisement
    {
        public int Id { get; set; }

        [Required]
        public Advertisement Advertisement { get; set; }

        [Required]
        public ApplicationIdentityUser User { get; set; }

        [Required]
        public int Clicks { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal CostPerClick { get; set; }

        [Required]
        public string RedirectId { get; set; }
    }
}
