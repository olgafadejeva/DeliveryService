using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;
using DeliveryService.Services;
using DeliveryService.Models.ShipperViewModels;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class RoutesController : ShipperController
    {

        public RouteCreationService routeService { get; set; }
        public RoutesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, RouteCreationService routeService) : base(context, contextAccessor)
        {
            this.routeService = routeService;
        }

        public async Task<IActionResult> Index()
        {
            List<Route> routes = company.Routes.OrderBy(r => r.DeliveryDate).ToList();
            List<RouteViewModel> viewModels = new List<RouteViewModel>();
            foreach (Route route in routes) {
                RouteViewModel model = new RouteViewModel();
                model.ID = route.ID;
                model.OverallDistance = route.OverallDistance;
                model.OverallTimeRequired = route.OverallTimeRequired;
                model.PickUpAddress = route.PickUpAddress;
                model.Driver = company.Team.Drivers.Where(d => d.ID == route.DriverID).FirstOrDefault();
                model.DeliverBy = route.DeliverBy;
                model.DeliveryDate = route.DeliveryDate;
                viewModels.Add(model);
            }

            return View(viewModels);
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.SingleOrDefaultAsync(m => m.ID == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.SingleOrDefaultAsync(m => m.ID == id);
            if (route == null)
            {
                return NotFound();
            }
            ViewData["AssignedToID"] = new SelectList(_context.Drivers, "ID", "ID", route.DriverID);
            ViewData["PickUpAddressID"] = new SelectList(_context.PickUpAddress, "ID", "City", route.PickUpAddressID);
            return View(route);
        }

        public async Task<IActionResult> Reassign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.SingleOrDefaultAsync(m => m.ID == id);
            if (route == null)
            {
                return NotFound();
            }


            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Driver driver in company.Team.Drivers) {
                list.Add(new SelectListItem() { Value = driver.ID.ToString(), Text = driver.User.NormalizedUserName });
            }
               


            ViewData["AssignedToID"] = new SelectList(list, "Value", "Text", route.DriverID);
            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reassign(int id, [Bind("ID,AssignedToID,DeliverBy,DeliveryDate,OverallDistance,OverallTimeRequired,PickUpAddressID,VehicleID")] Route route)
        {

            Route companyRoute = company.Routes.Where(r => r.ID == id).First();
            companyRoute.DriverID = route.DriverID;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AssignedToID,DeliverBy,DeliveryDate,OverallDistance,OverallTimeRequired,PickUpAddressID,VehicleID")] Route route)
        {
            if (id != route.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.ID))
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
            ViewData["AssignedToID"] = new SelectList(_context.Drivers, "ID", "ID", route.DriverID);
            ViewData["PickUpAddressID"] = new SelectList(_context.PickUpAddress, "ID", "City", route.PickUpAddressID);
            return View(route);
        }

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.SingleOrDefaultAsync(m => m.ID == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        public IActionResult RouteDeliveries(int? id) {
            Route route = company.Routes.Where(r => r.ID == id).FirstOrDefault();
            return View(route.Deliveries);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.SingleOrDefaultAsync(m => m.ID == id);
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDeliveryFromRoute(int id)
        {
            var delivery = _context.Deliveries.Where(d => d.ID == id).First();
            var route = company.Routes.Where(r => r.ID == delivery.RouteID).First();
            route.Deliveries.Remove(delivery);
            await routeService.updateRouteDetails(route);
            await _context.SaveChangesAsync();
            return RedirectToAction("RouteDeliveries", new { id = route.ID });
        }



        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.ID == id);
        }
    }
}