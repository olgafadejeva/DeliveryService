using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Models.ShipperViewModels;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Controllers
{
    public class ShipperDeliveriesControllerTest
    {
        [Fact]
        public async Task testCreateDeliveryValidModel()
        {
            var controller = await ControllerSupplier.getShipperDeliveriesController();

            //set Shipper to controller
            Shipper shipperEntity = await createShipperEntity(controller);
            controller.setShipper(shipperEntity);
            var dbContext = controller.getDbContext();

            Client client = ShipperDetailsHelper.getClient();
            shipperEntity.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            DeliveryDetails deliveryDetails = new DeliveryDetails();
            deliveryDetails.ClientID = client.ID;
            deliveryDetails.PickUpAddress = ShipperDetailsHelper.getDeliveryPickUpAddress();

            var createResult = await controller.Create(deliveryDetails);
            var result = (ViewResult) controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(dbContext.Deliveries.Count(), 1);
            Delivery delivery = dbContext.Deliveries.ToList().First<Delivery>();
            Assert.Equal(delivery.Client, client);
            Assert.Equal(delivery.DeliveryStatus.Status, Status.New);
            Assert.Equal(delivery.PickUpAddress, deliveryDetails.PickUpAddress);
        }

        private static async Task<Shipper> createShipperEntity(ShipperController controller)
        {
            var context = controller.getDbContext();
            var shipperEntity = new Shipper();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            shipperEntity.User = user;
            context.Shippers.Add(shipperEntity);
            await context.SaveChangesAsync();
            return shipperEntity;
        }
    }
}
