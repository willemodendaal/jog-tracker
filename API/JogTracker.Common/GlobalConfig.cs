using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Common
{
    public static class GlobalConfig
    {
        public static readonly string[] AllowedCorsOrigins = new[] { "https://localhost:44302", "http://localhost:3000" }; //TODO: Make configurable

        public static readonly string UserRole = "users";
        public static readonly string AdminRole = "administrator";
        public static readonly string UserManager = "userManager";
        public static readonly string FriendlyGenericError = "The API returned an error. Please try again in a moment.";

        public static readonly string JogTrackerEmail = "willem.odendaal@gmail.com"; //Email "From" address.
        public static readonly string SendGridUser = "azure_1cff9262a99ca75208395921384dd3c1@azure.com"; //TODO: Store in config.
        public static readonly string SendGridSmtpServer = "smtp.sendgrid.net"; 
        public static readonly string SendGridPassword = "kq9fAqOu4OFQxRa";

        public static readonly string ProdHost = "http://localhost:3000/";

        //Shared password validator, used in the database, and from the API.
        private static PasswordValidator _passwordValidator;
        public static PasswordValidator PasswordValidator
        {
            get
            {
                if (_passwordValidator == null)
                    _passwordValidator = new PasswordValidator()
                    {
                        RequiredLength = 6,
                        RequireNonLetterOrDigit = true,
                        RequireDigit = true
                    };

                return _passwordValidator;
            }
        }


    }
}
