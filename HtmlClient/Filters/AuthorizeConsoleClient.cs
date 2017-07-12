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

            var sessionGuid = HttpContext.Current.Request.Params["SessionGuid"];
            var currentSessionGuid = HttpContext.Current.Session["SessionGuid"];
            if ( sessionGuid != null && currentSessionGuid != null)
            {
                return currentSessionGuid.ToString() == sessionGuid;
            }
            return false;  
        }  

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)  
        {  
            filterContext.Result = new HttpUnauthorizedResult();  
        } 
    }
}

