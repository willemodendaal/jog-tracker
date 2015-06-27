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
        public static readonly string Login = Host + "Token";
    }
}
