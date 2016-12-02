using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public class RouteCreationService
    {
        public LocationService LocationService { get; set; }
        public ApplicationDbContext context { get; set; }

        public RouteCreationService( LocationService googleMapsUtil, ApplicationDbContext context)
        {
            this.LocationService = googleMapsUtil;
            this.context = context;
        }

        public async Task<List<Route>> createRoutes(List<RouteDelivery> allRoutes, Company company) {
            List<Route> routesCreatedInThisSession = new List<Route>();
            
            for (int i = 0; i < allRoutes.Count(); i++)
            {
                RouteDelivery routeDelivery = allRoutes.ElementAt(i);
                var deliveriesInARoute = company.Deliveries.Where(d => routeDelivery.ids.Contains(d.ID)).ToList();
                if (deliveriesInARoute.Count() != 0)
                {
                    Route route = new Route();
                    route.Deliveries = deliveriesInARoute;
                    route.DeliverBy = DateFilter.getEarliestDeliverByDate(deliveriesInARoute);
                    var depot = await LocationService.FindClosestDepotLocationForRoute(company.PickUpLocations, routeDelivery.center);
                    route.PickUpAddress = depot;
                    route.Status = RouteStatus.New;
                    route.PickUpAddressID = depot.ID;
                    RouteDetails details = await LocationService.getRouteDurationAndOverallDistance(depot, deliveriesInARoute);
                    route.OverallDistance = details.OverallDistance;

                    //adding a time that a driver might need for stopping at the client's location, 10 min per stop
                    double timeForStops = Math.Round((double)10 * deliveriesInARoute.Count() / 60, 2);
                    route.OverallTimeRequired = details.OverallTimeRequired + timeForStops;

                    routesCreatedInThisSession.Add(route);
                }
                
               // await context.SaveChangesAsync();
            }
            return routesCreatedInThisSession;
        }

        public async Task updateRouteDetails(Route route)
        {
            List<Route> routesCreatedInThisSession = new List<Route>();
            var deliveriesInARoute = route.Deliveries;
                if (deliveriesInARoute.Count() != 0)
                {
                    RouteDetails details = await LocationService.getRouteDurationAndOverallDistance(route.PickUpAddress, deliveriesInARoute.ToList());
                    route.OverallDistance = details.OverallDistance;
                    //adding a time that a driver might need for stopping at the client's location, 10 min per stop
                    double timeForStops = Math.Round((double)10 * deliveriesInARoute.Count() / 60, 2);
                    route.OverallTimeRequired = details.OverallTimeRequired + timeForStops;
                }
            }
        }


    }
