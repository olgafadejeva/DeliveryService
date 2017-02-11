using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.AndroidApi
{
    public class DriverInformation
    {
        public String email { get; set; }
        public DriverAddress driverAddress { get; set; }
        public List<DriverHoliday> holidays { get; set; }
    }
}
