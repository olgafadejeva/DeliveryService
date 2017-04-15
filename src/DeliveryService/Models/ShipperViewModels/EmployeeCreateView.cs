using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    /*
     * This model is populated when a shipper creates an employee
     */ 
    public class EmployeeCreateView
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "E-mail address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
