using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyRepoWebApp.Interfaces;

namespace MyRepoWebApp.Services.Templates
{
    /// <summary>
    /// Extension method for EmailTemplateSender classes
    /// </summary>
    public static class EmailTemplateSenderExtenstions
    {
        /// <summary>
        /// Extension the <see cref="SendGridEmailSender"/> into the services to handle the <see cref="IEmailSender"/> service.
        /// Extension method for any SendGrid Classes
        /// </summary>      
        public static IServiceCollection AddEmailTemplateSender(this IServiceCollection services)
        {
            // Inject the SendGridSender 
            services.AddTransient<IEmailTemplateSender, EmailTemplateSender>();

            //return collection for chaining
            return services;
        }

    }
}
