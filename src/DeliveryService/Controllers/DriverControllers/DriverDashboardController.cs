using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DeliveryService.Data;
using DeliveryService.Models.DriverViewModels;
using DeliveryService.Models.Entities;
using DeliveryService.Models;
using DeliveryService.Util;

/*
 * This controller is responsible for supplied model to driver's dashboard.
 * The data includes routes and deliveries as they are needed for the map generation
 * 
 * Extends a generic DriverController that allows access to this controller's methods by a user in driver's role
 */
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
            Company company = _context.Companies.Where(c => c.ID == driver.User.CompanyID).SingleOrDefault();
            if (company != null) {
                finalModel.depots = company.PickUpLocations.ToList();
            }
            return View(finalModel);
        }

    }
}