using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Common
{
    public static class SharedSecurity
    {
        //Initialized by Owin, but we need to share it with the userService. Declare as static here 
        //  and share with the UserAdminService (DI won't do the trick).
        //  See: http://stackoverflow.com/questions/27471363/no-iusertokenprovider-is-registered-when-using-structuremap-dependency-injecti
        public static IDataProtectionProvider DataProtectionProvider { get; set; }
    }
}
