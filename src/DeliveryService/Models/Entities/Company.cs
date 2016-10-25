using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Company
    {
        public Company()
        {
            Clients = new List<Client>();
            Deliveries = new List<Delivery>();
            PickUpLocations = new List<PickUpAddress>();
            Team = new Team();
        }

        [Key]
        public int ID { get; set; }

        public string CompanyName { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<PickUpAddress> PickUpLocations { get; set; }
        public virtual Team Team { get; set; }
    }
}
