using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyRepoWebApp.Data;
using System;
using MyRepoWebApp.Services;
using MyRepoWebApp.Interfaces;
using MyRepoWebApp.Services.Templates;
using MyRepoWebApp.IoC;
using Dna;
using Dna.AspNet;

namespace MyRepoWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        

        public void ConfigureServices(IServiceCollection services)
        {

            //Add SendGrid email sender
            services.AddSendGridEmailSender();

            // Add general template sender
            services.AddEmailTemplateSender();

            services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireClaim("Admin", "True"));

            });
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient();
            services.AddControllers();
            

            services.AddDbContext<MyRepoWebAppContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MyRepoWebAppContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            // Use Dna Framework
            app.UseDnaFramework();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
            });
            /* app.ApplicationServices.GetService<IEmailSender>().SendEmailAsync(new Models.SendEmailDetails
             {
                 content = "This is out first HTML email",
                 fromEmail = "82cplll@outlook.com",
                 fromName = "charlie",
                 toEmail = "82cpllll@outlook.com",
                 toName = "me",
                 isHTML = true,
                 subject = "This is sent from my web app"


             });
          */
            
            IoC.IoC.EmailTemplateSender.SendGeneralEmailAsync(new Models.SendEmailDetails
            {
                content = "This is out first HTML email",
                fromEmail = "82cp@outlook.com",
                fromName = "charlie",
                toEmail = "82cp@outlook.com",
                toName = "me",
                isHTML = true,
                subject = "This is sent from my web app"
            }, 
            "Verify Email", 
            "Hi Luke,",
            "Thanks for creating an account with us. <br/>To continue, please verify your email with us.",
            "Verify Email",
            "https://myrepowebappravor.azurewebsites.net/)");

        
        }
    }
}
