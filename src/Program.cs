using Dna.AspNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging;

namespace MyRepoWebApp
{
    public class Program
    {
        public static ILogger Logger;
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var LoggerF = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("BlazorDiceRoller", LogLevel.Warning)
                .AddConsole().AddAzureWebAppDiagnostics()
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning);
            });
            Logger = LoggerF.CreateLogger<Program>();
            host.Run();



        }
        //    CreateHostBuilder(args).Build().Run();


        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
             .ConfigureLogging(logging => logging.AddAzureWebAppDiagnostics())
             .ConfigureServices(serviceCollection => serviceCollection
                 .Configure<AzureFileLoggerOptions>(options =>
                 {
                     options.FileName = "azure-diagnostics-";
                     options.FileSizeLimit = 50 * 1024;
                     options.RetainedFileCountLimit = 5;
                 }).Configure<AzureBlobLoggerOptions>(options =>
                 {
                     options.BlobName = "log.txt";
                 })
             )
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseDnaFramework();
                 webBuilder.UseStartup<Startup>();
             });
    }
}

