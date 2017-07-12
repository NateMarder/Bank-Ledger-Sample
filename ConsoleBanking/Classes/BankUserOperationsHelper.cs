using System;
using System.Threading.Tasks;
using ConsoleBanking.Enums;
using ConsoleBanking.Models;


namespace ConsoleBanking.Classes
{
    public class BankUserOperationsHelper
    {
        private WebRequestHelper _requestHelper;

        public WebRequestHelper RequestHelper
            => _requestHelper ?? ( _requestHelper = new WebRequestHelper() );

        public BankUserOperationsHelper( WebRequestHelper requestHelper )
        {
            _requestHelper = requestHelper;
        }

        private ConsoleDialogHelper _dialogHelper;
        public ConsoleDialogHelper DialogHelper => _dialogHelper ?? ( _dialogHelper = new ConsoleDialogHelper() );

        public BankUserOperationsHelper()
        {
        }

        public async Task<bool> PresentInitialOptions()
        {
            var choice = DialogHelper.GetUserChoiceForInitialOptions();

            switch ( choice )
            {
                case UserChoice.Login:
                    var result = Login();
                    await result;
                    if ( result.Result.Status == SignInStatus.Success )
                    {
                        return PresentOptionsForLoggedInUser();
                    }
                    return false;
                case UserChoice.RegisterNewAccount:
                    var registerSuccess = CreateUser();
                    await registerSuccess;
                    if ( registerSuccess.Result )
                    {
                        return PresentOptionsForLoggedInUser();
                    }
                    return false;
                case UserChoice.Logout:
                    return true;
            }

            return false;
        }


        public bool PresentOptionsForLoggedInUser()
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
                        return Withdraw();
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

        public bool Logout()
        {
            return true;
        }

        public async Task<SigninStatusModel> Login( bool firstTime = true )
        {

            var model = new LoginViewModel
            {
                Email = ConsoleSession.Instance.Data["UserId"] ?? DialogHelper.GetUserEmailForLogin(),
                Password = ConsoleSession.Instance.Data["Password"] ?? DialogHelper.GetUserPasswordForLogin()
            };

            var result = await RequestHelper.SignInUser( model );

            if ( result.Status == SignInStatus.Success )
            {
                ConsoleSession.Instance.Data["UserId"] =  model.Email;
                ConsoleSession.Instance.Data["Password"] =  model.Password;
                ConsoleSession.Instance.Data["SessionID"] = result.Content;
            }

            return result;
        }

        public async Task<bool> CreateUser()
        {
            var model = new RegisterViewModel()
            {
                Email = DialogHelper.GetUserEmailForLogin(),
                Password = DialogHelper.GetUserPasswordForLogin()
            };

            var result =  RequestHelper.RegisterNewUser( model ).Result;
            if ( result.Status == RegistrationStatus.Success )
            {
                ConsoleSession.Instance.Data["UserId"] = model.Email;
                ConsoleSession.Instance.Data["Password"] = model.Password;
                var loginSuccess = Login();
                await loginSuccess;
                return loginSuccess.Result.Status == SignInStatus.Success;
            }

            return false;
        }
    }
}