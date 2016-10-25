using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Team
    {
        [Key]
        public int ID { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<EmployeeUser> Employees { get; set; }

        public Company Company { get; set; }
        public int CompanyID { get; set; }

        public Team() {
            Drivers = new List<Driver>();
            Employees = new List<EmployeeUser>();
        }
    }
}
