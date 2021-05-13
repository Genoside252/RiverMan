using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverMan.Data;

[assembly: HostingStartup(typeof(RiverMan.Areas.Identity.IdentityHostingStartup))]
namespace RiverMan.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<RiverManIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("RiverManIdentityContextConnection")));
            });
        }
    }
}