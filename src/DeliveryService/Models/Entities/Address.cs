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

        [Display(Name = "AddressLine 2")]
        public string LineTwo { get; set; }

        [Display(Name = "Town/City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Postcode")]
        [Required]
        public string PostCode { get; set; }
    }
}
