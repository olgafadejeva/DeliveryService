using DeliveryService.Controllers;
using DeliveryService.Models;
using DeliveryService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public static AccountController getAccountController()
        {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();


            var controller = new AccountController(userManager, signInManager, createMockEmailSender().Object, loggerFactory);
            controller.ControllerContext.HttpContext = httpContext;
           

            ConfigureRouteData(controller);
            return controller;
        }

        public static async Task<AccountController> getAccountControllerInstanceWithOneRegisteredUser()
        {

            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            var userId = Constants.USER_ID;

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = userId, UserName = Constants.DEFAULT_EMAIL, Email = Constants.DEFAULT_EMAIL },
                Constants.DEFAULT_PASSWORD);


            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();


            var controller = new AccountController(userManager, signInManager, createMockEmailSender().Object, loggerFactory);

            controller.ControllerContext.HttpContext = httpContext;
            ConfigureRouteData(controller);
            return controller;
        }

        private static void ConfigureRouteData(AccountController controller)
        {
            var routeData = new RouteData();
            routeData.Values.Add("default", "{controller=Home}/{action=Index}/{id?}");
            controller.ControllerContext.RouteData = routeData;

            var actionContext = new ActionContext();
            actionContext.RouteData = routeData;
            controller.Url = new UrlHelper(actionContext);
        }

        public static Mock<IEmailSender> createMockEmailSender()
        {
            var messageSender = new Mock<IEmailSender>();
            messageSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            return messageSender;
        }

    }
}
