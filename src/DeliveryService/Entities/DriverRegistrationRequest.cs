using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Entities
{
    public class DriverRegistrationRequest
    {
        [Key]
        public int ID { get; set; }

        public int TeamID { get; set; }

        public string DriverEmail { get; set; }

        public virtual Team Team { get; set; }
    }
}
