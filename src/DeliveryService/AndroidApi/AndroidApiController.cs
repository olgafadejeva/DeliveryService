using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Controllers.DriverControllers;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models.DriverViewModels;
using DeliveryService.Util;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;
using DeliveryService.Models.Entities;
using DeliveryService.Services;

/*
 * TThis class implements all API methods that are usedby the Android application.
 * In order to call the methods,  a client needs to be authenticated and provide an access token in the request 
 * 
 * This class also  extends the main driver controller that will guarantee that only drivers can access the methods
 * 
 */
namespace DeliveryService.AndroidApi
{
    [Route("api")]
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class AndroidApiController : DriverController
    {

        private DeliveryStatusUpdateService deliveryStatusUpdateService;

        public AndroidApiController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, DeliveryStatusUpdateService updateService) : base(context, contextAccessor) {

            this.deliveryStatusUpdateService = updateService;
        }

        /*
         * Gets all of the driver routes 
         */
        [HttpGet("routes")]
        public JsonResult Routes() {
            List<DriverRouteView> routes = EntityToModelConverter.convertDriverRouteToDisplayViews(driver);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(routes);
        }

        /*
         * Returns deliveries by route's id 
         */ 
        [HttpGet("routeDeliveries/{id}")]
        public JsonResult RouteDeliveries(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Id cannot be null");
            }
            Route route = driver.Routes.Where(r => r.ID == id).SingleOrDefault();
            if (route == null) {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Route not found");
            }

            List<DriverDeliveryView> deliveries = EntityToModelConverter.convertDeliveriesToView(route.Deliveries.ToList());
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(deliveries);
        }


        /*
         * Returns a delivery by specified id previously checking if it is in one of the driver's assigned routes
         */ 
        [HttpGet("delivery/{id}")]
        public JsonResult Delivery(int? id)
        {
            if (id == null) {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Id cannot be null");
            }
            List<Route> driverRoutes = driver.Routes.ToList();
            Delivery delivery = _context.Deliveries.Where(del => del.ID == id).SingleOrDefault();
            bool deliveryForThisDriver = false;
            if (delivery != null) {
                foreach (Route r in driverRoutes) {
                    if (r.Deliveries.Contains(delivery)) {
                        deliveryForThisDriver = true;
                        break;
                    }
                }
            }
            if (deliveryForThisDriver) {
                DriverDeliveryView deliveryJson = EntityToModelConverter.convertDeliveryToDriverView(delivery);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(deliveryJson);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Error occured");
        }

        /*
         * Returns all driver's vehicles in a JSON array
         */ 
        [HttpGet("vehicles")]
        public JsonResult Vehicles()
        {
            List<Vehicle> driverVehicles = driver.Vehicles.ToList();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(driverVehicles);
        }

        /*
         * Returns driver's details that include email address, address and a list of holidays
         */ 
        [HttpGet("driver")]
        public JsonResult DriverDetails()
        {
            DriverInformation driverInfo = new DriverInformation();
            driverInfo.driverAddress = driver.Address;
            driverInfo.email = driver.User.Email;
            driverInfo.holidays = driver.Holidays;
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(driverInfo);
        }


        /*
         * Updates a delivery status by ID
         */ 
        [HttpPost("statusUpdate")]
        public JsonResult UpdateStatus([FromBody] DeliveryStatusUpdate update)
        {
            Delivery delivery = _context.Deliveries.Where(d => d.ID == Convert.ToInt32(update.DeliveryID)).SingleOrDefault();
            Status status = StatusExtension.statusFromString(update.Status);
            if (delivery == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Delivery  not found");
            }
            bool statusUpdated = deliveryStatusUpdateService.UpdateDeliveryStatus(delivery, status);
            if (statusUpdated) {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("Status updated");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Invalid status transition");
        }
    }
}