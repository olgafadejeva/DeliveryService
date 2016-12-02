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

        [Display(Name = "Deliver by")]
        [DataType(DataType.Date)]
        public DateTime DeliverBy { get; set; }

        [Display(Name = "Delivery date")]
        [DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }

        public int? PickUpAddressID { get; set; }

        [Display(Name = "Overall route distance")]
        public double? OverallDistance { get; set; }

        public RouteStatus? Status { get; set; }

        [Display(Name = "Estimated route completion time")]
        public double? OverallTimeRequired { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual PickUpAddress PickUpAddress { get; set; }

        public int? DriverID { get; set; }

        public int? VehicleID { get; set; }


        public Route() {
            Deliveries = new List<Delivery>();
        }
    }

    public enum RouteStatus
    {
        New,
        InProgress,
        Completed,
        Pending
    }

    public static class RouteStatusExtension
    {

        public static string DisplayName(this RouteStatus status)
        {
            switch (status)
            {
                case RouteStatus.New:
                    return "New";
                case RouteStatus.InProgress:
                    return "In progress";
                case RouteStatus.Completed:
                    return "Completed";
                case RouteStatus.Pending:
                    return "Pending";
                default:
                    return "New";
            }
        }
    }
}
