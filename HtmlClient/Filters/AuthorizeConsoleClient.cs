using System.Web;
using System.Web.Mvc;

namespace HtmlClient.Filters
{
    public class AuthorizeConsoleClient: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)  
        {  
            if ( HttpContext.Current == null )
            {
                return false;
            }

            var token = HttpContext.Current.Request.Params["Token"];
            if ( token != null && HttpContext.Current.Session.SessionID != null)
            {
                return HttpContext.Current.Session.SessionID == token;
            }
            return false;  
        }  

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)  
        {  
            filterContext.Result = new HttpUnauthorizedResult();  
        } 
    }
}

