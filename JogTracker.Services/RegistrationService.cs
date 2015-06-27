using JogTracker.Common;
using JogTracker.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Services
{
    /// <summary>
    /// Abstracts the ASP.NET UserManager (we only need to use a small portion of it)
    /// </summary>
    public interface IRegistrationService
    {
        void Register(string userName, string email, string password);
    }

    public class RegistrationService : IRegistrationService
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<IdentityUser> userManager;

        public RegistrationService()
        {
            var context = new JogDbContext();
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            userManager.PasswordValidator = Config.PasswordValidator;
        }

        public void Register(string userName, string email, string password)
        {

            var user = new IdentityUser()
            {
                UserName = userName,
                Email = email
            };

            //TODO: store password more securely.
            var identityResult = userManager.Create(user, password);

            if (identityResult.Succeeded == false)
            {
                throw new Exception("Seed failed.");
            }

            identityResult = userManager.AddToRole(user.Id, Config.UserRole);
        }

    }
}
