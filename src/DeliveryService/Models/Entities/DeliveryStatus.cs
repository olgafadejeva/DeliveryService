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
        ClaimedByDriver,
        PickedUpByDriver,
        InTransit,
        Delivered
    }

    public static class StatusExtension
    {
        public static List<Status> NextAvailableStatus(this Status status)
        {
            switch (status)
            {
                case Status.New:
                    return new List<Status> { Status.PickedUpByDriver, Status.ClaimedByDriver };
                case Status.ClaimedByDriver:
                    return new List<Status> { Status.PickedUpByDriver, Status.ClaimedByDriver };
                case Status.PickedUpByDriver:
                    return new List<Status> { Status.InTransit };
                case Status.InTransit:
                    return new List<Status> { Status.Delivered };
                default:
                    return new List<Status> { Status.New };
            }
        }

        public static string DisplayName(this Status status)
        {
            switch (status)
            {
                case Status.New:
                    return "New";
                case Status.ClaimedByDriver:
                    return "Claimed by driver";
                case Status.PickedUpByDriver:
                    return "Picked up";
                case Status.InTransit:
                    return "Delivered";
                default:
                    return "New";
            }
        }
    }
}
