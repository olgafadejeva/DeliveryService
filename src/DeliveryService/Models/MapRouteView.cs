using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class MapRouteView
    {
        public List<string> waypoints { get; set; }

        public string depotAddress { get; set; }

        public string driverAddress { get; set; }

        public string overallRouteTime { get; set; }

        public string routeDistance { get; set; }

        public string scheduledOn { get; set; }

        public string deliverBy { get; set; }

        public MapRouteView()
        {
            waypoints = new List<string>();
        }
    }
}
