using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;
using DeliveryService.Services;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class PickUpLocationsController : ShipperController
    {

        public GoogleMapsUtil googleMaps { get; set; }
        public PickUpLocationsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, GoogleMapsUtil googleMapsUtil ) : base(context, contextAccessor)
        {
            this.googleMaps = googleMapsUtil;
        }

        // GET: PickUpLocations
        public async Task<IActionResult> Index()
        {
            return View(company.PickUpLocations);
        }

        // GET: PickUpLocations/Create
        public IActionResult Create()
        {
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,City,LineOne,LineTwo,PostCode,Default")] PickUpAddress pickUpAddress)
        {
            if (ModelState.IsValid)
            {
                await googleMaps.addLocationDataToAddress(pickUpAddress);
                company.PickUpLocations.Add(pickUpAddress);
                _context.Add(pickUpAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pickUpAddress);
        }

        // GET: PickUpLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickUpAddress = await _context.PickUpAddress.SingleOrDefaultAsync(m => m.ID == id);
            if (pickUpAddress == null)
            {
                return NotFound();
            }
            return View(pickUpAddress);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,City,LineOne,LineTwo,PostCode,Default")] PickUpAddress pickUpAddress)
        {
            if (id != pickUpAddress.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var addressEntity = _context.PickUpAddress.SingleOrDefault(a => a.ID == pickUpAddress.ID);
                    addressEntity.LineOne = pickUpAddress.LineOne;
                    addressEntity.LineTwo = pickUpAddress.LineTwo;
                    addressEntity.City = pickUpAddress.City;
                    addressEntity.PostCode = pickUpAddress.PostCode;
                    _context.Update(addressEntity);
                    _context.SaveChanges();
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PickUpAddressExists(pickUpAddress.ID))
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
            return View(pickUpAddress);
        }

        // GET: PickUpLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickUpAddress = await _context.PickUpAddress.SingleOrDefaultAsync(m => m.ID == id);
            if (pickUpAddress == null)
            {
                return NotFound();
            }

            return View(pickUpAddress);
        }

        // POST: PickUpLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pickUpAddress = await _context.PickUpAddress.SingleOrDefaultAsync(m => m.ID == id);
            _context.PickUpAddress.Remove(pickUpAddress);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PickUpAddressExists(int id)
        {
            return _context.PickUpAddress.Any(e => e.ID == id);
        }
    }
}
