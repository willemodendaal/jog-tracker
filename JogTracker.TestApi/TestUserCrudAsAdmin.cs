using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace JogTracker.TestApi
{
    [TestClass]
    public class TestUserCrudAsAdmin : TestBase
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
        public async Task TestCreateUserAsAdmin()
        {
            using (var client = new HttpClient())
            {
                await LoginAsAdmin(client);
                RegisterAsAdmin(_email, _password, _firstName, _lastName, client);

                //Login as the user should succeed.
                var loginResult = await base.Login(_email, _password, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult, "Login result should have been 200");
            }
        }

        [TestMethod]
        public void TestUnableToCreateIfNotAdmin_Anonymous()
        {
            using (var client = new HttpClient())
            {
                var result = RegisterAsAdmin(_email, _password, _firstName, _lastName, client);
                Assert.AreEqual(HttpStatusCode.Unauthorized, result);
            }
        }


        [TestMethod]
        public async Task TestUnableToCreateIfNotAdmin_OtherUser()
        {
            using (var client = new HttpClient())
            {
                base.Register(_email, _password, _firstName, _lastName, client);
                await base.Login(_email, _password, client);


                var result = RegisterAsAdmin(_email+"cc", _password, _firstName+"cc", _lastName+"cc", client);
                Assert.AreEqual(HttpStatusCode.Unauthorized, result);
            
            }
        }

        [TestMethod]
        public void TestUpdateUserEmail()
        {
        }

        [TestMethod]
        public void TestUpdateUserFirstName()
        {
        }

        [TestMethod]
        public void TestUpdateUserLastName()
        {
        }

        [TestMethod]
        public void TestListAllUsers_PageOne()
        {
        }


        private HttpStatusCode RegisterAsAdmin(string email, string password, string firstName, string lastName, HttpClient client)
        {
            //Register
            HttpResponseMessage response = client.PostAsJsonAsync(Uris.RegisterAsAdmin,
                new
                {
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                }).Result;

            return response.StatusCode;
        }
    }
}
