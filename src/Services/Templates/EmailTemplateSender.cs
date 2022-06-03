using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyRepoWebApp.Interfaces;
using MyRepoWebApp.Models;
using System.Text;
using MyRepoWebApp.DI;
using System.Diagnostics;
using MyRepoWebApp.Services.Logger;

namespace MyRepoWebApp.Services.Templates
{
    /// <summary>
    /// Handles sending templated emails
    /// </summary>
    public class EmailTemplateSender : IEmailTemplateSender
    {
        public async Task<SendEmailResponseModel> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
        {
            var templateText = default(string);
            //Read the general template from file
            //TODO: Replace with flat data provider
            try
            {
                var assembly = Assembly.GetEntryAssembly();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var stream = assembly.GetManifestResourceStream("MyRepoWebApp.Services.Templates.GeneralTemplate.htm") ?? Stream.Null;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                using var reader = new StreamReader(stream: stream, encoding: Encoding.UTF8);
                templateText = await reader.ReadToEndAsync();

        

            }
            catch(Exception ex)
            {
                WriteToLog.writeToLogInformation($@" SendGeneralEmailAsync exception: {ex.Message} {ex.InnerException}");
            }

            // replace special values with those inside the template

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            templateText = templateText.Replace("--Title--", title)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--Buttontext--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);                                        

            //set the details content to this template content
            details.content = templateText;

            //send email
            return await DIServices.EmailSender.SendEmailAsync(details);
        }
    }
}
