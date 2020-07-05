using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Helpers;
using Veteries.API.Controllers;
using Veteries.API.Extensions;

namespace Veteries.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public ILifetimeScope? AutofacContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.AddHttpContextAccessor();
            services.Configure<AppSettings>(appSettingsSection);
            services.AddControllers();
            services.AddVeteriesSwagger(Configuration);
            services.AddJwtAuthentication(Configuration);
            services.AddDatabaseContext(Configuration);
            services.AddApiIdentity(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new Application.DependencyInjection());
            builder.RegisterModule(new Persistence.DependencyInjection());
            builder.RegisterModule(new User.DependencyInjection());
            builder.RegisterModule(new Animal.DependencyInjection());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCors("default");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseVeteriesSwagger(Configuration);
        }
    }
}
