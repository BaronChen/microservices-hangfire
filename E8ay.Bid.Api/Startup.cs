﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E8ay.Bid.Services;
using E8ay.Common.Api;
using E8ay.Common.HangFire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace E8ay.Bid.Api
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

            const string mongoConnectionString = "mongodb://e8ay.mongo:27017/";
            const string database = "item";
            const string hangFireDb = "hangfire";

            services.AddJwtAuth();
            services.AddMongoOption(mongoConnectionString, database);

            services.AddHangFireServices(mongoConnectionString, hangFireDb);
            services.AddServicesLayer(mongoConnectionString);
            services.AddEventHandlers();
            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseHangFireServices(env, QueueConstants.BidQueue);

            serviceProvider.UseEventHandlers();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseMvc();
        }
    }
}
