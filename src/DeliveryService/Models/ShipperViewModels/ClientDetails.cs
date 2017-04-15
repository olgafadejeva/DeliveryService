using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.ShipperViewModels
{
    /*
     * This model is populated when a shipper creates a client
     */ 
    public class ClientDetails
    {

        public int ID { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Client's email is required for updating the client on a delivery status")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string AddressLineOne { get; set; }

        [Display(Name = "AddressLine 2")]
        public string AddressLineTwo { get; set; }

        [Required]
        [Display(Name = "Town/City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string PostCode { get; set; }
    }
}
