using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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
        public void TestUpdateUserEmailFirstNameAndLastName()
        {
        }


        [TestMethod]
        public async Task TestListUsers_PageOneAndTwo()
        {
            using (var client = new HttpClient())
            {
                await LoginAsAdmin(client);

                var query = HttpUtility.ParseQueryString(string.Empty);
                query["pageSize"] = "1";
                query["pageIndex"] = "0";
                var page1 = await client.GetAsync(Uris.ListUsers + "?" + query.ToString());

                query["pageSize"] = "1";
                query["pageIndex"] = "1";
                var page2 = await client.GetAsync(Uris.ListUsers + "?" + query.ToString());

                query["pageSize"] = "1";
                query["pageIndex"] = "0";
                var page1Again = await client.GetAsync(Uris.ListUsers + "?" + query.ToString());

                //Assertions...
                // - Ensure both responses are successful.
                // - Ensure they are not the same.
                // - Ensure the basic data is there (like firstName and lastName).
                Assert.AreEqual(HttpStatusCode.OK, page1.StatusCode);
                Assert.AreEqual(HttpStatusCode.OK, page2.StatusCode);

                dynamic json1 = GetJson(page1).Items[0]; //Get first user from json array.
                dynamic json2 = GetJson(page2).Items[0];
                dynamic json1Again = GetJson(page1Again).Items[0]; 

                Assert.IsTrue(!string.IsNullOrWhiteSpace(json1.firstName.Value));
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json1.id.Value));
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json1.lastName.Value));
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json1.email.Value));

                Assert.AreNotEqual(json1.id.Value, json2.id.Value);
                Assert.AreEqual(json1.id.Value, json1Again.id.Value);

            }
        }


        [TestMethod]
        public async Task TestListUsers_FailsIfNotAdmin()
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
