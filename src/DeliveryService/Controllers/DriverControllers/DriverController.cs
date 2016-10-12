using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DeliveryService.Controllers.DriverControllers
{
    [RequireHttps]
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
                   .Include(b => b.Vehicles)
                   .Include(c => c.Deliveries)
                        .ThenInclude(d => d.DeliveryStatus)
                    .Include(c=> c.Deliveries)
                        .ThenInclude(d=>d.Client)
                    .Include(c => c.Deliveries)
                        .ThenInclude(d => d.Client.Address)
                    .Include(c=> c.Deliveries)
                        .ThenInclude(d=>d.PickUpAddress)
                   .SingleOrDefault(m => m.User.Id == currentUserId);
            }
        }

        // for testing
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