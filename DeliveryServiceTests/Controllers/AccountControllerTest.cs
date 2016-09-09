using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DeliveryService.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading;
using DeliveryService.Services;
using Microsoft.Extensions.Options;
using DeliveryService.Controllers;
using Microsoft.Extensions.Logging;

namespace DeliveryServiceTests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {
        public AccountControllerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        
        public static SignInManager<TUser> createSignInManager<TUser>(Mock<IUserStore<TUser>> store) where TUser : class
        {   
            var context = new Mock<HttpContext>();
            var manager = createUserManager<TUser>(store);
            return new SignInManager<TUser>(manager,
                new HttpContextAccessor { HttpContext = context.Object },
                new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                null, null);
        }

        public static UserManager<TUser> createUserManager<TUser>(Mock<IUserStore<TUser>> store) where TUser : class
        {
            IList<IUserValidator<TUser>> UserValidators = new List<IUserValidator<TUser>>();
            IList<IPasswordValidator<TUser>> PasswordValidators = new List<IPasswordValidator<TUser>>();
            
            UserValidators.Add(new UserValidator<TUser>());
            PasswordValidators.Add(new PasswordValidator<TUser>());
            var mgr = new UserManager<TUser>(store.Object, null, null, UserValidators, PasswordValidators,null, null, null, null);
            return mgr;
        }

        public static Mock<AuthMessageSender> createMockEmailSender() {
            var options = new Mock<IOptions<AuthMessageSenderOptions>>();
            
            var messageSender = new Mock<AuthMessageSender>(options);
            messageSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            return messageSender;
        }

        public static Mock<ILoggerFactory> createMockLogger() {
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            return mockLoggerFactory;
        }


        [TestMethod]
        public void TestMethod1()
        {
            var dummyUser = new ApplicationUser() { UserName = "PinkWarrior", Email = "PinkWarrior@PinkWarrior.com" };
            
            var mockStore = new Mock<IUserStore<ApplicationUser>>();

            var userManager = createUserManager<ApplicationUser>(mockStore);
            var cancellationToken = new CancellationToken();
            mockStore.Setup(x => x.CreateAsync(dummyUser, cancellationToken ))
                 .Returns(Task.FromResult(IdentityResult.Success));

            mockStore.Setup(x => x.FindByNameAsync(dummyUser.UserName, cancellationToken))
                        .Returns(Task.FromResult(dummyUser));


            //Act
            Task<IdentityResult> tt = (Task<IdentityResult>)mockStore.Object.CreateAsync(dummyUser, cancellationToken);
            var user = userManager.FindByNameAsync("PinkWarrior");
            
            Assert.IsNull(userManager.FindByNameAsync("mewo").Result);

            //Assert
            Assert.AreEqual("PinkWarrior", user.Result.UserName);
            

         AccountController accountController = new AccountController(userManager, createSignInManager(mockStore), createMockEmailSender().Object, createMockLogger().Object);
        }
    }
}
