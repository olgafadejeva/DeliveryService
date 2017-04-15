using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using DeliveryServiceTests.MockAndTestUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace DeliveryServiceTests.ServicesTests
{
    /*
     * Verifies drivers are assigned as desired based on the input conditions
     */ 
    public class DriverAssignmentServiceTest
    {
        [Fact]
        public void testDriverSelectionWhenOneDriverIsNotavailableOnADeliveryDate() {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Route driverOneRoute = new Route();
            driverOneRoute.DeliverBy = new DateTime(2015, 12, 12);
            driverOne.Routes.Add(driverOneRoute);

            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            Vehicle vehicle = new Vehicle();
            vehicle.MaxLoad = 300;
            vehicle.Width = 100;
            vehicle.Height = 100;
            vehicle.Length = 100;
            driverTwo.Vehicles.Add(vehicle);
            driverTwo.Address = (DriverAddress) getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne , responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route route = new Route();
            route.DeliverBy = new DateTime(2015, 12, 13);
            route.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            route.Deliveries.Add(delivery);

            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);

            DriverAssignmentResult result = assignmentService.getBestDriverForRoute(route, drivers, new List<Driver>(), new List<Driver>(), new DateTime(2015,12,12));
            Assert.Equal(result.Driver, driverTwo);
        }


        [Fact]
        public void testDriverSelectionWhenOneDriverDoesNotHaveAVehicle()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
           

            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            Vehicle vehicle = new Vehicle();
            vehicle.MaxLoad = 300;
            vehicle.Width = 100;
            vehicle.Height = 100;
            vehicle.Length = 100;
            driverTwo.Vehicles.Add(vehicle);
            driverTwo.Address = (DriverAddress)getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route route = new Route();
            route.DeliverBy = new DateTime(2015, 12, 13);
            route.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            route.Deliveries.Add(delivery);

            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);

            DriverAssignmentResult result = assignmentService.getBestDriverForRoute(route, drivers, new List<Driver>(), new List<Driver>(), new DateTime(2015, 12, 12));
            Assert.Equal(result.Driver, driverTwo);
        }

        [Fact]
        public void testDriverSelectionWhenOneDriversVehicleIsTooSmall()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(5, 9, 9, 9);
            driverOne.Vehicles.Add(vehicleOne);


            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            Vehicle vehicleTwo = new Vehicle(300, 100, 100, 100);
            driverTwo.Vehicles.Add(vehicleTwo);
            driverTwo.Address = (DriverAddress)getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route route = new Route();
            route.DeliverBy = new DateTime(2015, 12, 13);
            route.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            route.Deliveries.Add(delivery);

            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            DriverAssignmentResult result = assignmentService.getBestDriverForRoute(route, drivers, new List<Driver>(), new List<Driver>(), new DateTime(2015, 12, 12));
            Assert.Equal(result.Driver, driverTwo);
            Assert.Equal(result.Vehicle, vehicleTwo);
        }

        [Fact]
        public void testDriverSelectionWithDifferentDistanceFromDepotLocation()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);


            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            driverTwo.Vehicles.Add(vehicleOne);
            driverTwo.Address = (DriverAddress)getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route route = new Route();
            route.DeliverBy = new DateTime(2015, 12, 13);
            route.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            route.Deliveries.Add(delivery);

            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);

            DriverAssignmentResult result = assignmentService.getBestDriverForRoute(route, drivers, new List<Driver>(), new List<Driver>(), new DateTime(2015, 12, 12));
            Assert.Equal(result.Driver, driverOne);
        }

        [Fact]
        public void testDriverSelectionWhenBothDriversAreNotAvailable()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);
            Route driverRoute = new Route();
            driverRoute.DeliverBy = new DateTime(2015, 12, 12);
            driverOne.Routes.Add(driverRoute);


            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            driverTwo.Vehicles.Add(vehicleOne);
            driverTwo.Address = (DriverAddress)getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };
            driverTwo.Routes.Add(driverRoute);

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route route = new Route();
            route.DeliverBy = new DateTime(2015, 12, 13);
            route.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            route.Deliveries.Add(delivery);

            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);

            DriverAssignmentResult result = assignmentService.getBestDriverForRoute(route, drivers, new List<Driver>(), new List<Driver>(), new DateTime(2015, 12, 12));
            Assert.Equal(result.Driver, driverOne);
            Assert.Equal(result.DeliverByDate, new DateTime(2015, 12, 13));
        }

        [Fact]
        public void testDriverSelectionWithOneUnavailableDriver()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);

            
            List<Driver> drivers = new List<Driver> { driverOne };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

           
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route route = new Route();
            route.DeliverBy = new DateTime(2015, 12, 13);
            route.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            route.Deliveries.Add(delivery);

            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);

            DriverAssignmentResult result = assignmentService.getBestDriverForRoute(route, drivers, new List<Driver> { driverOne }, new List<Driver>(), new DateTime(2015, 12, 12));
            Assert.Equal(null, result.Driver);
        }

        [Fact]
        public void testAssignMultipleRoutesAllForOneDayWithOneDriver() {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);


            List<Driver> drivers = new List<Driver> { driverOne };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route routeOne = new Route();
            routeOne.DeliverBy = new DateTime(2015, 12, 13);
            routeOne.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            routeOne.Deliveries.Add(delivery);

            Route routeTwo = new Route();
            routeTwo.DeliverBy = new DateTime(2015, 12, 13);
            routeTwo.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery deliveryTwo = new Delivery();
            deliveryTwo.ItemWeight = 20;
            deliveryTwo.ItemSize = ItemSize.Small;
            routeTwo.Deliveries.Add(delivery);

            List<Route> routes = new List<Route> { routeOne, routeTwo };
            

            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            RouteAssignment result = assignmentService.assignMultipleRoutes(routes, drivers);
            Assert.Equal(2, result.TempRouteData.Count());
            Assert.Equal(driverOne, result.TempRouteData.ElementAt(0).Driver);
            Assert.Equal(null, result.TempRouteData.ElementAt(1).Driver);

        }

        [Fact]
        public void testAssignMultipleRoutesAllForDifferentDaysWithOneDriver()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);


            List<Driver> drivers = new List<Driver> { driverOne };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route routeOne = new Route();
            routeOne.DeliverBy = new DateTime(2015, 12, 13);
            routeOne.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            routeOne.Deliveries.Add(delivery);

            Route routeTwo = new Route();
            routeTwo.DeliverBy = new DateTime(2015, 12, 14);
            routeTwo.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery deliveryTwo = new Delivery();
            deliveryTwo.ItemWeight = 20;
            deliveryTwo.ItemSize = ItemSize.Small;
            routeTwo.Deliveries.Add(delivery);

            List<Route> routes = new List<Route> { routeOne, routeTwo };


            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            RouteAssignment result = assignmentService.assignMultipleRoutes(routes, drivers);
            Assert.Equal(2, result.TempRouteData.Count());
            Assert.Equal(driverOne, result.TempRouteData.ElementAt(0).Driver);
            Assert.Equal(driverOne, result.TempRouteData.ElementAt(1).Driver);

        }

        [Fact]
        public void testAssignMultipleRoutesAllForDifferentDaysWithTwoDrivers()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);

            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            driverTwo.Vehicles.Add(vehicleOne);
            driverTwo.Address = (DriverAddress)getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo, responseMessageOne, responseMessageTwo, responseMessageOne, responseMessageTwo};
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route routeOne = new Route();
            routeOne.DeliverBy = new DateTime(2015, 12, 13);
            routeOne.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            routeOne.Deliveries.Add(delivery);

            Route routeTwo = new Route();
            routeTwo.DeliverBy = new DateTime(2015, 12, 14);
            routeTwo.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery deliveryTwo = new Delivery();
            deliveryTwo.ItemWeight = 20;
            deliveryTwo.ItemSize = ItemSize.Small;
            routeTwo.Deliveries.Add(delivery);

            List<Route> routes = new List<Route> { routeOne, routeTwo };


            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            RouteAssignment result = assignmentService.assignMultipleRoutes(routes, drivers);
            Assert.Equal(2, result.TempRouteData.Count());
            Assert.Equal(driverOne, result.TempRouteData.ElementAt(0).Driver);
            Assert.Equal(driverTwo, result.TempRouteData.ElementAt(1).Driver);

        }

        [Fact]
        public void testAssignMultipleRoutesAllForoDayWithTwoDrivers()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);

            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            driverTwo.Vehicles.Add(vehicleOne);
            driverTwo.Address = (DriverAddress)getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo, responseMessageOne, responseMessageTwo, responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route routeOne = new Route();
            routeOne.DeliverBy = new DateTime(2015, 12, 13);
            routeOne.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            routeOne.Deliveries.Add(delivery);

            Route routeTwo = new Route();
            routeTwo.DeliverBy = new DateTime(2015, 12, 13);
            routeTwo.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery deliveryTwo = new Delivery();
            deliveryTwo.ItemWeight = 20;
            deliveryTwo.ItemSize = ItemSize.Small;
            routeTwo.Deliveries.Add(delivery);

            List<Route> routes = new List<Route> { routeOne, routeTwo };


            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            RouteAssignment result = assignmentService.assignMultipleRoutes(routes, drivers);
            Assert.Equal(2, result.TempRouteData.Count());
            Assert.Equal(driverOne, result.TempRouteData.ElementAt(0).Driver);
            Assert.Equal(driverTwo, result.TempRouteData.ElementAt(1).Driver);

        }

        [Fact]
        public void testAssignMultipleRoutesDifferentDaysWithOneDriverHavingARouteAlready()
        {
            Driver driverOne = new Driver();
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);
            Route driverRoute = new Route();
            driverRoute.DeliverBy = new DateTime(2015, 12, 12);
            driverOne.Routes.Add(driverRoute);

            Driver driverTwo = new Driver();
            driverTwo.Address = new DriverAddress();
            driverTwo.Vehicles.Add(vehicleOne);
            driverTwo.Address = (DriverAddress)getAddress(true);
            List<Driver> drivers = new List<Driver> { driverOne, driverTwo };

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageTwo, responseMessageOne, responseMessageTwo, responseMessageOne, responseMessageTwo };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route routeOne = new Route();
            routeOne.DeliverBy = new DateTime(2015, 12, 13);
            routeOne.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery delivery = new Delivery();
            delivery.ItemWeight = 20;
            delivery.ItemSize = ItemSize.Small;
            routeOne.Deliveries.Add(delivery);

            Route routeTwo = new Route();
            routeTwo.DeliverBy = new DateTime(2015, 12, 14);
            routeTwo.PickUpAddress = (PickUpAddress)getAddress(false);
            Delivery deliveryTwo = new Delivery();
            deliveryTwo.ItemWeight = 20;
            deliveryTwo.ItemSize = ItemSize.Small;
            routeTwo.Deliveries.Add(delivery);

            List<Route> routes = new List<Route> { routeOne, routeTwo };


            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            RouteAssignment result = assignmentService.assignMultipleRoutes(routes, drivers);
            Assert.Equal(2, result.TempRouteData.Count());
            Assert.Equal(driverTwo, result.TempRouteData.ElementAt(0).Driver);
            Assert.Equal(driverTwo, result.TempRouteData.ElementAt(1).Driver);
        }

        [Fact]
        public void testAssignmentWhenDriverIsOnHolidayButDeliveryIsRolledBackACoupleOfDays() {
            Driver driverOne = new Driver();
            DriverHoliday hols = new DriverHoliday();
            hols.From = new DateTime(2012, 12, 12);
            hols.To = new DateTime(2012, 12, 20);
            driverOne.Holidays.Add(hols);
            driverOne.Address = new DriverAddress();
            Vehicle vehicleOne = new Vehicle(300, 100, 109, 100);
            driverOne.Vehicles.Add(vehicleOne);
            Route driverRoute = new Route();
            driverRoute.DeliverBy = new DateTime(2015, 12, 12);
            driverOne.Routes.Add(driverRoute);

            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

           
            List<HttpResponseMessage> responses = new List<HttpResponseMessage> { responseMessageOne, responseMessageOne, responseMessageOne, responseMessageOne, responseMessageOne };
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            Route routeOne = new Route();
            routeOne.PickUpAddress = (PickUpAddress)getAddress(false);
            routeOne.DeliverBy = new DateTime(2012, 12, 13);
            List<Route> routes = new List<Route> { routeOne };


            LocationService locationService = new LocationService(googleMaps);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            RouteAssignment result = assignmentService.assignMultipleRoutes(routes, new List<Driver> { driverOne });
            Assert.Equal(1, result.TempRouteData.Count());
            Assert.Equal(driverOne, result.TempRouteData.ElementAt(0).Driver);
        }

        [Fact]
        public void testAssignmentWhenDriverIsOnHoliday()
        {
            Driver driverOne = new Driver();
            DriverHoliday hols = new DriverHoliday();
            hols.From = new DateTime(2012, 12, 12);
            hols.To = new DateTime(2012, 12, 20);
            driverOne.Holidays.Add(hols);

            Route routeOne = new Route();
            routeOne.DeliverBy = new DateTime(2012, 12, 17);
            List<Route> routes = new List<Route> { routeOne };


            LocationService locationService = new LocationService(null);
            DriverAssignmentService assignmentService = new DriverAssignmentService(locationService);
            RouteAssignment result = assignmentService.assignMultipleRoutes(routes, new List<Driver> { driverOne });
            Assert.Equal(1, result.TempRouteData.Count());
            Assert.Null( result.TempRouteData.ElementAt(0).Driver);
        }


        private Address getAddress(bool driver) {
            Address address = null;
            if (driver)
            {
                 address = new DriverAddress();
            }
            else {
                 address = new PickUpAddress();
            }
            address.City = "City";
            address.LineOne = "LineOne";
            address.PostCode = "PostCode";
            return address;       
        }
    }
}
