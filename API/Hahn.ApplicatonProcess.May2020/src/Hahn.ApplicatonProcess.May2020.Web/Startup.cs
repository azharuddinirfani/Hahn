using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hahn.ApplicatonProcess.May2020.Data;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Persistence;
using Hahn.ApplicatonProcess.May2020.Domain.Services;
using Hahn.ApplicatonProcess.May2020.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Net;

namespace Hahn.ApplicatonProcess.May2020.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddTransient<IValidator<ApplicantForCreationDto>, ApplicantBaseValidator<ApplicantForCreationDto>>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient(nameof(Models.CountryValidator), config =>
            {
                Uri uri = new Uri(Configuration.GetValue<string>("AppConfig:ValidateCountryUrl"));
                config.BaseAddress = uri;
                config.Timeout = new TimeSpan(0, 0, Configuration.GetValue<int>("AppConfig:RequestTimeoutSec"));
                ServicePoint sp = ServicePointManager.FindServicePoint(uri);
            });

            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<IRepository<Applicant>, ApplicantRepo>();
            services.AddDbContext<ApplicantContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
