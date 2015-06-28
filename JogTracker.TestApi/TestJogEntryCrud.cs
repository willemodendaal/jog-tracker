using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JogTracker.TestApi
{
    [TestClass]
    public class TestJogEntryCrud : TestBase
    {
        private HttpClient _client;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _password;
        private string _queryFrom;
        private string _queryTo;
        private string _dateTimeFormat = "yyyy-MM-ddThh:mm:ss.fffZ";

        [TestInitialize]
        public void Init()
        {
            string id = GetUniqueId();
            _email = "user1" + id + "@email.com";
            _password = "B@nanas!B@nanas!123";
            _firstName = "fn_" + id;
            _lastName = "ln_" + id;

            //Date from/to query strings
            _queryFrom = DateTime.Now.AddMonths(-2).ToString(_dateTimeFormat);
            _queryTo = DateTime.Now.ToString(_dateTimeFormat);

            //Register and login.
            _client = new HttpClient();
            Register(_email, _password, _firstName, _lastName, _client);
            var loginResult = Login(_email, _password, _client).Result;
            Assert.AreEqual(HttpStatusCode.OK, loginResult, "Login result should have been 200");
        }

        [TestCleanup]
        public void TestTeardown()
        {
            _client.Dispose();
        }

        [TestMethod]
        public async Task TestCreateEntries()
        {
            DateTime firstDate = DateTime.Now;
            dynamic json1 = await CreateJogEntry(firstDate, new TimeSpan(2, 0, 2), 15.5f);
            dynamic json2 = await CreateJogEntry(DateTime.Now.AddMonths(-2), new TimeSpan(3, 1, 3), 25.4f);
            dynamic json3 = await CreateJogEntry(DateTime.Now.AddMonths(-3), new TimeSpan(1, 0, 2), 10.1f);

            Assert.IsFalse(string.IsNullOrWhiteSpace(json1.id.Value), "JogEntry response did not have an Id.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(json2.id.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(json3.id.Value));

            Assert.AreEqual(firstDate.ToString(_dateTimeFormat), json1.date.Value);
            Assert.AreEqual("02:00:02", json1.duration.Value);
            Assert.AreEqual("15.50", json1.distanceKm.Value);
        }

        [TestMethod]
        public async Task TestListJogs()
        {
            await CreateJogEntry(DateTime.Now.AddMonths(-2), new TimeSpan(2, 0, 2), 15.5f);
            await CreateJogEntry(DateTime.Now.AddMonths(-2), new TimeSpan(3, 1, 3), 25.4f);
            await CreateJogEntry(DateTime.Now.AddMonths(-3), new TimeSpan(1, 0, 2), 10.1f);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["pageSize"] = "1";
            query["pageIndex"] = "0";
            query["fromDate"] = _queryFrom;
            query["toDate"] = _queryFrom;
            var page1 = await _client.GetAsync(Uris.GetJogs + "?" + query);

            query["pageSize"] = "1";
            query["pageIndex"] = "1";
            var page2 = await _client.GetAsync(Uris.GetJogs + "?" + query);

            query["pageSize"] = "1";
            query["pageIndex"] = "0";
            var page1Again = await _client.GetAsync(Uris.GetJogs + "?" + query);

            //Assertions...
            // - Ensure both responses are successful.
            // - Ensure they are not the same.
            // - Ensure the basic data is there (like firstName and lastName).
            Assert.AreEqual(HttpStatusCode.OK, page1.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, page2.StatusCode);

            dynamic json1 = GetJson(page1).Items[0]; //Get first user from json array.
            dynamic json2 = GetJson(page2).Items[0];
            dynamic json1Again = GetJson(page1Again).Items[0];

            Assert.IsFalse(string.IsNullOrWhiteSpace(json1.id.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(json1.date.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(json1.duration.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(json1.distanceKm.Value));

            Assert.AreNotEqual(json1.id.Value, json2.id.Value);
            Assert.AreEqual(json1.id.Value, json1Again.id.Value);
        }

        [TestMethod]
        public async Task TestListJogs_NoResults_BecauseOfDateFilters()
        {
            string queryFrom = DateTime.Now.AddYears(-10).ToString(_dateTimeFormat); //*** Ten years in the past should not have any data.
            string queryTo = DateTime.Now.AddYears(-10).AddMonths(1).ToString(_dateTimeFormat);

            await CreateJogEntry(DateTime.Now.AddMonths(-2), new TimeSpan(2, 0, 2), 15.5f);
            await CreateJogEntry(DateTime.Now.AddMonths(-2), new TimeSpan(3, 1, 3), 25.4f);
            await CreateJogEntry(DateTime.Now.AddMonths(-3), new TimeSpan(1, 0, 2), 10.1f);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["pageSize"] = "1";
            query["pageIndex"] = "0";
            query["fromDate"] = queryFrom;
            query["toDate"] = queryTo;
            var page1 = await _client.GetAsync(Uris.GetJogs + "?" + query);

            //Assertions...
            Assert.AreEqual(HttpStatusCode.OK, page1.StatusCode);
            
            int totalCount = GetJson(page1).TotalResults.Value;
            Assert.AreEqual(0, totalCount);
        }

        [TestMethod]
        public void TestEditEntry()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestDeleteEntry()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestListEntries_AsAdmin()
        {
            //Ensure admin can list other users jog entries.
            Assert.Fail();
        }


        private async Task<dynamic> CreateJogEntry(DateTime dateTime, TimeSpan duration, float distance)
        {
            var response = await _client.PostAsJsonAsync(Uris.CreateJogEntry,
                    new
                    {
                        Date = dateTime,
                        Duration = duration,
                        Distance = distance
                    });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var json = GetJson(response);
            return json;
        }
    }
}