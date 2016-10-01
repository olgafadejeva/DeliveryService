using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DeliveryService.Models;
using Microsoft.AspNetCore.Identity;
using DeliveryService.Data;

namespace DeliveryService.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private bool noUserLoggedIn;
        private string loggedInUsersRole;
        public HomeController(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, ApplicationDbContext context) {
            var currentUserId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                noUserLoggedIn = true;
            }
            else {
                var user = context.ApplicationUsers.SingleOrDefault<ApplicationUser>(b => b.Id == currentUserId);
                var userRole = userManager.GetRolesAsync(user).Result;

                if (userRole.Contains(AppRole.DRIVER))
                {
                    loggedInUsersRole = AppRole.DRIVER;//return RedirectToAction(nameof(DriverDashboardController.Index), "DriverDashboard");
                }
                else if (userRole.Contains(AppRole.SHIPPER))
                {
                    loggedInUsersRole = AppRole.SHIPPER;
                    //return RedirectToAction(nameof(ShipperDashboardController.Index), "ShipperDashboard");
                }
            }
        }
        public IActionResult Index()
        {
           // if (!)
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
