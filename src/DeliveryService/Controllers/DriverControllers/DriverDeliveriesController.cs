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
using System.Net;

namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverDeliveriesController : DriverController
    {

        private DeliveryStatusUpdateService statusUpdateService { get; set; }
        private IDirectionsService directionsService { get; set; }

        public DriverDeliveriesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, DeliveryStatusUpdateService statusUpdateService, IDirectionsService directionsService) : base(context, contextAccessor)
        {
            this.statusUpdateService = statusUpdateService;
            this.directionsService = directionsService;
        }

        public IActionResult Index()
        {
            return View(driver.Routes);
        }
        
        [HttpPost]
        public JsonResult UpdateStatus([FromBody]StatusUpdateRequest updateRequest)
        {
            Status updateStatus = StatusExtension.statusFromString(updateRequest.status);
            if (updateRequest.id == null)
            {
                Response.StatusCode = 400;
                return Json(HttpStatusCode.BadRequest);
            }
            Delivery delivery = _context.Deliveries
                .Include(d => d.DeliveryStatus)
                .SingleOrDefault(d => d.ID == updateRequest.id);
            if (delivery == null)
            {
                Response.StatusCode = 400;
                return Json(HttpStatusCode.BadRequest);
            }
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, updateStatus);

            if (!statusUpdated)
            {
                TempData["StatusUpdate"] = "Unable to update status from " + delivery.DeliveryStatus.Status + " to " + updateStatus;
                Response.StatusCode = 400;
                return Json(HttpStatusCode.BadRequest);
            }
            else
            {
                TempData["StatusUpdate"] = "Delivery status successfully updated!";
            }
            delivery.DeliveryStatus.Status = updateStatus;
            Response.StatusCode = 200;
            return Json(StatusExtension.DisplayName(delivery.DeliveryStatus.Status));

        }

        [HttpGet]
        public IActionResult Navigation(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Delivery delivery = _context.Deliveries
                .Include(d => d.Client)
                .Include(d => d.Client.Address)
                .SingleOrDefault(d => d.ID == id);
            if (delivery == null)
            {
                return RedirectToAction("Index");
            }
            var directions = directionsService.getDirectionsFromAddresses(new PickUpAddress(), delivery.Client.Address);
            return View(directions);

        }


    }

    public class StatusUpdateRequest
    {
        public int id { get; set; }
        public string status { get; set; }
    }
}