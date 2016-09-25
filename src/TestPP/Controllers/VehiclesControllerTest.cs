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
            var driverEntity = new DeliveryService.Models.Entities.Driver();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            driverEntity.User = user;
            context.Drivers.Add(driverEntity);
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

        [Fact]
        public async Task testEditVehicle() {
            var controller = ControllerSupplier.getVehiclesController().Result;

            //set Driver to controller
            var context = controller.getDbContext();
            var driverEntity = new DeliveryService.Models.Entities.Driver();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            driverEntity.User = user;
            context.Drivers.Add(driverEntity);
            Vehicle vehicle = new Vehicle();
            vehicle.RegistrationNumber = "123456";
            driverEntity.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();
            controller.setDriver(driverEntity);


            var result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(result.Model, driverEntity.Vehicles);

            var vehicleViewModel = new Vehicle();
            vehicleViewModel.ID = vehicle.ID;
            vehicleViewModel.RegistrationNumber = "1234567";

            await controller.Edit(vehicle.ID, vehicleViewModel);
            Assert.Equal(driverEntity.Vehicles.Count, 1);
            Assert.Equal(driverEntity.Vehicles.ToList<Vehicle>().First().RegistrationNumber, "1234567");

        }

        [Fact]
        public async Task testDeleteVehicle()
        {
            var controller = ControllerSupplier.getVehiclesController().Result;

            //set Driver to controller
            var context = controller.getDbContext();
            var driverEntity = new DeliveryService.Models.Entities.Driver();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            driverEntity.User = user;
            context.Drivers.Add(driverEntity);
            Vehicle vehicle = new Vehicle();
            vehicle.RegistrationNumber = "123456";
            driverEntity.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();
            controller.setDriver(driverEntity);

            await controller.DeleteConfirmed(vehicle.ID);
            Assert.Equal(driverEntity.Vehicles.Count, 0);

        }


    }
}
