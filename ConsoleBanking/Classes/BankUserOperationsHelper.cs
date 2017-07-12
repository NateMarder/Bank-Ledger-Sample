using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ConsoleBanking.Enums;
using ConsoleBanking.Models;
using ConsoleBanking.Properties;


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
                        ConsoleSession.Instance.Data["SessionID"] = result.Result.Content;
                        
                        return PresentOptionsForLoggedInUser();
                    }
                    return false;
                case UserChoice.RegisterNewAccount:
                    return CreateUser();
                case UserChoice.Logout:
                    return Logout();
            }

            return false;
        }


        public async Task<SigninStatusModel> Login( bool firstTime = true )
        {

            var model = new LoginViewModel
            {
                Email = DialogHelper.GetUserEmailForLogin(),
                Password = DialogHelper.GetUserPasswordForLogin()
            };

            var result = await RequestHelper.UserSignInGetModel( model );

            if ( result.Status == SignInStatus.Success )
            {
                ConsoleSession.Instance.Data["UserId"] = model.Email;
                ConsoleSession.Instance.Data["Password"] = model.Password;
                ConsoleSession.Instance.Data["Token"] = result.Content;
            }
            
            return result;
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


        public bool ViewRecentTransactionsAsync()
        {
            try
            {
                var result = RequestHelper.GetTransactionHistory();
                Console.WriteLine(result.Result.Content);
                return PresentOptionsForLoggedInUser();
            }
            catch( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return false;
            }
        }

        public bool Deposit()
        {
            Console.WriteLine( "Welcome to the deposit process..." );
            Console.ReadLine();
            return true;
        }

        public bool Withdraw()
        {
            Console.WriteLine( "Welcome to the withdraw process..." );
            Console.ReadLine();
            return true;
        }

        public bool Logout()
        {
            Console.WriteLine( "Welcome to logout process..." );
            Console.ReadLine();
            return true;
        }

        public bool CreateUser()
        {
            Console.WriteLine( "Welcome to user creation..." );
            Console.ReadLine();
            return true;
        }

        private bool ValidateOption( string input, char[] validInputs )
        {
            if ( input == null ) return false;
            if ( input.Length > 1 ) return false;
            if ( input.Length < 1 ) return false;

            return validInputs.Contains( input.ToCharArray()[0] );
        }
    }
}