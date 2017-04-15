using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    /*
     * Model gets populated when a driver adds their details
     */ 
    public class DriverDetails
    {
        [Required]
        [Display(Name = "First name")]
        public String FirstName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }
}
