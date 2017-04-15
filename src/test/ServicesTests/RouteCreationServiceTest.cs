using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using DeliveryServiceTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.ServicesTests
{
    public class RouteCreationServiceTest
    {
        private readonly RouteCreationService routeCreationService;
        public ApplicationDbContext context { get; set; }
        public RouteCreationServiceTest()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            routeCreationService = new RouteCreationService(getMockGoogleMaps(), context);
        }

        [Fact]
        public void testCreateRouteWithNoDeliveries() {
            List<RouteDelivery> allRoutes = new List<RouteDelivery>();
            allRoutes.Add(new RouteDelivery());
            Company company = new Company();
            var result = routeCreationService.createRoutes(allRoutes, company).Result;
            Assert.Empty(result);
        }

        [Fact]
        public async Task testUpdateRouteDetails() {
            DeliveryService.Models.Entities.Route route = new DeliveryService.Models.Entities.Route();
            Delivery delivery = new Delivery();
            route.Deliveries.Add(delivery);

            PickUpAddress address = ShipperDetailsHelper.getDeliveryPickUpAddress();
            route.PickUpAddress = address;

            await routeCreationService.updateRouteDetails(route);
            Assert.Equal(route.OverallDistance, 11);
        }

        private LocationService getMockGoogleMaps()
        {
            var mockGoogleMaps = new Mock<LocationService>();
            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"results\":[{\"geometry\":{\"location\":{\"lat\":10,\"lng\":20}}}]}");
            mockGoogleMaps.SetupSequence(gm => gm.addLocationDataToAddress(It.IsAny<Address>()))
                .Returns(Task.FromResult(responseMessageOne));

            RouteDetails routeDetails = new RouteDetails(11,12);
            mockGoogleMaps.SetupSequence(gm => gm.getRouteDurationAndOverallDistance(It.IsAny<PickUpAddress>(), It.IsAny<List<Delivery>>()))
                .Returns(Task.FromResult(routeDetails));
            return mockGoogleMaps.Object;
        }
    }
}
