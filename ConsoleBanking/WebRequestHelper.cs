using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleBanking.Classes;
using ConsoleBanking.Enums;
using ConsoleBanking.Interfaces;
using ConsoleBanking.Models;
using ConsoleBanking.Enums;

namespace ConsoleBanking
{
    public class WebRequestHelper : IWebRequestHelper
    {
        private XDocument _sharedSettingsDocument;

        public XDocument SharedSettingsDocument
            => _sharedSettingsDocument
               ?? ( _sharedSettingsDocument = XDocument.Load( "../SharedResources/Settings.xml" ) );

        public WebRequestHelper()
        {
        }

        public async Task<int> UserSignIn( LoginViewModel model )
        {
            SignInStatus status;
            HttpClient client = new HttpClient();

            var url = "http://localhost:54194/Login/LoginFromConsole/"
                      + "?Email=" + model.Email
                      + "&Password=" + model.Password;
            try
            {

                var response = await client.GetAsync( url );
                response.EnsureSuccessStatusCode();
                status = SignInStatus.Success;

            }
            catch ( Exception )
            {
                status = SignInStatus.Failure;
            }

            client.Dispose();
            return (int)status;
        }

        public async Task<SigninStatusModel> UserSignInGetModel( LoginViewModel model )
        {

            var signInStatus = new SigninStatusModel();
            HttpClient client = new HttpClient();

            var url = "http://localhost:54194/Login/LoginFromConsole/"
                      + "?Email=" + model.Email
                      + "&Password=" + model.Password;
            try
            {
                var response = await client.GetAsync( url );
                response.EnsureSuccessStatusCode();
                signInStatus.Content =  await response.Content.ReadAsStringAsync();
                signInStatus.Status = SignInStatus.Success;
            }
            catch ( Exception ex)
            {
                signInStatus.Content = ex.Message;
                signInStatus.Status = SignInStatus.Failure;
            }

            client.Dispose();
            return signInStatus;
        }

        public async Task<int> RegisterNewUser( LoginViewModel model )
        {
            var client = new HttpClient();
            string stringResponse;

            var registerNewUserUrl = "http://localhost:54194/Register/Register";

            Console.WriteLine( "Attempting to login at: " + registerNewUserUrl );
            Console.WriteLine( "  email: " + model.Email );
            Console.WriteLine( "  password: " + model.Password );

            registerNewUserUrl += "?Email=" + model.Email + "&Password=" + model.Password + "&RememberMe=false";

            try
            {
                var response = await client.GetAsync( registerNewUserUrl );
                response.EnsureSuccessStatusCode();
                stringResponse = await response.Content.ReadAsStringAsync();
            }
            catch ( Exception )
            {
                stringResponse = Enum.GetName( typeof( SignInStatus ), SignInStatus.Failure );
            }

            client.Dispose();
            return int.Parse( stringResponse );
        }


        public async Task<TransactionHistoryModel> GetTransactionHistory( TransactionRequestModel model )
        {
            var requestResponse = new TransactionHistoryModel();
            var client = new HttpClient();
            var type = Enum.GetName( typeof( TransactionType ), model.Type );
            var amount = model.Amount ?? 0;

            var url = "http://localhost:54194/Transaction/ConsoleTransaction/"
                      + "?Type=" + type
                      + "&Amount=" + amount
                      + "&SessionGuid=" + ConsoleSession.Instance.Data["SessionGuid"];

            Console.WriteLine("attempting: "+url);

            try
            {
                
                var response = await client.GetAsync( url );
                response.EnsureSuccessStatusCode();
                requestResponse.Content =  await response.Content.ReadAsStringAsync();
                //history.Transactions = response.Headers["Data"];
                //history.Transactions = response.Headers["Data"];
            }
            catch ( Exception ex )
            {
                requestResponse.Status = HttpStatusCode.BadRequest;
                requestResponse.Content = ex.Message;
            }

            client.Dispose();
            return requestResponse;
        }
    }
}