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
using DeliveryService.Util;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class ShipperDeliveriesController : ShipperController
    {

        public ShipperDeliveriesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        // GET: ShipperDeliveries
        public IActionResult Index()
        {
            return View(shipper.Deliveries);
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

        // GET: ShipperDeliveries/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(shipper.Clients, "ID", "FirstName");
            ViewBag.Shipper = shipper;
            DeliveryDetails details = new DeliveryDetails();
            if (shipper.DefaultPickUpAddress != null)
            {
                details.useDefaultDeliveryAddress = true;
            }
            return View(details);
        }

        // POST: ShipperDeliveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClientID,useDefaultDeliveryAddress,PickUpAddress")] DeliveryDetails deliveryDetails)
        {

            var address = deliveryDetails.PickUpAddress;
            if (address.City == null && address.LineOne == null && address.PostCode == null) { deliveryDetails.PickUpAddress = null; }
            if (DeliveryDetailsModelValidator.IsValid(deliveryDetails))
            {

                Delivery delivery = new Delivery();
                Client client = shipper.Clients.SingleOrDefault(c => c.ID == deliveryDetails.ClientID);
                delivery.Client = client;

                if (deliveryDetails.useDefaultDeliveryAddress)
                {
                    PickUpAddress pickUpAddress = new PickUpAddress();
                    pickUpAddress.City = shipper.DefaultPickUpAddress.City;
                    pickUpAddress.PostCode = shipper.DefaultPickUpAddress.PostCode;
                    pickUpAddress.LineOne = shipper.DefaultPickUpAddress.LineOne;
                    pickUpAddress.LineTwo = shipper.DefaultPickUpAddress.LineTwo;
                    delivery.PickUpAddress = pickUpAddress;
                }
                else {
                    delivery.PickUpAddress = deliveryDetails.PickUpAddress;
                }
                shipper.Deliveries.Add(delivery);
                _context.Add(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ClientID"] = new SelectList(shipper.Clients, "ID", "FirstName", deliveryDetails.ClientID);
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
            return View(delivery);
        }

        private async Task<Delivery> getDelivery(int? id)
        {
            return await _context.Deliveries.Include(d => d.PickUpAddress)
                .Include(d => d.Client)
                .SingleOrDefaultAsync(m => m.ID == id);
        }

        // POST: ShipperDeliveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClientID,PickUpAddress")] Delivery delivery)
        {
            if (id != delivery.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Client client = shipper.Clients.SingleOrDefault(c => c.ID == delivery.ClientID);
                    var deliveryEntity = _context.Deliveries.SingleOrDefault(d => d.ID == delivery.ID);
                    deliveryEntity.Client = client;
                    deliveryEntity.PickUpAddress = delivery.PickUpAddress;
                    deliveryEntity.ClientID = delivery.ClientID;
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
