﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogTracker.Api.Models
{
    public class RegisterBindingModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}