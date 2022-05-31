using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Services.SendGrid
{
    /// <summary>
    /// Handles sending emails specfic to the Repo Web App
    /// </summary>
    public static class RepoEmailSender
    {
        /// <summary>
        /// Sends a verification email to the specfied user
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="email"></param>
        /// <param name="verificiationUrl"></param>
        /// <returns></returns>
        public static async Task<SendEmailResponseModel> SendUserVerificationEmailAsync(string displayName, string email, string verificiationUrl)
        {

            return await IoC.IoC.EmailTemplateSender.SendGeneralEmailAsync(new Models.SendEmailDetails
            {            
                fromEmail = "82cp@outlook.com",
                fromName = "CharliesWebApp",
                toEmail = email,
                toName = displayName,
                isHTML = true,
                subject = "Verify your Email - Charlies Repo"
            },
           "Verify Email",
           $"Hi {displayName ?? "there"}", //if display is blank, use stranger
           "Thanks for creating an account with us. <br/>To continue, please verify your email with us.",
           "Verify Email",
           verificiationUrl
           );

        }

    }
}
