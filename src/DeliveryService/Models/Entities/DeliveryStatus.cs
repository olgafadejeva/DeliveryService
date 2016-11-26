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

        public Status Status { get; set; }

        public DateTime? DeliveredDate { get; set; }
    }

    public enum Status
    {
        New,
        PickedUpByDriver,
        InTransit,
        Delivered,
        FailedDelivery
    }

    public static class StatusExtension
    {
        public static List<Status> NextAvailableStatus(this Status status)
        {
            switch (status)
            {
                case Status.New:
                    return new List<Status> { Status.PickedUpByDriver };
                case Status.PickedUpByDriver:
                    return new List<Status> { Status.InTransit };
                case Status.InTransit:
                    return new List<Status> { Status.Delivered };
                case Status.FailedDelivery:
                    return new List<Status> { Status.New };
                default:
                    return new List<Status> { Status.New };
            }
        }

        public static Status statusFromString(this string statusSting) {
            switch (statusSting)
            {
                case "Delivered":
                    return  Status.Delivered;
                case "New":
                    return Status.New;
                case "PickedUpByDriver":
                    return Status.PickedUpByDriver;
                case "InTransit":
                    return Status.InTransit;
                case "FailedDelivery":
                    return Status.FailedDelivery;
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
                case Status.PickedUpByDriver:
                    return "Picked up";
                case Status.InTransit:
                    return "In transit";
                case Status.Delivered:
                    return "Delivered";
                case Status.FailedDelivery:
                    return "Failed delivery";
                default:
                    return "New";
            }
        }
    }
}
