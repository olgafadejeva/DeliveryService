using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Controllers.DriverControllers;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models.DriverViewModels;
using DeliveryService.Util;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DeliveryService.Models;
using AspNet.Security.OAuth.Validation;
using DeliveryService.Models.Entities;

namespace DeliveryService.AndroidApi
{
    public class AndroidActionsApiController : DriverController
    {
        
        public AndroidActionsApiController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor) { }

        [HttpGet]
        //[Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public JsonResult Routes() {
            List<DriverRouteView> routes = EntityToModelConverter.convertDriverRouteToDisplayViews(driver);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(routes);
        }

        [HttpGet]
       // [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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



        [HttpGet]
      //  [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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


        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public async Task<String> Message()
        {
            return "All ok";
        }

    }
}