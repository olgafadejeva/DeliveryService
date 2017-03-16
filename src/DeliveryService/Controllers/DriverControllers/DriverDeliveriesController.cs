using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Services;
using DeliveryService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Globalization;

/*
 * Controller responsible for actions the a driver invokes on deliveries
 * The most important method is a status update 
 * 
 * Extends a generic DriverController that allows access to this controller's methods by a user in driver's role
 */ 
namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverDeliveriesController : DriverController
    {

        private DeliveryStatusUpdateService statusUpdateService { get; set; }
        private IDirectionsService directionsService { get; set; }
        private INotificationService notificationService { get; set; }

        public DriverDeliveriesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, DeliveryStatusUpdateService statusUpdateService, IDirectionsService directionsService, INotificationService notService) : base(context, contextAccessor)
        {
            this.statusUpdateService = statusUpdateService;
            this.directionsService = directionsService;
            this.notificationService = notService;
        }

        /*
         * Update the delivery status and sends and e-mail to the client if status is In transit or Picked up. 
         * Also sends an e-mail to the company's team members about all status transitions
         * The response is in a JSON format as the method is called using AJAX
         */ 
        [HttpPost]
        public JsonResult UpdateStatus([FromBody]StatusUpdateRequest updateRequest)
        {
            Status updateStatus = StatusExtension.statusFromString(updateRequest.status);
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

            Company company = _context.Companies.Include(c=> c.Team.Employees).Where(c => c.ID == driver.User.CompanyID).SingleOrDefault();

            notificationService.SendStatusUpdateEmailToAdminAsync(updateStatusString, delivery, company.Team.Employees.ToList());
            if (updateStatus.Equals(Status.InTransit) || updateStatus.Equals(Status.PickedUpByDriver))
            {
                notificationService.SendStatusUpdateEmailToClientAsync(updateStatus, delivery, delivery.Client);
            }

            notificationService.SendStatusUpdateEmailToAdminAsync(updateStatusString, delivery, company.Team.Employees.ToList());
            Response.StatusCode = 200;
            _context.SaveChanges();
            return Json(updateStatusString);
        }

        /*
         * Updates delivery status to 'dailed'. It is separate from the main update method as it also involves adding the 
         * reason why delivery has failed 
         * The result is a JSON as the method is called using AJAX
         */
        [HttpPost]
        public JsonResult UpdateStatusToFailed([FromBody]FailedStatusUpdateRequest updateRequest)
        {
            Status updateStatus = Status.FailedDelivery;
            if (updateRequest.id == 0)
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

            Company company = _context.Companies.Include(c => c.Team.Employees).Where(c => c.ID == driver.User.CompanyID).SingleOrDefault();
            notificationService.SendStatusUpdateEmailToAdminAsync(updateStatusString, delivery, company.Team.Employees.ToList());
            delivery.DeliveryStatus.ReasonFailed = updateRequest.reason;
            Response.StatusCode = 200;
            _context.SaveChanges();
            return Json(updateStatusString);
        }

    }

    /*
     * Helper classes for passing parameters from the AJAX methods on the page
     */ 
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