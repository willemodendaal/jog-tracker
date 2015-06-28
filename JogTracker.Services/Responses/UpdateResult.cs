using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Services.Responses
{
    public class UpdateResult
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }

        private UpdateResult()
        {   
            //Must construct using static methods.
        }

        public static UpdateResult Failure(string errorMessage)
        {
            return new UpdateResult()
            {
                Succeeded = false,
                ErrorMessage = errorMessage,
            };
            
        }

        public static UpdateResult Success()
        {
            return new UpdateResult()
            {
                Succeeded = true,
                ErrorMessage = null,
            };
        }
 
    }
}
