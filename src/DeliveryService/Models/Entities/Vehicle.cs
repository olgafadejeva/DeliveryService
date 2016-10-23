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

        [Display(Name = "Registration number")]
        [MinLength(5, ErrorMessage="Registration number must be longer than 5 characters")]
        public string RegistrationNumber { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double Height { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double Width { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double Length { get; set; }


        [Display(Name = "Max load in kg")]
        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double MaxLoad { get; set; }
    }
}
