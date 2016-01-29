using Canvass.Api.Helpers;
using Canvass.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Canvass.Api.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private IQueueManager _queueManager;
        private AppSettings _appSettings;
        public AuthMessageSender(IQueueManager queueManager, IConfiguration configuration)
        {
            _queueManager = queueManager;
            _appSettings = configuration.Get<AppSettings>();
        }
        public async Task SendEmailAsync(string address, string subject, string message)
        {
            var email = new EmailMessage()
            {
                Destination = address,
                Subject = subject,
                Body = message
            };
            var json = JsonConvert.SerializeObject(email);
            var queueMessage = new CloudQueueMessage(json);
            await _queueManager.AddQueueMessageAsync(_appSettings.Azure.CommunicationsQueueName, queueMessage);
            return;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
