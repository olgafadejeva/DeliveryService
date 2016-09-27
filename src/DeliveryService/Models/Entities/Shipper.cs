using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Shipper
    {
        public Shipper() {
            Clients = new List<Client>();
            Deliveries = new List<Delivery>();
        }

        [Key]
        public int ID { get; set; }
        

        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Team Team { get; set; }
    }
}
