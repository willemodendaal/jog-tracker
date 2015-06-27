using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.TestApi
{
    [TestClass]
    public class TestRegistration : TestBase
    {
        string _userName;
        string _email;
        string _password;

        [TestInitialize]
        public void Init()
        {
            string id = base.GetUniqueId();
            _userName = "user1" + id;
            _email = "user1" + id + "@email.com";
            _password = "B@nanas!B@nanas!123";
        }

        [TestMethod]
        public async Task RegisterAndLogin()
        {
            using (var client = new HttpClient())
            {
                //Register
                HttpResponseMessage response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        UserName = _userName,
                        Email = _email,
                        Password = _password,
                    });

                string responseBody = base.GetResponseBody(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Register result should have been 200");

                //Login
                var loginResult = await base.Login(_userName, _password, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult, "Login result should have been 200");
  
            }
        }

        [TestMethod]
        public async Task RegisterFailsIfPasswordNotComplexEnough()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        UserName = _userName,
                        Email = _email,
                        Password = "1",
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [TestMethod]
        public async Task RegisterFailsIfEmailInvalid()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        UserName = _userName,
                        Email = "bananas",
                        Password = _password,
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [TestMethod]
        public async Task RegisterFailsIfNotAllFieldsSpecified()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        UserName = _userName,
                        Email = _email,
                        Password = "",
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        
    }
}
