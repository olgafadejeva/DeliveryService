using System;
using Microsoft.Extensions.DependencyInjection;

using DeliveryService.Data;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using DeliveryService.Data.Initializer;
using DeliveryService.Services.Config;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DeliveryServiceTests.MockServices;
using DeliveryService.Services;

namespace DeliveryServiceTests.Helpers
{
    public static class ServiceBuilder
    {
        public static IServiceProvider getServiceProvider()
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            var services = new ServiceCollection();
            services.AddOptions();
            services
                .AddDbContext<ApplicationDbContext>(b => b.UseInMemoryDatabase().UseInternalServiceProvider(efServiceProvider));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders()
                     .AddErrorDescriber<CustomIdentityErrorDescriber>();

            services.AddLogging();
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature() { Handler = new TestAuthHandler() });
            services.AddSingleton<IHttpContextAccessor>(
                new HttpContextAccessor()
                {
                    HttpContext = context,
                });
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<HttpContext>(context);

            services.AddLogging();
            services.AddMvc();

            services.AddTransient<IEmailSender, MockAuthMessageSender>();
            services.AddTransient<IUserService, MockUserService>();
            services.AddRouting();

            var serviceProvider = services.BuildServiceProvider();
            DatabaseInitializer.Initialize(serviceProvider);

            return serviceProvider;
        }

        public static IServiceProvider getServiceProviderWithInMemoryDatabase() {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            var services = new ServiceCollection();
            services.AddOptions();
            services
                .AddDbContext<ApplicationDbContext>(b => b.UseInMemoryDatabase().UseInternalServiceProvider(efServiceProvider));

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}