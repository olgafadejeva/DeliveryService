using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.DriverViewModels
{
    /*
     * A model that carried delivery information in driver's view, a simplification here includes adding a status string 
     * rather than accessing the object from the view
     */ 
    public class DriverDeliveryView : Delivery
    {
        public string StatusString { get; set; }
    }
}
