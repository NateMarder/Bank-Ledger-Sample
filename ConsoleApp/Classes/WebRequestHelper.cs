using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Libraries.Enums;
using Libraries.Models;
using Libraries.Properties;
using Libraries.SessionManagement;

namespace ConsoleApp.Classes
{
    public interface IWebRequestHelper
    {
        Task<SigninStatusModel> SignInUser( LoginViewModel model );
        Task<RegistrationStatusModel> RegisterNewUser( RegisterViewModel model );
        Task<bool> TenderTransaction( double amount, bool isDeposit );
    }

    public class WebRequestHelper : IWebRequestHelper
    {
        public async Task<SigninStatusModel> SignInUser( LoginViewModel model )
        {
            var signInStatus = new SigninStatusModel();
            var client = new HttpClient();
            SetConsoleToken();
            var url = $"http://localhost:54194/Login/LoginFromConsole/" +
                      $"?Email={model.Email}" +
                      $"&Password={model.Password}";
            try
            {
                var response = await client.GetAsync( url );
                response.EnsureSuccessStatusCode();
                ConsoleSession.Instance.Data["SessionToken"] = await response.Content.ReadAsStringAsync();
                signInStatus.Content = ConsoleSession.Instance.Data["SessionToken"];
                signInStatus.Status = SignInStatus.Success;
            }
            catch ( Exception ex )
            {
                signInStatus.Content = ex.Message;
                signInStatus.Status = SignInStatus.Failure;
            }

            client.Dispose();
            Console.WriteLine( string.Format(Resources.LoginStatusMessage, signInStatus.Status)  );
            return signInStatus;
        }

        public async Task<RegistrationStatusModel> RegisterNewUser( RegisterViewModel model )
        {
            var registrationStatus = new RegistrationStatusModel();
            var client = new HttpClient();

            var url = $"http://localhost:54194/Register/Register/?Email={model.Email}&Password={model.Password}";
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
            Console.WriteLine( string.Format(Resources.RegistrationStatusMessage, registrationStatus.Status)  );
            return registrationStatus;
        }

        public async Task<TransactionHistoryModel> GetTransactionHistory()
        {
            var transactionModel = new TransactionHistoryModel();
            var client = new HttpClient();
            var userId = ConsoleSession.Instance.Data["UserId"];
            var sessionId = ConsoleSession.Instance.Data["SessionToken"];

            var url = "http://localhost:54194/Transaction/ConsoleTransaction/" +
                $"?Type={TransactionType.GetHistory}&UserId={userId}&Amount={0}&SessionId={sessionId}";
         
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

        public async Task<bool> TenderTransaction( double amount, bool isDeposit )
        {
            var client = new HttpClient();
            var userId = ConsoleSession.Instance.Data["UserId"];
            var sessionId = ConsoleSession.Instance.Data["SessionToken"];
            var type = isDeposit ? TransactionType.Deposit : TransactionType.Withdraw;

            var url = "http://localhost:54194/Transaction/ConsoleTransaction/" +
                      $"?Type={type}&UserId={userId}&Amount={amount}&SessionId={sessionId}";

            try
            {
                var task = await client.GetAsync( url );
                task.EnsureSuccessStatusCode();
                return task.IsSuccessStatusCode;
            }
            catch ( Exception )
            {
                //todo: start logging things!
            }

            client.Dispose();
            return false;
        }

        private void SetConsoleToken()
        {
            var ConsoleToken = Guid
                .NewGuid()
                .ToString( "N" )
                .Replace( '"', ' ' )
                .Trim();

            ConsoleSession.Instance.Data["ConsoleToken"] = ConsoleToken;
        }
    }
}