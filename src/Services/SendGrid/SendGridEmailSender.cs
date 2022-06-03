using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using MyRepoWebApp.Models;
using MyRepoWebApp.Interfaces;
using System.Diagnostics;
using Newtonsoft.Json;
using MyRepoWebApp.Services.Logger;

namespace MyRepoWebApp.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        //constuctor
        public SendGridEmailSender()
        {

        }
        public async Task<SendEmailResponseModel> SendEmailAsync(SendEmailDetails details)
        {

            var a = new SendEmailResponseModel();


            // using SendGrid's C# Library
            // https://github.com/sendgrid/sendgrid-csharp

            var apiKey = "";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(details.fromEmail, details.fromName);
            var subject = details.subject;
            var to = new EmailAddress(details.toEmail, details.toName);            
            var Content = details.content;
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                //plain content
                details.isHTML ? null : details.content,
                //html content
                details.isHTML ? details.content : null);
            
            //finally send the email...
            var response = await client.SendEmailAsync(msg);

            //if we succeeded...
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                return new SendEmailResponseModel();

            //otherwise it failed...

            try
            {
                var bodyResult = await response.Body.ReadAsStringAsync();

                var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponseModel>(bodyResult);

                //Add any errors to the response
                var errorResponse = new SendEmailResponseModel
                {
                    Errors = sendGridResponse.Errors.Select(f => f.message).ToList()
                };

                // Make sure we have at least one error
                if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                    //add an unknown error#
                    //TODO:
                    errorResponse.Errors = new List<string>(new[] { "Unknown error from email sending service. Please contact support :) " });
                
                //returns the error response
                return errorResponse;
            }
            catch (Exception ex)
            {
                //TODO: if something went wrong, return message
                WriteToLog.writeToLogError(@$"Exception occurred sending Sendgrid email: {ex.Message} {ex.InnerException}");                
                //return result baed on response
                return new SendEmailResponseModel
                {
                    Errors = new List<string>(new[] { "Unknown error occurred" })
                };

            }
        }
    }
}
   
