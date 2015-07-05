using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using JogTracker.Common;
using JogTracker.Data;
using JogTracker.DomainModel;
using JogTracker.Services.Responses;
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
        Task<RegistrationResult> RegisterAsync(string email, string password, string firstName, string lastName, bool admin);

        Task RequestResetPasswordAsync(string email);

        /// <summary>
        /// Reset password, or return user-friendly error string.
        /// </summary>
        Task<string> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<PagedModel<JogTrackerUser>> GetUsersAsync(int pageIndex, int pageSize);
        Task<JogTrackerUser> GetUserAsync(string userId);
        Task<UpdateResult> UpdateAsync(string userId, string firstName, string lastName, string email = null);
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
        public async Task<RegistrationResult> RegisterAsync(string email, string password, string firstName, string lastName, bool admin)
        {
            var user = new JogTrackerUser()
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            var identityResult = await _userManager.CreateAsync(user, password);

            if (identityResult.Succeeded == false)
            {
                string errorMessage = (string.Concat("Seed failed. ", String.Join(";", identityResult.Errors)));
                return RegistrationResult.Failure(errorMessage, user);
            }

            await _userManager.AddToRoleAsync(user.Id, GlobalConfig.UserRole);

            if (admin)
            {
                await _userManager.AddToRoleAsync(user.Id, GlobalConfig.AdminRole);
            }

            return RegistrationResult.Success(user);
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

        public async Task<PagedModel<JogTrackerUser>> GetUsersAsync(int pageIndex, int pageSize)
        {
            int totalCount = _dbContext.Users.Count();
                
            var resultList = await ( _dbContext.Users
                .OrderBy(u => u.UserName)
                .Skip(pageIndex*pageSize)
                .Take(pageSize))
                .ToListAsync();

            return new PagedModel<JogTrackerUser>(pageIndex, pageSize, totalCount, resultList);
        }

        public async Task<JogTrackerUser> GetUserAsync(string userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<UpdateResult> UpdateAsync(string userId, string firstName, string lastName, string email = null)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return UpdateResult.Failure(string.Concat(userId, " is an invalid user id."));
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email ?? user.Email;
            await _dbContext.SaveChangesAsync();

            return UpdateResult.Success();
        }

    }
}