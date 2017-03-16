using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models.DriverViewModels;
using DeliveryService.Services;
using DeliveryService.Models;
using System.Globalization;
using DeliveryService.Util;
using Newtonsoft.Json;

/*
 * Conroller responsible for driver's actions with routes
 * 
 * Extends a generic DriverController that allows access to this controller's methods by a user in driver's role
 */ 
namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverRoutesController : DriverController
    {

        public DriverRoutesController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        { 
        }
        
        /*
         * Returns the view with the model that contains Driver's route views
         */ 
        public IActionResult Index()
        {
            List<DriverRouteView> viewModels = EntityToModelConverter.convertDriverRouteToDisplayViews(driver);
            string json = JsonConvert.SerializeObject(viewModels);
            return View(viewModels);
        }
        
        /*
         * Returns all deliveries of the route based on the ID
         * Deliveries are previously converted into the views
         */ 
        public IActionResult RouteDeliveries(int? id)
        {
            Route route = driver.Routes.Where(r => r.ID == id).FirstOrDefault();
            List<Delivery> deliveries = route.Deliveries.ToList();
            List<DriverDeliveryViewModel> modelsList = new List<DriverDeliveryViewModel>();
            foreach (Delivery delivery in deliveries) {
                DriverDeliveryViewModel model = new DriverDeliveryViewModel();
                model.ClientName = delivery.Client.FirstName + " " + delivery.Client.LastName;
                model.DeliverBy = delivery.DeliverBy;

                string deliverytatusString = StatusExtension.DisplayName(delivery.DeliveryStatus.Status);
                if (delivery.DeliveryStatus.Status.Equals(Status.Delivered))
                {
                    DateTime deliveredDate = delivery.DeliveryStatus.DeliveredDate.Value;
                    deliverytatusString += " " + deliveredDate.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                else if (delivery.DeliveryStatus.Status.Equals(Status.FailedDelivery)) {

                     deliverytatusString = "Failed, reason: " + delivery.DeliveryStatus.ReasonFailed;
                }

                model.DeliveryStatusString = deliverytatusString;
                model.ID = delivery.ID;
                model.ItemSizeString = ItemSizeDimensionsExtension.getItemDimensionsBasedOnSize(delivery.ItemSize).ToString();
                model.ItemWeight = delivery.ItemWeight;
                model.ClientAddress = delivery.Client.Address;
                modelsList.Add(model);
            }
            return View(modelsList);
        }

        /*
         * Constructs a MapRouteView model which contains all waypoints(client's addresses) for the map to be displayed
         */ 
        public IActionResult MapRoute(int? id)
        {
            Route route = driver.Routes.Where(r => r.ID == id).FirstOrDefault();
            MapRouteView model = new MapRouteView();
            foreach (Delivery delivery in route.Deliveries)
            {
                model.waypoints.Add(DirectionsService.getStringFromAddressInLatLngFormat(delivery.Client.Address));
            }
            model.depotAddress = DirectionsService.getStringFromAddressInLatLngFormat(route.PickUpAddress);
            model.overallRouteTime = route.OverallTimeRequired + "h";
            model.routeDistance = route.OverallDistance + "mi";


            return View(model);
        }


        [HttpGet]
        public IActionResult MapDelivery(int? id)
        {
            var delivery = _context.Deliveries
                 .Include(d => d.Client)
                 .Include(d => d.Client.Address)
                 .Include(d => d.DeliveryStatus)
                 .SingleOrDefault(d => d.ID == id);
            double locationLat = delivery.Client.Address.Lat;
            double locationLng = delivery.Client.Address.Lng;
            string clientName = delivery.Client.FirstName + " " + delivery.Client.LastName;

            string deliverytatusString = StatusExtension.DisplayName(delivery.DeliveryStatus.Status);
            if (delivery.DeliveryStatus.Status.Equals(Status.Delivered))
            {
                DateTime deliveredDate = delivery.DeliveryStatus.DeliveredDate.Value;
                deliverytatusString += " " + deliveredDate.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
           
            string addressString = DirectionsService.getStringFromAddress(delivery.Client.Address);
            string deliverByDate = delivery.DeliverBy.Value.Date.ToString();
            string deliverByString = deliverByDate.Substring(0, deliverByDate.IndexOf(" "));
            DriverSingleDeliveryMapView model = new DriverSingleDeliveryMapView(locationLat, locationLng, clientName, deliverByString, deliverytatusString, addressString);
            return View(model);
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.ID == id);
        }
    }
}
