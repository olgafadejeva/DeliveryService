using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Models;
using DeliveryService.Models.Entities;

namespace DeliveryService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
          //  base.Database.EnsureCreated();
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DefaultConnection, Configuration>());
            base.OnModelCreating(builder);
            
           
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<DeliveryStatus> DeliveryStatus { get; set; }

    }
}
