using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class RouteDetails
    {
        public double OverallDistance { get; set; }
        
        public double OverallTimeRequired { get; set; }

        public RouteDetails(double distance, double duration) {
            this.OverallDistance = distance;
            this.OverallTimeRequired = duration;
        }
    }
}
