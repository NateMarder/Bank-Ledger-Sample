using System.Web.Mvc;
using HtmlApp.Filters;

namespace HtmlApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add( new AuthorizeConsoleAppUser() );
        }
    }
}