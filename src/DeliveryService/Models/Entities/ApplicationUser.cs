using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DeliveryService.Models.Entities;
using System.Linq;

namespace DeliveryService.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
    }
}
