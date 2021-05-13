using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Identity;
using RiverMan.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using RiverMan.Models;
using RiverMan.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using RiverMan.Services;

namespace RiverMan
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
            services.AddDbContext<RiverManIdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RiverManContextConnection")));
            services.AddDbContext<RiverManContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RiverManContextConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<RiverManContext>()
            .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<Services.IEmailSender, EmailSender>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, RiverManContext context)
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

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            

            // Seed Data
            DBInitialiser.Initialise(userManager, roleManager, context);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
