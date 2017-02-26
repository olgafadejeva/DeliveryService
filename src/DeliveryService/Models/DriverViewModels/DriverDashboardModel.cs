using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.DriverViewModels
{
    public class DriverDashboardModel
    {
        public Driver Driver { get; set; }
        public List<MapRouteView> routesModel { get; set; }
        public List<PickUpAddress> depots { get; set; }
    }
}
