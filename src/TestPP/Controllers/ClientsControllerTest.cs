using DeliveryService.Controllers;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Controllers
{
    public class ClientsControllerTest
    {
        [Fact]
        public async Task testGetIndexPageAndCreateClient()
        {
            var controller = ControllerSupplier.getClientsController().Result;

            //set Shipper to controller
            Shipper shipperEntity = await createShipperEntity(controller);
            controller.setShipper(shipperEntity);


            var result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(result.Model, shipperEntity.Clients);

            Client client = getClient();
            var createResult = await controller.Create(client);
            result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(shipperEntity.Clients.Count, 1);
            Assert.Equal(result.Model, shipperEntity.Clients);
        }

        [Fact]
        public async Task testEditClient() {
            var controller = ControllerSupplier.getClientsController().Result;

            //set Shipper to controller
            var context = controller.getDbContext();
            var shipperEntity = new Shipper();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            shipperEntity.User = user;
            Client client = getClient();

            shipperEntity.Clients.Add(client);
            context.Shippers.Add(shipperEntity);
            await context.SaveChangesAsync();
            controller.setShipper(shipperEntity);


            var result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(result.Model, shipperEntity.Clients);

            var clientModel = new Client();
            //change email and leave the rest as it was 
            clientModel.ID = client.ID;
            clientModel.Email = "test2@example.com";
            clientModel.FirstName = client.FirstName;
            clientModel.LastName = client.LastName;

            await controller.Edit(client.ID, clientModel);
            Assert.Equal(shipperEntity.Clients.ToList<Client>().First().Email, "test2@example.com");
            Assert.Equal(shipperEntity.Clients.ToList<Client>().First().FirstName, client.FirstName);
            Assert.Equal(shipperEntity.Clients.ToList<Client>().First().LastName, client.LastName);
        }

        [Fact]
        public async Task testDeleteClient()
        {
            var controller = ControllerSupplier.getClientsController().Result;

            //set Shipper to controller
            var context = controller.getDbContext();
            var shipperEntity = new Shipper();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            shipperEntity.User = user;
            Client client = getClient();

            shipperEntity.Clients.Add(client);
            context.Shippers.Add(shipperEntity);
            await context.SaveChangesAsync();
            controller.setShipper(shipperEntity);

            await controller.DeleteConfirmed(client.ID);
            Assert.Equal(shipperEntity.Clients.Count, 0);
        }

        private static Client getClient()
        {
            Client client = new Client();
            client.Email = "test@example.com";
            client.FirstName = "Bob";
            client.LastName = "Marley";
            return client;
        }

        private static async Task<Shipper> createShipperEntity(ClientsController controller)
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
