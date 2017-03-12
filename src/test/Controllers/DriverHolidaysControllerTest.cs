using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Controllers
{
    public class DriverHolidaysControllerTest
    {
        [Fact]
        public async Task testGetIndexPageAndAddHoliday()
        {
            var controller = await ControllerSupplier.getDriverHolidaysController();

            //set Driver to controller
            var context = controller.getDbContext();
            var driverEntity = new DeliveryService.Models.Entities.Driver();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            // driverEntity.User = user;
            context.Drivers.Add(driverEntity);
            await context.SaveChangesAsync();
            controller.setDriver(driverEntity);


            var result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(result.Model, driverEntity.Holidays);

            DriverHoliday holiday = new DriverHoliday();
            DateTime to = new DateTime(2016, 12, 12);
            DateTime from = new DateTime(2016, 10, 12);
            holiday.To = to;
            holiday.From = from;

            var createResult = await controller.Create(holiday);
            result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(driverEntity.Holidays.Count, 1);
            Assert.Equal(result.Model, driverEntity.Holidays);
        }

        [Fact]
        public async Task testEditHoliday()
        {
            var controller = await ControllerSupplier.getDriverHolidaysController();

            //set Driver to controller
            var context = controller.getDbContext();
            var driverEntity = new DeliveryService.Models.Entities.Driver();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            context.Drivers.Add(driverEntity);

            DriverHoliday holiday = new DriverHoliday();
            DateTime to = new DateTime(2016, 12, 12);
            DateTime from = new DateTime(2016, 10, 12);
            holiday.To = to;
            holiday.From = from;
            driverEntity.Holidays.Add(holiday);
            await context.SaveChangesAsync();
            controller.setDriver(driverEntity);


            var result = (ViewResult)controller.Index().Result;
            Assert.NotNull(result.Model);
            Assert.Equal(result.Model, driverEntity.Holidays);

            DriverHoliday hoolidayViewModel = new DriverHoliday();
            hoolidayViewModel.ID = holiday.ID;
            hoolidayViewModel.To = to.AddDays(5);

            await controller.Edit(holiday.ID, hoolidayViewModel);
            Assert.Equal(driverEntity.Holidays.Count, 1);
            Assert.Equal(driverEntity.Holidays.ToList<DriverHoliday>().First().To, to.AddDays(5));
        }

        [Fact]
        public async Task testDeleteHoliday()
        {
            var controller = await ControllerSupplier.getDriverHolidaysController();

            //set Driver to controller
            var context = controller.getDbContext();
            var driverEntity = new DeliveryService.Models.Entities.Driver();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            context.Drivers.Add(driverEntity);

            DriverHoliday holiday = new DriverHoliday();
            DateTime to = new DateTime(2016, 12, 12);
            DateTime from = new DateTime(2016, 10, 12);
            holiday.To = to;
            holiday.From = from;
            driverEntity.Holidays.Add(holiday);
            await context.SaveChangesAsync();
            controller.setDriver(driverEntity);

            await controller.DeleteConfirmed(holiday.ID);
            Assert.Equal(driverEntity.Holidays.Count, 0);
        }
    }
}
