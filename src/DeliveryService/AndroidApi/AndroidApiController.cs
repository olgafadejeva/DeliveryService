using DeliveryService.Data;
using DeliveryService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace DeliveryService.AndroidApi
{
    [Route("api")]
    public class AndroidApiController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AndroidApiController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)  {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Logout() {
            await signInManager.SignOutAsync();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Success");
        }
    }
}