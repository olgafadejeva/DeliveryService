using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Delivery
    {
        public int ID { get; set; }

        public int ClientID { get; set; }
        public int DeliveryStatusID { get; set; }
        public int? RouteID { get; set; }
        
        [Display(Name = "Item size")]
        public ItemSize ItemSize { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeliverBy { get; set; }


        [Display(Name = "Item weight in kg")]
        public double ItemWeight { get; set; }

        public virtual DeliveryStatus DeliveryStatus { get; set; }

        public virtual Client Client { get; set; }

        public Delivery() {
        }
    }

    public enum ItemSize
    {
        Small,
        Medium,
        Large
    }

    public static class ItemSizeDimensionsExtension
    {
        public static DeliveryItemDimensions getItemDimensionsBasedOnSize(this ItemSize size)
        {
            switch (size)
            {
                case ItemSize.Small:
                    return new DeliveryItemDimensions(20, 35, 45);
                case ItemSize.Medium:
                    return new DeliveryItemDimensions(45, 45, 60);
                case ItemSize.Large:
                    return new DeliveryItemDimensions(60, 60, 80);
                default:
                    return new DeliveryItemDimensions(60, 60, 80);
            }
        }

    }
}
