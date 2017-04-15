using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class TempRoute
    {

        public int RouteId { get; set; }

        public DateTime ModifiedDeliverByDate { get; set; }

        public  Vehicle DriversVehicle { get; set; }

        public  Driver Driver { get; set; }
    }
}
