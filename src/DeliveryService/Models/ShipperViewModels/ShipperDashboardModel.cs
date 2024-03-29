﻿using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    /*
     * Shipper's dashboard model with routes, depots and company information
     */ 
    public class ShipperDashboardModel
    {
        public Company company { get; set; }
        public List<MapRouteView> routesModel { get; set; }
        public List<PickUpAddress> depots { get; set; }
    }
}
