using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JogTracker.Api.Utils;
using JogTracker.DomainModel;

namespace JogTracker.Api.Models.JsonResults
{
    /// <summary>
    /// Will be serialized to JSON and returned to the client.
    /// </summary>
    internal class UserJsonResult : IMappable<JogEntryUser, UserJsonResult>
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        public UserJsonResult Map(JogEntryUser model)
        {
            return new UserJsonResult()
            {
                id = model.Id,
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email
            };
        }
    }

}