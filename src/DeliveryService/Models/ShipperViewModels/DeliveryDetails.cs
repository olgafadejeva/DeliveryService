using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DeliveryService.Models.Entities;

namespace DeliveryService.Models.ShipperViewModels
{
    /*
     * Model that gets populated when shipper creates a delivery
     */ 
    public class DeliveryDetails 
    {

        public int ID { get; set; }
        public int ClientID { get; set; } //foreign key to client 
        
        [Display(Name = "Item size")]
        public ItemSize ItemSize { get; set; }


        [Display(Name = "Item weight in kg")]
        public double ItemWeight { get; set; }

        [Display(Name = "Deliver by")]
        [DataType(DataType.Date)]
        public DateTime DeliverBy { get; set; }


    }
}
