using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyRepoWebApp.Interfaces;

namespace MyRepoWebApp.Services
{
    /// <summary>
    /// injects the <see cref="SendGridEmailSender"/> into the services to handle the <see cref="IEmailSender"/> service.
    /// Extension method for any SendGrid Classes
    /// </summary>
    public static class SendGridExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
        {
            // Inject the SendGridSender 
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            //return collection for chaining
            return services;
        }
    }
}
