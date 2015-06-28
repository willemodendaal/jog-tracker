using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogTracker.Api.Models.JsonResults
{
    public class JsonResultBase
    {
        protected string SafeJsonDate(DateTime dateTime)
        {
            //Return safe ISO 8601 date string.
            //Json.net can sometimes interfere.
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
    }
}