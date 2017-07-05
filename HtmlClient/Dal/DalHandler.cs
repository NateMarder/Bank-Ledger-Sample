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
        public string UserDataXmlPath 
            => _userDataXmlPath ?? (_userDataXmlPath = Directory.GetFiles( 
                    AppDomain.CurrentDomain.BaseDirectory + "\\Dal",
                    "UserDataStore*" )[0]);

        private string _sharedAppSettingsPath;
        private string _hostName;

        public DalHandler()
        {
            _hostName = Settings.Default.LocalDomainWithPort;
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
            doc.Load( UserDataXmlPath );

            //Create New User Node
            var userNode = doc.CreateElement( "User" );

            // add email
            var email = doc.CreateElement( "Email" );
            email.InnerText = model.Email;
            userNode.AppendChild( email );

            // add password
            var password = doc.CreateElement( "Password" );
            password.InnerText = model.Password;
            userNode.AppendChild( password );

            // add balance
            var balance = doc.CreateElement( "Balance" );
            balance.InnerText = "0";
            userNode.AppendChild( balance );

            // add login
            var lastLogin = doc.CreateElement( "LastLogin" );
            lastLogin.InnerText = DateTime.UtcNow.ToShortTimeString();
            userNode.AppendChild( lastLogin );

            doc.DocumentElement?.AppendChild( userNode );
            doc.Save( UserDataXmlPath );

            return true;
        }

        public bool VerifyPasswordEmailComboExists( UserViewModel model )
        {
            var doc = new XmlDocument();
            doc.Load( UserDataXmlPath );
            var root = doc.DocumentElement;  // <UserDataStore>  is root
            var userNodes = root?.SelectNodes( "//User" ); //all <User> Nodes

            if ( userNodes == null ) return false;

            foreach ( XmlNode userNode in userNodes )
            {
                var emailNode = userNode.FirstChild;
                if ( emailNode == null || emailNode.InnerText != model.Email ) continue;
                var passwordNode = emailNode.NextSibling;
                if ( passwordNode != null && passwordNode.InnerText == model.Password )
                {
                    return true;
                }
            }

            return false;
        }

        public bool EmailExists( string searchEmail )
        {
            var doc = new XmlDocument();
            doc.Load( UserDataXmlPath );
            var root = doc.DocumentElement;  // <UserDataStore>  is root
            var userNodes = root?.SelectNodes( "//User" ); //all <User> Nodes

            if ( userNodes == null ) return false;

            foreach ( XmlNode userNode in userNodes )
            {
                var emailNode = userNode.FirstChild;
                if ( emailNode != null && emailNode.InnerText == searchEmail )
                {
                    return true;
                }
            }

            return false;
        }

    }
}