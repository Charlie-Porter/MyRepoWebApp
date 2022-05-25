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

            return new SendEmailResponseModel();
        
        }
    }
}
   
