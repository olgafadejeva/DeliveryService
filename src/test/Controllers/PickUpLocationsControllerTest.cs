using DeliveryService.Models.Entities;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace DeliveryServiceTests.Controllers
{
    public class PickUpLocationsControllerTest
    {
        [Fact]
        public async Task testCreatePickUpLocation()
        {
            var controller = await ControllerSupplier.getPickUpLocationsController();
            var context = controller.getDbContext();
            var address = ShipperDetailsHelper.getDeliveryPickUpAddress();
            var company = new Company();
            
            context.Companies.Add(company);
            await context.SaveChangesAsync();
            controller.setCompany(company);

            var result = await controller.Create(address) as RedirectToActionResult;
            
            Assert.NotNull(result);
            Assert.Equal(context.PickUpAddress.ToList().Count, 1);
        }


        [Fact]
        public async Task testDeletePickUpLocationInvalidID()
        {
            var controller = await ControllerSupplier.getPickUpLocationsController();
            var context = controller.getDbContext();
            var address = ShipperDetailsHelper.getDeliveryPickUpAddress();
            var company = new Company();

            var deletionResult = await controller.Delete(123);
            Assert.NotNull(deletionResult);
            Assert.Equal(deletionResult.GetType(), typeof(NotFoundResult));
        }

        [Fact]
        public async Task testDeletePickUpLocationNullID()
        {
            var controller = await ControllerSupplier.getPickUpLocationsController();
            var context = controller.getDbContext();
            var address = ShipperDetailsHelper.getDeliveryPickUpAddress();
            var company = new Company();

            var deletionResult = await controller.Delete(null);
            Assert.NotNull(deletionResult);
            Assert.Equal(deletionResult.GetType(), typeof(NotFoundResult));
        }

        [Fact]
        public async Task testDeletePickUpLocationValidID()
        {
            var controller = await ControllerSupplier.getPickUpLocationsController();
            var context = controller.getDbContext();
            var address = ShipperDetailsHelper.getDeliveryPickUpAddress();
            var company = new Company();

            context.Companies.Add(company);
            await context.SaveChangesAsync();
            controller.setCompany(company);
            await controller.Create(address);

            var deletionResult = await controller.Delete(address.ID);
            var viewResult = Assert.IsType<ViewResult>(deletionResult);
            Assert.Equal(viewResult.Model, address);
        }

        [Fact]
        public async Task testConfirmDeletion()
        {
            var controller = await ControllerSupplier.getPickUpLocationsController();
            var context = controller.getDbContext();
            var address = ShipperDetailsHelper.getDeliveryPickUpAddress();
            var company = new Company();

            context.Companies.Add(company);
            await context.SaveChangesAsync();
            controller.setCompany(company);
            await controller.Create(address);

            var deletionResult = await controller.DeleteConfirmed(address.ID) as RedirectToActionResult;

            Assert.NotNull(deletionResult);
            Assert.Equal(deletionResult.ActionName, "Index");
            Assert.Equal(context.PickUpAddress.ToList().Count, 0);
        }

    }
}
