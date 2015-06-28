using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Net;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JogTracker.TestApi
{
    public class TestBase
    {
        private string _adminEmail = "willem.odendaal@gmail.com";
        private string _adminPassword = "runUpYonderHills!";

        protected string GetUniqueId()
        {
            return Guid.NewGuid().ToString().Replace("-",""); //Used to make info distinct, since we're modifying state with these tests.
        }

        protected string GetResponseBody(HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        protected async Task<HttpStatusCode> Login(string email, string password, HttpClient client)
        {
            var data = new Dictionary<string, string>()
            {
                { "grant_type", "password"},
                { "username", email },
                { "password", password }
            };

            var formData = new FormUrlEncodedContent(data);
            var result = await client.PostAsync(Uris.Login, formData);

            return result.StatusCode;
        }

        internal void Register(string email, string password, string firstName, string lastName, HttpClient client)
        {
            //Register
            HttpResponseMessage response = client.PostAsJsonAsync(Uris.Register,
                new
                {
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                }).Result;

            string responseBody = GetResponseBody(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Register result should have been 200");

        }

        /// <summary>
        /// Login and set bearer token on the http client.
        /// </summary>
        internal async Task LoginAsAdmin(HttpClient client)
        {
            var data = new Dictionary<string, string>()
            {
                { "grant_type", "password"},
                { "username", _adminEmail },
                { "password", _adminPassword }
            };

            var formData = new FormUrlEncodedContent(data);
            var result = await client.PostAsync(Uris.Login, formData);

            dynamic jsonResult = GetJson(result);
            client.DefaultRequestHeaders.Add("Authorization", "bearer " + jsonResult.access_token);

        }

        private dynamic GetJson(HttpResponseMessage response)
        {
            string bodyText = GetResponseBody(response);
            JObject json = JObject.Parse(bodyText);
            return json;
        }
    }
}
