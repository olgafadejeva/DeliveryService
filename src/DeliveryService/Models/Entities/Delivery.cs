using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Delivery
    {
        
        public int ID { get; set; }

        private int ClientID { get; set; } //foreign key to client 

        private string Location { get; set; }

        private string Destination { get; set; }

        public virtual Client Client { get; set; }
    }
}
