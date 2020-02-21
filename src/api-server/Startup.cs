using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TwentyFiveHours.API.Azure;
using TwentyFiveHours.API.Models;
using TwentyFiveHours.API.Services;

namespace TwentyFiveHours.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add client models from MongoDB.
            services.Configure<ClientDatabaseSettings>(
                this.Configuration.GetSection(nameof(ClientDatabaseSettings))
            );
            services.AddSingleton<IMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ClientDatabaseSettings>>().Value
            );
            services.AddSingleton<MongoService<ClientModel>>();

            // Add Azure connection settings from environment variables.
            services.Configure<AzureSettings>((settings) =>
            {
                settings.TextAnalyticsAPIKey = Environment.GetEnvironmentVariable("AZURE_TEXTANALYTICS_KEY");
                settings.TextAnalyticsEndpoint = Environment.GetEnvironmentVariable("AZURE_TEXTANALYTICS_ENDPOINT");
                settings.SpeechRecognitionAPIKey = Environment.GetEnvironmentVariable("AZURE_SPEECHRECOGNITION_KEY");
                settings.SpeechRecognitionRegion = Environment.GetEnvironmentVariable("AZURE_SPEECHRECOGNITION_REGION");
            });

            // Add ASP.NET Core MVC patterns.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                var serviceProvider = app.ApplicationServices;
                var mongodbProvider = serviceProvider.GetService<MongoService<ClientModel>>();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
