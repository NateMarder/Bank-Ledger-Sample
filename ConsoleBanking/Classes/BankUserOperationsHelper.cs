using System;
using System.Threading.Tasks;
using ConsoleBanking.Models;
using ConsoleBanking.Properties;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;


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

        public BankUserOperationsHelper()
        {
        }

        public bool GreetUserBeforeLogin()
        {
            Console.WriteLine( Resources.AsciiArtDollarBill );
            Console.WriteLine( "\r\n" + Resources.InitialGreeting );
            Console.WriteLine( Resources.UserOptionsChoiceSet1 );

            var input = int.TryParse( Console.ReadLine(), out int userSelection );

            while ( !input || userSelection != 1 && userSelection != 2 )
            {
                Console.WriteLine( Resources.UserOptionsChoiceSet1 );
                input = int.TryParse( Console.ReadLine(), out userSelection );
            }

            var bankingAction = (UserChoice) userSelection;
            switch ( bankingAction )
            {
                case UserChoice.Login:
                    Login();
                    return true;
                case UserChoice.RegisterNewAccount:
                    CreateUser();
                    return true;
            }

            return false;
        }


        public async void Login()
        {
            Console.WriteLine( "welcome to log in process" );

            var model = new LoginFromConsoleViewModel
            {
                Email = "admin@email.com",
                Password = "HireNate1!",
                RememberMe = false
            };

            var result = await RequestHelper.UserSignIn( model );
            Console.WriteLine( "Result: " + Enum.GetName( typeof(SignInStatus), result ) );
        }

        public void Logout()
        {
        }

        public void CreateUser()
        {
            Console.WriteLine( "Welcome to account creation..." );
        }

        public string GreetBankCustomer()
        {
            return null;
        }
    }
}