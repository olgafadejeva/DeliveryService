using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class DeliveryStatus
    {
        [Key]
        public int ID { get; set; }

        public int? AssignedToId { get; set; }
        public int? PickedUpById { get; set; }

        public Status Status { get; set; }

        public virtual Driver AssignedTo { get; set; }
        public virtual Driver PickedUpBy { get; set; }
    }

    public enum Status
    {
        New,
        AcceptedByDriver,
        PickUpByDriver,
        InTransit,
        Delivered
    }
}
