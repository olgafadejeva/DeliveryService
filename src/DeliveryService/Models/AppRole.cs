using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class AppRole
    {
        public static string DRIVER = "Driver";
        public static string SHIPPER = "Shipper";
        public static string ADMIN = "Admin";

        public static string[] getAllRoles() {
            return new string[] { DRIVER, SHIPPER, ADMIN };
        }

    }
}
