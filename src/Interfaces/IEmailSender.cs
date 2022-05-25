using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRepoWebApp.Models;

namespace MyRepoWebApp.Interfaces
{
    /// <summary>
    /// A service that Handles sending emails
    /// </summary>
    public interface IEmailSender
    {
        Task<SendEmailResponseModel> SendEmailAsync(SendEmailDetails details);
    }
}
