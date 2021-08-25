using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public class AutoServDbContext : IdentityDbContext<ApplicationUser>
    {
        public AutoServDbContext(DbContextOptions<AutoServDbContext> options):
            base(options)
        {

        }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Vehicle> Vehicles{ get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        //    {
        //        relationship.DeleteBehavior = DeleteBehavior.Restrict;
        //    }
        //}
    }
}
