using DeliveryService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryServiceTests.MockServices
{
    public class MockAuthMessageSender : IEmailSender
    {
        public int messageCount { get; set; }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            messageCount += 1;
            return Task.FromResult(0);
        }
    }
}
