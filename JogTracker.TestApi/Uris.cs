using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.TestApi
{
    internal class Uris
    {
        //public static readonly string Host = "https://localhost/"; //Local IIS, to make testing easier.
        public static readonly string Host = "https://localhost:44301/"; 
        public static readonly string Base = Host + "api/v1/";

        public static readonly string Register = Base + "account/register";
        public static readonly string RegisterAsAdmin = Base + "account/registerAsAdmin";
        public static readonly string RequestResetPassword = Base + "account/requestResetPwd";
        public static readonly string ResetPassword = Base + "account/resetPwd";

        public static readonly string ListUsers = Base + "user/";
        public static readonly string UpdateUser = Base + "user/{0}/update/";
        public static readonly string GetUser = Base + "user/{0}";
        
        public static readonly string GetJogs = Base + "jog";
        public static readonly string CreateJogEntry = Base + "jog/new";
        
        public static readonly string Login = Host + "Token";
    }
}
