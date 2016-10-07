using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverDeliveriesController : DriverController
    {
        public DriverDeliveriesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor) { }

        public IActionResult Index()
        {
            return View(driver.Deliveries);
        }
    }
}