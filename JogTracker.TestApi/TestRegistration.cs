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
        string _firstName;
        string _lastName;

        [TestInitialize]
        public void Init()
        {
            string id = base.GetUniqueId();
            _email = "user1" + id + "@email.com";
            _password = "B@nanas!B@nanas!123";
            _firstName = "fn_" + id;
            _lastName = "ln_" + id;

        }

        [TestMethod]
        public async Task RegisterAndLogin()
        {
            using (var client = new HttpClient())
            {
                base.Register(_email, _password, _firstName, _lastName, client);

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
                        FirstName = _firstName,
                        LastName = _lastName
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
                        FirstName = _firstName,
                        LastName = _lastName
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [TestMethod]
        public async Task RegisterFailsIfNotAllFieldsSpecified_NoPassword()
        {
            using (var client = new HttpClient())
            {
                //No password
                var response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        Email = _email,
                        Password = "",
                        FirstName = _firstName,
                        LastName = _lastName
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }


        [TestMethod]
        public async Task RegisterFailsIfNotAllFieldsSpecified_NoLastName()
        {
            using (var client = new HttpClient())
            {
                //No password
                var response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        Email = _email,
                        Password = _password,
                        FirstName = _firstName,
                        LastName = ""
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [TestMethod]
        public async Task RegisterFailsIfNotAllFieldsSpecified_NoFirstName()
        {
            using (var client = new HttpClient())
            {
                //No password
                var response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        Email = _email,
                        Password = _password,
                        FirstName = "",
                        LastName = _lastName
                    });

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        
    }
}
