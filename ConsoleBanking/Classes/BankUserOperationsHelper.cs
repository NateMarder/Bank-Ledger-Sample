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

        public void GreetUserBeforeLogin()
        {
            Console.WriteLine( Resources.InitialGreeting );
            Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );

            var input = Console.ReadLine();
            while ( !ValidateOption( input, new[] {'1', '2', '3'} ) )
            {
                Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );
                input = Console.ReadLine();
            }

            switch ( (UserChoice) int.Parse( input ) )
            {
                case UserChoice.Login:
                    Login();
                    break;
                case UserChoice.RegisterNewAccount:
                    CreateUser();
                    break;
                case UserChoice.Logout:
                    Logout();
                    break;
            }
        }


        public async void Login( bool firstTime = true )
        {
            if ( firstTime )
            {
                Console.WriteLine( "\nWelcome to log in process" );
            }
            else
            {
                Console.WriteLine( "\nSorry, that didn't work shall we try again?" );
                Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );
                var input = Console.ReadLine();
                while ( !ValidateOption( input, new[] {'1', '2', '3'} ) )
                {
                    Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );
                    input = Console.ReadLine();
                }

                switch ( (UserChoice) int.Parse( input ) )
                {
                    case UserChoice.Login:
                        break;
                    case UserChoice.RegisterNewAccount:
                        CreateUser();
                        break;
                    case UserChoice.Logout:
                        Logout();
                        break;
                }
            }


            var email = DialogHelper.GetUserEmailForLogin();
            var password = DialogHelper.GetUserPasswordForLogin();

            var model = new LoginViewModel
            {
                Email = email,
                Password = password
            };

            var result = await RequestHelper.UserSignInGetModel( model );


            switch ( result.Status )
            {
                case SignInStatus.Failure:
                    Console.WriteLine( "\nResult: " + Enum.GetName( typeof( SignInStatus ), result.Status ) );
                    Login();
                    break;
                case SignInStatus.Success:
                    Console.WriteLine( "\nResult: " + Enum.GetName( typeof( SignInStatus ), result.Status ) );
                    ConsoleSession.Instance.Data["SessionGuid"] = result.Content.Replace( '"', ' ' ).Trim();
                    ConsoleSession.Instance.Data["Email"] = email;
                    ConsoleSession.Instance.Data["Password"] = password;
                    var guid = ConsoleSession.Instance.Data["SessionGuid"] ?? "Unknown Guid";
                    CommenceBanking();
                    break;
            }
        }


        public void CommenceBanking()
        {
            Console.WriteLine( "Welcome to the bank! Select one of the following options: " +
                           "\n   [4] to view transactions   " +
                           "\n   [5] to deposit funds   " +
                           "\n   [6] to withdraw funds" +
                           "\n   [3] to exit the bank");

            string userInput = Console.ReadLine();
            Console.ReadLine();
            Console.WriteLine( "you chose: " + userInput );

            try
            {
                switch ( (UserChoice) int.Parse( userInput ) )
                {
                    case UserChoice.ViewRecentTransactions:
                        ViewRecentTransactions();
                        break;
                    case UserChoice.DepositMoney:
                        Deposit();
                        break;
                    case UserChoice.WithdrawMoney:
                        Withdraw();
                        break;
                    case UserChoice.Logout:
                        Withdraw();
                        break;
                }
            }
            catch ( Exception ex )
            {

                Console.WriteLine( ex.Message );
                CommenceBanking();
            }
           

        }


        public void ViewRecentTransactions()
        {
            Console.WriteLine( "Welcome to the view transaction process..." );
            Console.ReadLine();
            CommenceBanking();

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


            //CommenceBanking();
        }

        public void Deposit()
        {
            Console.WriteLine( "Welcome to the deposit process..." );
            Console.ReadLine();
            CommenceBanking();
        }

        public void Withdraw()
        {
            Console.WriteLine( "Welcome to the withdraw process..." );
            Console.ReadLine();
            CommenceBanking();
        }

        public bool Logout()
        {
            Console.WriteLine( "Welcome to logout process..." );
            Console.ReadLine();
            return true;
        }

        public void CreateUser()
        {
            Console.WriteLine( "Welcome to user creation..." );
            Console.ReadLine();
        }

        private bool ValidateOption( string input, char[] validInputs )
        {
            Console.WriteLine( "about to validate input: " + input );
            Console.ReadLine();

            if ( input == null ) return false;
            if ( input.Length > 1 ) return false;
            if ( input.Length < 1 ) return false;

            return validInputs.Contains( input.ToCharArray()[0] );
        }
    }
}