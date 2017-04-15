using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Models.ShipperViewModels;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Controllers
{
    /*
     * Tests interactions with shipper's deliveries controller
     */
    public class ShipperDeliveriesControllerTest
    {
        [Fact]
        public async Task testCreateDeliveryValidModel()
        {
            var controller = await ControllerSupplier.getShipperDeliveriesController();

            //set Shipper to controller
            Company company = await createCompany(controller);
            controller.setCompany(company);
            var dbContext = controller.getDbContext();

            Client client = ShipperDetailsHelper.getClient();
            company.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            DeliveryDetails deliveryDetails = new DeliveryDetails();
            deliveryDetails.ClientID = client.ID;

            var createResult = await controller.Create(deliveryDetails);
            var result = (ViewResult) controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(dbContext.Deliveries.Count(), 1);
            Delivery delivery = dbContext.Deliveries.ToList().First<Delivery>();
            Assert.Equal(delivery.Client, client);
            Assert.Equal(delivery.DeliveryStatus.Status, Status.New);
        }

        private static async Task<Company> createCompany(ShipperController controller)
        {
            var context = controller.getDbContext();
            var company = new Company();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            context.Companies.Add(company);
            await context.SaveChangesAsync();
            return company;
        }
    }
}
