﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class ClientAddress : Address
    {


        public Client Client { get; set; }
        public int ClientId { get; set; }
    }
}
