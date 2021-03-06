using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hahn.ApplicatonProcess.May2020.Data;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Persistence;
using Hahn.ApplicatonProcess.May2020.Domain.Services;
using Hahn.ApplicatonProcess.May2020.Web.Models;
using Hahn.ApplicatonProcess.May2020.Web.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net;
using System.Reflection;

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
            services.AddCors(options =>
               {
                   options.AddPolicy("DefaultPolicy",
                       builder =>
                       {
                           builder.WithOrigins("*")
                                               .AllowAnyHeader()
                                               .AllowAnyMethod();
                       });
               });
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;

            })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddTransient<IValidator<ApplicantForCreationDto>, ApplicantBaseValidator<ApplicantForCreationDto>>();
            services.AddTransient<IValidator<ApplicantForUpdateDto>, ApplicantBaseValidator<ApplicantForUpdateDto>>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient(nameof(Validators.CountryValidator), config =>
            {
                Uri uri = new Uri(Configuration.GetValue<string>("AppConfig:ValidateCountryUrl"));
                config.BaseAddress = uri;
                config.Timeout = new TimeSpan(0, 0, Configuration.GetValue<int>("AppConfig:RequestTimeoutSec"));
                ServicePoint sp = ServicePointManager.FindServicePoint(uri);
            });

            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<IRepository<Applicant>, ApplicantRepo>();
            services.AddDbContext<ApplicantContext>();
            services.AddSwaggerGen(setUpAction =>
           {
               setUpAction.SwaggerDoc("ApplicantApiOpenAPISpecifications", new Microsoft.OpenApi.Models.OpenApiInfo
               {
                   Title = "ApplicantApi",
                   Version = "1"
               });
               setUpAction.ExampleFilters();

               var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               setUpAction.IncludeXmlComments(xmlCommentsFile);
               setUpAction.AddFluentValidationRules();
           });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
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


            app.UseSwagger();

            app.UseSwaggerUI(setUpAction =>
            {
                setUpAction.SwaggerEndpoint("/swagger/ApplicantApiOpenAPISpecifications/swagger.json", "Applicant API");
                setUpAction.RoutePrefix = "";

            });

            app.UseRouting();

            app.UseCors("DefaultPolicy");


            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
