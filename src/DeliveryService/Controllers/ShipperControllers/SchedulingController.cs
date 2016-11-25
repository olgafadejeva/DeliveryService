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
using Newtonsoft.Json;

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
            List<DeliveryViewModelWithAddressString> delsWithAddress = new List<DeliveryViewModelWithAddressString>();
            foreach (Delivery delivery in deliveries) {
                DeliveryViewModelWithAddressString model = new DeliveryViewModelWithAddressString();
                model.Client = delivery.Client;
                model.ClientAddressString = DirectionsService.getStringFromAddress(delivery.Client.Address);
                model.DeliveryStatus = delivery.DeliveryStatus;
                model.ID = delivery.ID;
                delsWithAddress.Add(model);
            }
            MapObjects objects = new MapObjects(deliveries, depots, company.Routes.ToList(), delsWithAddress);
            return View(objects);
        }

        public JsonResult DeliverWithinDays(string days)
        {
            var deliveries = DateFilter.getDeliveriesWithinDays(company.Deliveries.ToList(), Convert.ToInt32(days));
            var routes = DateFilter.getRoutesWithinDays(company.Routes.ToList(), Convert.ToInt32(days));
            Response.StatusCode = (int)HttpStatusCode.OK;
            List<DeliveryViewModelWithAddressString> delsWithAddress = new List<DeliveryViewModelWithAddressString>();
            foreach (Delivery delivery in deliveries)
            {
                DeliveryViewModelWithAddressString model = new DeliveryViewModelWithAddressString();
                model.Client = delivery.Client;
                model.ClientAddressString = DirectionsService.getStringFromAddress(delivery.Client.Address);
                model.DeliveryStatus = delivery.DeliveryStatus;
                model.ID = delivery.ID;
                delsWithAddress.Add(model);
            }
            MapObjects result = new MapObjects(deliveries, routes, delsWithAddress);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Data([FromBody] List<RouteDelivery> allRoutes)
        {
            List<Route> routesCreatedInThisSession = new List<Route>();
            if (allRoutes != null)
            {
                routesCreatedInThisSession = await RouteCreationService.createRoutes(allRoutes, company);
                foreach (Route r in routesCreatedInThisSession)
                {
                    company.Routes.Add(r);
                }
                await _context.SaveChangesAsync();


            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(routesCreatedInThisSession);
        }

        [HttpGet]
        public IActionResult Assign()
        {
            var routesToAssign = company.Routes.Where(r => r.DriverID == null);
            return View(routesToAssign);
        }

        [HttpPost]
        public async Task<IActionResult> AutomaticAssign()
        {
            var routesToAssign = company.Routes.Where(r => r.DriverID == null).ToList();
            RouteAssignment assignemnt = DriverAssignmentService.assignMultipleRoutes(routesToAssign, company.Team.Drivers.ToList());

            TempData["Assignment"] = JsonConvert.SerializeObject(assignemnt);
            var TempRouteData = assignemnt.TempRouteData;
            foreach (TempRoute tempRoute in TempRouteData)
            {
                Route route = company.Routes.Where(r => r.ID == tempRoute.RouteId).First();
                if (tempRoute.Driver != null)
                {
                    Driver driverEntity = _context.Drivers.Where(d => d.ID == tempRoute.Driver.ID).FirstOrDefault();
                    route.DriverID = driverEntity.ID;
                }
                route.DeliveryDate = tempRoute.ModifiedDeliverByDate.AddDays(-1);
                if (tempRoute.DriversVehicle != null)
                {
                    route.VehicleID = tempRoute.DriversVehicle.ID;
                }
                _context.Routes.Update(route);
            }
            await _context.SaveChangesAsync();
            
            return RedirectToAction("AssignmentResult");
        }

        [HttpGet]
        public IActionResult AssignmentResult()
        {
            var assignment = JsonConvert.DeserializeObject<RouteAssignment>((string)TempData["Assignment"]);

            TempData["Assignment"] = JsonConvert.SerializeObject(assignment);
            return View(assignment);
        }
        
    }


        public class RouteDelivery
        {
            public int[] ids { get; set; }
            public Center center { get; set; }
        }

        public class Center
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

    }

    
