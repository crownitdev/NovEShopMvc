using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NovEShop.Data;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Share.Constants;
using NovEShop.Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            // Register the Swagger generator, define 1 or more Swagger documents
            // This adds Swagger generator to service collections
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddControllersWithViews();
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

            // Enable middleware to serve generated Swagger as JSON endpoints.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, CSS, JS, etc.)
            app.UseSwaggerUI(c => 
            {
                // Specifying the Swagger JSON endpoint.
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
