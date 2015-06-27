using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Net;

namespace JogTracker.TestApi
{
    public class TestBase
    {
        protected string GetUniqueId()
        {
            return Guid.NewGuid().ToString(); //Used to make info distinct, since we're modifying state with these tests.
        }


        protected async Task<HttpStatusCode> Login(string userName, string password, HttpClient client)
        {
            var data = new Dictionary<string, string>()
            {
                { "grant_type", "password"},
                { "username", userName },
                { "password", password }
            };

            var formData = new FormUrlEncodedContent(data);
            var result = await client.PostAsync(Uris.Login, formData);

            return result.StatusCode;
        }
    }
}
