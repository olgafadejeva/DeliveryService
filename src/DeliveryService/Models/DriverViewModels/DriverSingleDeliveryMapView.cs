using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.DriverViewModels
{
    public class DriverSingleDeliveryMapView :SingleDeliveryMapViewModel
    {

        public DriverSingleDeliveryMapView(double locationLat, double locationLng, string clientName, string deliverBy, string status, string address)
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
