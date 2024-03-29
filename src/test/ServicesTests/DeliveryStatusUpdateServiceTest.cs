﻿using DeliveryService.Data;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using DeliveryServiceTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Services
{
    /*
     * Verifies status updates are sequential
     */ 
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
            Route route = new Route();
            route.Deliveries.Add(delivery);
            context.Routes.Add(route);
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.PickedUpByDriver);
            Assert.Equal(RouteStatus.InProgress, route.Status);
            Assert.True(statusUpdated);
        }
        

        [Fact]
        public async Task testValidTransitionFromPickedUpByDriver()
        {
            Delivery delivery = new Delivery();
            context.Deliveries.Add(delivery);
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.PickedUpByDriver;
            Route route = new Route();
            route.Deliveries.Add(delivery);
            context.Routes.Add(route);
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.InTransit);
            Assert.Equal(RouteStatus.InProgress, route.Status);
            Assert.True(statusUpdated);
        }

        [Fact]
        public async Task testValidTransitionFromInTransit()
        {
            Delivery delivery = new Delivery();
            context.Deliveries.Add(delivery);
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.InTransit;
            Route route = new Route();
            route.Deliveries.Add(delivery);
            context.Routes.Add(route);
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.Delivered);
            Assert.Equal(RouteStatus.Completed, route.Status);
            Assert.True(statusUpdated);
        }

        [Fact]
        public async Task testValidTransitionFromInTransitToDelivered()
        {
            Delivery delivery = new Delivery();
            context.Deliveries.Add(delivery);
            delivery.DeliveryStatus = new DeliveryStatus();
            delivery.DeliveryStatus.Status = Status.InTransit;
            Route route = new Route();
            route.Deliveries.Add(delivery);
            context.Routes.Add(route);
            await context.SaveChangesAsync();
            bool statusUpdated = statusUpdateService.UpdateDeliveryStatus(delivery, Status.Delivered);
            Assert.Equal(RouteStatus.Completed, route.Status);
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
