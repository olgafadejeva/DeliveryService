using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Route
    {
        public int ID { get; set; }

        [Display(Name="Deliver by")]
        [DataType(DataType.Date)]
        public DateTime DeliverBy { get; set; }

        public int? PickUpAddressID { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual PickUpAddress PickUpAddress { get; set; }

        public Route() {
            Deliveries = new List<Delivery>();
        }
    }
}
