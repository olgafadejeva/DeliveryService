using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Models.Entities;
using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        
        public IActionResult EditDefaultPickUpAddress()
        {
            ShippersDefaultPickUpAddress address = shipper.DefaultPickUpAddress;
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDefaultPickUpAddress(int id, [Bind("ID,City,LineOne,LineTwo,PostCode")] ShippersDefaultPickUpAddress pickUpAddress)
        {
            if (id != shipper.DefaultPickUpAddress.ID)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    shipper.DefaultPickUpAddress = pickUpAddress;
                    var addressEntity = await _context.Addresses.SingleOrDefaultAsync(m => m.ID == id);
                    addressEntity.City = pickUpAddress.City;
                    addressEntity.LineOne = pickUpAddress.LineOne;
                    addressEntity.LineTwo = pickUpAddress.LineTwo;
                    addressEntity.PostCode = pickUpAddress.PostCode;
                    //_context.Attach(addressEntity);
                    
                    _context.Update(addressEntity);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View("Index", shipper);
        }
    }
}