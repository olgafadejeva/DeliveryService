using DeliveryService.Data;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using DeliveryServiceTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Services
{
    public class DeliverySearchServiceTest
    {
        DeliverySearchService service;
        ApplicationDbContext context; 

        public DeliverySearchServiceTest() {
            
        }

        [Fact]
        public async Task testDeliveryReturnedWithinFiveMiles() {
            var mockGoogleMaps = new Mock<LocationService>();
            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content =new  StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}"); 
            mockGoogleMaps.SetupSequence(gm => gm.createDistanceUriAndGetResponse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(responseMessageOne))
                .Returns(Task.FromResult(responseMessageTwo));
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context, mockGoogleMaps.Object);
            await populateContextWithData(context);

            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8662053, -0.0884891, 5, 5);
            Assert.Equal(1, deliveries.Count());
            Assert.Equal(context.Clients.First(), deliveries.First().Client);
        }

        [Fact]
        public async Task testDeliveryReturnedWithinZeroMiles()
        {
            var mockGoogleMaps = new Mock<LocationService>();
            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            mockGoogleMaps.SetupSequence(gm => gm.createDistanceUriAndGetResponse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(responseMessageOne))
                .Returns(Task.FromResult(responseMessageTwo));
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context, mockGoogleMaps.Object);
            await populateContextWithData(context);
            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8662053, -0.0884891, 0, 0);
            Assert.Empty(deliveries);
        }

        [Fact]
        public async Task testNoDeliveriesReturnedWhenOneConditionIsNotSatisfied()
        {
            var mockGoogleMaps = new Mock<LocationService>();
            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            mockGoogleMaps.SetupSequence(gm => gm.createDistanceUriAndGetResponse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(responseMessageOne))
                .Returns(Task.FromResult(responseMessageTwo));
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context, mockGoogleMaps.Object);
            await populateContextWithData(context);
            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8372963, -0.12143259999999999, 1, 2);
            Assert.Empty(deliveries);
        }

        [Fact]
        public async Task testDeliveriesReturnedWhenConditionsAreMet()
        {
            var mockGoogleMaps = new Mock<LocationService>();
            var responseMessageOne = new HttpResponseMessage();
            responseMessageOne.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"0.8 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");

            var responseMessageTwo = new HttpResponseMessage();
            responseMessageTwo.Content = new StringContent("{\"destination_addresses\":[\"Village Way, Brighton BN1, United Kingdom\"],\"origin_addresses\":[\"Arts Rd, Falmer, Brighton BN1 9QN, United Kingdom\"],\"rows\":[{\"elements\":[{\"distance\":{\"text\":\"3.6 mi\",\"value\":1326},\"duration\":{\"text\":\"4 min\",\"value\":235},\"status\":\"OK\"}]}],\"status\":\"OK\"}");
            mockGoogleMaps.SetupSequence(gm => gm.createDistanceUriAndGetResponse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpClient>()))
                .Returns(Task.FromResult(responseMessageOne))
                .Returns(Task.FromResult(responseMessageTwo));
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context, mockGoogleMaps.Object);
            
            await populateContextWithData(context);
            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8372963, -0.12143259999999999, 1, 9);
            Assert.Equal(1, deliveries.Count());
        }

        [Fact]
        public void jsontest() {
            var jsonString = "{\"destination_addresses\":[\"\"],\"origin_addresses\":[\"\"],\"rows\":[{\"elements\":[{\"status\":\"NOT_FOUND\"}]}],\"status\":\"OK\"}";
            JObject json = JObject.Parse(jsonString);
            var distanceValue = (string)json.SelectToken("rows[0].elements[0].distance.text");
            //var valueInDouble = Convert.ToDouble(distanceValue.Replace("mi", ""));

        }

        private async Task  populateContextWithData(ApplicationDbContext context)
        {
            Client client = new Client
            {
                Email = Constants.DEFAULT_EMAIL,
                FirstName = Constants.DEFAULT_NAME,
                LastName = Constants.DEFAULT_NAME
            };
            ClientAddress clientAddress = new ClientAddress
            {
                LineOne = "Village Way",
                LineTwo = null,
                City = "Brighton",
                PostCode = "BN1 9BL"
            };
            context.Addresses.Add(clientAddress);
            context.Clients.Add(client);
            client.Address = clientAddress;

            Delivery delivery = new Delivery();
            delivery.DeliveryStatus = new DeliveryStatus
            {
                Status = Status.New
            };
            
            PickUpAddress address = new PickUpAddress
            {
                LineOne = "72 Newmarket Street",
                LineTwo = null,
                City = "Brighton",
                PostCode = "BN2 3QF"
            };

            context.Addresses.Add(address);
            delivery.Client = client;
            context.Deliveries.Add(delivery);
            await context.SaveChangesAsync();
        
        }
    }
}
