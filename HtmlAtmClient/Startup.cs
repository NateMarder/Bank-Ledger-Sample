﻿using HtmlAtmClient;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup( typeof( Startup ) )]

namespace HtmlAtmClient
{
    public partial class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            ConfigureAuth( app );
        }
    }
}