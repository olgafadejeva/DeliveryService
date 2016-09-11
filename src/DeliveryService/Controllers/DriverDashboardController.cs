using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryService.Controllers
{   
    [Authorize]
    public class DriverDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}