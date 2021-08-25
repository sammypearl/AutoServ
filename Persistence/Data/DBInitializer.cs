using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Persistence.Helpers;
using Models;

namespace Persistence.Data
{
    public class DBInitializer : IDBInitializer
    {
        private readonly AutoServDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(AutoServDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async void Initialize()    
        {
            //Add pending migration if exists
            if (_db.Database.GetPendingMigrations().Count()==0)
            {
                _db.Database.Migrate();
            }

            //Exit if role already exists
            if (_db.Roles.Any(r => r.Name == Helpers.Roles.Admin)) return;

            //Create Admin role
            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();
            
            //Create Admin user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin",
                Email = "Admin@gmail.com",
                EmailConfirmed = true,
            },"Admin@123").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("Admin"),Roles.Admin);


            //Create Admin2 role
            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();


            //Create Admin2 user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin2",
                Email = "Admin2@gmail.com",
                EmailConfirmed = true,
            }, "Admin@123").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("Admin"), Roles.Admin);

            //Create Admin3 role
            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();


            //Create Admin3 user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin3",
                Email = "Admin3@gmail.com",
                EmailConfirmed = true,
            }, "Admin@123").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("Admin"), Roles.Admin);

            //Create Admin role
            _roleManager.CreateAsync(new IdentityRole(Roles.Seller)).GetAwaiter().GetResult();

            //Create Seller user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Seller",
                Email = "Seller@gmail.com",
                EmailConfirmed = true,
            }, "Seller@123").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("Seller"), Roles.Seller);

        }
    }
}
