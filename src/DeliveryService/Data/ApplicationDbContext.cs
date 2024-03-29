﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Entities;
using DeliveryService.Models.ShipperViewModels;
using OpenIddict;

namespace DeliveryService.Data
{
    /*
     * Represents database context created by entity framework. Used to access and modify database entities 
     */ 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<DeliveryStatus> DeliveryStatus { get; set; }
        public DbSet<DriverRegistrationRequest> DriverRegistrationRequests { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public DbSet<PickUpAddress> PickUpAddress { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<DriverHoliday> DriverHolidays { get; set; }


    }
}
