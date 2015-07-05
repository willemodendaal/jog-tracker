using JogTracker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JogTracker.Services
{
    internal class ResetEmailBodyGenerator
    {

        //Move to file, not code.
        public static readonly string ResetPasswordEmailTemplate =
@"Dear {0}

Please click on this link to reset your password:
{1}
If you did not request this email, please ignore it.

Regards,
The Jogging Tracker team";

        public string GetEmailBody(string firstName, string userId, string token)
        {
            string encodedUserId = HttpUtility.UrlEncode(userId);
            string encodedToken = HttpUtility.UrlEncode(token);

            string url = string.Format("{0}#/choosePassword?uid={1}&token={2}", GlobalConfig.ProdHost, encodedUserId, encodedToken);

            string message = string.Format(ResetPasswordEmailTemplate, firstName, url);
            return message;
        }
    }
}
