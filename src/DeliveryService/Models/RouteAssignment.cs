using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class RouteAssignment
    {
        public List<Route> Routes { get; set; }
        public List<TempRoute> TempRouteData { get; set; }

        public RouteAssignment(List<Route> routes, List<TempRoute> tempRoutes) {
            this.Routes = routes;
            this.TempRouteData = tempRoutes;
        }
    }
}
