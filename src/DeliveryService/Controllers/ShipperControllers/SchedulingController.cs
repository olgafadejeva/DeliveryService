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
using System.Windows;
using DeliveryService.Models.ShipperViewModels;

namespace DeliveryService.Controllers.ShipperControllers
{
    public class SchedulingController : ShipperController
    {
        public SchedulingController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        public IActionResult Index()
        {
            var deliveries = DateFilter.getDeliveriesWithinDays(company.Deliveries.ToList(), 2);
            var depots = company.PickUpLocations.ToList();
            MapObjects objects = new MapObjects(deliveries, depots);
            return View(objects);
        }
        
        public JsonResult DeliverWithinDays(string days) {
            var deliveries = DateFilter.getDeliveriesWithinDays(company.Deliveries.ToList(), Convert.ToInt32(days));

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(deliveries);
        }

        [HttpPost]
        public async Task Data([FromBody] List<int[]> allRoutes)
        {
            if (allRoutes != null)
            {
                for (int i = 0; i < allRoutes.Count(); i++) {
                    var deliveriesInARoute = company.Deliveries.Where(d => allRoutes[i].Contains(d.ID)).ToList();
                    Route route = new Route();
                    route.Deliveries = deliveriesInARoute;
                    route.DeliverBy = DateFilter.getEarliestDeliverByDate(deliveriesInARoute);
                    _context.Routes.Add(route);
                    company.Routes.Add(route);
                    
                }
                await _context.SaveChangesAsync();
            }
            Response.StatusCode = (int)HttpStatusCode.OK;

        }

        public void test() {

        }
    }
}
    
