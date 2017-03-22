using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;

namespace DeliveryService.Services
{

    public class AuthMessageSender : IEmailSender
    {
        
        public AppProperties options { get; } //set only via Secret Manager

        public IHostingEnvironment env;

        public AuthMessageSender(IOptions<AppProperties> optionsAccessor, IHostingEnvironment env)
        {
            options = optionsAccessor.Value;
            this.env = env;
        }
        

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Delivery Service", "noreply@deliveryservice.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                client.Connect(options.SmtpHost, options.SmtpPort, SecureSocketOptions.None);
                if (!env.IsDevelopment())
                {
                    client.Authenticate(options.SmtpUsername, options.SmtpPassword);
                }
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
            
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
