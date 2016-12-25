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

namespace DeliveryService.AndroidApi
{
    public class AndroidActionsApiController : DriverController
    {

        public AndroidActionsApiController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor) { }

        [HttpGet]
        public JsonResult Routes() {
            List<DriverRouteView> viewModels = EntityToModelConverter.convertDriverRouteToDisplayViews(driver);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(viewModels);
        }
       
    }
}