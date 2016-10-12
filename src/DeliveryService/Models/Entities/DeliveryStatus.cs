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
        PickedUpByDriver,
        InTransit,
        Delivered
    }

    public static class StatusExtension
    {
        public static Status NextAvailableStatus(this Status status)
        {
            switch (status)
            {
                case Status.New:
                    return Status.PickedUpByDriver;
                case Status.AcceptedByDriver:
                    return Status.PickedUpByDriver;
                case Status.PickedUpByDriver:
                    return Status.InTransit;
                case Status.InTransit:
                    return Status.Delivered;
                default:
                    return Status.New;
            }
        }

        public static string DisplayName(this Status status)
        {
            switch (status)
            {
                case Status.New:
                    return "New";
                case Status.AcceptedByDriver:
                    return "Accepted";
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
