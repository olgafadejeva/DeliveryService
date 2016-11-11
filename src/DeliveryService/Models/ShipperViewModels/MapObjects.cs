using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    public class MapObjects
    {
        public MapObjects(List<Delivery> deliveries, List<PickUpAddress> depots)
        {
            this.Deliveries = deliveries;
            this.Depots = depots;
        }

        public List<Delivery> Deliveries { get; set; }

        public  List<PickUpAddress> Depots { get; set; }
    }
}
