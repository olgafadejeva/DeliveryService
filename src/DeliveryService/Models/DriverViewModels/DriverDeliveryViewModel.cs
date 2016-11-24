using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.DriverViewModels
{
    public class DriverDeliveryViewModel
    {
        public int ID { get; set; }
        
        [Display(Name = "Item size")]
        public string ItemSizeString { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeliverBy { get; set; }
        
        [Display(Name = "Item weight in kg")]
        public double ItemWeight { get; set; }

        public string DeliveryStatusString { get; set; }

        public string ClientName { get; set; }

        public ClientAddress ClientAddress { get; set; }
        
    }
}
