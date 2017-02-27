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
        
        [Display(Name = "Name")]
        [Required]
        public string VehicleName { get; set; }

        [Display(Name = "Registration number")]
        [MinLength(5, ErrorMessage="Registration number must be longer than 5 characters")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Height (in cm)")]
        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double Height { get; set; }

        [Display(Name = "Width (in cm)")]
        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double Width { get; set; }

        [Display(Name = "Length (in cm)")]
        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double Length { get; set; }


        [Display(Name = "Max load in kg")]
        [Range(0, double.MaxValue, ErrorMessage = "Must be a positive number")]
        public double MaxLoad { get; set; }

        public Vehicle() { }

        public Vehicle(double Load, double Height, double Width, double Length) {
            this.MaxLoad = Load;
            this.Height = Height;
            this.Width = Width;
            this.Length = Length;
        }
    }
}
