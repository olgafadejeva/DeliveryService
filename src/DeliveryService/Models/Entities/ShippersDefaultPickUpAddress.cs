using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class ShippersDefaultPickUpAddress : Address
    {
        public int ShipperId { get; set; }
        public virtual Shipper Shipper { get; set; }
    }
}
