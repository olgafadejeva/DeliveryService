using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    public class DeliveryViewModelWithAddressString
    {
        public int ID { get; set; }

        public  DeliveryStatus DeliveryStatus { get; set; }

        public  Client Client { get; set; }
        public string ClientAddressString { get; set; }
    }
}
