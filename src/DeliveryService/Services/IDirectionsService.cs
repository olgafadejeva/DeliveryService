using DeliveryService.Models;
using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public interface IDirectionsService
    {
        Directions getDirectionsFromAddresses(PickUpAddress pickUpAddress, ClientAddress ClientAddress);
    }
}
