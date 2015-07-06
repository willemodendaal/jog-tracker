using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JogTracker.TestApi
{
    [TestClass]
    public class TestUserCrudAsUserManager : TestBase
    {
        private string _userManagerEmail;
        private string _firstName;
        private string _lastName;
        private string _userManagerPassword;

        [TestInitialize]
        public void Init()
        {
            string id = GetUniqueId();
            _userManagerEmail = "user1" + id + "@email.com";
            _userManagerPassword = "B@nanas!B@nanas!123";
            _firstName = "fn_" + id;
            _lastName = "ln_" + id;
        }

        [TestMethod]
        public async Task TestCreateUserManager()
        {
            using (var client = new HttpClient())
            {
                //Login as top level admin. Create a userManager.
                await LoginAsAdmin(client);
                RegisterAsAdmin(_userManagerEmail, _userManagerPassword, _firstName, _lastName, client, false, true);

                //Login as the userManager should succeed.
                var loginResult = await Login(_userManagerEmail, _userManagerPassword, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult, "User manager login result should have been 200");

                //Now create users as the userManager.
                string id = GetUniqueId();
                string plainUserEmail = "plain" + id + "@email.com";
                string pwd = "B@nanas!B@nanas!123";
                string firstName = "fn_" + id;
                string lastName = "ln_" + id;
                RegisterAsAdmin(plainUserEmail, pwd, firstName, lastName, client, false, false);

                //User listing should contain users.
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["pageSize"] = "1";
                query["pageIndex"] = "0";
                var page1 = await client.GetAsync(Uris.ListUsers + "?" + query);

                //Check if data comes back as expected
                Assert.AreEqual(HttpStatusCode.OK, page1.StatusCode);
                dynamic json1 = GetJson(page1).Items[0]; //Get first user from json array.
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json1.id.Value));
                
            }
        }

      

    }
}