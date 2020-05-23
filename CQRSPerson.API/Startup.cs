using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;
using AutoMapper;
using CQRSPerson.Domain.Options;
using CQRSPerson.Infrastructure;
using System.Reflection;
using System.IO;
using CQRSPerson.API.Map;
using CQRSPerson.API.Controllers;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Infrastructure.Logging;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.OpenApi.Models;
using CQRSPerson.API.Person.GetPersons;
using CQRSPerson.API.Person.Command;

namespace CQRSPerson.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IWebHostEnvironment env)
        {
            Environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(Startup));

            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddDbContext<DBContext>();

            services.AddScoped<IApplicationLogger<PersonController>,ApplicationLogger<PersonController>>();
            services.AddScoped<IApplicationLogger<GetPersonsHandler>, ApplicationLogger<GetPersonsHandler>>();
            services.AddScoped<IApplicationLogger<CreatePersonHandler>, ApplicationLogger<CreatePersonHandler>>();


            services.AddScoped<IPersonQueryRepository, PersonQueryRepository>();
            services.AddScoped<IPersonCommandRepository, PersonCommandRepository>();

            services.AddScoped<IValidator<CreatePersonCommand>, CreatePersonValidator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Health Catalyst API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddSingleton(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
