using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DeliveryService.Data;
using DeliveryService.Models.DriverViewModels;
using DeliveryService.Models.Entities;
using DeliveryService.Models;
using DeliveryService.Services;
using DeliveryService.Util;

namespace DeliveryService.Controllers.DriverControllers
{   
    [Authorize(Roles = "Driver")]
    public class DriverDashboardController : DriverController
    {
        public DriverDashboardController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor) { }

        public IActionResult Index()
        {
           
            List<Route> routes = driver.Routes.Where(r => r.Status.Equals(RouteStatus.New) || r.Status.Equals(RouteStatus.InProgress)).ToList();
            List<MapRouteView> routesModel = EntityToModelConverter.convertRoutesForDashboardView(routes);
            DriverDashboardModel finalModel = new DriverDashboardModel();
            finalModel.Driver = driver;
            finalModel.routesModel = routesModel;
            return View(finalModel);
        }

    }
}