using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JogTracker.DomainModel;
using Microsoft.Owin.Security.Notifications;

namespace JogTracker.Services.Responses
{
    /// <summary>
    /// Details about whether or not a registration attempt was successful or not.
    /// </summary>
    public class RegistrationResult
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public JogTrackerUser User { get; set; }

        private RegistrationResult()
        {   
            //Must construct using static methods.
        }

        public static RegistrationResult Failure(string errorMessage, JogTrackerUser user)
        {
            return new RegistrationResult()
            {
                Succeeded = false,
                ErrorMessage = errorMessage,
                User = user
            };
            
        }
        
        public static RegistrationResult Success(JogTrackerUser user)
        {
            return new RegistrationResult()
            {
                Succeeded = true,
                ErrorMessage = null,
                User = user
            };
        }
 
    }
}
