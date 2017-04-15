using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using DeliveryService.ShipperControllers;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
namespace DeliveryServiceTests.Controllers
{
    public class RoutesControllerTest
    {
        [Fact]
        public async Task testGetDetailsNullID()
        {
            var controller = ControllerSupplier.getRoutesController().Result;

            //set Shipper to controller
            var context = controller.getDbContext();
            var company = new Company();
            var route = new Route();
            company.Routes.Add(route);
            await context.SaveChangesAsync();
            controller.setCompany(company);


            var result = controller.Details(null).Result;
            Assert.NotNull(result);
            Assert.Equal(result.GetType(), typeof(NotFoundResult));
        }

        [Fact]
        public async Task testGetDetailsInvalidId()
        {
            var controller = ControllerSupplier.getRoutesController().Result;

            //set Shipper to controller
            var context = controller.getDbContext();
            var company = new Company();
            var route = new Route();
            company.Routes.Add(route);
            await context.SaveChangesAsync();
            controller.setCompany(company);


            var result = controller.Details(123).Result;
            Assert.NotNull(result);
            Assert.Equal(result.GetType(), typeof(NotFoundResult));
        }

        [Fact]
        public void testGetDetailsValidId()
        {
            var controller = ControllerSupplier.getRoutesController().Result;

            //set Shipper to controller
            var context = controller.getDbContext();
            var company = new Company();
            var route = new Route();
            company.Routes.Add(route);
            context.Companies.Add(company);
            context.SaveChanges();
            controller.setCompany(company);

            var result = controller.Details(route.ID).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewResult.Model, route);
        }

        [Fact]
        public void testDeleteDeliveryFromRoute()
        {
            var controller = ControllerSupplier.getRoutesController().Result;

            //set Shipper to controller
            var context = controller.getDbContext();
            var company = new Company();
            var route = new Route();
            var delivery = new Delivery();

            route.Deliveries.Add(delivery);
            company.Routes.Add(route);
            context.Companies.Add(company);
            context.SaveChanges();
            controller.setCompany(company);
            
            Assert.Equal(route.Deliveries.ToList().Count, 1);

            var result = controller.DeleteDeliveryFromRoute(delivery.ID);
            Assert.Equal(route.Deliveries.ToList().Count, 0);
        }

        [Fact]
        public async Task testConfirmDeletion()
        {
            var controller = ControllerSupplier.getRoutesController().Result;
            
            var context = controller.getDbContext();
            var company = new Company();
            var route = new Route();
            company.Routes.Add(route);
            context.Companies.Add(company);
            context.SaveChanges();
            controller.setCompany(company);

            var deletionResult = await controller.DeleteConfirmed(route.ID) as RedirectToActionResult;

            Assert.NotNull(deletionResult);
            Assert.Equal(deletionResult.ActionName, "Index");
            Assert.Equal(context.Routes.ToList().Count, 0);
        }
    }
}
