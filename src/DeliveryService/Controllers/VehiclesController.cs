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

namespace DeliveryService.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Driver")]
    public class VehiclesController : Controller
    {
        private ApplicationDbContext _context;
        private readonly HttpContextAccessor _contextAcessor;
        private string currentUserId;
        private Driver driver;

        public VehiclesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAcessor = (HttpContextAccessor)contextAccessor;
            currentUserId = _contextAcessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null)
            {
                driver = context.Drivers.Include(b => b.User)
                   .Include(b => b.Vehicles)
                   .SingleOrDefault(m => m.User.Id == currentUserId);
            }
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
            if (vehicle == null)
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

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            if (vehicle == null)
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

        // for testing
        public void setDriver(Driver driver) {
            this.driver = driver;
        }

        public ApplicationDbContext getDbContext()
        {
            return _context;
        }
    }
}
