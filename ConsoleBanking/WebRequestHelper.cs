using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleBanking.Classes;
using ConsoleBanking.Enums;
using ConsoleBanking.Models;

namespace ConsoleBanking
{

    public interface IWebRequestHelper
    {
        Task<int> UserSignIn( LoginViewModel model );
        Task<int> RegisterNewUser( LoginViewModel model );
    }


    public class WebRequestHelper : IWebRequestHelper
    {
        private XDocument _sharedSettingsDocument;

        public XDocument SharedSettingsDocument
            => _sharedSettingsDocument
               ?? ( _sharedSettingsDocument = XDocument.Load( "../SharedResources/Settings.xml" ) );

        public async Task<int> UserSignIn( LoginViewModel model )
        {
            SignInStatus status;
            var client = new HttpClient();

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

        public async Task<SigninStatusModel> AttemptUserSignIn( LoginViewModel model )
        {
            var signInStatus = new SigninStatusModel();
            var client = new HttpClient();

            var url = "http://localhost:54194/Login/LoginFromConsole/"
                      + "?Email=" + model.Email
                      + "&Password=" + model.Password;
            try
            {
                var response = await client.GetAsync( url );
                response.EnsureSuccessStatusCode();
                ConsoleSession.Instance.Data["SessionID"] = await response.Content.ReadAsStringAsync();
                signInStatus.Content = ConsoleSession.Instance.Data["SessionID"];
                signInStatus.Status = SignInStatus.Success;
            }
            catch ( Exception ex )
            {
                signInStatus.Content = ex.Message;
                signInStatus.Status = SignInStatus.Failure;
            }

            client.Dispose();
            Console.WriteLine( "Login Success Status: " + signInStatus.Status );
            return signInStatus;
        }

        public async Task<RegistrationStatusModel> AttemptUserRegistration( RegisterViewModel model )
        {
            var registrationStatus = new RegistrationStatusModel();
            var client = new HttpClient();

            var url = "http://localhost:54194/Register/Register/"
                      + "?Email=" + model.Email
                      + "&Password=" + model.Password;
            try
            {
                var response = await client.GetAsync( url );
                registrationStatus.Content = response.Content.ToString();
                registrationStatus.Status = RegistrationStatus.Success;
            }
            catch ( Exception ex )
            {
                registrationStatus.Content = ex.Message;
                registrationStatus.Status = RegistrationStatus.Failure;
            }

            client.Dispose();
            Console.WriteLine( "Registration Success Status: " + registrationStatus.Status );
            return registrationStatus;
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
                      + "&Token=" + ConsoleSession.Instance.Data["SessionID"];

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

        public async Task<bool> AttemptTransaction( double amount, bool isDeposit )
        {
            var client = new HttpClient();
            var type = isDeposit ? TransactionType.Deposit : TransactionType.Withdraw;

            var url = "http://localhost:54194/Transaction/ConsoleTransaction/"
                      + "?Type=" + type
                      + "&UserId=" + ConsoleSession.Instance.Data["UserId"]
                      + "&Amount=" + amount
                      + "&Token=" + ConsoleSession.Instance.Data["SessionID"];

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