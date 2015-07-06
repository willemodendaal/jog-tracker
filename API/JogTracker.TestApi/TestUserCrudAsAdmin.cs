using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JogTracker.TestApi
{
    [TestClass]
    public class TestUserCrudAsAdmin : TestBase
    {
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _password;

        [TestInitialize]
        public void Init()
        {
            string id = GetUniqueId();
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
                var loginResult = await Login(_email, _password, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult, "Login result should have been 200");
            }
        }

        [TestMethod]
        public void TestUnableToCreateIfNotAdmin_Anonymous()
        {
            using (var client = new HttpClient())
            {
                var result = RegisterAsAdmin(_email, _password, _firstName, _lastName, client);
                Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestUnableToCreateIfNotAdmin_OtherUser()
        {
            using (var client = new HttpClient())
            {
                Register(_email, _password, _firstName, _lastName, client);
                await Login(_email, _password, client);

                var result = RegisterAsAdmin(_email + "cc", _password, _firstName + "cc", _lastName + "cc", client);
                Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestGetSingleUser()
        {
            using (var client = new HttpClient())
            {
                await LoginAsAdmin(client);
                var registerResult = RegisterAsAdmin(_email, _password, _firstName, _lastName, client);
                dynamic registerJson = GetJson(registerResult);
                string newUserId = registerJson.id.Value;

                var userResponse = await client.GetAsync(string.Format(Uris.GetUser, newUserId));
                dynamic newUserJson = GetJson(userResponse);

                Assert.AreEqual(newUserId, newUserJson.id.Value);
                Assert.AreEqual(_email, newUserJson.email.Value);
                Assert.AreEqual(_firstName, newUserJson.firstName.Value);
                Assert.AreEqual(_lastName, newUserJson.lastName.Value);

            }
        }

        
        [TestMethod]
        public async Task TestUpdateUserEmailFirstNameAndLastName()
        {
            using (var client = new HttpClient())
            {
                await LoginAsAdmin(client);
                var registerResult = RegisterAsAdmin(_email, _password, _firstName, _lastName, client);
                dynamic registerJson = GetJson(registerResult);
                string newUserId = registerJson.id.Value;

                HttpResponseMessage updateResponse = client.PutAsJsonAsync(string.Format(Uris.UpdateUser, newUserId),
                new
                {
                    Email = _email + "cc", //Add "cc" at the end of everything, to see if values changed.
                    FirstName = _firstName + "cc",
                    LastName = _lastName + "cc"
                }).Result;

                Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

                //Fetch user and check if values are as expected (with "cc"s at the end)
                var userResponse = await client.GetAsync(string.Format(Uris.GetUser, newUserId));
                dynamic newUserJson = GetJson(userResponse);
                Assert.AreEqual(newUserId, newUserJson.id.Value);
                Assert.AreEqual(_email + "cc", newUserJson.email.Value);
                Assert.AreEqual(_firstName + "cc", newUserJson.firstName.Value);
                Assert.AreEqual(_lastName + "cc", newUserJson.lastName.Value);
            }
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
                var page1 = await client.GetAsync(Uris.ListUsers + "?" + query);

                query["pageSize"] = "1";
                query["pageIndex"] = "1";
                var page2 = await client.GetAsync(Uris.ListUsers + "?" + query);

                query["pageSize"] = "1";
                query["pageIndex"] = "0";
                var page1Again = await client.GetAsync(Uris.ListUsers + "?" + query);

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
            using (var client = new HttpClient())
            {
                Register(_email, _password, _firstName, _lastName, client);
                await Login(_email, _password, client);

                var query = HttpUtility.ParseQueryString(string.Empty);
                query["pageSize"] = "1";
                query["pageIndex"] = "0";
                var page1 = await client.GetAsync(Uris.ListUsers + "?" + query);

                Assert.AreEqual(HttpStatusCode.Unauthorized, page1.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestAdminCanSeeOthersJogs()
        {
            using (var client = new HttpClient())
            {
                //Make sure we a admin
                await base.LoginAsAdmin(client);

                //Register plain user
                string id = GetUniqueId();
                string plainUserEmail = "plain" + id + "@email.com";
                string pwd = "B@nanas!B@nanas!123";
                string firstName = "fn_" + id;
                string lastName = "ln_" + id;
                HttpResponseMessage registerResponse = RegisterAsAdmin(plainUserEmail, pwd, firstName, lastName, client, false, false);
                Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode, "Registering plain user failed.");

                //Login as plain user
                var loginResult = await Login(plainUserEmail, pwd, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult, "Login as plain user failed.");

                //Create the jog
                dynamic jogJson =await CreateJogEntry(DateTime.Now, new TimeSpan(1, 1, 1), 12f, client);
                string jogId = jogJson.id.Value;

                //Login as admin again, and try to find the jog.
                await LoginAsAdmin(client);
                dynamic allJogs = await GetAllJogs(client);
                bool found = FindSpecificJog(allJogs, jogId);

                Assert.IsTrue(found, "Admin unable to find jog that other user created.");

            }
        }

        private bool FindSpecificJog(dynamic allJogs, string jogId)
        {
            foreach (dynamic jog in allJogs)
            {
                if (jog.id == jogId)
                    return true;
            }

            return false;
        }

        private async Task<dynamic> GetAllJogs(HttpClient client)
        {
            var page1 = await client.GetAsync(Uris.GetAllJogs);
            var allJson = GetJson(page1).Items;

            return allJson;
        }

        private async Task<dynamic> CreateJogEntry(DateTime dateTime, TimeSpan duration, float distance, HttpClient client)
        {
            var response = await client.PostAsJsonAsync(Uris.CreateJogEntry,
                new
                {
                    DateTime = dateTime,
                    Duration = duration,
                    DistanceKM = distance
                });
            var json = GetJson(response);
            Assert.IsTrue(response.IsSuccessStatusCode);
            return json;
        }


        
    }
}