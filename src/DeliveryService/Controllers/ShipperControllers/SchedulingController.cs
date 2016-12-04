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
        public INotificationService notificationService { get; set; }

        public SchedulingController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, RouteCreationService routeService, DriverAssignmentService driverService, INotificationService notService) : base(context, contextAccessor)
        {
            this.RouteCreationService = routeService;
            this.DriverAssignmentService = driverService;
            this.notificationService = notService;
        }

        public IActionResult Index()
        {
            var deliveries = company.Deliveries.Where(d => d.DeliveryStatus.Status.Equals(Status.New)).ToList();
            var depots = company.PickUpLocations.ToList();
            List<ShipperSingleDeliveryMapViewModel> delsWithAddress = new List<ShipperSingleDeliveryMapViewModel>();
            foreach (Delivery delivery in deliveries) {
                ShipperSingleDeliveryMapViewModel model = new ShipperSingleDeliveryMapViewModel();
                model.Client = delivery.Client;
                model.addressString = DirectionsService.getStringFromAddress(delivery.Client.Address);
                model.ID = delivery.ID;

                string clientName = delivery.Client.FirstName + " " + delivery.Client.LastName;
                string currentStatus = StatusExtension.DisplayName(delivery.DeliveryStatus.Status);
                string deliverByDate = delivery.DeliverBy.Value.Date.ToString();
                string deliverByString = deliverByDate.Substring(0, deliverByDate.IndexOf(" "));

                model.clientName = clientName;
                model.currentStatus = currentStatus;
                model.deliverBy = deliverByString;
                delsWithAddress.Add(model);
            }
            MapObjects objects = new MapObjects(deliveries, depots, company.Routes.ToList(), delsWithAddress, company);
            return View(objects);
        }

        public JsonResult DeliverWithinDays(string days)
        {
            var deliveries = DateFilter.getDeliveriesWithinDays(company.Deliveries.ToList(), Convert.ToInt32(days));
            var routes = DateFilter.getRoutesWithinDays(company.Routes.ToList(), Convert.ToInt32(days));
            Response.StatusCode = (int)HttpStatusCode.OK;
            List<ShipperSingleDeliveryMapViewModel> delsWithAddress = new List<ShipperSingleDeliveryMapViewModel>();
            foreach (Delivery delivery in deliveries)
            {
                ShipperSingleDeliveryMapViewModel model = new ShipperSingleDeliveryMapViewModel();
                model.Client = delivery.Client;
                model.addressString = DirectionsService.getStringFromAddress(delivery.Client.Address);
                model.ID = delivery.ID;

                string clientName = delivery.Client.FirstName + " " + delivery.Client.LastName;
                string currentStatus = StatusExtension.DisplayName(delivery.DeliveryStatus.Status);
                string deliverByDate = delivery.DeliverBy.Value.Date.ToString();
                string deliverByString = deliverByDate.Substring(0, deliverByDate.IndexOf(" "));

                model.clientName = clientName;
                model.currentStatus = currentStatus;
                model.deliverBy = deliverByString;
                delsWithAddress.Add(model);
            }
            MapObjects result = new MapObjects(deliveries, routes, delsWithAddress, company);
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
                    await notificationService.SendAnEmailToDriverAboutAssignedRouteAsync(route, driverEntity.User);
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

    
