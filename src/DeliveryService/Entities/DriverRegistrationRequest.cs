using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Entities
{
    /*
     * An entity that keeps a reference to a team when a driver registration request has been sent out
     */ 
    public class DriverRegistrationRequest
    {
        [Key]
        public int ID { get; set; }

        public int TeamID { get; set; }

        public string DriverEmail { get; set; }

        public virtual Team Team { get; set; }
    }
}
