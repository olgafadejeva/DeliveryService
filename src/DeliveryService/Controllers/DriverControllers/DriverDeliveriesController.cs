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

        public DriverDeliveriesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, DeliveryStatusUpdateService statusUpdateService) : base(context, contextAccessor) {
            this.statusUpdateService = statusUpdateService;
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



    }
}