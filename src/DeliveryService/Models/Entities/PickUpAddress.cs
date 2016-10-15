using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class PickUpAddress : Address
    {
        public PickUpAddress(string LineOne, string LineTwo, string City, string PostCode) : base(LineOne, LineTwo, City, PostCode)
        {
        }

        public PickUpAddress() { }
    }
}
