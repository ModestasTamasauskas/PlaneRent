﻿using CarRental.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CarRental.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
                                        {
                                            AuthenticationType = "Cookies"
                                        });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                                               {
                                                   SignInAsAuthenticationType = "Cookies",
                                                   Authority = "http://localhost:5000", //ID Server
                                                   RedirectUri = "http://localhost:5002",
                                                   ClientId = "mvc",
                                                   ResponseType = "id_token"
                                                   ,
                                               });
        }
    }
}