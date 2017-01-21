using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Controllers.DriverControllers;
using DeliveryService.Data;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models.DriverViewModels;
using DeliveryService.Util;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DeliveryService.Models;
using AspNet.Security.OAuth.Validation;

namespace DeliveryService.AndroidApi
{
    public class AndroidActionsApiController : DriverController
    {
        
        public AndroidActionsApiController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor) { }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public JsonResult Routes() {
            List<DriverRouteView> routes = EntityToModelConverter.convertDriverRouteToDisplayViews(driver);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(routes);
        }
      
        
        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        public async Task<String> Message()
        {
            return "All ok";
        }

    }
}