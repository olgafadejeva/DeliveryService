using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Services;
using DeliveryService.Models.Entities;
using Microsoft.EntityFrameworkCore;

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
            Delivery delivery = _context.Deliveries
                .Include(d => d.DeliveryStatus)
                .Include(d => d.PickUpAddress)
                .SingleOrDefault(d => d.ID == id);
            if (delivery == null)
            {
                return RedirectToAction("Index");
            }
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, updateStatus);
            if (statusUpdated && status.Equals(Status.ClaimedByDriver))
            {
                delivery.DeliveryStatus.AssignedTo = driver;
                driver.Deliveries.Add(delivery);
                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
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
        }

        [HttpGet]
        public IActionResult Navigation(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Delivery delivery = _context.Deliveries
                .Include(d=>d.PickUpAddress)
                .Include(d=>d.Client)
                .Include(d=>d.Client.Address)
                .SingleOrDefault(d => d.ID == id);
            if (delivery == null)
            {
                return RedirectToAction("Index");
            }
            var directions = directionsService.getDirectionsFromAddresses(delivery.PickUpAddress, delivery.Client.Address);
            return View(directions);
            
        }


    }
}