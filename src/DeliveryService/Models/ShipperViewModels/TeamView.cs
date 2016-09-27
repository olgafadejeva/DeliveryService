using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    public class TeamView
    {

        [DisplayName("Company name")]
        public string CompanyName { get; set; }


        [DisplayName("Company description")]
        public string Description { get; set; }


        [DisplayName("Drivers")]
        public IList<string> Drivers { get; set; }
    }
}
