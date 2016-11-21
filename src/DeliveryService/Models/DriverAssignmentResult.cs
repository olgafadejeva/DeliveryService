using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class DriverAssignmentResult
    {
        public int RouteID { get; set; }

        public DateTime DeliverByDate { get; set; }

        public Vehicle Vehicle { get; set; }

        public Driver Driver { get; set; }

        public double AssignmentProfit { get; set; }

        public DriverAssignmentResult(int id, DateTime deliverByDate, Driver driver) {
            this.RouteID = id;
            this.DeliverByDate = deliverByDate;
            this.Driver = driver;
        }

        public DriverAssignmentResult() { }
    }
}
