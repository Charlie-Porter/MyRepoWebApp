﻿using Dna.AspNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MyRepoWebApp
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseDnaFramework();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
