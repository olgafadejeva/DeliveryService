using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Controllers
{
    public class VehiclesControllerTest
    {

        [Fact]
        public async Task testGetIndexPageAndAddVehicle() {
            var controller =  ControllerSupplier.getVehiclesController().Result;

            //set Driver to controller
            var context = controller.getDbContext();
            var driverEntity = new Driver();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            driverEntity.User = user;
            context.Driver.Add(driverEntity);
            await context.SaveChangesAsync();
            controller.setDriver(driverEntity);


            var result = (ViewResult) controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(result.Model, driverEntity.Vehicles);

            Vehicle vehicle = new Vehicle();
            vehicle.RegistrationNumber = "123456";
            var createResult = await controller.Create(vehicle);
            result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(driverEntity.Vehicles.Count, 1);
            Assert.Equal(result.Model, driverEntity.Vehicles);

        }


    }
}
