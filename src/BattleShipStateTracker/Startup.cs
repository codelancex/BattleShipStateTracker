using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using BattleShipStateTracker.Response;
using BattleShipStateTracker.StateTracker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace BattleShipStateTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add IBoardsManager singleton. It will be injected to the BattleShipController.
            services.AddSingleton<IBoardsManager, GameBoardsManager>();

            // Update JSON serialization options for serializing JSON response.
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            // Customize request Validation error response to make it consistent with
            // the defined general error response.
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    var response = new ErrorResponse
                    {
                        ErrorMessages = actionContext.ModelState
                            .Where(modelError => modelError.Value.Errors.Count > 0)
                            .Select(modelError => modelError.Value.Errors.FirstOrDefault().ErrorMessage).ToList()
                    };
                    return new BadRequestObjectResult(response);
                };
            });

            // Return all supported API versions in the response header.
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
            });

            // Register the Swagger services
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "BattleShip State Tracker Api";
                settings.GenerateExamples = true;
                settings.Version = "v1.0";
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Register log4net
            loggerFactory.AddLog4Net("log4net.config");
        }
    }
}
