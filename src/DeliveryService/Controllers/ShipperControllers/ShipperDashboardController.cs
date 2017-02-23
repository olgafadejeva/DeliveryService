using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Models.Entities;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models;
using DeliveryService.Models.ShipperViewModels;
using DeliveryService.Util;

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
            List<MapRouteView> routesModel = EntityToModelConverter.convertRoutesForDashboardView(routes);
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