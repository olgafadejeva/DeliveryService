using DeliveryService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using DeliveryServiceTests.MockAndTestUtil;
using System.Net.Http;
using DeliveryService.Models.Entities;
using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Models;

namespace DeliveryServiceTests.ServicesTests
{
    /*
     * Tests location service with mock google maps responses
     */
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

        [Fact]
        public async Task testAddLocationDataToAddress() {
            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"results\":[{\"geometry\":{\"location\":{\"lat\":50.5,\"lng\":-10.5}}}],\"status\":\"OK\"}");

            List <HttpResponseMessage> responses = new List<HttpResponseMessage>();
            responses.Add(responseMessageOne);
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);


            LocationService service = new LocationService(googleMaps);
            Address address = new ClientAddress();
            await service.addLocationDataToAddress(address);
            Assert.Equal(50.5, address.Lat);
            Assert.Equal(-10.5, address.Lng);
        }

        [Fact]
        public async Task testGetRouteDurationAndOverallDistance() {
            var responseMessageOne = new HttpResponseMessage();
            string jsonResult = "{\"geocoded_waypoints\":[],\"routes\":[{\"legs\":[{\"distance\":{\"text\":\"141 mi\",\"value\":200},\"duration\":{\"text\":\"1 h. 54 min.\",\"value\":3600}},{\"distance\":{\"text\":\"70 m\",\"value\":200},\"duration\":{\"text\":\"1 min.\",\"value\":3600}}]}]}";
            responseMessageOne.Content = new StringContent(jsonResult);

            List<HttpResponseMessage> responses = new List<HttpResponseMessage>();
            responses.Add(responseMessageOne);
            TestGoogleMapsUtil googleMaps = new TestGoogleMapsUtil(responses);

            LocationService service = new LocationService(googleMaps);
            List<Delivery> deliveries = new List<Delivery>();
            PickUpAddress depot = new PickUpAddress();
            RouteDetails result = await service.getRouteDurationAndOverallDistance(depot, deliveries);
            Assert.Equal(0.4, result.OverallDistance);
            Assert.Equal(2, result.OverallTimeRequired);
        }
    }
}
