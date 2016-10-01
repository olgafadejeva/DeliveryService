using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using DeliveryService.Controllers.DriverControllers;

namespace DeliveryService.DriverControllers
{
    public class VehiclesController : DriverController
    {
        public VehiclesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
           
          //  driver = _context.Driver.SingleOrDefault(d => d.User.Id == currentUserId);
            if (driver == null)
            {
                var user = _context.ApplicationUsers.SingleOrDefault(m => m.Id == currentUserId);
                var driverEntity = new Driver();
                driverEntity.User = user;
                _context.Drivers.Add(driverEntity);
                await _context.SaveChangesAsync();
                driver = driverEntity;
            }
            return View(driver.Vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null || !driver.Vehicles.Contains(vehicle))
            {
                return NotFound();
            }

            return View(vehicle);
            
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RegistrationNumber")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                driver.Vehicles.Add(vehicle);
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null || !driver.Vehicles.Contains(vehicle))
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RegistrationNumber")] Vehicle vehicle)
        {
            if (id != vehicle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vehicleEntity = await _context.Vehicles.SingleOrDefaultAsync(m => m.ID == id);
                    vehicleEntity.RegistrationNumber = vehicle.RegistrationNumber;
                    _context.Update(vehicleEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null || !driver.Vehicles.Contains(vehicle))
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.ID == id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.ID == id);
        }
    }
}
