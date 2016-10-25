﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Driver
    {
        public Driver() {
            Vehicles = new List<Vehicle>();
            Deliveries = new List<Delivery>();
        }

        [Key]
        public int ID { get; set; }
        
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual DriverUser User { get; set; }

    }
}
