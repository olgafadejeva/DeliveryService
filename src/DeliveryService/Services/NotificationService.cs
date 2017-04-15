using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;
using DeliveryService.Models.Entities;

namespace DeliveryService.Services
{
    /*
     * Sending e-mail notifications to company staff and clients upon delivery status changes
     */ 
    public class NotificationService : INotificationService
    {
        public IEmailSender emailSender { get; set; }
        public NotificationService(IEmailSender messageSender) {
            this.emailSender = messageSender;
        }

        public async Task SendStatusUpdateEmailToAdminAsync(string statusString, Delivery delivery, List<EmployeeUser> employees)
        {
            foreach (EmployeeUser user in employees) {
                string emailMessage = "Hi " + user.NormalizedUserName;
                emailMessage += "The status of delivery for the address " + DirectionsService.getStringFromAddress(delivery.Client.Address) + " dated " + delivery.DeliverBy.ToString() + " has been changed to " + statusString;
                string emailAddress = user.Email;
                string emailSubject = "Delivery status change to: " + statusString;
                await emailSender.SendEmailAsync(emailAddress, emailSubject, emailMessage);
            }
        }

        public async Task SendStatusUpdateEmailToClientAsync(Status status, Delivery delivery, Client client)
        {
            string emailMessage = "Hi " + client.FirstName;
            if (!status.Equals(Status.FailedDelivery))
            {
                emailMessage += "The status of delivery for the address " + DirectionsService.getStringFromAddress(delivery.Client.Address) + " dated " + delivery.DeliverBy.ToString() + " has been changed to " + status.DisplayName();
                string emailAddress = client.Email;
                string emailSubject = "The status of your order has changed to: " + status.DisplayName();
                await emailSender.SendEmailAsync(emailAddress, emailSubject, emailMessage);
            }
        }

        public async Task SendAnEmailToDriverAboutAssignedRouteAsync(Route route, ApplicationUser user)
        {
            string emailMessage = "Hi " + user.NormalizedUserName;
            emailMessage += "You have been assigned a new route!";
            emailMessage += "The delivery date is: " + route.DeliveryDate.Value.ToString("dd/MM/yyyy");
            emailMessage += "The number of stops in route is: " + route.Deliveries.ToList().Count;
            string emailAddress = user.Email;
            string emailSubject = "You've got a new route";
            await emailSender.SendEmailAsync(emailAddress, emailSubject, emailMessage);
        }
    }
}
