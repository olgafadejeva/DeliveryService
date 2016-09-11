using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models.Entities
{
    public class Team
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Shipper { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
    }
}
