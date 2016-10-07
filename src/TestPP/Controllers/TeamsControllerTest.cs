using DeliveryService.Controllers;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
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
    public class TeamsControllerTest
    {
        [Fact]
        public async Task testGetIndexPageAndCreateTeam()
        {
            var controller = ControllerSupplier.getTeamsController().Result;

            //set Shipper to controller
            Shipper shipperEntity = await ShipperDetailsHelper.createShipperEntity(controller);
            controller.setShipper(shipperEntity);
            var dbContext = controller.getDbContext();

            var result = (ViewResult)controller.Index();
            Assert.Null(result.Model);

            Team team = ShipperDetailsHelper.getTeam();
            var createResult = await controller.Create(team);
            result = (ViewResult)controller.Index();
            Assert.NotNull(result.Model);
            Assert.Equal(shipperEntity.Team, team);
            Assert.Equal(dbContext.Teams.Count(), 1);
        }

       // [Fact]
        public async Task testPostAddDriver() {
            var controller = ControllerSupplier.getTeamsController().Result;

            //set Shipper to controller
            Shipper shipperEntity = await ShipperDetailsHelper.createShipperEntity(controller);
            controller.setShipper(shipperEntity);
            Team team = ShipperDetailsHelper.getTeam();
            await controller.Create(team);

            DriverDetails driverDetails = new DriverDetails();
            driverDetails.Email = Constants.DEFAULT_EMAIL;
            driverDetails.FirstName = Constants.DEFAULT_NAME;

            var result = await controller.AddDriver(team.ID, team, driverDetails);
            var dbContext = controller.getDbContext();
            Assert.Equal(dbContext.DriverRegistrationRequests.Count(), 1);


        }

        [Fact]
        public async Task testPostDeleteDriver() {
            var controller = ControllerSupplier.getTeamsController().Result;

            //set Shipper to controller
            Shipper shipperEntity = await ShipperDetailsHelper.createShipperEntity(controller);
            controller.setShipper(shipperEntity);
            var dbContext = controller.getDbContext();
            Team team = ShipperDetailsHelper.getTeam();
            await controller.Create(team);

            Driver driver = new Driver();
            driver.TeamID = team.ID;
            driver.Team = team;
            driver.User = dbContext.ApplicationUsers.First<ApplicationUser>();
             dbContext.Drivers.Add(driver);
            await dbContext.SaveChangesAsync();
            Assert.Equal(team.Drivers.Count(), 1);
            Assert.True(team.Drivers.Contains(driver));

            var result = await controller.DeleteDriver(team.ID, driver.ID);
            Assert.Equal(team.Drivers.Count(), 0);
            Assert.Null(driver.Team);
            Assert.Null(driver.TeamID);
        }
    }
}
