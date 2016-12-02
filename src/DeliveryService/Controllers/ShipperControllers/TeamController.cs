using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;
using DeliveryService.Models;
using DeliveryService.Services;
using DeliveryService.Models.ShipperViewModels;
using DeliveryService.Controllers.ShipperControllers;

namespace DeliveryService.ShipperControllers
{
    public class TeamController : ShipperController
    {

        public IUserService userService { get; set; }

        public TeamController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IUserService userService) : base(context, contextAccessor)
        {
            this.userService = userService;
        }
        
        public  IActionResult Index()
        { 
            Team team = company.Team;
            return View(team);
        }
        
        public IActionResult DriverDetails(int? id)
        {
            var driver = company.Team.Drivers.SingleOrDefault(d => d.ID == id);
            if (driver == null) {
                return NotFound();
            }

            return View(driver);
        }


        public IActionResult AddEmployee() {
            EmployeeCreateView model = new EmployeeCreateView();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(EmployeeCreateView model)
        {
            var result = await userService.CreateEmployeeUserAsync(model, company);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


        public IActionResult AddDriver()
        {
            return View(new DriverDetails());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDriver( DriverDetails driver)
        {
            if (ModelState.IsValid)
            {
                var callbackUrl = Url.Action("RegisterDriver", "Account", new { teamId = company.Team.ID, email = driver.Email, firstName = driver.FirstName }, "https");
                await userService.GenerateDriverRegistrationRequestAsync(driver, company,  callbackUrl);
                return RedirectToAction("Index");
            }
            return View(company.Team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDriver(int? driverId)
        {
            var team = company.Team;
            var drivers = team.Drivers;
            foreach (Driver driver in drivers) {
                if (driver.ID == driverId) {
                    team.Drivers.Remove(driver);
                    await _context.SaveChangesAsync();
                    break;
                }
            }
            
            return View(team);
        }
        
        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID == id);
        }
        
    }
}
