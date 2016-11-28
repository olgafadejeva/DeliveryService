using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public abstract class Address
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Address Line 1")]
        [Required]
        public string LineOne { get; set; }

        [Display(Name = "Address Line 2")]
        public string LineTwo { get; set; }

        [Display(Name = "Town/City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Postcode")]
        [Required]
        [RegularExpression(@"^([a-zA-Z]{1,2})([0-9][0-9a-zA-Z]?) ([0-9])([a-zA-Z]{2})$", ErrorMessage = "Must be a valid UK postcode")]
        public string PostCode { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public Address(string LineOne, string LineTwo, string City, string PostCode) {
            this.LineOne = LineOne;
            this.LineTwo = LineTwo;
            this.City = City;
            this.PostCode = PostCode;
        }

        public Address() { }
    }
}
