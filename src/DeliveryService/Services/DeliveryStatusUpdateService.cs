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
            if (!StatusExtension.NextAvailableStatus(currentDeliveryStatus).Equals(status) ){
                return false;
            }
            delivery.DeliveryStatus.Status = status;
            context.Update(delivery.DeliveryStatus);
            context.SaveChangesAsync();
            return true;
        }
    }
}
