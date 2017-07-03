using System;
using System.IO;
using System.Reflection;
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
            var rootPath = Path.GetDirectoryName( AppDomain.CurrentDomain.BaseDirectory );
            if ( rootPath != null )
            {
                rootPath = rootPath.Replace( "HtmlClient", "SharedResources" );
                _sharedAppSettingsPath = rootPath + "\\Settings.xml";
                _userDataXmlPath = rootPath + "\\UserDataStore.xml";
            }
            _hostName = Settings.Default.LocalDomainWithPort;
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

        public virtual bool RegisterNewUser( UserViewModel model )
        {
            try
            {
                using ( var streamWriter = new StreamWriter( _userDataXmlPath ) )
                {
                    var user = new UserViewModel()
                    {
                        Email = model.Email,
                        Password = model.Password
                    };
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer( user.GetType() );
                    xmlSerializer.Serialize( streamWriter, user );
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}