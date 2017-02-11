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

namespace DeliveryService.AndroidApi
{
    [Route("api")]
    public class AndroidActionsApiController : DriverController
    {

        private DeliveryStatusUpdateService deliveryStatusUpdateService;

        public AndroidActionsApiController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, DeliveryStatusUpdateService updateService) : base(context, contextAccessor) {

            this.deliveryStatusUpdateService = updateService;
        }

        [HttpGet("routes")]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public JsonResult Routes() {
            List<DriverRouteView> routes = EntityToModelConverter.convertDriverRouteToDisplayViews(driver);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(routes);
        }

        [HttpGet("routeDeliveries/{id}")]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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



        [HttpGet("delivery/{id}")]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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

        [HttpGet("vehicles")]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public JsonResult Vehicles()
        {
            List<Vehicle> driverVehicles = driver.Vehicles.ToList();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(driverVehicles);
        }

        [HttpGet("driver")]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public JsonResult DriverDetails()
        {
            DriverInformation driverInfo = new DriverInformation();
            driverInfo.driverAddress = driver.Address;
            driverInfo.email = driver.User.Email;
            driverInfo.holidays = driver.Holidays;
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(driverInfo);
        }



        [HttpPost("statusUpdate")]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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