using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Client
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Client's email is required for updating the client on a delivery status")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ClientAddress Address { get; set; }

        public Client() {
           Address = new ClientAddress();
        }
    }
}
