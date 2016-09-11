using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Delivery
    {
        private Client Client { get; set; }

        private string Location { get; set; }

        private string Destination { get; set; }

        private Driver PickedUpBy { get; set; }

        private Driver AssignedTo { get; set; }
    }
}
