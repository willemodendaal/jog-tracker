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
        string _userName = "test1"; //Must not exist in the db yet.
        string _email = "willem.odendaal@gmail.com"; //Must be a real email.
        string _password = "test1@34Bana";

        [TestMethod]
        public async Task TestResetPasswordSteps()
        {
            using (var client = new HttpClient())
            {
                base.Register(_userName, _email, _password, client);
            }

            /*
             * Uncomment the desired steps below. This has to be done manually
             *  because there is a manual email required to do a password reset.
             */
            await RequestResetWithUserName(_userName);
            //await RequestResetWithEmail(_email);
            //await ResetAndLogin(_userName, "***token****", "P@@sswwoorrdd123");

            Assert.IsTrue(true); //Just to give MSTest something to test.
        }

        public static async Task RequestResetWithUserName(string userName)
        {
            using (var client = new HttpClient())
            {

                var response = await client.PostAsJsonAsync(Uris.RequestResetPassword,
                    new
                    {
                        UserName = userName
                    });

                Assert.AreEqual(HttpStatusCode.OK, response);

                //At this point an email would have been sent. Get the code from the email
                //  and run the resetPassword test.
            }
        }


        public async Task RequestResetWithEmail(string email)
        {
            using (var client = new HttpClient())
            {

                var response = await client.PostAsJsonAsync(Uris.RequestResetPassword,
                    new
                    {
                        Email = email
                    });

                Assert.AreEqual(HttpStatusCode.OK, response);

                //At this point an email would have been sent. Get the code from the email
                //  and run the resetPassword test.
            }
        }


        public async Task ResetAndLogin(string userName, string resetToken, string newPassword)
        {
            
            using (var client = new HttpClient())
            {
                    var response = await client.PostAsJsonAsync(Uris.ResetPassword,
                        new
                        {
                            Token = resetToken,
                            UserName = userName,
                            NewPassword = newPassword
                        });

                Assert.AreEqual(HttpStatusCode.OK, response);

                //Log in with new password must succeed.
                var loginResult = await base.Login(_userName, newPassword, client);
                Assert.AreEqual(HttpStatusCode.OK, loginResult, "Login result should have been 200");
            }
        }
        

    }
}
