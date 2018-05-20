using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E8ay.Common;
using E8ay.Common.Api;
using E8ay.Common.HangFire;
using E8ay.Item.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

            ApiConfig.ConfigureJwtAuth(services);
            ApiConfig.ConfigureMongoOption(services, mongoConnectionString, database);
            ServicesInstaller.ConfigureServices(services, mongoConnectionString);

            HangfireConfig.ConfigureServices(services, mongoConnectionString, hangFireDb);

            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            HangfireConfig.ConfigureApp(app, env, "item-queue");

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
