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

        public int ClientID { get; set; } 
        public int DeliveryStatusID { get; set; } 
        public int? PickUpAddressID { get; set; } 

        public virtual PickUpAddress PickUpAddress { get; set; }

        public virtual DeliveryStatus DeliveryStatus { get; set; }

        public virtual Client Client { get; set; }
    }
}
