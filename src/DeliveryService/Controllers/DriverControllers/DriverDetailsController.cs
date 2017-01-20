using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models.Entities;
using DeliveryService.Services;

namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverDetailsController : DriverController
    {

        public LocationService googleMaps { get; set; }

        public DriverDetailsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, LocationService googleMapsUtil) : base(context, contextAccessor)
        {
            this.googleMaps = googleMapsUtil;
        }

        public IActionResult Index()
        {
            return View(driver);
        }
        // GET: PickUpLocations/Create
        public IActionResult Create()
        {
            return View();
        }


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


        public async Task<IActionResult> Edit(int? id)
        {
            var driverAddress =  _context.Addresses.Where(m => m.ID == id).SingleOrDefault();
            if (driverAddress == null)
            {
                return NotFound();
            }
            return View(driverAddress);
        }

        // POST: DriverHolidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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