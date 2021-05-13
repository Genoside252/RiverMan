using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RiverMan.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RiverMan.DataAccessLayer
{
    public class RiverManContext : IdentityDbContext<IdentityUser>
    {
        public RiverManContext(DbContextOptions<RiverManContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<SubscriptionService> SubscriptionServices { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceType>().ToTable("ServiceType");
            modelBuilder.Entity<ServiceType>().HasKey(c => c.Id);
            modelBuilder.Entity<SubscriptionService>().ToTable("SubscriptionService");
            modelBuilder.Entity<SubscriptionService>().HasKey(c => c.Id);
            modelBuilder.Entity<UserSubscription>().ToTable("UserSubscription");
            modelBuilder.Entity<UserSubscription>().HasKey(c => new { c.UserId, c.SubscriptionId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
