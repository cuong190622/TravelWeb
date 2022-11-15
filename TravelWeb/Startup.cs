using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(TravelWeb.Startup))]
namespace TravelWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var authOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/NotAuth"),
                ExpireTimeSpan = TimeSpan.FromMinutes(8),
            };
            app.UseCookieAuthentication(authOptions);
        }
    }
}
