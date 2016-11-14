using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using System.Net;
using DeliveryService.Models.ShipperViewModels;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class SchedulingController : ShipperController
    {
        public LocationService GoogleMaps;
        public SchedulingController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, LocationService googleMapsUtil) : base(context, contextAccessor)
        {
            this.GoogleMaps = googleMapsUtil;
        }

        public IActionResult Index()
        {
            var deliveries = DateFilter.getDeliveriesWithinDays(company.Deliveries.ToList(), 2);
            var depots = company.PickUpLocations.ToList();
            MapObjects objects = new MapObjects(deliveries, depots, company.Routes.ToList());
            return View(objects);
        }
        
        public JsonResult DeliverWithinDays(string days) {
            var deliveries = DateFilter.getDeliveriesWithinDays(company.Deliveries.ToList(), Convert.ToInt32(days));
            var routes = DateFilter.getRoutesWithinDays(company.Routes.ToList(), Convert.ToInt32(days));
            Response.StatusCode = (int)HttpStatusCode.OK;
            MapObjects result = new MapObjects(deliveries, routes);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Data([FromBody] List<RouteDelivery> allRoutes)
        {
            List<Route> routesCreatedInThisSession = new List<Route>();
            if (allRoutes != null)
            {
                for (int i = 0; i < allRoutes.Count(); i++) {
                    RouteDelivery routeDelivery = allRoutes.ElementAt(i);
                    var deliveriesInARoute = company.Deliveries.Where(d => routeDelivery.ids.Contains(d.ID)).ToList();
                    if (deliveriesInARoute.Count() != 0)
                    {
                        Route route = new Route();
                        route.Deliveries = deliveriesInARoute;
                        route.DeliverBy = DateFilter.getEarliestDeliverByDate(deliveriesInARoute);
                        _context.Routes.Add(route);
                        company.Routes.Add(route);
                        var depot = await GoogleMaps.FindClosestDepotLocationForRoute(company.PickUpLocations, routeDelivery.center);
                        route.PickUpAddress = depot;
                        route.PickUpAddressID = depot.ID;
                        routesCreatedInThisSession.Add(route);
                    }
                }
                await _context.SaveChangesAsync();
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(routesCreatedInThisSession);
        }
       
    }

    public class RouteDelivery {
        public int[] ids { get; set; }
        public Center center { get; set; }
    }

    public class Center {
        public float lat { get; set; }
        public float lng { get; set; }
    }

}
    
