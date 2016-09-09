using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using DeliveryService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using DeliveryService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeliveryServiceTests.Helpers;
using DeliveryService.Services;
using DeliveryService.Models.AccountViewModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DeliveryServiceTests.Controllers
{
    public class AccountControllerTest
    {
        private readonly IServiceProvider _serviceProvider;
        private const String CONFIRM_CODE = "12345";
        private const String USER_ID = "6789";
        private const String DEFAULT_EMAIL = "test@test.com";
        private const String DEFAULT_PASSWORD = "Password123";

        public AccountControllerTest()
        {
            _serviceProvider = ServiceBuilder.getServiceProvider();
        }

        [Fact]
        public async Task PassingTest()
        {
            var userId = "TestUserA";
            var phone = "abcdefg";

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = userId, UserName = "Test", TwoFactorEnabled = true, PhoneNumber = phone },
                "Pass@word1");
            Assert.True(userManagerResult.Succeeded);


            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();


            var controller = new AccountController(userManager, signInManager, null, loggerFactory);
            controller.ControllerContext.HttpContext = httpContext;


            var result = controller.Login("return/url");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewResult.ViewData["ReturnUrl"], "return/url");
        }

        [Fact]
        public void testGetLogin() {
            var controller = getAccountController();
            var result = controller.Login("return/url");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewResult.ViewData["ReturnUrl"], "return/url");
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "Login");
        }

        [Fact]
        public async Task testPostLoginAttemptWithInvalidModel() {
            LoginViewModel model = new LoginViewModel();
            var controller = getAccountController();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");
            var result = (ViewResult) await controller.Login(model);
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostLoginUnknownUser() {
            LoginViewModel model = new LoginViewModel();
            model.Email = DEFAULT_EMAIL;
            model.Password = DEFAULT_PASSWORD;
            var controller = getAccountController();
            var result = (ViewResult)await controller.Login(model);
            Assert.NotEmpty(controller.ViewData.ModelState.Values);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.Equal(allErrors.First().ErrorMessage, "Invalid login attempt.");
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostLoginIncorrectPassword() {
            LoginViewModel model = new LoginViewModel();
            model.Email = DEFAULT_EMAIL;
            model.Password = "wrong_password";
            var controller = getAccountControllerInstanceWithOneRegisteredUser().Result;
            var userManager = controller.getUserManager();
            var user = await userManager.FindByIdAsync(USER_ID);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, code);

            var result = (ViewResult)await controller.Login(model);
            Assert.NotEmpty(controller.ViewData.ModelState.Keys);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.Equal(allErrors.First().ErrorMessage, "Invalid login attempt.");
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostLoginEmailNotConfirmed() {
            LoginViewModel model = new LoginViewModel();
            model.Email = DEFAULT_EMAIL;
            model.Password = DEFAULT_PASSWORD;
            var controller = getAccountControllerInstanceWithOneRegisteredUser().Result;
            var result = (ViewResult)await controller.Login(model);
            Assert.NotEmpty(controller.ViewData.ModelState.Keys);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.Equal(allErrors.First().ErrorMessage, "You must have a confirmed email to log in.");
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostLoginSuccess() {
            LoginViewModel model = new LoginViewModel();
            model.Email = DEFAULT_EMAIL;
            model.Password = DEFAULT_PASSWORD;
            var controller = getAccountControllerInstanceWithOneRegisteredUser().Result;

            var userManager = controller.getUserManager();
            var user = await userManager.FindByIdAsync(USER_ID);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, code);

            var result = await controller.Login(model, "Register") as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal(result.ControllerName, "Home");
            Assert.Equal(result.ActionName, "Index");

        }

        [Fact]
        public void testGetRegister() {
            var controller = getAccountController();
            var result = controller.Register("return/url");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewResult.ViewData["ReturnUrl"], "return/url");
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "Register");
        }

        [Fact]
        public async Task testPostRegisterInvalidModel() {
            RegisterViewModel model = new RegisterViewModel();
            var controller = getAccountController();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");
            var result = (ViewResult)await controller.Register(model);
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostRegisterUserAlreadyExists() {
            RegisterViewModel model = new RegisterViewModel();
            model.Email = DEFAULT_EMAIL;
            model.Password = DEFAULT_PASSWORD;
            model.FirstName = "Matt";
            var controller = getAccountControllerInstanceWithOneRegisteredUser().Result;
            var result = (ViewResult) await controller.Register(model);
            Assert.Equal(result.Model, model);
            Assert.True(controller.ViewData.ModelState.ErrorCount == 1);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.True(allErrors.First().ErrorMessage.Contains("address already exists"));
        }

        [Fact]
        public async Task testPostRegisterUserWithInvalidPassword()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.Email = "email@test.com";
            model.Password = "123";
            var controller = getAccountControllerInstanceWithOneRegisteredUser().Result;
            var result = (ViewResult)await controller.Register(model);
            Assert.Equal(result.Model, model);
            Assert.True(controller.ViewData.ModelState.ErrorCount == 1);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.True(allErrors.First().ErrorMessage.Contains("Password"));
        }

        [Fact]
        public async Task testGetConfimEmailWithNullUserId() {
            var controller = getAccountController();
            var result = await controller.ConfirmEmail(null, CONFIRM_CODE);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "Error");
        }

        [Fact]
        public async Task testGetConfimEmailWithNullConfirmCode()
        {
            var controller = getAccountController();
            var result = await controller.ConfirmEmail(USER_ID, null);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "Error");
        }

        [Fact]
        public async Task testGetConfirmEmailForExistingUserWithWrongCode() {
            var controller = getAccountControllerInstanceWithOneRegisteredUser().Result;
            var result = await controller.ConfirmEmail(USER_ID, "123");
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "Error");
        }

        [Fact]
        public async Task testGetConfirmEmailForExistingUser()
        {
            var controller = getAccountControllerInstanceWithOneRegisteredUser().Result;
            var userManager = controller.getUserManager();
            
            var user = await userManager.FindByIdAsync(USER_ID);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await controller.ConfirmEmail(USER_ID, code);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "ConfirmEmail");
        }

        [Fact]
        public void testGetResetPasswodWithNullCode() {
            var controller = getAccountController();
            var result = controller.ResetPassword();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "Error");
        }

        [Fact]
        public void testGetResetPasswod()
        {
            var controller = getAccountController();
            var result = controller.ResetPassword("123");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "ResetPassword");
        }

        private async Task<AccountController> getAccountControllerInstanceWithOneRegisteredUser() {
            var userId = USER_ID;

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = userId, UserName = DEFAULT_EMAIL, Email = DEFAULT_EMAIL },
                DEFAULT_PASSWORD);
            Assert.True(userManagerResult.Succeeded);


            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();


            var controller = new AccountController(userManager, signInManager, null, loggerFactory);
            controller.ControllerContext.HttpContext = httpContext;

            controller.ControllerContext.RouteData = new RouteData();

            var actionContext = new ActionContext();
            controller.Url = new UrlHelper(actionContext);
            

            return controller;
        }

        private AccountController getAccountController() {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();


            var controller = new AccountController(userManager, signInManager, null, loggerFactory);
            controller.ControllerContext.HttpContext = httpContext;
            controller.ControllerContext.RouteData = new RouteData();

            var actionContext = new ActionContext();
            controller.Url = new UrlHelper(actionContext);
            return controller;
        }
        /*
        public static Mock<AuthMessageSender> createMockEmailSender()
        {
            var options = new Mock<IOptions<AuthMessageSenderOptions>>();

            var messageSender = new Mock<AuthMessageSender>(options);
            messageSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            return messageSender;
        }
        */
    }
}

