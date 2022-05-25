using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using MyRepoWebApp.Models;
using MyRepoWebApp.Interfaces;

namespace MyRepoWebApp.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender()
        {

        }
        public async Task<SendEmailResponseModel>SendEmailAsync(SendEmailDetails details)
        {

            // using SendGrid's C# Library
            // https://github.com/sendgrid/sendgrid-csharp

           /* var apiKey ="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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("test@example.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);*/

            return new SendEmailResponseModel();
        
        }
    }
}
   
