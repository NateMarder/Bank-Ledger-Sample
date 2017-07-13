using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HtmlApp.Classes;

namespace HtmlApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            var session = Tuple.Create( false, Session.SessionID, string.Empty );
            ActiveSessions.Sessions.Add( session );
        }

        protected void Session_End(object sender, EventArgs e)
        {
            ActiveSessions.RemoveSession( Session.SessionID );
        }
    }
}
