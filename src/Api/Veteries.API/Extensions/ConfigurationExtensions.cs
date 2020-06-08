using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Domain;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentValidation.AspNetCore;

namespace Veteries.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddVeteriesSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Veteries API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { 
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                c.CustomSchemaIds(x => x.FullName);
            });
        }

        public static void AddDatabseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DomainDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DomainDbContext")));
        }

        public static void AddApiIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 8;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DomainDbContext>()
            .AddDefaultTokenProviders();
        }


        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings").GetSection("Secret").Value);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetSection("AppSettings").GetSection("ValidIssuer").Value,
                        ValidAudience = configuration.GetSection("AppSettings").GetSection("ValidAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
        }

        public static void UseVeteriesSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Veteries API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
