using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogTracker.Web.Models
{
    public class JogFilterBindingModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}