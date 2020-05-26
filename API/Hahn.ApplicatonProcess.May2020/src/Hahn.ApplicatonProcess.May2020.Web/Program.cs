using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Hahn.ApplicatonProcess.May2020.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostBuildercontext, configurationBuilder) =>
                {
                    configurationBuilder
                    //.AddEnvironmentVariables()
                    .AddJsonFile("config/appsettings.json", false, true)
                    .AddJsonFile("config/serilog.json", false, true)
                    ;
                })
          
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .CaptureStartupErrors(true);
                })
            .UseSerilog((hostBuilder, logggerConfig) =>
            {

                logggerConfig.ReadFrom.Configuration(hostBuilder.Configuration);
            })
            ;
    }
}
