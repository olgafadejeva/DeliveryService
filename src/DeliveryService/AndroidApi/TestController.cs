using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;

namespace DeliveryService.AndroidApi
{
    public class TestController : Controller
    {
        
        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public JsonResult test()
        {
            return Json("success hahaha");
        }
    }
}