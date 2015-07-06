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

        [TestInitialize]
        public void Init()
        {
            string id = GetUniqueId();
            _email = "user1" + id + "@email.com";
            _password = "B@nanas!B@nanas!123";
            _firstName = "fn_" + id;
            _lastName = "ln_" + id;

            //Date from/to query strings
            _queryFrom = DateTime.Now.AddMonths(-2).ToString(base.DateTimeFormat);
            _queryTo = DateTime.Now.ToString(base.DateTimeFormat);

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

            DateTime jsonDate = json1.date.Value;
            Assert.AreEqual(firstDate.ToString(), jsonDate.ToString());
            Assert.AreEqual("02:00:02", json1.duration.Value);
            Assert.AreEqual(15.50, json1.distanceKm.Value);
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
            query["toDate"] = _queryTo;
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
            Assert.IsTrue(json1.date.Value > DateTime.MinValue);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json1.duration.Value));
            Assert.IsTrue(json1.distanceKm.Value > 0);

            Assert.AreNotEqual(json1.id.Value, json2.id.Value);
            Assert.AreEqual(json1.id.Value, json1Again.id.Value);
        }

        [TestMethod]
        public async Task TestListJogs_NoResults_BecauseOfDateFilters()
        {
            string queryFrom = DateTime.Now.AddYears(-10).ToString(base.DateTimeFormat);
            //*** Ten years in the past should not have any data.
            string queryTo = DateTime.Now.AddYears(-10).AddMonths(1).ToString(base.DateTimeFormat);

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

            long totalCount = GetJson(page1).TotalResults.Value;
            Assert.AreEqual(0, totalCount);
        }

        [TestMethod]
        public async Task TestUpdateEntry()
        {
            var entryJson = await CreateJogEntry(DateTime.Now.AddMonths(-2), new TimeSpan(2, 0, 2), 15.5f);
            string id = entryJson.id.Value;

            //Edit...
            DateTime newDate = DateTime.Now.AddMonths(-3);
            var response = await UpdateJogEntry(id, newDate, new TimeSpan(3, 0, 3), 10.5f);
            Assert.IsTrue(response.IsSuccessStatusCode);

            //Fetch again and compare
            var singleResponse = await GetJogEntry(id);
            var updatedJson = GetJson(singleResponse);
            Assert.AreEqual(HttpStatusCode.OK, singleResponse.StatusCode);

            Assert.AreEqual(id, updatedJson.id.Value);
            Assert.AreEqual("03:00:03", updatedJson.duration.Value);
            Assert.AreEqual(10.5f, updatedJson.distanceKm.Value);
            Assert.AreEqual(newDate.ToString(), updatedJson.date.Value.ToString());
        }

        [TestMethod]
        public async Task TestDeleteEntry()
        {
            //Create entry, then delete.
            var jogJson = await CreateJogEntry(DateTime.Now, new TimeSpan(0, 0, 11), 11f);
            string jogId = jogJson.id.Value;

            var response = await _client.DeleteAsync(string.Format(Uris.DeleteJogEntry, jogId));
            Assert.IsTrue(response.IsSuccessStatusCode);

            //Try and find it. Check for a 404.
            var getResponse = await GetJogEntry(jogId);
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [TestMethod]
        public async Task TestGettingEntryForAnotherUserFails()
        {
            //Create a jog entry
            var jogJson = await CreateJogEntry(DateTime.Now, new TimeSpan(0, 0, 10), 10f);
            string jogId = jogJson.id.Value;

            //Register and sign in as someone else
            Register(_email + "AAA", _password, _firstName, _lastName, _client);
            await Login(_email + "AAA", _password, _client);

            //Attempt to access first user's jog.
            var response = await GetJogEntry(jogId); 

            //Expect a "BadRequest" back.
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task TestViewEntry_AsAdmin()
        {
            //Create entry.
            var entry1 = await CreateJogEntry(DateTime.Now.AddMonths(-1), new TimeSpan(2, 2, 2), 22f);
            string jogId = entry1.id.Value;

            //Sign in as admin and access the entry.
            LoginAsAdmin(_client);
            var entryFetchResult = await GetJogEntry(jogId);
            Assert.IsTrue(entryFetchResult.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task TestListEntries_AsAdmin()
        {
            //Create new Admin that has no data.
            await LoginAsAdmin(_client);
            var registerResponse = base.RegisterAsAdmin(_email + "Adm", _password, _firstName, _lastName, _client, true);
            var json = GetJson(registerResponse);
            Assert.IsTrue(registerResponse.IsSuccessStatusCode, "Registration of new admin user failed. " + json);
            await Login(_email + "Adm", _password, _client);

            //List data and expect some.
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["pageSize"] = "10";
            query["pageIndex"] = "0";
            query["fromDate"] = DateTime.Now.AddYears(-5).ToString(base.DateTimeFormat);
            query["toDate"] = DateTime.Now.AddMonths(1).ToString(base.DateTimeFormat);
            var page1 = await _client.GetAsync(Uris.GetJogs + "?" + query);
            var page1Json = GetJson(page1).Items;

            //Assertions...

            Assert.AreEqual(HttpStatusCode.OK, page1.StatusCode);

            long totalCount = GetJson(page1).TotalResults.Value;
            Assert.IsTrue(page1Json.Count > 0);
            Assert.IsTrue(totalCount > 2);
        }

        private async Task<HttpResponseMessage> UpdateJogEntry(string id, DateTime dateTime, TimeSpan duration,
            float distance)
        {
            string uri = string.Format(Uris.UpdateJogEntry, id);
            var response = await _client.PutAsJsonAsync(uri,
                new
                {
                    DateTime = dateTime,
                    Duration = duration,
                    DistanceKM = distance
                });

            return response;
        }

        private async Task<dynamic> CreateJogEntry(DateTime dateTime, TimeSpan duration, float distance)
        {
            var response = await _client.PostAsJsonAsync(Uris.CreateJogEntry,
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

        private async Task<HttpResponseMessage> GetJogEntry(string entryId)
        {
            var response = await _client.GetAsync(string.Format(Uris.GetJogEntry, entryId));
            return response;
        }
    }
}