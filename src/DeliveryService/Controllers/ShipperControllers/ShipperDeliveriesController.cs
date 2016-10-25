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
using DeliveryService.Models.ShipperViewModels;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class ShipperDeliveriesController : ShipperController
    {

        public ShipperDeliveriesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        // GET: ShipperDeliveries
        public async Task<IActionResult> Index()
        {
           // Delivery[] deliveries = await getShippersDeliveries();
            return View(company.Deliveries);
        }

        private async Task<Delivery[]> getShippersDeliveries()
        {
            var deliveries = company.Deliveries.ToArray();

            for (int i = 0; i < deliveries.Length; ++i)
            {
                deliveries[i] = await getDelivery(deliveries[i].ID);
                deliveries[i].DeliveryStatus = await getDeliveryStatus(deliveries[i].DeliveryStatus.ID);
            }

            return deliveries;
        }

        // GET: ShipperDeliveries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await getDelivery(id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // GET: ShipperDeliveries/Assign/4
        public async Task<IActionResult> Assign(int? id)
        {
            var delivery = await getDelivery(id);
            var team =await  _context.Teams.Include(d => d.Drivers)
                .SingleOrDefaultAsync(m => m.ID == company.Team.ID);
            ViewData["AssignedToId"] = new SelectList(team.Drivers, "ID", "User.NormalizedUserName");
            ViewBag.Company = company;
            return View(delivery.DeliveryStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign([Bind("ID,DriverID,AssignedToId,Status,PickedUpBy")] DeliveryStatus deliveryStatus)
        {

            Driver driver = _context.Drivers.SingleOrDefault(c => c.ID == deliveryStatus.AssignedToId);
            DeliveryStatus status = _context.DeliveryStatus.SingleOrDefault(d => d.ID == deliveryStatus.ID);
            status.AssignedTo = driver;
            status.AssignedToId = driver.ID;

            var delivery = _context.Deliveries.SingleOrDefault(d => d.DeliveryStatus.ID == status.ID);
            driver.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            Delivery[] deliveries = await getShippersDeliveries();
            return RedirectToAction("Index");
        }


        // GET: ShipperDeliveries/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(company.Clients, "ID", "FirstName");
            ViewBag.Company = company;
            DeliveryDetails details = new DeliveryDetails();
            ViewData["PickUpAddressID"] = new SelectList(company.PickUpLocations, "ID", "LineOne");
            return View(details);
        }


        // POST: ShipperDeliveries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeliveryDetails deliveryDetails)
        {
            if (ModelState.IsValid)
            {

                Delivery delivery = new Delivery();
                Client client = company.Clients.SingleOrDefault(c => c.ID == deliveryDetails.ClientID);
                delivery.Client = client;
                delivery.ItemSize = deliveryDetails.ItemSize;
                delivery.ItemWeight = deliveryDetails.ItemWeight;

                PickUpAddress address = company.PickUpLocations.SingleOrDefault(a => a.ID == deliveryDetails.PickUpAddressID);
                delivery.PickUpAddress = address;
                
                
                DeliveryStatus status = new DeliveryStatus();
                status.Status = Status.New;
                _context.DeliveryStatus.Add(status);
                delivery.DeliveryStatus = status;
                _context.Add(delivery);
                company.Deliveries.Add(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ClientID"] = new SelectList(company.Clients, "ID", "FirstName", deliveryDetails.ClientID);
            ViewData["PickUpAddressID"] = new SelectList(company.PickUpLocations, "ID", "LineOne");
            return View(deliveryDetails);
        }

        // GET: ShipperDeliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Delivery delivery = await getDelivery(id);
            if (delivery == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Email", delivery.ClientID);
            ViewData["PickUpAddressID"] = new SelectList(company.PickUpLocations, "ID", "LineOne");
            return View(delivery);
        }

        private async Task<DeliveryStatus> getDeliveryStatus(int? id) {
            return await _context.DeliveryStatus
               .Include(d => d.AssignedTo)
               .SingleOrDefaultAsync(m => m.ID == id);
        }

        private async Task<Delivery> getDelivery(int? id)
        {
            return await _context.Deliveries.Include(d => d.PickUpAddress)
                .Include(d => d.Client)
                .Include(d => d.DeliveryStatus)
                .SingleOrDefaultAsync(m => m.ID == id);
        }
        
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Delivery delivery)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var deliveryEntity = _context.Deliveries.SingleOrDefault(d => d.ID == delivery.ID);
                    Client client = company.Clients.SingleOrDefault(c => c.ID == delivery.ClientID);
                    deliveryEntity.Client = client;

                    PickUpAddress address = company.PickUpLocations.SingleOrDefault(a => a.ID == delivery.PickUpAddressID);
                    deliveryEntity.PickUpAddress = address;

                    deliveryEntity.ItemSize = delivery.ItemSize;
                    deliveryEntity.ItemWeight = delivery.ItemWeight;
                    _context.Update(deliveryEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.ID))
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Email", delivery.ClientID);
            ViewData["PickUpAddressID"] = new SelectList(company.PickUpLocations, "ID", "LineOne");
            return View(delivery);
        }

        // GET: ShipperDeliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await getDelivery(id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // POST: ShipperDeliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var delivery = await getDelivery(id);
            _context.Addresses.Remove(delivery.PickUpAddress);
            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DeliveryExists(int id)
        {
            return _context.Deliveries.Any(e => e.ID == id);
        }

      

    }
}
