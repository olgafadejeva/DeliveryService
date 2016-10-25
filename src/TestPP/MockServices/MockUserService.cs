using DeliveryService.Services;
using System.Threading.Tasks;
using DeliveryService.Models;
using DeliveryService.Models.AccountViewModels;
using DeliveryService.Models.Entities;
using DeliveryService.Models.ShipperViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DeliveryServiceTests.MockServices
{
    public class MockUserService : IUserService
    {
        public int driversCreated { get; set; }
        public int employeesCreated { get; set; }
        public int driverRegistrationRequestsCreated { get; set; }
        public int CompaniesSignedUp { get; set; }

        public Task<IdentityResult> CreateDriverUserAsync(DriverRegisterViewModel model)
        {
            driversCreated += 1;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> CreateEmployeeUserAsync(EmployeeCreateView model, Company company)
        {
            employeesCreated += 1;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task GenerateDriverRegistrationRequestAsync(DriverDetails driver, Company company, string callBackUrl)
        {
            driverRegistrationRequestsCreated += 1;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> SignUpCompanyAsync(CompanyRegistrationViewModel model, IUrlHelper url, HttpContext httpContext)
        {
            CompaniesSignedUp = +1;
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
