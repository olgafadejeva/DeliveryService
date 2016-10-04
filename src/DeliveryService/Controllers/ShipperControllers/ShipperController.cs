using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DeliveryService.Models.Entities;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Controllers.ShipperControllers
{
    [Authorize(Roles = "Shipper")]
    [RequireHttps]
    public abstract class ShipperController : Controller
    {
        protected  ApplicationDbContext _context;
        protected HttpContextAccessor _contextAcessor;
        protected string currentUserId;
        protected Shipper shipper;

        public ShipperController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAcessor = (HttpContextAccessor)contextAccessor;
            currentUserId = _contextAcessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null)
            {
                shipper = context.Shippers.Include(b => b.User)
                   .Include(b => b.Clients)
                   .Include(b => b.Deliveries)
                   .Include(b => b.Team)
                   .Include(b => b.User)
                   .Include(b => b.DefaultPickUpAddress)
                   .SingleOrDefault(m => m.User.Id == currentUserId);
            }
        }

        //for testing
        public void setShipper(Shipper shipper)
        {
            this.shipper = shipper;
        }

        public ApplicationDbContext getDbContext()
        {
            return _context;
        }
    }
}