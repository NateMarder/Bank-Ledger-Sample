using System;
using System.Linq;
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
                    var result = LoginRequest();
                    await result;
                    if ( result.Result.Status == SignInStatus.Success )
                    {
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


        public async Task<SigninStatusModel> LoginRequest( bool firstTime = true )
        {
            Console.WriteLine( "\nWelcome to log in process" );
            var email = DialogHelper.GetUserEmailForLogin();
            var password = DialogHelper.GetUserPasswordForLogin();

            var model = new LoginViewModel
            {
                Email = email,
                Password = password
            };

            var result = await RequestHelper.UserSignInGetModel( model );
            
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
                        return ViewRecentTransactions();
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


        public bool ViewRecentTransactions()
        {
            Console.WriteLine( "Welcome to the view transaction process..." );
            Console.ReadLine();
            return true;

            ////TransactionRequestModel model
            //var model = new TransactionRequestModel
            //{
            //    Type = TransactionType.GetHistory,
            //    Amount = null
            //};

            //try
            //{
            //    Console.WriteLine( "checkpoint delta" );
            //    Console.ReadLine();
            //    var result = await RequestHelper.GetTransactionHistory( model );
            //    foreach ( var next in result.Transactions )
            //    {
            //        Console.WriteLine( next );
            //    }
            //    Console.ReadLine();
            //}
            //catch ( Exception ex )
            //{
            //    Console.WriteLine( ex.Message );
            //    Console.ReadLine();
            //}


            //PresentOptionsForLoggedInUser();
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