using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HtmlClient.Models;

namespace HtmlClient
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        protected void UpdateSharedXmlFile()
        {
            var projectPath = Path.GetDirectoryName( Server.MapPath( "~" ) );
            var solutionPath = Path.GetDirectoryName( projectPath );
            var xmlPath = solutionPath + "\\SharedResources.xml";
            const string ulrPrefix = "http://localhost:54194/";

            using ( var streamWriter = new StreamWriter( xmlPath ) )
            {
                var link = new XmlLink
                {
                    Name = "signin",
                    LinkValue = ulrPrefix + "Account/LoginFromConsole/"
                };
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer( link.GetType() );
                xmlSerializer.Serialize( streamWriter, link );
            }
        }

    }
}
