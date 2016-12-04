using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    public class MapRouteWithDeliveryViewModelsAsDeliveries
    {
        public List<ShipperSingleDeliveryMapViewModel> Deliveries { get; set; }

        public MapRouteWithDeliveryViewModelsAsDeliveries() {
            Deliveries = new List<ShipperSingleDeliveryMapViewModel>();
        }
    }
}
