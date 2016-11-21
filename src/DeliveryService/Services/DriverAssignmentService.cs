﻿using DeliveryService.Models;
using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public class DriverAssignmentService
    {
        public LocationService locationService { get; set; }
        public int numberOfTries { get; set; }

        public DriverAssignmentService(LocationService location) {
            this.locationService = location;
        }

        public RouteAssignment assignMultipleRoutes(List<Route> routes, List<Driver> drivers) {
            var tempRouteData = new List<TempRoute>();
            List<Driver> alreadyAssignedDriversForADay = new List<Driver>();
            List<Driver> driversAlreadyAssignedARouteThisSession = new List<Driver>();
            Route prevRoute = null;
            foreach (Route route in routes)
            {
                if (prevRoute != null && !prevRoute.DeliverBy.Equals(route.DeliverBy)) {
                    alreadyAssignedDriversForADay.Clear();
                }
                DriverAssignmentResult assignmentResult = getBestDriverForRoute(route, drivers, alreadyAssignedDriversForADay, driversAlreadyAssignedARouteThisSession);
                TempRoute parameters = new TempRoute();
                parameters.DriversVehicle = assignmentResult.Vehicle;
                parameters.ModifiedDeliverByDate = assignmentResult.DeliverByDate;
                parameters.Driver = assignmentResult.Driver;
                parameters.RouteId = route.ID;
                alreadyAssignedDriversForADay.Add(assignmentResult.Driver);
                tempRouteData.Add(parameters);
                prevRoute = route;
                driversAlreadyAssignedARouteThisSession.Add(assignmentResult.Driver);
            }
            RouteAssignment assignemnt = new RouteAssignment(routes, tempRouteData);
            return assignemnt;
        }

        private List<Driver> sortDiversInTheOrderOfBusiness(List<Driver> allDrivers, List<Driver> driversAlreadyAssignedARouteInTheSession) {
           // driversAlreadyAssignedARouteInTheSession = driversAlreadyAssignedARouteInTheSession.OrderBy(d => d.Routes.Count()).ToList();
            //allDrivers = allDrivers.OrderBy(d => d.Routes.Count()).ToList();
            //List<Driver> finalList = new List<Driver>();
            foreach (Driver dr in allDrivers) {
                if (driversAlreadyAssignedARouteInTheSession.Contains(dr)) {
                    dr.Routes.Add(new Route());
                }
            }
            allDrivers = allDrivers.OrderBy(d => d.Routes.Count()).ToList();
            foreach (Driver dr in allDrivers)
            {
                if (driversAlreadyAssignedARouteInTheSession.Contains(dr))
                {
                    dr.Routes.Remove(dr.Routes.ElementAt(dr.Routes.Count - 1));
                }
            }
            return allDrivers;
        }

        public DriverAssignmentResult getBestDriverForRoute(Route route, List<Driver> drivers, List<Driver> unavailableDrivers, List<Driver> driversAlreadyAssignedRoutesInThisSession) {
            DriverAssignmentResult[] assignmentCosts = new DriverAssignmentResult[drivers.Count()];
            List<Driver> copyOfUnavailableDrivers = unavailableDrivers.ToList();


            List<Driver> sortedDriverList = sortDiversInTheOrderOfBusiness(drivers.ToList(), copyOfUnavailableDrivers);
            foreach (Driver driver in sortedDriverList) {
                if (!unavailableDrivers.Contains(driver))
                {
                    DriverAssignmentResult result = getCostForDriverBeingAssignedToRoute(driver, route);
                    assignmentCosts[drivers.IndexOf(driver)] = result;
                }
                else {
                    assignmentCosts[drivers.IndexOf(driver)] = new DriverAssignmentResult();
                }

            }

            List<DriverAssignmentResult> results = assignmentCosts.OrderByDescending(i => i.AssignmentProfit).ToList();
            DriverAssignmentResult maxValueResult = results.FirstOrDefault();
            if ( maxValueResult !=null && maxValueResult.AssignmentProfit != 0)
            {
                var result = getBestResult(results, driversAlreadyAssignedRoutesInThisSession);
                if (result == null)
                {
                    return maxValueResult; //if everyone has been assigned smth, then we can only select the one that is the best in terms of profit 
                }
                else {
                    return result;
                }
            }

            else {
                if (numberOfTries < 3)
                {
                    numberOfTries++;
                    route.DeliverBy = route.DeliverBy.AddDays(-1);
                    return getBestDriverForRoute(route, drivers, unavailableDrivers, driversAlreadyAssignedRoutesInThisSession);
                } else {
                    return new DriverAssignmentResult();
                }
            }

        }

        private DriverAssignmentResult getBestResult(List<DriverAssignmentResult> results, List<Driver> alreadyBusyDrivers) {
            DriverAssignmentResult firstResult = results.FirstOrDefault();
            if (firstResult != null && firstResult.AssignmentProfit != 0 && !alreadyBusyDrivers.Contains(firstResult.Driver))
            {
                return firstResult;
            }
            else if (results.Count() >= 1)
            {
                return getBestResult(results.GetRange(1, results.Count()-1), alreadyBusyDrivers);
            }
            else {
                return null;
            }
        }

        private DriverAssignmentResult getCostForDriverBeingAssignedToRoute(Driver driver, Route route)
        {
            double profit = 0;
            DriverAssignmentResult result = new DriverAssignmentResult(route.ID, route.DeliverBy, driver);

            //if there is already a delivery on a day before deliveryby day 
            if (driver.Routes.Where(r => r.DeliverBy.Equals(route.DeliverBy.AddDays(-1))).ToList().Count() != 0) {
               // return 0; //ifdriver is not available on a day, no need to check other 
                result.AssignmentProfit = 0;
                return result;
            } else {
                profit += 5;
            }

            double weightOfDeliveriesInRoute = 0;
            double capacityOfBoxes = 0;
            for (int i= 0; i< route.Deliveries.Count(); i++) {
                Delivery delivery = route.Deliveries.ElementAt(i);
                weightOfDeliveriesInRoute += delivery.ItemWeight;
                capacityOfBoxes += calculateCapacity(ItemSizeDimensionsExtension.getItemDimensionsBasedOnSize(delivery.ItemSize));
            }

            Vehicle suitableVehicle=null;
            for (int i = 0; i < driver.Vehicles.Count(); i++) {
                Vehicle vehicle = driver.Vehicles.ElementAt(i);
                if (capacityOfBoxes <= calculateCapacity(new DeliveryItemDimensions(vehicle.Height, vehicle.Width, vehicle.Length)) && vehicle.MaxLoad >=weightOfDeliveriesInRoute) {
                    suitableVehicle = vehicle;
                    break;
                }
            }

            //add 0 weight if there is no
            if (suitableVehicle == null) {
                result.AssignmentProfit = 0;
                return result;
                //if no vehicle available then no point looking further 
            } else {
                result.Vehicle = suitableVehicle;
                profit += 5;
            }
            var distanceFromDepotLocation = locationService.getDistanceBetweenTwoAddresses(driver.Address, route.PickUpAddress).Result;

            profit += 100 / distanceFromDepotLocation;
            result.AssignmentProfit = profit;
            return result;
        }

        private double calculateCapacity(DeliveryItemDimensions dimensions) {
            return dimensions.Height * dimensions.Length * dimensions.Width;
        }
    }
}
