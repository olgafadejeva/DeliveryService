using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryServiceTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Data
{
    public class EntitiesTest
    {
        public ApplicationDbContext context;
        public UserManager<ApplicationUser> userManager;
        public EntitiesTest() {
            IServiceProvider _serviceProvider = ServiceBuilder.getServiceProvider();
            context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        }

        [Fact]
        public async Task test()
        {
            await populateDatabaseWithUser();
            Assert.Equal(context.ApplicationUsers.ToList<ApplicationUser>().Count, 1);
            var userId = Constants.USER_ID;

            var user = context.ApplicationUsers.SingleOrDefault(m => m.Id == userId);

            var driverEntity = new Driver();
            driverEntity.User = user;
            context.Driver.Add(driverEntity);
            await context.SaveChangesAsync();


            var driver = context.Driver.Include(b => b.User)
                .Include(b => b.Vehicles)
                .SingleOrDefault(m => m.User.Id == userId);

            driver.Vehicles.Add(new Vehicle());
        }
        private async Task populateDatabaseWithUser()
        {
            
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = Constants.USER_ID, UserName = Constants.DEFAULT_EMAIL, Email = Constants.DEFAULT_EMAIL },
                Constants.DEFAULT_PASSWORD);

            Assert.True(userManagerResult.Succeeded);
        }

    }
}
