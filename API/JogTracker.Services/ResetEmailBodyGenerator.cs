using JogTracker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string GetEmailBody(string userName, string userId, string token)
        {
            string url = string.Format("{0}#/choosePassword?uid={1}&token={2}", GlobalConfig.ProdHost, userId, token);
            return url;
        }
    }
}
