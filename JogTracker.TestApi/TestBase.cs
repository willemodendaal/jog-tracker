﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Net;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JogTracker.TestApi
{
    public class TestBase
    {
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
    }
}
