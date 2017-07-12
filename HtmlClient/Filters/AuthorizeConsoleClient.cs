using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HtmlClient.Filters
{
    public class AuthorizeConsoleClient: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)  
        {
            // first check for all possible nulls
            if ( httpContext?.Session?["ConsoleToken"] == null )
            {
                return false;
            }

            // then make sure tokens match
            return HttpContext.Current.Request.Params["ConsoleToken"] 
                == httpContext.Session["ConsoleToken"].ToString();
        }  

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)  
        {  
            filterContext.Result = new HttpStatusCodeResult( HttpStatusCode.Forbidden );
        } 
    }
}

