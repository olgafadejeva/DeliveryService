using System;
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
           // Team = new Team();
        }

        [Key]
        public int ID { get; set; }

        public int? TeamID { get; set; }

        // public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Team Team { get; set; }

    }
}
