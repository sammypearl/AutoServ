using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Persistence;
using Persistence.Data;
using Persistence.Repository;
using Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI
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
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AutoServDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddDbContext<AutoServDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("AutoDbConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IDBInitializer, DBInitializer>();
            services.AddScoped<IMakeRepository, MakeRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IApplicationUser, ApplicationUserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddControllersWithViews()
                .AddNewtonsoftJson().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddCloudscribePagination();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDBInitializer dBInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            dBInitializer.Initialize();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
