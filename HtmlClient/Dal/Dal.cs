using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using HtmlClient.Models;
using HtmlClient.Properties;

namespace HtmlClient.Dal
{
    public class Dal
    {
        private readonly string _hostName;

        private readonly string _sharedAppSettingsPath;
        private string _userDataXmlPath;

        public Dal( string sessionId = null )
        {
            //todo: implement a more rigorous session validation process
            _hostName = Settings.Default.LocalDomainWithPort;
            _sharedAppSettingsPath = _hostName + "Settings.xml";
        }

        public string UserDataXmlPath
            => _userDataXmlPath ?? ( _userDataXmlPath = Directory.GetFiles(
                   AppDomain.CurrentDomain.BaseDirectory + "\\Dal",
                   "DataStore*" )[0] );

        protected void UpdateLoginFromConsoleUrl()
        {
            using ( var streamWriter = new StreamWriter( _sharedAppSettingsPath ) )
            {
                var link = new XmlLink
                {
                    Name = "signin",
                    LinkValue = _hostName + "Account/LoginFromConsole/"
                };
                var xmlSerializer = new XmlSerializer( link.GetType() );
                xmlSerializer.Serialize( streamWriter, link );
            }
        }

        public virtual bool RegisterNewUser( UserViewModel model )
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
            var root = doc.DocumentElement; // <DataStore>  is root
            var userNodes = root?.SelectNodes( "//User" ); //all <User> Nodes

            if ( userNodes == null ) return false;

            foreach ( XmlNode userNode in userNodes )
            {
                var emailNode = userNode.FirstChild;
                if ( emailNode == null || emailNode.InnerText != model.Email ) continue;
                var passwordNode = emailNode.NextSibling;
                if ( passwordNode != null && passwordNode.InnerText == model.Password )
                    return true;
            }

            return false;
        }

        public bool EmailExists( string searchEmail )
        {
            var doc = new XmlDocument();
            doc.Load( UserDataXmlPath );
            var root = doc.DocumentElement; // <DataStore>  is root
            var userNodes = root?.SelectNodes( "//User" ); //all <User> Nodes

            if ( userNodes == null ) return false;

            foreach ( XmlNode userNode in userNodes )
            {
                var emailNode = userNode.FirstChild;
                if ( emailNode != null && emailNode.InnerText == searchEmail )
                    return true;
            }

            return false;
        }

        public bool SubmitTransaction( TransactionViewModel model )
        {
            var doc = new XmlDocument();
            doc.Load( UserDataXmlPath );
            var root = doc.DocumentElement;
            var userNodes = root?.SelectNodes( "//User" );

            if ( userNodes == null ) return false;

            foreach ( XmlNode userNode in userNodes )
            {
                if ( userNode.FirstChild.InnerText != model.UserEmail ) continue;

                //Create New User Node
                var transaction = doc.CreateElement( "Transaction" );

                // add date
                var transactionDate = doc.CreateElement( "TransactionDate" );
                transactionDate.InnerText = DateTime.UtcNow.ToString( CultureInfo.InvariantCulture );
                transaction.AppendChild( transactionDate );

                // delta amount
                var amount = doc.CreateElement( "Amount" );
                amount.InnerText = model.Amount.ToString( CultureInfo.InvariantCulture );
                transaction.AppendChild( amount );

                // add type
                var typeDeposit = doc.CreateElement( "IsDeposit" );
                typeDeposit.InnerText = model.IsDeposit.ToString();
                transaction.AppendChild( typeDeposit );

                var typeWithdraw = doc.CreateElement( "IsWithdraw" );
                typeWithdraw.InnerText = model.IsWithdraw.ToString();
                transaction.AppendChild( typeWithdraw );

                userNode?.AppendChild( transaction );
                doc.Save( UserDataXmlPath );

                return true;
            }

            return false;
        }

        public TransactionViewModel[] GetTransactionHistory( string userId )
        {
            var doc = new XmlDocument();
            doc.Load( UserDataXmlPath );
            var root = doc.DocumentElement;
            var transactionArray = new List<TransactionViewModel>();
            var transactionNodes = root?.SelectNodes( "//User[Email='"+userId+"']//Transaction" );

            if ( transactionNodes == null )
            {
                return transactionArray.ToArray();
            }

            foreach ( XmlNode transaction in transactionNodes )
            {
                var nextModel = new TransactionViewModel
                {
                    Date = transaction.SelectSingleNode( "TransactionDate" )?.InnerText,
                    Amount = double.Parse( transaction.SelectSingleNode( "Amount" )?.InnerText ),
                    IsDeposit = transaction.SelectSingleNode( "IsDeposit" )?.InnerText == "True",
                    IsWithdraw = transaction.SelectSingleNode( "IsWithdraw" )?.InnerText == "True"
                };
                transactionArray.Add( nextModel );
            }
            return transactionArray.ToArray();
        }
    }
}