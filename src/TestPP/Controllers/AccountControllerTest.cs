using System;
using System.Threading.Tasks;
using Xunit;
using DeliveryService.Models;
using DeliveryService.Controllers;
using Microsoft.AspNetCore.Mvc;
using DeliveryServiceTests.Helpers;
using DeliveryService.Models.AccountViewModels;
using System.Linq;
using DeliveryService.Models.Entities;

namespace DeliveryServiceTests.Controllers
{
    public class AccountControllerTest
    {
        private readonly IServiceProvider _serviceProvider;

        public AccountControllerTest()
        {
            _serviceProvider = ServiceBuilder.getServiceProvider();
        }

        
    
        [Fact]
        public void testGetLogin() {
            var controller = ControllerSupplier.getAccountController();
            var result = controller.Login("return/url");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewResult.ViewData["ReturnUrl"], "return/url");
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "Login");
        }

        [Fact]
        public async Task testPostLoginAttemptWithInvalidModel() {
            LoginViewModel model = new LoginViewModel();
            var controller = ControllerSupplier.getAccountController();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");
            var result = (ViewResult) await controller.Login(model);
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostLoginUnknownUser() {
            LoginViewModel model = new LoginViewModel();
            model.Email = Constants.DEFAULT_EMAIL;
            model.Password = Constants.DEFAULT_PASSWORD;
            var controller = ControllerSupplier.getAccountController();
            var result = (ViewResult)await controller.Login(model);
            Assert.NotEmpty(controller.ViewData.ModelState.Values);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.Equal(allErrors.First().ErrorMessage, "Invalid login attempt.");
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostLoginIncorrectPassword() {
            LoginViewModel model = new LoginViewModel();
            model.Email = Constants.DEFAULT_EMAIL;
            model.Password = "wrong_password";
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var userManager = controller.getUserManager();
            var user = await userManager.FindByIdAsync(Constants.USER_ID);
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
            model.Email = Constants.DEFAULT_EMAIL;
            model.Password = Constants.DEFAULT_PASSWORD;
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var result = (ViewResult)await controller.Login(model);
            Assert.NotEmpty(controller.ViewData.ModelState.Keys);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.Equal(allErrors.First().ErrorMessage, "You must have a confirmed email to log in.");
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostLoginSuccessWhenLoginNotFromHome() {
            LoginViewModel model = new LoginViewModel();
            model.Email = Constants.DEFAULT_EMAIL;
            model.Password = Constants. DEFAULT_PASSWORD;
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var userManager = controller.getUserManager();
            var user = await userManager.FindByIdAsync(Constants.USER_ID);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, code);

            var result = await controller.Login(model, "Register") as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal(result.ControllerName, "Home");
            Assert.Equal(result.ActionName, "Index");

        }

        [Fact]
        public async Task testPostLoginSuccessWhenLoginFromHomeShipperRole()
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = Constants.DEFAULT_EMAIL;
            model.Password = Constants.DEFAULT_PASSWORD;
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);

            var userManager = controller.getUserManager();
            var user = await userManager.FindByIdAsync(Constants.USER_ID);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, code);

            await userManager.AddToRoleAsync(user, AppRole.SHIPPER);

            var result = await controller.Login(model, "Login") as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal(result.ControllerName, "ShipperDashboard");
            Assert.Equal(result.ActionName, "Index");
        }

        [Fact]
        public async Task testPostLoginSuccessWhenLoginFromHomeDriverRole()
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = Constants.DEFAULT_EMAIL;
            model.Password = Constants.DEFAULT_PASSWORD;
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);

            var userManager = controller.getUserManager();
            var user = await userManager.FindByIdAsync(Constants.USER_ID);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, code);

            await userManager.AddToRoleAsync(user, AppRole.DRIVER);

            var result = await controller.Login(model, "Login") as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal(result.ControllerName, "DriverDashboard");
            Assert.Equal(result.ActionName, "Index");
        }


        [Fact]
        public void testGetRegister() {
            var controller = ControllerSupplier.getAccountController();
            var result = controller.Register("return/url");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewResult.ViewData["ReturnUrl"], "return/url");
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "Register");
        }

        [Fact]
        public async Task testPostRegisterInvalidModel() {
            RegisterViewModel model = new RegisterViewModel();
            var controller = ControllerSupplier.getAccountController();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");
            var result = (ViewResult)await controller.Register(model);
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostRegisterUserAlreadyExists() {
            RegisterViewModel model = new RegisterViewModel();
            model.Email = Constants.DEFAULT_EMAIL;
            model.Password = Constants.DEFAULT_PASSWORD;
            model.FirstName = "Matt";
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
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
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var result = (ViewResult)await controller.Register(model);
            Assert.Equal(result.Model, model);
            Assert.True(controller.ViewData.ModelState.ErrorCount == 1);
            var allErrors = controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.True(allErrors.First().ErrorMessage.Contains("Password"));
        }

       // [Fact]
        public async Task testPostRegisterSuccess() {
            RegisterViewModel model = new RegisterViewModel();
            model.Email = "email@test.com";
            model.Password = "123TestPassword";
            var controller = ControllerSupplier.getAccountController();
            var result = (ViewResult)await controller.Register(model);
        }

        [Fact]
        public async Task testPostLogOff() {
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            //sign in a user
            var userManager = controller.getUserManager();
            var  user = await userManager.FindByEmailAsync(Constants.DEFAULT_EMAIL);
            var signInManager = controller.getSignInManager();
            await signInManager.PasswordSignInAsync(user.Email, Constants.DEFAULT_PASSWORD, false, lockoutOnFailure: false);

            var result = await controller.LogOff() as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal(result.ControllerName, "Home");
            Assert.Equal(result.ActionName, "Index");
        }

        [Fact]
        public async Task testGetConfimEmailWithNullUserId() {
            var controller = ControllerSupplier.getAccountController();
            var result = await controller.ConfirmEmail(null, Constants.CONFIRM_CODE);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "Error");
        }

        [Fact]
        public async Task testGetConfimEmailWithNullConfirmCode()
        {
            var controller = ControllerSupplier.getAccountController();
            var result = await controller.ConfirmEmail(Constants.USER_ID, null);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "Error");
        }

        [Fact]
        public async Task testGetConfirmEmailForExistingUserWithWrongCode() {
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var result = await controller.ConfirmEmail(Constants.USER_ID, "123");
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "Error");
        }

        [Fact]
        public async Task testGetConfirmEmailForExistingUserNotDriverOrShipper()
        {
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var userManager = controller.getUserManager();
            
            var user = await userManager.FindByIdAsync(Constants.USER_ID);
            await userManager.AddToRoleAsync(user, AppRole.ADMIN);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await controller.ConfirmEmail(Constants.USER_ID, code);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "ConfirmEmail");

            var dbContext = controller.getApplicationContext();
            Assert.Equal(dbContext.Drivers.ToList<Driver>().Count, 0);
            Assert.Equal(dbContext.Shippers.ToList<Shipper>().Count, 0);
        }

        [Fact]
        public async Task testGetConfirmEmailForExistingUserDriver()
        {
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var userManager = controller.getUserManager();

            var user = await userManager.FindByIdAsync(Constants.USER_ID);
            await userManager.AddToRoleAsync(user, AppRole.DRIVER);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await controller.ConfirmEmail(Constants.USER_ID, code);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "ConfirmEmail");
            var dbContext = controller.getApplicationContext();
            Assert.Equal(dbContext.Drivers.ToList<Driver>().Count, 1);
            Assert.Equal(dbContext.Shippers.ToList<Shipper>().Count, 0);
        }

        [Fact]
        public async Task testGetConfirmEmailForExistingUserShipperRole()
        {
            var controller = ControllerSupplier.getAccountController();
            await populateDatabaseWithUser(controller);
            var userManager = controller.getUserManager();

            var user = await userManager.FindByIdAsync(Constants.USER_ID);
            await userManager.AddToRoleAsync(user, AppRole.SHIPPER);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await controller.ConfirmEmail(Constants.USER_ID, code);
            Assert.NotNull(result);
            var viewName = ((ViewResult)result).ViewName;
            Assert.Equal(viewName, "ConfirmEmail");
            var dbContext = controller.getApplicationContext();
            Assert.Equal(dbContext.Drivers.ToList<Driver>().Count, 0);
            Assert.Equal(dbContext.Shippers.ToList<Shipper>().Count, 1);
        }

        [Fact]
        public void testGetResetPasswodWithNullCode() {
            var controller = ControllerSupplier.getAccountController();
            var result = controller.ResetPassword();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "Error");
        }

        [Fact]
        public void testGetResetPasswod()
        {
            var controller = ControllerSupplier.getAccountController();
            var result = controller.ResetPassword("123");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
            Assert.Equal(viewResult.ViewName, "ResetPassword");
        }

        [Fact]
        public async Task testResetPasswordInvalidModelState() {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();
            var controller = ControllerSupplier.getAccountController();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");
            var result = (ViewResult)await controller.ForgotPassword(model);
            Assert.Equal(result.Model, model);
        }

        [Fact]
        public async Task testPostResetPasswordUserIsNull() {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();
            model.Email = "test_wrong-Email@email.com";
            var controller = ControllerSupplier.getAccountController();
            var result = await controller.ForgotPassword(model) as ViewResult;
            Assert.Equal(result.ViewName, "ForgotPasswordConfirmation");


        }

        private async Task populateDatabaseWithUser(AccountController controller) {

            var userManager = controller.getUserManager();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = Constants.USER_ID, UserName = Constants.DEFAULT_EMAIL, Email = Constants.DEFAULT_EMAIL },
                Constants.DEFAULT_PASSWORD);

            Assert.True(userManagerResult.Succeeded);
        }
    }
}

