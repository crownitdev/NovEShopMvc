using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NovEShop.Data;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Accounts.Dtos;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Validators;
using NovEShop.Share.Constants;
using NovEShop.Share.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovEShop.Api
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
            services.AddDbContext<NovEShopDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.NovEShopConnectionStringKey));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<NovEShopDbContext>()
                .AddDefaultTokenProviders();


            services.AddTransient<IFileStorageHelper, FileStorageHelper>();
            services.AddMediatR(typeof(IBroker).Assembly);
            services.AddTransient<IBroker, Broker>();

            //services.AddTransient<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();

            var jwtIssuer = Configuration["JwtOptions:Issuer"];
            var jwtAudience = Configuration["JwtOptions:Audience"];
            var signingKey = Configuration["JwtOptions:SigningKey"];
            services.AddAuthentication(authConfig =>
            {
                authConfig.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authConfig.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAudience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                    };
                });

            services.AddControllers();
            services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();

            // Register the Swagger generator, define 1 or more Swagger documents
            // This adds Swagger generator to service collections
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Add Authorization header for Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using Bearer Scheme. \r\n\r\n
                                    Enter 'Bearer' [space] and then your token in the text input below.
                                    \r\n\r\nExample: 'Bearer 123456abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as JSON endpoints.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, CSS, JS, etc.)
            app.UseSwaggerUI(c =>
            {
                // Specifying the Swagger JSON endpoint.
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
