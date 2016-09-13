using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Vehicle
    {

        [Key]
        public int ID { get; set; }

        [DisplayName("Registration Number")]
        [MinLength(5, ErrorMessage="Registration number must be longer than 5 characters")]
        public string RegistrationNumber { get; set; }
    }
}
