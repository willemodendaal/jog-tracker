using JogTracker.DomainModel;

namespace JogTracker.Data.Migrations
{
    using JogTracker.Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JogTracker.Data.JogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JogDbContext context)
        {
            // Logic here to seed with initial data.
            CreateDefaultRoles(context);
            AddDefaultAdminUser(context);
        }

        bool AddDefaultAdminUser(JogDbContext context)
        {
            var userManager = new UserManager<JogEntryUser>(new UserStore<JogEntryUser>(context));

            var user = new JogEntryUser()
            {
                UserName = "willem.odendaal@gmail.com",
                Email = "willem.odendaal@gmail.com"
            };

            //TODO: store password more securely.
            IdentityResult identityResult = userManager.Create(user, "runUpYonderHills!");

            if (identityResult.Succeeded == false)
            {
                throw new Exception("Seed failed.");
                return false;
            }

            identityResult = userManager.AddToRole(user.Id, GlobalConfig.AdminRole);
            return identityResult.Succeeded;
        }

        private static void CreateDefaultRoles(JogDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleManager.Create(new IdentityRole(GlobalConfig.AdminRole));
            roleManager.Create(new IdentityRole(GlobalConfig.UserRole));
        }
    }
}
