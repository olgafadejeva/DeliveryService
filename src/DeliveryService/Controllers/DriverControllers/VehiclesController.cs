using DeliveryService.Controllers.DriverControllers;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

/*
 * Controller responsible for actions that are undertaken by driver in relation to vehicles
 * 
 * Extends a generic DriverController that allows access to this controller's methods by a user in driver's role
 */ 
namespace DeliveryService.DriverControllers
{
    public class VehiclesController : DriverController
    {
        public VehiclesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        
        public IActionResult Index()
        {
            return View(driver.Vehicles);
        }

        // GET: Vehicles/Create
        /*
         * Returns a creates form for a cehicle
         */ 
        public IActionResult Create()
        {
            return View();
        }
        
        /*
         * Receives the vehicle parameters and creates a vehicle entity
         */ 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RegistrationNumber,Height,Length,Width,MaxLoad,VehicleName")] Vehicle vehicle)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,RegistrationNumber,Length,Width,Height,MaxLoad,VehicleName")] Vehicle vehicle)
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
                    vehicleEntity.Length = vehicle.Length;
                    vehicleEntity.Height = vehicle.Height;
                    vehicleEntity.Width = vehicle.Width;
                    vehicleEntity.MaxLoad = vehicle.MaxLoad;
                    vehicleEntity.VehicleName = vehicle.VehicleName;
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
