using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Interfaces
{
    /// <summary>
    /// Sends emails using the <see cref="IEmailSender"/> and creating the HTML
    /// email from specific templates
    /// </summary>
    public interface IEmailTemplateSender
    {
        /// <summary>
        /// Sends a email with the given details using the general template
        /// </summary>
        /// <param name="details">The email message details. Note the Content property is ignored and replaced with the template</param>
        /// <param name="title"></param>
        /// <param name="content1"></param>
        /// <param name="content2"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonUrl"></param>
        /// <returns></returns>
        Task<SendEmailResponseModel> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl);
    }
}
