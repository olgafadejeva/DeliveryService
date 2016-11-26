using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    public class ShipperSingleDeliveryMapViewModel : SingleDeliveryMapViewModel
    {

        public int ID { get; set; }
        
        public Client Client { get; set; }

        public ShipperSingleDeliveryMapViewModel(double locationLat, double locationLng, string clientName, string deliverBy, string status, string address) : base(locationLat, locationLng, clientName, deliverBy, status, address)
        {
        }

        public ShipperSingleDeliveryMapViewModel() { }
    }
}
