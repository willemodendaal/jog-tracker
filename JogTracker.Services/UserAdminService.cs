using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JogTracker.Common;
using JogTracker.Data;
using JogTracker.DomainModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace JogTracker.Services
{
    /// <summary>
    /// Abstracts the ASP.NET UserManager (we only need to use a small portion of it)
    /// </summary>
    public interface IUserAdminService
    {
        /// <summary>
        /// Register, or return user-friendly error string.
        /// </summary>
        Task<string> RegisterAsync(string email, string password, string firstName, string lastName);

        Task RequestResetPasswordAsync(string email);

        /// <summary>
        /// Reset password, or return user-friendly error string.
        /// </summary>
        Task<string> ResetPasswordAsync(string userId, string token, string newPassword);

        PagedModel<JogTrackerUser> GetUsers(int pageIndex, int pageSize);

        JogTrackerUser GetUser(string userId);
    }

    public class UserAdminService : IUserAdminService
    {
        private readonly JogDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<JogTrackerUser> _userManager;

        public UserAdminService(IEmailService emailService, JogDbContext dbContext)
        {
            var context = new JogDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            _userManager = new UserManager<JogTrackerUser>(new UserStore<JogTrackerUser>(context));
            _userManager.PasswordValidator = GlobalConfig.PasswordValidator;
            _emailService = emailService;
            _dbContext = dbContext;

            //Assign token provider used to generate Reset password tokens.
            var dataProtectorProvider = GlobalSharedSecurity.DataProtectionProvider;
            //Use the same provider we used when auth was initialized.
            var dataProtector = dataProtectorProvider.Create("My Asp.Net Identity");
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<JogTrackerUser, string>(dataProtector)
            {
                TokenLifespan = TimeSpan.FromHours(24), //Reset token lifespan.
            };
        }

        /// <summary>
        /// Register, or return errors.
        /// </summary>
        public async Task<string> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            var user = new JogTrackerUser()
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            //TODO: store password more securely.
            var identityResult = await _userManager.CreateAsync(user, password);

            if (identityResult.Succeeded == false)
            {
                return (string.Concat("Seed failed. ", String.Join(";", identityResult.Errors)));
            }

            await _userManager.AddToRoleAsync(user.Id, GlobalConfig.UserRole);
            return null;
        }

        public async Task RequestResetPasswordAsync(string email)
        {
            JogTrackerUser user = _userManager.FindByEmail(email);
            string token = _userManager.GeneratePasswordResetToken(user.Id);

            string emailBody = _emailService.GetResetPasswordEmailBody(user.Id, token, user.UserName);
            await _emailService.SendEmailTo(user.Email, "Password reset request", emailBody);
        }

        /// <summary>
        /// Reset password, or return user-friendly error string.
        /// </summary>
        public async Task<string> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            JogTrackerUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult resetResult = await _userManager.ResetPasswordAsync(userId, token, newPassword);

            if (resetResult.Succeeded == false)
            {
                return (string.Concat("Reset password failed. ", String.Join(";", resetResult.Errors)));
            }

            return null; //Success.
        }

        public PagedModel<JogTrackerUser> GetUsers(int pageIndex, int pageSize)
        {
            int totalCount = _dbContext.Users.Count();
                
            var resultList = _dbContext.Users
                .OrderBy(u => u.UserName)
                .Skip(pageIndex*pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedModel<JogTrackerUser>(pageIndex, pageSize, totalCount, resultList);
        }

        public JogTrackerUser GetUser(string userId)
        {
            return _dbContext.Users.Find(userId);
        }
    }
}