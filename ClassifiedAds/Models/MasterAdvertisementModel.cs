using ClassifiedAds.Data.Dtos;
using System.ComponentModel.DataAnnotations;

namespace ClassifiedAds.Models
{
    public class MasterAdvertisementModel
    {
        public Advertisement Advertisement { get; set; }

        [Display(Name = "Clicks")]
        public int TotalClicks { get; set; }

        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }
    }
}
