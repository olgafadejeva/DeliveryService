using DeliveryService.Controllers;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Models.ShipperViewModels;
using DeliveryService.ShipperControllers;
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
            var controller = await ControllerSupplier.getClientsController();

            //set Shipper to controller
            Shipper shipperEntity = await createShipperEntity(controller);
            controller.setShipper(shipperEntity);


            var result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(result.Model, shipperEntity.Clients);

            Client newClient = getClient();
            var createResult = await controller.Create(newClient);
            result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(shipperEntity.Clients.Count, 1);

            Client client = shipperEntity.Clients.First<Client>();
            Assert.Equal(result.Model, shipperEntity.Clients);
            Assert.NotNull(client.Address);
            Assert.Equal(client.Address.City, Constants.DEFAULT_CITY);
            Assert.Equal(client.Address.LineOne, Constants.DEFAULT_ADDRESS_LINE_ONE);
            Assert.Equal(client.Address.PostCode, Constants.DEFAULT_POSTCODE);
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
            clientModel.Address.City = Constants.DEFAULT_CITY;

            await controller.Edit(client.ID, clientModel);
            Assert.Equal(shipperEntity.Clients.ToList<Client>().First().Email, "test2@example.com");
            Assert.Equal(shipperEntity.Clients.ToList<Client>().First().FirstName, client.FirstName);
            Assert.Equal(shipperEntity.Clients.ToList<Client>().First().LastName, client.LastName);
            Assert.Equal(shipperEntity.Clients.ToList<Client>().First().Address.City, client.Address.City);
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
            client.Address.City = Constants.DEFAULT_CITY;

            shipperEntity.Clients.Add(client);
            context.Shippers.Add(shipperEntity);
            await context.SaveChangesAsync();

            Assert.Equal(context.Clients.Count<Client>(), 1);
            Assert.Equal(context.Addresses.Count<Address>(), 1);
            controller.setShipper(shipperEntity);

            await controller.DeleteConfirmed(client.ID);
            Assert.Equal(shipperEntity.Clients.Count, 0);
            Assert.Equal(context.Clients.Count<Client>(), 0);
            Assert.Equal(context.Addresses.Count<Address>(), 0);
        }

        private static Client getClient() {
            Client client = new Client();
            client.Email = Constants.DEFAULT_EMAIL;
            client.FirstName = Constants.DEFAULT_NAME;
            client.LastName = Constants.DEFAULT_NAME;
            client.Address.LineOne = Constants.DEFAULT_ADDRESS_LINE_ONE;
            client.Address.City = Constants.DEFAULT_CITY;
            client.Address.PostCode = Constants.DEFAULT_POSTCODE;
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
