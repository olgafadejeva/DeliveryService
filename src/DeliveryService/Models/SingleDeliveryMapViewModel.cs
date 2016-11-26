using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class SingleDeliveryMapViewModel
    {
        public double locationLat { get; set; }
        public double locationLng { get; set; }

        public string deliverBy { get; set; }

        public string currentStatus { get; set; }

        public string clientName { get; set; }

        public string addressString { get; set; }

        public SingleDeliveryMapViewModel() { }

        public SingleDeliveryMapViewModel(double locationLat, double locationLng, string clientName, string deliverBy, string status, string address)
        {
            this.locationLat = locationLat;
            this.locationLng = locationLng;
            this.clientName = clientName;
            this.deliverBy = deliverBy;
            this.currentStatus = status;
            this.addressString = address;
        }
    }
}
