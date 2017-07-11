using System;
using System.Linq;
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

        public bool GreetUserBeforeLogin()
        {
            Console.WriteLine( Resources.InitialGreeting );
            Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );

            var input = Console.ReadLine();

            while ( !ValidateOption( input, new[] {'1', '2', '3'} ) )
            {
                Console.WriteLine( Resources.UserOptionsForLoggedInUser );
                input = Console.ReadLine().Trim();
            }

            switch ( (UserChoice) int.Parse(input) )
            {
                case UserChoice.Login:
                    Login();
                    return true;
                case UserChoice.RegisterNewAccount:
                    CreateUser();
                    return true;
                case UserChoice.Logout:
                    Logout();
                    return true;
            }

            return false;
        }


        public async void Login(bool firstTime = true)
        {
            if ( firstTime )
            {
                Console.WriteLine( "\nWelcome to log in process" );
            }
            else
            {
                Console.WriteLine( "\nSorry, that didn't work shall we try again?" );
                Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );
                var input = int.TryParse( Console.ReadLine(), out int userSelection );
                while ( !input || userSelection != 1 && userSelection != 2 && userSelection != 3)
                {
                    Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );
                    input = int.TryParse( Console.ReadLine(), out userSelection );
                }

                switch ( (UserChoice) userSelection )
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
                   Console.WriteLine( "\nResult: " + Enum.GetName( typeof(SignInStatus), result.Status ) );
                   Login();
                   break;
               case SignInStatus.Success:
                   Console.WriteLine( "\nResult: " + Enum.GetName( typeof(SignInStatus), result.Status ) );
                   ConsoleSession.Instance.Data["SessionGuid"] = result.Content.Replace( '"', ' ' ).Trim();
                   ConsoleSession.Instance.Data["Email"] = email;
                   ConsoleSession.Instance.Data["Password"] = password;
                   var guid = ConsoleSession.Instance.Data["SessionGuid"] ?? "Unknown Guid";
                   commenceBanking();
                   break;
            }
        }


        public void commenceBanking()
        {
            var bankingAction = UserChoice.Undefined;
            while ( bankingAction != UserChoice.Logout )
            {
                Console.WriteLine( Resources.UserOptionsForLoggedInUser );
                var input = Console.ReadLine();

                while ( !ValidateOption( input, new[] {'4', '5', '6', '3'} ) )
                {
                    Console.WriteLine( Resources.UserOptionsForLoggedInUser );
                    input = Console.ReadLine().Trim();
                }

                bankingAction = (UserChoice) int.Parse(input);
                switch ( bankingAction )
                {
                    case UserChoice.ViewRecentTransactions:
                        Console.WriteLine( "checkpoint charlie" );
                        viewRecentTransactions();
                        break;
                    case UserChoice.DepositMoney:
                        Deposit();
                        break;
                    case UserChoice.WithdrawMoney:
                        Withdraw();
                        break;
                }
                break;
            }

            Logout();
        }

        private bool ValidateOption( string input, char[] validInputs )
        {
            if ( input == null ) return false;
            if ( input.Length > 1 ) return false;
            if ( input.Length == 0 ) return false;

            var charInput = input.ToCharArray()[0];
            return char.IsNumber( charInput ) && validInputs.Contains( charInput );
        }

        public async void viewRecentTransactions()
        {
            //TransactionRequestModel model
            var model = new TransactionRequestModel
            {
                Type = TransactionType.GetHistory,
                Amount = null
            };

            try
            {
                Console.WriteLine( "checkpoint delta" );
                Console.Read();
                var result = await RequestHelper.GetTransactionHistory( model );
                foreach ( var next in result.Transactions )
                {
                    Console.WriteLine(next);
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine(ex.Message);
            }





            commenceBanking();

        }

        public void Deposit()
        {
            
        }

        public void Withdraw()
        {


            //add param: SessionGuid=Session.Instance["SessionGuid"];
        }

        public void presentBankingOptions()
        {
            
        }

        public void Logout()
        {
            Console.WriteLine( "Welcome to logout process..." );
            Console.ReadLine();
        }

        public void CreateUser()
        {
            Console.WriteLine( "Welcome to user creation..." );
            Console.ReadLine();
        }

        public string GreetBankCustomer()
        {
            return null;
        }
    }
}