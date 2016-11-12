using DeliveryService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DeliveryServiceTests.MockAndTestUtil;
using System.Net.Http;
using DeliveryService.Models.Entities;
using DeliveryService.Controllers.ShipperControllers;

namespace DeliveryServiceTests.ServicesTests
{
    public class LocationServiceTest
    {
        [Fact]
        public void testGetClosestDepotLocation() {
            
            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":2},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":10},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            List<HttpResponseMessage> responses = new List<HttpResponseMessage>();
            responses.Add(responseMessageOne);
            responses.Add(responseMessageTwo);
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);


            LocationService service = new LocationService(googleMaps);

            IList<PickUpAddress> addressList = new List<PickUpAddress>();
            PickUpAddress depot1 = new PickUpAddress();
            addressList.Add(depot1);

            PickUpAddress depot2 = new PickUpAddress();
            addressList.Add(depot2);

            Center center = new Center();
            center.lat = 1;
            center.lng = 1;
            var result = service.FindClosestDepotLocationForRoute(addressList, center);
            Assert.Equal(depot1, result.Result);
        }
    }
}
