﻿using JogTracker.Common;
using JogTracker.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
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
    public interface IUserAdminService
    {
        /// <summary>
        /// Register, or return error string.
        /// </summary>
        string Register(string email, string password);
        Task RequestResetPassword(string email);
        void ResetPassword(string userId, string token, string newPassword);
    }

    public class UserAdminService : IUserAdminService
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        private IEmailService _emailService;

        public UserAdminService(IEmailService emailService)
        {
            var context = new JogDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            _userManager.PasswordValidator = GlobalConfig.PasswordValidator;
            _emailService = emailService;

            //Assign token provider used to generate Reset password tokens.
            var dataProtectorProvider = GlobalSharedSecurity.DataProtectionProvider; //Use the same provider we used when auth was initialized.
            var dataProtector = dataProtectorProvider.Create("My Asp.Net Identity");
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser, string>(dataProtector)
            {
                TokenLifespan = TimeSpan.FromHours(24), //Reset token lifespan.
            };
        }

        /// <summary>
        /// Register, or return errors.
        /// </summary>
        public string Register(string email, string password)
        {
            var user = new IdentityUser()
            {
                UserName = email,
                Email = email
            };

            //TODO: store password more securely.
            var identityResult = _userManager.Create(user, password);

            if (identityResult.Succeeded == false)
            {
                return(string.Concat("Seed failed. ", String.Join(";", identityResult.Errors) ));
            }

            _userManager.AddToRole(user.Id, GlobalConfig.UserRole);
            return null;
        }


        public async Task RequestResetPassword(string email)
        {
            IdentityUser user = _userManager.FindByEmail(email);
            string token = _userManager.GeneratePasswordResetToken(user.Id);

            string emailBody = _emailService.GetResetPasswordEmailBody(user.Id, token, user.UserName);
            await _emailService.SendEmailTo(user.Email, "Password reset request", emailBody);
        }

        public void ResetPassword(string userId, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

    }
}
