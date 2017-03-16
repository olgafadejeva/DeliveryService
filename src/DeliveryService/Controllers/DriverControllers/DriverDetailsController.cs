using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models.Entities;
using DeliveryService.Services;

/*
 * Driver controller that os responsive for actions that involve driver details updates and adding of holidays 
 * 
 * Extends a generic DriverController that allows access to this controller's methods by a user in driver's role
 */  
namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverDetailsController : DriverController
    {

        public LocationService googleMaps { get; set; }

        //inject GoogleMapsUtil for getting/updating coordinates when address is added or edited 
        public DriverDetailsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, LocationService googleMapsUtil) : base(context, contextAccessor)
        {
            this.googleMaps = googleMapsUtil;
        }

        /*
         * Returns the index page with the driver as a model for displaying driver's holidays
         */ 
        public IActionResult Index()
        {
            return View(driver);
        }

        /*
         * Returns the view with a form for a driver to create an address
         */ 
        public IActionResult Create()
        {
            return View();
        }

        /*
         * Receives a form with newly created driver's address and creates an entity
         */ 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,City,LineOne,LineTwo,PostCode")] DriverAddress driverAddress)
        {
            if (ModelState.IsValid)
            {
                await googleMaps.addLocationDataToAddress(driverAddress);
                driver.Address = driverAddress;
                _context.Addresses.Add(driverAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(driverAddress);
        }

        /*
         * Returns the screen for editing driver's address
         */ 
        public IActionResult Edit(int? id)
        {
            var driverAddress = _context.Addresses.Where(m => m.ID == id).SingleOrDefault();
            if (driverAddress == null)
            {
                return NotFound();
            }
            return View(driverAddress);
        }

        /*
         * Receives the form of address corrections and updates it
         */ 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DriverAddress driverAddress)
        {

            if (ModelState.IsValid)
            {
                DriverAddress address = driver.Address;
                address.City = driverAddress.City;
                address.PostCode = driverAddress.PostCode;
                address.LineOne = driverAddress.LineOne;
                address.LineTwo = driverAddress.LineTwo;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(driverAddress);
        }
    }
}
