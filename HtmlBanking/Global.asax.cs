using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml;
using HtmlBanking.Models;

namespace HtmlBanking
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure( WebApiConfig.Register );
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );
            UpdateSharedXmlFile();
        }

        protected void UpdateSharedXmlFile()
        {
            var projectPath = Path.GetDirectoryName( Server.MapPath( "~" ) );
            var solutionPath = Path.GetDirectoryName( projectPath );
            var xmlPath = solutionPath + "\\SharedResources.xml";
            var ulrPrefix = "http://localhost:59160/";

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