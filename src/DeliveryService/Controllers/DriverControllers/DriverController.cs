using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

/*
 * This is the main controller that loads the driver entity as a current user and all its related entities
 * The methods here are acessed by a user in driver's role only 
 */
namespace DeliveryService.Controllers.DriverControllers
{
    //[RequireHttps]
    [Authorize(Roles = "Driver")]
    public abstract class DriverController : Controller
    {
        protected ApplicationDbContext _context;
        protected readonly HttpContextAccessor _contextAcessor;
        protected string currentUserId;
        protected Driver driver;

        public DriverController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAcessor = (HttpContextAccessor)contextAccessor;
            currentUserId = _contextAcessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null)
            {
                driver = context.Drivers
                   .Include(b => b.User)
                   .Include(b => b.Address)
                   .Include(b => b.Holidays)
                   .Include(b => b.Vehicles)
                   .Include(c => c.Routes)
                        .ThenInclude(d => d.Deliveries)
                    .Include(c=> c.Routes)
                        .ThenInclude(d=>d.PickUpAddress)
                    .Include(c => c.Routes)
                        .ThenInclude(d=>d.Deliveries)
                            .ThenInclude(d=>d.Client)
                                .ThenInclude( c=> c.Address)
                    .Include(c => c.Routes)
                        .ThenInclude(d => d.Deliveries)
                            .ThenInclude(d => d.DeliveryStatus)
                   .SingleOrDefault(m => m.User.Id == currentUserId);
            }
        }

        // for testing only
        public void setDriver(Driver driver)
        {
            this.driver = driver;
        }

        public ApplicationDbContext getDbContext()
        {
            return _context;
        }
    }
}