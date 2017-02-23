using System.Linq;
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
   // [RequireHttps]
    public abstract class ShipperController : Controller
    {
        protected  ApplicationDbContext _context;
        protected HttpContextAccessor _contextAcessor;
        protected string currentUserId;
        protected Company company;

        public ShipperController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAcessor = (HttpContextAccessor)contextAccessor;
            currentUserId = _contextAcessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null)
            {
                var applicationUser = context.ApplicationUsers
                    .SingleOrDefault(u => u.Id == currentUserId);
                company = context.Companies
                    .Include(c => c.Clients)
                        .ThenInclude(c => c.Address)
                    .Include(c => c.Deliveries)
                        .ThenInclude(delivery => delivery.Client)
                   .Include(b => b.Deliveries)
                        .ThenInclude(delivery => delivery.DeliveryStatus)
                    .Include(c => c.Routes)
                    .Include(c => c.Team)
                        .ThenInclude(t => t.Drivers)
                        .ThenInclude(d => d.Holidays)
                    .Include(c => c.Team.Drivers)
                        .ThenInclude(t => t.User)
                    .Include(c => c.Team.Drivers)
                        .ThenInclude(t => t.Vehicles)
                    .Include(c => c.Team.Drivers)
                        .ThenInclude(t => t.Address)
                    .Include(c => c.Team)
                        .ThenInclude(c => c.Employees)
                    .Include (c => c.PickUpLocations)
                    .SingleOrDefault(c => applicationUser.CompanyID == c.ID);
            }
        }

        //for testing
        public void setCompany(Company company)
        {
            this.company = company;
        }

        public ApplicationDbContext getDbContext()
        {
            return _context;
        }
    }
}