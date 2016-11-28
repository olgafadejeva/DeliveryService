using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Driver
    {
        public Driver() {
            Vehicles = new List<Vehicle>();
            Routes = new List<Route>();
            Holidays = new List<DriverHoliday>();
        }

        [Key]
        public int ID { get; set; }

        public DriverAddress Address { get; set; }

        public List<DriverHoliday> Holidays { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Route> Routes { get; set; }
        public virtual DriverUser User { get; set; }

    }
}
