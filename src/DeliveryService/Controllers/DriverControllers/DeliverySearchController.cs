using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using DeliveryService.Services;
using Microsoft.Extensions.Options;
using DeliveryService.Models.Entities;
using DeliveryService.Models;

namespace DeliveryService.Controllers.DriverControllers
{
    [RequireHttps]
    [Authorize(Roles = "Driver")]
    public class DeliverySearchController : DriverController
    {
        public AppProperties options { get; set; }
        public DeliverySearchService searchService  { get; set; }

        public DeliverySearchController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IOptions<AppProperties> optionsAccessor, DeliverySearchService searchService) : base(context, contextAccessor) {
            options = optionsAccessor.Value;
            this.searchService = searchService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SearchForDeliveries(DeliverySearchModel model)
        {
            IList<Delivery> deliveries = await searchService.searchForDeliveriesWithinDistance(model.Latitude, model.Longtitude, model.PickUpWithin, model.DeliveryWithin);

            return View("SearchResults", deliveries);
        }

        [HttpGet]
        public IActionResult SearchResults(IList<Delivery> deliveries) {
            return View(deliveries);
        }

        [HttpGet]
        public async Task<IActionResult> AllAvailableDeliveries() {
            IList<Delivery> deliveries = await searchService.findAllAvailableDeliveries();

            return View("SearchResults",  deliveries );
        }
    }
}