﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JogTracker.Api.Startup))]

namespace JogTracker.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            System.Diagnostics.Trace.TraceInformation("Owin initialized OK. Starting app...");

            ConfigureAuth(app);
            
        }
    }
}
