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
using DeliveryService.Models;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class SchedulingController : ShipperController
    {
        public RouteCreationService RouteCreationService;
        public DriverAssignmentService DriverAssignmentService { get; set; }
        public SchedulingController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, RouteCreationService routeService, DriverAssignmentService driverService) : base(context, contextAccessor)
        {
            this.RouteCreationService = routeService;
            this.DriverAssignmentService = driverService;
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
                routesCreatedInThisSession = await RouteCreationService.createRoutes(allRoutes, company);
                foreach (Route r in routesCreatedInThisSession) {
                    company.Routes.Add(r);
                }
                await _context.SaveChangesAsync();


            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(routesCreatedInThisSession);
        }

        [HttpGet]
        public IActionResult Assign() {

            var routesToAssign = company.Routes.Where(r => r.AssignedTo == null);
            return View(routesToAssign);
        }

        [HttpPost]
        public async Task<IActionResult> AutomaticAssign() {
            var routesToAssign = company.Routes.Where(r => r.AssignedTo == null).ToList();
            RouteAssignment assignemnt =  DriverAssignmentService.assignMultipleRoutes(routesToAssign, company.Team.Drivers.ToList());
           /* var tempRouteData = new List<TempRoute>();
            List<Driver> alreadyAssignedDrivers = new List<Driver>();
            foreach (Route route in routesToAssign) {
                DriverAssignmentResult assignmentResult = DriverAssignmentService.getBestDriverForRoute(route, company.Team.Drivers.ToList(), alreadyAssignedDrivers);
                TempRoute parameters = new TempRoute();
                parameters.DriversVehicle = assignmentResult.Vehicle;
                parameters.ModifiedDeliverByDate = assignmentResult.DeliverByDate;
                parameters.Driver = assignmentResult.Driver;
                parameters.RouteId = route.ID;
                alreadyAssignedDrivers.Add(assignmentResult.Driver);
                tempRouteData.Add(parameters);
            }
            RouteAssignment assignemnt = new RouteAssignment(routesToAssign, tempRouteData);*/
            return View("ConfirmAssignment", assignemnt);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAssignment(List<TempRoute> tempRouteData) {

            foreach (TempRoute tempRoute in tempRouteData) {
                Route route = company.Routes.Where(r => r.ID == tempRoute.RouteId).First();
                route.AssignedTo = tempRoute.Driver;
                route.DeliverBy = tempRoute.ModifiedDeliverByDate;
                route.VehicleID = tempRoute.DriversVehicle.ID;
                _context.Routes.Update(route);
            }
            await _context.SaveChangesAsync();
            return View("RoutesSummary", company.Routes);
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
    
