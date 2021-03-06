﻿using System.Net;
using System.Web.Mvc;
using HtmlApp.Classes;

namespace HtmlApp.Filters
{
    public class AuthorizeConsoleAppUser: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var incomingSessionToken = filterContext.HttpContext.Request.Params["SessionId"];
            var incomingEmail = filterContext.HttpContext.Request.Params["UserId"];

            if ( incomingSessionToken == null || incomingEmail == null )
            {
                filterContext.Result = new HttpStatusCodeResult( HttpStatusCode.Forbidden );
                return;
            }

            var validParams = ActiveSessions.SessionIsAuthenticated( incomingSessionToken, incomingEmail );
            if (!validParams)
            {
                filterContext.Result = new HttpStatusCodeResult( HttpStatusCode.Forbidden );
            }
        }  
    }
}

