using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.DriverViewModels
{
    public class DriverDeliveryView : Delivery
    {
        public string StatusString { get; set; }
    }
}
