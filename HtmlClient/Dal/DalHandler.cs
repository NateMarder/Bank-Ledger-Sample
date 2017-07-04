using System;
using System.IO;
using System.Linq;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using HtmlClient.Models;
using Settings = HtmlClient.Properties.Settings;

namespace HtmlClient.Dal
{
    public class DalHandler
    {
        private string _userDataXmlPath;
        private string _sharedAppSettingsPath;
        private string _hostName;

        public DalHandler()
        {
            _hostName = Settings.Default.LocalDomainWithPort;
            _userDataXmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\Dal";
            _sharedAppSettingsPath = _hostName + "Settings.xml";
        }

        protected void UpdateLoginFromConsoleUrl()
        {
            using ( var streamWriter = new StreamWriter( _sharedAppSettingsPath ) )
            {
                var link = new XmlLink
                {
                    Name = "signin",
                    LinkValue = _hostName + "Account/LoginFromConsole/"
                };
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer( link.GetType() );
                xmlSerializer.Serialize( streamWriter, link );
            }
        }

        public virtual bool RegisterNewUser( RegisterViewModel model )
        {
            var doc = new XmlDocument();

            var localFiles = Directory.GetFiles( _userDataXmlPath, "UserDataStore*" );
            var xmlFile = localFiles[0];
            doc.Load( xmlFile );

            //Create New User Node
            var userNode = doc.CreateElement( "User" );

            // last login
            var lastLogin = doc.CreateElement( "LastLogin" );
            lastLogin.InnerText = DateTime.UtcNow.ToShortDateString();
            userNode.AppendChild( lastLogin );

            // balance (starts at zero)
            var balance = doc.CreateElement( "Balance" );
            balance.InnerText = "0";
            userNode.AppendChild( balance );

            var emailAttribute = doc.CreateAttribute( "email" );
            emailAttribute.Value = model.Email;
            userNode.Attributes.SetNamedItem( emailAttribute );

            var passwordAttribute = doc.CreateAttribute( "pw" );
            passwordAttribute.Value = model.Password;
            userNode.Attributes.SetNamedItem( passwordAttribute );

            doc.DocumentElement?.AppendChild( userNode );
            doc.Save( xmlFile );

            return true;
        }

        public bool VerifyPasswordEmailComboExists( UserViewModel model )
        {
            var doc = new XmlDocument();
            doc.Load( _userDataXmlPath );
            var root = doc.DocumentElement;
            var nodes = root?.SelectNodes( "UserViewModel" );

            if ( nodes == null ) return false;

            return (
                from XmlNode node in nodes
                let email = node.SelectSingleNode( "Email" )
                let password = node.SelectSingleNode( "Password" )
                where email != null && password != null
                where email.Value.ToString() == model.Email
                      && password.Value.ToString() == model.Password
                select email
            ).Any();
        }

        public bool EmailExists( string searchEmail )
        {
            var doc = new XmlDocument();
            doc.Load( _userDataXmlPath );
            var root = doc.DocumentElement;
            var nodes = root?.SelectNodes( "UserViewModel" ); // You can also use XPath here

            if ( nodes == null ) return false;

            return (
                from XmlNode node in nodes
                let email = node.SelectSingleNode( "Email" )
                where email.Value.ToString() == searchEmail
                select email
            ).Any();
        }
    }
}