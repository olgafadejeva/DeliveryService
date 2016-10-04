using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Models.Entities;
using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class ShipperDashboardController : ShipperController
    {
        public ShipperDashboardController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
        public IActionResult Index()
        {
            return View(shipper);
        }

        [HttpGet]
        public IActionResult AddDefaultPickUpAddress()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDefaultPickUpAddress([Bind("ID,City,LineOne,LineTwo,PostCode")] ShippersDefaultPickUpAddress pickUpAddress)
        {
            if (ModelState.IsValid)
            {
                shipper.DefaultPickUpAddress = pickUpAddress;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Index", shipper);
        }
    }
}