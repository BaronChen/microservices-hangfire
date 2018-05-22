using E8ay.Common.Api;
using E8ay.Common.HangFire;
using E8ay.Item.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace E8ay.Item.Api
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
            services.AddServicesLayer(mongoConnectionString);

            services.AddHangFireServices(mongoConnectionString, hangFireDb);
            services.AddEventHandlers();

            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseHangFireServices(env, new string[] { QueueConstants.AuctionEndFinalise, QueueConstants.ItemQueue });

            serviceProvider.UseEventHandlers();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var task = serviceScope.ServiceProvider.GetService<IItemService>().SeedAuctionItems();
                    task.GetAwaiter().GetResult();
                }
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
