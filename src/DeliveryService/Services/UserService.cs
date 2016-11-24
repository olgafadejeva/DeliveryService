using DeliveryService.Data;
using DeliveryService.Entities;
using DeliveryService.Models;
using DeliveryService.Models.AccountViewModels;
using DeliveryService.Models.Entities;
using DeliveryService.Models.ShipperViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private ApplicationDbContext _context { get; }

        public UserService(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {

            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<IdentityResult> CreateDriverUserAsync(DriverRegisterViewModel model)
        {
            Company company = _context.Companies.SingleOrDefault(c => c.Team.ID == Convert.ToInt32(model.DriverTeamId));
            var user = new DriverUser { UserName = model.Email, Email = model.Email, CompanyID = company.ID };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AppRole.DRIVER);
                await ConfirmUserEmail(user);

                var driverRegistrationRequest = _context.DriverRegistrationRequests.SingleOrDefault(m => m.DriverEmail == model.Email);
                var teamIdForDriver = driverRegistrationRequest.TeamID.ToString();
                await CreateDriverEntity(user, teamIdForDriver);
            }
            return result;
        }

        public async Task<IdentityResult> SignUpCompanyAsync(CompanyRegistrationViewModel model, IUrlHelper Url, HttpContext httpContext)
        {
            Company company = new Company();
            company.CompanyName = model.CompanyName;
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            var user = new EmployeeUser { UserName = model.Email, Email = model.Email, CompanyID = company.ID, PrimaryContact = true };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                company.Team.Employees.Add(user);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, AppRole.SHIPPER);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: httpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
            }
            return result;
        }


        public async Task<IdentityResult> CreateEmployeeUserAsync(EmployeeCreateView model, Company company)
        {
            string DEFAULT_NEW_EMPLOYEE_PASSWORD = company.CompanyName.ToUpper() + "123"; ;
            ApplicationUser employee = new ApplicationUser { Email = model.Email, CompanyID = company.ID, UserName = model.Email };
            var user = new EmployeeUser { UserName = model.Email, Email = model.Email, CompanyID = company.ID, PrimaryContact = false };
            var result = await _userManager.CreateAsync(user, DEFAULT_NEW_EMPLOYEE_PASSWORD);
            if (result.Succeeded)
            {
                company.Team.Employees.Add(user);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, AppRole.SHIPPER);
                //confirm email for the user
                await ConfirmUserEmail(user);

                await _emailSender.SendEmailAsync(model.Email, "You have been created an accout",
                    $"Please log into the system with the default password " + DEFAULT_NEW_EMPLOYEE_PASSWORD + " , please dont forget to change it ");
            }
            return result;
        }

        private async Task ConfirmUserEmail(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, code);
        }

        private async Task CreateDriverEntity(DriverUser user, string teamId)
        {
            var driverEntity = new Driver();
            driverEntity.User = user;
            int idInt = Convert.ToInt32(teamId);
            _context.Drivers.Add(driverEntity);
            var company = _context.Companies.
                Include(c => c.Team)
                .ThenInclude(t => t.Drivers)
                .SingleOrDefault(c => c.ID == user.CompanyID);
            Company userCompany = _context.Companies.Where(c => c.ID == user.CompanyID).First();
            var team = userCompany.Team;
            team.Drivers.Add(driverEntity);
            var completedDriverRequest = await _context.DriverRegistrationRequests.SingleOrDefaultAsync(m => m.DriverEmail == user.Email);
            _context.DriverRegistrationRequests.Remove(completedDriverRequest);
            await _context.SaveChangesAsync();
        }

        public async Task GenerateDriverRegistrationRequestAsync(DriverDetails driver, Company company, string callbackUrl)
        {
            await _emailSender.SendEmailAsync(driver.Email, "You were invited to register",
                   $"You were invited as a driver,  please register: <a href='{callbackUrl}'>link</a>");
            DriverRegistrationRequest request = new DriverRegistrationRequest();
            request.TeamID = company.Team.ID;
            request.DriverEmail = driver.Email;
            _context.DriverRegistrationRequests.Add(request);
            await _context.SaveChangesAsync();
        }
    }
}
