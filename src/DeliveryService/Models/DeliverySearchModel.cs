using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class DeliverySearchModel
    {
        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        [Display(Name = "Pick up within")]
        public double PickUpWithin { get; set; }

        [Display(Name = "Deliver wihin")]
        public double DeliveryWithin { get; set; }
    }
}
