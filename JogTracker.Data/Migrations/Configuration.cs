namespace JogTracker.Data.Migrations
{
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
            AddDefaultAdminUser(context);
        }

        bool AddDefaultAdminUser(JogDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityResult identityResult = roleManager.Create(new IdentityRole("administrator"));
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));

            var user = new IdentityUser()
            {
                UserName = "jogSuperUser",
            };

            //TODO: store password more securely.
            identityResult = userManager.Create(user, "runUpYonderHills!");

            if (identityResult.Succeeded == false)
            {
                return false;
            }

            identityResult = userManager.AddToRole(user.Id, "administrator");
            return identityResult.Succeeded;
        }
    }
}
