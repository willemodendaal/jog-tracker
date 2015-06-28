using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.TestApi
{
    [TestClass]
    public class TestResetPassword : TestBase
    {
        string _email = "willem.odendaal@gmail.com"; //Must be a real email.
        string _password = "test1@34Bana";
        string _firstName = "Willem";
        string _lastName = "Odendaal";

        [TestMethod]
        public async Task TestResetPasswordSteps()
        {
            
            /*
             * Uncomment the desired steps below. This has to be done manually
             *  because there is a manual email required to do a password reset.
             */
            //RegisterUser(_email, _password, _firstName, _lastName);
            //await RequestResetWithEmail(_email);
            //await ResetAndLogin(
            //    _email,
            //    "6466dcab-a54a-4ccb-b6c0-991dbbafbf8e",
            //    "s/9Al0phGjlttuhVoJR/JUeMC4XSgeyDYzNfgvZDq4KYtJAMdo4qjyiqokK7w7dYvG0t6RH1WRQRNnc3VixJhkBubSuywOzTE464UHN/dLO7a38IUNm/n846YSyS5H1BZAH2uLP8Lw/wYAKRRqBMQwEwl9WMQzzWyiySKmnvwkxGNXX8fql9UG8VRjl+sAeA/502ezs4fujuXXf4ylWmoQ==",
            //    "P@@sswwoorrdd123");

            Assert.IsTrue(true); //Just to give MSTest something to test.
        }

        private void RegisterUser(string email, string password, string firstName, string lastName)
        {
            using (var client = new HttpClient())
            {
                base.Register(email, password, firstName, lastName, client);
            }
        }

        private static async Task RequestResetWithEmail(string email)
        {
            using (var client = new HttpClient())
            {

                var response = await client.PostAsJsonAsync(Uris.RequestResetPassword,
                    new
                    {
                        Email = email
                    });

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                //At this point an email would have been sent. Get the code from the email
                //  and run the resetPassword test.
            }
        }


        private async Task ResetAndLogin(string email, string userId, string resetToken, string newPassword)
        {
            
            using (var client = new HttpClient())
            {
                    var response = await client.PostAsJsonAsync(Uris.ResetPassword,
                        new
                        {
                            Token = resetToken,
                            UserId = userId,
                            NewPassword = newPassword
                        });

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                //Log in with *new password* must succeed.
                var loginResult = await base.Login(email, newPassword, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult, "Login result should have been 200");
            }
        }
        

    }
}
