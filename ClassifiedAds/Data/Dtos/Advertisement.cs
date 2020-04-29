using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassifiedAds.Data.Dtos
{
    public class Advertisement
    {
        public int Id { get; set; }

        [Required]
        public ApplicationIdentityUser Owner { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [Display(Name = "Current Cost Per Click")]
        public decimal CostPerClick { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public IEnumerable<UserAdvertisement> UserAdvertisements { get; set; }
    }
}

