using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Helpers;
using Persistence.Domain;
using Veteries.API.Extensions;
using Veteries.API.Profiles;
using CustomMapper = Mapper;

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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<AppSettings>(appSettingsSection);
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddVeteriesSwagger(Configuration);
            services.AddApiIdentity(Configuration);
            services.AddJwtAuthentication(Configuration);
            services.AddDatabaseContext(Configuration);
            services.AddMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new Application.DependencyInjection());
            builder.RegisterModule(new Persistence.DependencyInjection());
            builder.RegisterModule(new CustomMapper.DependencyInjection(
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Profile)))
                    .Select(x => x.GetTypeInfo().Assembly)));
            builder.RegisterModule(new User.DependencyInjection());
            builder.RegisterModule(new Services.DependencyInjection());
            builder.RegisterModule(new Animals.DependencyInjection());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DomainDbContext dbContext)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.EnsureCreated();

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