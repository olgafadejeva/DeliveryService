using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using DeliveryService.Services;
using Microsoft.Extensions.Options;

namespace DeliveryService.Controllers.DriverControllers
{
    [RequireHttps]
    [Authorize(Roles = "Driver")]
    public class DeliverySearchController : DriverController
    {
        public AppProperties options { get; set; }

        public DeliverySearchController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IOptions<AppProperties> optionsAccessor) : base(context, contextAccessor) {
            options = optionsAccessor.Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SearchForDeliveries(double Latitude, double Longtitude, double PickUpWithin, double DeliveryWithin)
        {
            return null;
        }
    }
}