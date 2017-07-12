using System.Web.Mvc;
using HtmlClient.Filters;

namespace HtmlClient
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add( new AuthorizeConsoleClient() );
        }
    }
}