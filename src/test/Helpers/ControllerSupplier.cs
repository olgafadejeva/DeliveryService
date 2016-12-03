using DeliveryService.Controllers;
using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Data;
using DeliveryService.DriverControllers;
using DeliveryService.Models;
using DeliveryService.Services;
using DeliveryService.ShipperControllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace DeliveryServiceTests.Helpers
{
    public class ControllerSupplier
    {
        public static  AccountController getAccountController()
        {

            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var emailSender = _serviceProvider.GetRequiredService<IEmailSender>();
            var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();

            var controller = new AccountController(userManager, signInManager, emailSender, context, userService);
            controller.ControllerContext.HttpContext = httpContext;
            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            SetRouteData(_serviceProvider, httpContextAccessor, controller);
            return controller;
        }

        public async static Task<VehiclesController> getVehiclesController()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = Constants.USER_ID, UserName = Constants.DEFAULT_EMAIL, Email = Constants.DEFAULT_EMAIL },
                Constants.DEFAULT_PASSWORD);
            await signInManager.PasswordSignInAsync(Constants.DEFAULT_EMAIL, Constants.DEFAULT_PASSWORD, false, lockoutOnFailure: false);
            var controller = new VehiclesController(context, httpContextAccessor);

            var actionContext = new ActionContext();
            controller.Url = new UrlHelper(actionContext);
            return controller;
        }

        public async static Task<ClientsController> getClientsController()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = Constants.USER_ID, UserName = Constants.DEFAULT_EMAIL, Email = Constants.DEFAULT_EMAIL },
                Constants.DEFAULT_PASSWORD);
            await signInManager.PasswordSignInAsync(Constants.DEFAULT_EMAIL, Constants.DEFAULT_PASSWORD, false, lockoutOnFailure: false);
            var controller = new ClientsController(context, httpContextAccessor, null);
            SetRouteData(_serviceProvider, httpContextAccessor, controller);
            return controller;
        }

        public async static Task<TeamController> getTeamsController()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = Constants.USER_ID, UserName = Constants.DEFAULT_EMAIL, Email = Constants.DEFAULT_EMAIL },
                Constants.DEFAULT_PASSWORD);
            await signInManager.PasswordSignInAsync(Constants.DEFAULT_EMAIL, Constants.DEFAULT_PASSWORD, false, lockoutOnFailure: false);

            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var controller = new TeamController(context, httpContextAccessor, null);
            SetRouteData(_serviceProvider, httpContextAccessor, controller);
            return controller;
        }

        private static void SetRouteData(IServiceProvider _serviceProvider, IHttpContextAccessor httpContextAccessor, Controller controller)
        {
            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextAccessor.HttpContext;
            var routeData = new RouteData();
            routeData.Values.Add("default", "{controller=Home}/{action=Index}/{id?}");

            actionContext.RouteData = routeData;
            actionContext.ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor();

            var contextAccessor = _serviceProvider.GetRequiredService<IActionContextAccessor>();
            contextAccessor.ActionContext = actionContext;
            var helperFactory = _serviceProvider.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = helperFactory.GetUrlHelper(contextAccessor.ActionContext);
            controller.ControllerContext = new ControllerContext(actionContext);
            controller.Url = urlHelper;
        }

        public async static Task<ShipperDeliveriesController> getShipperDeliveriesController()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = Constants.USER_ID, UserName = Constants.DEFAULT_EMAIL, Email = Constants.DEFAULT_EMAIL },
                Constants.DEFAULT_PASSWORD);
            await signInManager.PasswordSignInAsync(Constants.DEFAULT_EMAIL, Constants.DEFAULT_PASSWORD, false, lockoutOnFailure: false);

            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var controller = new ShipperDeliveriesController(context, httpContextAccessor);
            SetRouteData(_serviceProvider, httpContextAccessor, controller);
            return controller;
        }
        
        public static Mock<IEmailSender> createMockEmailSender()
        {
            var messageSender = new Mock<IEmailSender>();
            messageSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            return messageSender;
        }
    }
}
