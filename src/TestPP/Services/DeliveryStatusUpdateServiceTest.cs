using DeliveryService.Data;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using DeliveryServiceTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Services
{
    public class DeliveryStatusUpdateServiceTest
    {

        private readonly DeliveryStatusUpdateService statusUpdateService;
        public ApplicationDbContext context { get; set; }
        public DeliveryStatusUpdateServiceTest()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProviderWithInMemoryDatabase();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            statusUpdateService = new DeliveryStatusUpdateService(context);
        }

        [Fact]
        public async Task testValidTransitionFromNew() {
            Delivery delivery = new Delivery();
            context.Deliveries.Add(delivery);
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.New;
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.PickedUpByDriver);
            Assert.True(statusUpdated);
        }

        [Fact]
        public async Task testValidTransitionFromAcceptedByDriver()
        {
            Delivery delivery = new Delivery();
            context.Deliveries.Add(delivery);
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.ClaimedByDriver;
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.PickedUpByDriver);
            Assert.True(statusUpdated);
        }

        [Fact]
        public async Task testValidTransitionFromPickedUpByDriver()
        {
            Delivery delivery = new Delivery();
            context.Deliveries.Add(delivery);
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.PickedUpByDriver;
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.InTransit);
            Assert.True(statusUpdated);
        }

        [Fact]
        public async Task testValidTransitionFromInTransit()
        {
            Delivery delivery = new Delivery();
            context.Deliveries.Add(delivery);
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.InTransit;
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.Delivered);
            Assert.True(statusUpdated);
        }

        [Fact]
        public void testInvalidTransitionsFromNew()
        {
            Delivery delivery = new Delivery();
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.New;
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.InTransit);
            Assert.False(statusUpdated);

            statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.Delivered);
            Assert.False(statusUpdated);
           
        }

        [Fact]
        public void testInvalidTransitionsFromPickedUpByDriver()
        {
            Delivery delivery = new Delivery();
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.PickedUpByDriver;
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.New);
            Assert.False(statusUpdated);

            statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.Delivered);
            Assert.False(statusUpdated);

            statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.PickedUpByDriver);
            Assert.False(statusUpdated);
        }
    }
}
