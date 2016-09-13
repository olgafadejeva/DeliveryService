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

        private string FirstName { get; set; }

        private string LastName { get; set; }

        private string Email { get; set; }
    }
}
