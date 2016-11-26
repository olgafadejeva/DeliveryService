using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    public class MapObjects
    {
        private object routes;

        public MapObjects(List<Delivery> deliveries, List<Route> routes, List<ShipperSingleDeliveryMapViewModel> delWithAddress)
        {
            this.Deliveries = deliveries;
            this.ExistingRoutes = routes;
            this.DeliveriesWithAddressString = delWithAddress;
        }

        public MapObjects(List<Delivery> deliveries, List<PickUpAddress> depots, List<Route> routes, List<ShipperSingleDeliveryMapViewModel> delWithAddress)
        {
            this.Deliveries = deliveries;
            this.Depots = depots;
            this.ExistingRoutes = routes;
            this.DeliveriesWithAddressString = delWithAddress;
        }

        public List<Delivery> Deliveries { get; set; }
        public List<ShipperSingleDeliveryMapViewModel> DeliveriesWithAddressString { get; set; }

        public  List<PickUpAddress> Depots { get; set; }
        public List<Route> ExistingRoutes { get; set; }
    }
}
