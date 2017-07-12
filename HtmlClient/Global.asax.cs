using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;

namespace HtmlClient
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UpdateSharedXmlFile();
        }


        protected void UpdateSharedXmlFile()
        {
            var projectPath = Path.GetDirectoryName( Server.MapPath( "~" ) );
            var solutionPath = Path.GetDirectoryName( projectPath );
            var xmlPath = solutionPath + "\\SharedResources\\Settings.xml";
            const string ulrPrefix = "http://localhost:54194/";

            var doc = new XmlDocument();
            doc.Load( xmlPath );

            //add link for console-signin
            var loginUrl = doc.CreateElement( "LoginUrl" );
            loginUrl.InnerText = ulrPrefix + "Login/LoginFromConsole/";
            doc.DocumentElement?.AppendChild( loginUrl );


            doc.Save( xmlPath );
        }

    }
}
