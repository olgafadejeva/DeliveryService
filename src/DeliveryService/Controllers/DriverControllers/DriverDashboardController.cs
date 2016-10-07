using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DeliveryService.Data;

namespace DeliveryService.Controllers.DriverControllers
{   
    [Authorize(Roles = "Driver")]
    public class DriverDashboardController : DriverController
    {
        public DriverDashboardController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor) { }

        public IActionResult Index()
        {
            return View(driver);
        }


    }
}