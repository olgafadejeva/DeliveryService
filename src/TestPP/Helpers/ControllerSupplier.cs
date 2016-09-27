﻿using DeliveryService.Controllers;
using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Services;
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
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();

            var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();


            var controller = new AccountController(userManager, signInManager, createMockEmailSender().Object, loggerFactory, context);
            controller.ControllerContext.HttpContext = httpContext;
            controller.ControllerContext.RouteData = new RouteData();

            var actionContext = new ActionContext();
            controller.Url = new UrlHelper(actionContext);
            ConfigureRouteData(controller);
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
            var controller = new ClientsController(context, httpContextAccessor);

            var actionContext = new ActionContext();
            controller.Url = new UrlHelper(actionContext);
            return controller;
        }

        public async static Task<TeamsController> getTeamsController()
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
            var controller = new TeamsController(context, httpContextAccessor, createMockEmailSender().Object);

          var contextAccessor =   _serviceProvider.GetRequiredService<IActionContextAccessor>();
            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextAccessor.HttpContext;
            var routeData = new RouteData();
            routeData.Values.Add("default", "{controller=Home}/{action=Index}/{id?}");
            
            actionContext.RouteData = routeData;
            actionContext.ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor();
            contextAccessor.ActionContext = actionContext;
            var helperFactory = _serviceProvider.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = helperFactory.GetUrlHelper(contextAccessor.ActionContext);

         /*   var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextAccessor.HttpContext;
            var routeData = new RouteData();
            routeData.Values.Add("default", "{controller=Home}/{action=Index}/{id?}");
            actionContext.RouteData = routeData;
            actionContext.ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor();*/
            controller.ControllerContext = new ControllerContext(actionContext);



            controller.Url = urlHelper;
            
           // ConfigureRouteData(controller);
            return controller;
        }




        private static void ConfigureRouteData(Controller controller)
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
