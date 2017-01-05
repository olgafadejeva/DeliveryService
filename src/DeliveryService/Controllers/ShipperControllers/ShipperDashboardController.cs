using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Models.Entities;
using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Models;
using DeliveryService.Services;
using DeliveryService.Models.ShipperViewModels;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class ShipperDashboardController : ShipperController
    {
        public ShipperDashboardController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
        public IActionResult Index()
        {
            List<Route> routes = company.Routes.Where(r => r.Status.Equals(RouteStatus.New) || r.Status.Equals(RouteStatus.InProgress)).ToList();
            List<MapRouteView> routesModel = new List<MapRouteView>();
            foreach (Route route in routes) {
                MapRouteView model = new MapRouteView();
                foreach (Delivery delivery in route.Deliveries)
                {
                    model.waypoints.Add(DirectionsService.getStringFromAddressInLatLngFormat(delivery.Client.Address));
                }
                model.depotAddress = DirectionsService.getStringFromAddressInLatLngFormat(route.PickUpAddress);
                model.overallRouteTime = route.OverallTimeRequired + "h";
                model.routeDistance = route.OverallDistance + "mi";
                if (route.DeliveryDate != null)
                {
                    model.scheduledOn = route.DeliveryDate.Value.ToString("dd/MM/yyyy"); ;
                }
                model.deliverBy = route.DeliverBy.ToString("dd/MM/yyyy");
                
                
                routesModel.Add(model);
            }
            ShipperDashboardModel finalModel = new ShipperDashboardModel();
            finalModel.company = company;
            finalModel.routesModel = routesModel;
            return View(finalModel);
        }

        [HttpGet]
        public IActionResult AddDefaultPickUpAddress()
        {
            return View();
        }
    }
}