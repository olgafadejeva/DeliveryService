using DeliveryService.Data;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using DeliveryServiceTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task testDeliveryReturnedWithinThreeMiles() {

            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context);
            AppProperties props = new AppProperties();
            props.GoogleMapsApiKey = "AIzaSyCruDMQXf92lZMvfCEO_L9E2oYjvuRfPaI";
            service.options = props;
            await populateContextWithData(context);
            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8662053, -0.0884891, 5, 5);
            Assert.Equal(1, deliveries.Count());

        }

        [Fact]
        public async Task testDeliveryReturnedWithinZeroMiles()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context);
            AppProperties props = new AppProperties();
            props.GoogleMapsApiKey = "AIzaSyCruDMQXf92lZMvfCEO_L9E2oYjvuRfPaI";
            service.options = props;
            await populateContextWithData(context);
            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8662053, -0.0884891, 0, 0);
            Assert.Empty(deliveries);
        }

        [Fact]
        public async Task testNoDeliveriesReturnedWhenOneConditionIsNotSatisfied()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context);
            AppProperties props = new AppProperties();
            props.GoogleMapsApiKey = "AIzaSyCruDMQXf92lZMvfCEO_L9E2oYjvuRfPaI";
            service.options = props;
            await populateContextWithData(context);
            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8372963, -0.12143259999999999, 1, 2);
            Assert.Empty(deliveries);
        }

        [Fact]
        public async Task testDeliveriesReturnedWhenConditionsAreMet()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            service = new DeliverySearchService(context);
            AppProperties props = new AppProperties();
            props.GoogleMapsApiKey = "AIzaSyCruDMQXf92lZMvfCEO_L9E2oYjvuRfPaI";
            service.options = props;
            await populateContextWithData(context);
            IEnumerable<Delivery> deliveries = await service.searchForDeliveriesWithinDistance(50.8372963, -0.12143259999999999, 1, 9);
            Assert.Equal(1, deliveries.Count());
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
                Status = Status.New,
                AssignedTo = null
            };
            
            PickUpAddress address = new PickUpAddress
            {
                LineOne = "72 Newmarket Street",
                LineTwo = null,
                City = "Brighton",
                PostCode = "BN2 3QF"
            };

            context.Addresses.Add(address);
            delivery.PickUpAddress = address;
            delivery.Client = client;
            context.Deliveries.Add(delivery);
            await context.SaveChangesAsync();
        
        }
    }
}
