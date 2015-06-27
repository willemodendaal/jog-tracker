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
            _password = "B@nanas!B@nanas!";
        }

        [TestMethod]
        public async Task RegisterAndLogin()
        {
            using (var client = new HttpClient())
            {
                //Register
                var response = await client.PostAsJsonAsync(Uris.Register,
                    new
                    {
                        UserName = _userName,
                        Email = _email,
                        Password = _password,
                    });

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                //Login
                var loginResult = base.Login(_userName, _password, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult);
  
            }
        }

        [TestMethod]
        public async Task RegisterFailsIfPasswordNotComplexEnough()
        {
        }

        
    }
}
