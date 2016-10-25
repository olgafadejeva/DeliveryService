using DeliveryService.Models;
using DeliveryService.Models.AccountViewModels;
using DeliveryService.Models.Entities;
using DeliveryService.Models.ShipperViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Services
{
    public interface IUserService
    {
         Task<IdentityResult> CreateEmployeeUserAsync(EmployeeCreateView model, Company company);

         Task<IdentityResult> CreateDriverUserAsync(DriverRegisterViewModel model);
        
         Task GenerateDriverRegistrationRequestAsync(DriverDetails driver, Company company, string callBackUrl);

        Task<IdentityResult> SignUpCompanyAsync(CompanyRegistrationViewModel model, IUrlHelper url, HttpContext httpContext);
    }
}
