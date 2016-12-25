using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Controllers.DriverControllers;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models;
using Microsoft.AspNetCore.Identity;
using DeliveryService.Models.AccountViewModels;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;

namespace DeliveryService.AndroidApi
{
    public class AndroidApiController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AndroidApiController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)  {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<JsonResult> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Require the user to have a confirmed email before they can log in
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var userRole = userManager.GetRolesAsync(user).Result;
                    if (!userRole.Contains(AppRole.DRIVER))
                    {

                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("Must be a driver");
                    }
                    else
                    {
                        var result = await signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                        if (result.Succeeded)
                        {

                            Response.StatusCode = (int)HttpStatusCode.OK;
                            return Json("Success");
                        }
                        else
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json("Invalid attempt");
                        }
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("User does not exist");
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Invalid model");
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Logout() {
            await signInManager.SignOutAsync();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Success");
        }

        [HttpGet]
        [Authorize(Roles = "Driver")]
        public JsonResult test() {
            return Json("success hahaha");
        }
    }
}