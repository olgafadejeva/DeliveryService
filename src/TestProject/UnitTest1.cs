using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore.Infrastructure;
using DeliveryService.Models;
using DeliveryService.Data;
using DeliveryService.Controllers;
using Xunit;

namespace DeliveryServiceTests.Controllers
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    public class UnitTest1
    {
        private readonly IServiceProvider _serviceProvider;
        public UnitTest1()
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();


            var services = new ServiceCollection();
            services.AddOptions();
            services
                .AddDbContext<ApplicationDbContext>(b => b.UseInMemoryDatabase().UseInternalServiceProvider(efServiceProvider));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddLogging();
            services.AddOptions();

            // IHttpContextAccessor is required for SignInManager, and UserManager
            var context = new DefaultHttpContext();
            services.AddSingleton<IHttpContextAccessor>(
                new HttpContextAccessor()
                {
                    HttpContext = context,
                });

            _serviceProvider = services.BuildServiceProvider();
        }

      

        [Fact]
        public async Task TestMethod2()
        {
            var userId = "TestUserA";
            var phone = "abcdefg";
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = userId, UserName = "Test", TwoFactorEnabled = true, PhoneNumber = phone },
                "Pass@word1");
            Assert.True(userManagerResult.Succeeded);

            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

            //   var controller = new AccountController(userManager, signInManager, null, null);
        }
    }
}
