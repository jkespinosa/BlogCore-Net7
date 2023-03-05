using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Initializer
{
    public class InitializerDB : IInitializerDB
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUSer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InitializerDB(ApplicationDbContext db, UserManager<ApplicationUSer> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception) { }

            if (_db.Roles.Any(ro => ro.Name == CNT.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(CNT.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.User)).GetAwaiter().GetResult();

            //create user initial
            _userManager.CreateAsync(new ApplicationUSer
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                Name = "Admin"
            }, "Admin123#").GetAwaiter().GetResult();

            ApplicationUSer userNew = _db.ApplicationUSers
                .Where(us => us.Email == "admin@admin.com")
                .FirstOrDefault();
            _userManager.AddToRoleAsync(userNew, CNT.Admin).GetAwaiter().GetResult();

        }
    }
}
