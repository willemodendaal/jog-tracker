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
        string _email;
        string _password;

        [TestInitialize]
        public void Init()
        {
            string id = base.GetUniqueId();
            _email = "user1" + id + "@email.com";
            _password = "B@nanas!B@nanas!123";
        }

        [TestMethod]
        public async Task RegisterAndLogin()
        {
            using (var client = new HttpClient())
            {
                base.Register(_email, _password, client);

                //Login
                var loginResult = await base.Login(_email, _password, client);
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
                        Email = _email,
                        Password = "",
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        
    }
}
