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
using DeliveryService.Models.DriverViewModels;
using System.Threading;
using System.Globalization;

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
                Response.StatusCode = 400;
                return Json(HttpStatusCode.BadRequest);
            }

            delivery.DeliveryStatus.Status = updateStatus;
            string updateStatusString = StatusExtension.DisplayName(delivery.DeliveryStatus.Status);
            if (updateStatus.Equals(Status.Delivered)) {
                DateTime deliveredDate = DateTime.Now;
                delivery.DeliveryStatus.DeliveredDate = deliveredDate;
                updateStatusString += " " + deliveredDate.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            Response.StatusCode = 200;
            _context.SaveChanges();
            return Json(updateStatusString);
        }

        [HttpPost]
        public JsonResult UpdateStatusToFailed([FromBody]FailedStatusUpdateRequest updateRequest)
        {
            Status updateStatus = Status.FailedDelivery;
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
                Response.StatusCode = 400;
                return Json(HttpStatusCode.BadRequest);
            }

            delivery.DeliveryStatus.Status = updateStatus;
            string updateStatusString = "Failed, reason: " + updateRequest.reason;
            delivery.DeliveryStatus.ReasonFailed = updateRequest.reason;
            Response.StatusCode = 200;
            _context.SaveChanges();
            return Json(updateStatusString);
        }



    }

    public class StatusUpdateRequest
    {
        public int id { get; set; }
        public string status { get; set; }
    }

    public class FailedStatusUpdateRequest : StatusUpdateRequest
    {
        public string reason { get; set; }
    }
}