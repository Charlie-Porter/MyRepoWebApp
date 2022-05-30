using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyRepoWebApp.Interfaces;
using MyRepoWebApp.Models;
using System.Text;
using MyRepoWebApp.IoC;
using System.Diagnostics;

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
                using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("MyRepoWebApp.Services.Templates.GeneralTemplate.htm"), Encoding.UTF8))
                {
                    templateText = await reader.ReadToEndAsync();
                }
            }
            catch(Exception ex)
            {
                //break if we are debugging
                if (Debugger.IsAttached)
                    Debugger.Break();
                //if something went wrong, return message
            }

            // replace special values with those inside the template

            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--Buttontext--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);                                        

            //set the details content to this template content
            details.content = templateText;

            //send email
            return await IoC.IoC.EmailSender.SendEmailAsync(details);
        }
    }
}
