﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DeliveryService.Models.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryService.Models.ShipperViewModels
{
    public class DeliveryDetails 
    {
        public int ClientID { get; set; } //foreign key to client 

        public bool useDefaultDeliveryAddress { get; set; }

        public PickUpAddress PickUpAddress { get; set; }


        [Display(Name = "Item size")]
        public ItemSize ItemSize { get; set; }


        [Display(Name = "Item weight in kg")]
        public double ItemWeight { get; set; }


    }
}
