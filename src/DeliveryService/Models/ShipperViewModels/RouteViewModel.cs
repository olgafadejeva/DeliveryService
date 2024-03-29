﻿using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    public class RouteViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Deliver by")]
        [DataType(DataType.Date)]
        public DateTime DeliverBy { get; set; }

        [Display(Name = "Delivery date")]
        [DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Overall route distance")]
        public double? OverallDistance { get; set; }

        [Display(Name = "Estimated route completion time")]
        public double? OverallTimeRequired { get; set; }

        public string RouteStatusString { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        public PickUpAddress PickUpAddress { get; set; }

        public Driver Driver { get; set; }

        [Display(Name = "Driver's vehicle")]
        public Vehicle DriversVehicle { get; set; }


        public RouteViewModel()
        {
            Deliveries = new List<Delivery>();
        }
    }
}
