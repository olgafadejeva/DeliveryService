using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Services;
using DeliveryService.Models.Entities;

namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverDeliveriesController : DriverController
    {

        private DeliveryStatusUpdateService statusUpdateService { get; set; }
        private IDirectionsService directionsService { get; set; }

        public DriverDeliveriesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, DeliveryStatusUpdateService statusUpdateService, IDirectionsService directionsService) : base(context, contextAccessor) {
            this.statusUpdateService = statusUpdateService;
            this.directionsService = directionsService;
        }

        public IActionResult Index()
        {
            return View(driver.Deliveries);
        }

      
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult UpdateStatus(int? id, Status status) {
            Status updateStatus = status;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Delivery delivery = _context.Deliveries.SingleOrDefault(d => d.ID == id);
            if (delivery == null)
            {
                return RedirectToAction("Index");
            }
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, updateStatus);
            if (!statusUpdated)
            {
                TempData["StatusUpdate"] = "Unable to update status from " + delivery.DeliveryStatus.Status + " to " + updateStatus;
            }
            else
            {
                TempData["StatusUpdate"] = "Delivery status successfully updated!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Navigation(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Delivery delivery = _context.Deliveries.SingleOrDefault(d => d.ID == id);
            if (delivery == null)
            {
                return RedirectToAction("Index");
            }
            var directions = directionsService.getDirectionsFromAddresses(delivery.PickUpAddress, delivery.Client.Address);
            return View(directions);
            
        }


    }
}