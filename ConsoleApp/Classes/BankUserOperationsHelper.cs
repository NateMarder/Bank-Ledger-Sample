using System;
using System.Threading.Tasks;
using Libraries.Enums;
using Libraries.Models;
using Libraries.SessionManagement;


namespace ConsoleApp.Classes
{
    public class BankUserOperationsHelper
    {

        private WebRequestHelper _requestHelper;
        public WebRequestHelper RequestHelper
            => _requestHelper ?? ( _requestHelper = new WebRequestHelper() );

        private ConsoleDialogHelper _dialogHelper;
        public ConsoleDialogHelper DialogHelper 
            => _dialogHelper ?? ( _dialogHelper = new ConsoleDialogHelper() );

        public BankUserOperationsHelper( WebRequestHelper requestHelper )
        {
            _requestHelper = requestHelper;
        }

        public BankUserOperationsHelper()
        {
        }

        public async Task<bool> PresentInitialOptions()
        {
            Libraries.Enums.UserChoice choice = DialogHelper.GetUserChoiceForInitialOptions();

            switch ( choice )
            {
                case UserChoice.Login:
                    var loginResult = Login();
                    await loginResult;
                    if ( loginResult.Result.Status == SignInStatus.Success )
                    {
                        return PresentOptionsForLoggedInUser();
                    }
                    return true;

                case UserChoice.RegisterNewAccount:
                    var registerResult = CreateUser();
                    await registerResult;
                    if ( registerResult.Result )
                    {
                        return await PresentInitialOptions();
                    }
                    return true;

                case UserChoice.Logout:
                    break;
            }

            return false;
        }

        private bool PresentOptionsForLoggedInUser()
        {
            var choice = DialogHelper.GetUserChoiceForLoggedInOptions();

            try
            {
                switch ( choice )
                {
                    case UserChoice.ViewRecentTransactions:
                        return ViewRecentTransactionsAsync();
                    case UserChoice.DepositMoney:
                        return Deposit();
                    case UserChoice.WithdrawMoney:
                        return Withdraw();
                    case UserChoice.Logout:
                        return true;
                    case UserChoice.Login:
                        return PresentOptionsForLoggedInUser();
                    case UserChoice.RegisterNewAccount:
                        return PresentOptionsForLoggedInUser();
                    case UserChoice.Undefined:
                        return PresentOptionsForLoggedInUser();
                    default:
                        return false;
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return false;
            }
        }

        private bool ViewRecentTransactionsAsync()
        {
            try
            {
                var result = RequestHelper.GetTransactionHistory();
                Console.WriteLine( result.Result.Content );
                return PresentOptionsForLoggedInUser();
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return false;
            }
        }

        private bool Deposit()
        {
            var amount = DialogHelper.GetDepositAmount();
            try
            {
                var result = RequestHelper.TenderTransaction( amount, true );
                var displayMessage = result.Result
                    ? "Your deposit of $" + amount + " was tendered succussfully"
                    : "Your deposit of $" + amount + " was unsuccessful";

                Console.WriteLine( displayMessage );
                return PresentOptionsForLoggedInUser();
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return false;
            }
        }

        private bool Withdraw()
        {
            var amount = DialogHelper.GetWithdrawalAmount();
            try
            {
                var result = RequestHelper.TenderTransaction( amount, false );
                var displayMessage = result.Result
                    ? "Your withdrawal of $" + amount + " was tendered succussfully"
                    : "Your withdrawal of $" + amount + " was unsuccessful";

                Console.WriteLine( displayMessage );
                return PresentOptionsForLoggedInUser();
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return false;
            }
        }

        private async Task<SigninStatusModel> Login()
        {
            var model = new LoginViewModel
            {
                Email = DialogHelper.GetUserEmailForLogin(),
                Password = DialogHelper.GetUserPasswordForLogin()
            };

            var result = await RequestHelper.SignInUser( model );
            if ( result.Status == SignInStatus.Success )
            {
                ConsoleSession.Instance.Data["UserId"] = model.Email;
                ConsoleSession.Instance.Data["Password"] = model.Password;
                ConsoleSession.Instance.Data["SessionID"] = result.Content;
            }

            return result;
        }

        private async Task<bool> CreateUser()
        {
            var model = new RegisterViewModel
            {
                Email = DialogHelper.GetUserEmailForLogin(),
                Password = DialogHelper.GetUserPasswordForLogin()
            };

            var task = RequestHelper.RegisterNewUser( model );
            await task;

            if ( task.Result.Status == RegistrationStatus.Success )
            {
                ConsoleSession.Instance.Data["UserId"] = model.Email;
                ConsoleSession.Instance.Data["Password"] = model.Password;
                return true;
            }

            return false;
        }
    }
}