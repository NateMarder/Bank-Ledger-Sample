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
            return (int) status;
        }

        public async Task<SigninStatusModel> UserSignInGetModel( LoginViewModel model )
        {
            var signInStatus = new SigninStatusModel();
            var client = new HttpClient();

            var url = "http://localhost:54194/Login/LoginFromConsole/"
                      + "?Email=" + model.Email
                      + "&Password=" + model.Password;
            try
            {
                Console.WriteLine( "\n... attempting login at: " + url + "\n" );
                var response = await client.GetAsync( url );
                response.EnsureSuccessStatusCode();

                ConsoleSession.Instance.Data["SessionGuid"] = await response.Content.ReadAsStringAsync();
                signInStatus.Content = ConsoleSession.Instance.Data["SessionGuid"];
                signInStatus.Status = SignInStatus.Success;
            }
            catch ( Exception ex )
            {
                signInStatus.Content = ex.Message;
                signInStatus.Status = SignInStatus.Failure;
            }

            client.Dispose();
            Console.WriteLine( "Login Status: " + signInStatus.Status );
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


        public async Task<TransactionHistoryModel> GetTransactionHistory()
        {
            var transactionModel = new TransactionHistoryModel();
            var client = new HttpClient();

            var url = "http://localhost:54194/Transaction/ConsoleTransaction/"
                      + "?Type=" + TransactionType.GetHistory
                      + "&UserId=" + ConsoleSession.Instance.Data["UserId"]
                      + "&Amount=" + 0
                      + "&Token=" + ConsoleSession.Instance.Data["Token"];

            try
            {
                var task = await client.GetAsync( url );
                task.EnsureSuccessStatusCode();
                transactionModel.Content = await task.Content.ReadAsStringAsync();
                transactionModel.Status = task.StatusCode;
            }
            catch ( Exception ex )
            {
                transactionModel.Status = HttpStatusCode.BadRequest;
                transactionModel.Content = ex.Message;
            }

            client.Dispose();
            return transactionModel;
        }

        public async Task<bool> DepositFunds(double amount, bool isDeposit)
        {
            var client = new HttpClient();
            var type = isDeposit ? TransactionType.Deposit : TransactionType.Withdraw;

            var url = "http://localhost:54194/Transaction/ConsoleTransaction/"
                      + "?Type=" + type
                      + "&UserId=" + ConsoleSession.Instance.Data["UserId"]
                      + "&Amount=" + amount
                      + "&Token=" + ConsoleSession.Instance.Data["Token"];

            try
            {
                var task = await client.GetAsync( url );
                task.EnsureSuccessStatusCode();
                return task.IsSuccessStatusCode;
            }
            catch ( Exception ex )
            {
                //todo: start logging things!
            }

            client.Dispose();
            return false;
        }
    }
}