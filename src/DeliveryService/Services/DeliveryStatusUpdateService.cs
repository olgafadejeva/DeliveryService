using DeliveryService.Data;
using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public class DeliveryStatusUpdateService
    {

        private ApplicationDbContext context { get; set; }

        public DeliveryStatusUpdateService(ApplicationDbContext context) {
            this.context = context;
        }

        public bool UpdateDeliveryStatus(Delivery delivery, Status status) {
            Status currentDeliveryStatus = delivery.DeliveryStatus.Status;
            if (!StatusExtension.NextAvailableStatus(currentDeliveryStatus).Contains(status) ){
                return false;
            }
          //  delivery.DeliveryStatus.Status = status;
            var deliveryStatus = context.DeliveryStatus.SingleOrDefault(st => st.ID == delivery.DeliveryStatus.ID);
            deliveryStatus.Status = status;
            Route route = context.Routes.Where(r => r.ID == delivery.RouteID).SingleOrDefault();
            bool allDelivered = true;
            foreach (Delivery del in route.Deliveries)
            {
                if (del.DeliveryStatus.Status != Status.Delivered)
                {
                    allDelivered = false;
                    break;
                }
            }

            if (status.Equals(Status.FailedDelivery))
            {
                route.Status = RouteStatus.Pending;
            }
            else
            {
                if (allDelivered)
                {
                    route.Status = RouteStatus.Completed;
                }
                else
                {
                    route.Status = RouteStatus.InProgress;
                }
            }
            
            context.SaveChangesAsync();
            return true;
        }
    }
}
