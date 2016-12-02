using DeliveryService.Models;
using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public interface INotificationService
    {
        Task SendStatusUpdateEmailToAdminAsync(string statusString, Delivery delivery, List<EmployeeUser> employees);
        Task SendAnEmailToDriverAboutAssignedRouteAsync(Route route, ApplicationUser user);
        Task SendStatusUpdateEmailToClientAsync(Status status, Delivery delivery, Client client);
    }
}
