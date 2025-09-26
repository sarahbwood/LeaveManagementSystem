using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Extensions.Configuration;

namespace LeaveManagementSystem.Application.Services.Email
{
    public class EmailService(IConfiguration _configuration) : IEmailService
    {
        public async Task EmailManagers(string email, string htmlMessage, MemoryStream calendarEvent)
        {
            Attachment calendarFile = new Attachment(calendarEvent, "calendar.ics", "text/calendar");

            var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"];
            var smtpServer = _configuration["EmailSettings:Server"];
            var smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]);

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = "Leave Notification",
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));
            message.Attachments.Add(calendarFile);

            using var client = new SmtpClient(smtpServer, smtpPort);
            await client.SendMailAsync(message);
        }
    }
}
