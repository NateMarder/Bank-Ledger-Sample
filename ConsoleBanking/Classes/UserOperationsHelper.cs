using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;


namespace ConsoleBanking.Classes
{
    public class UserOperationsHelper
    {
        private WebRequestHelper _requestHelper;
        public WebRequestHelper RequestHelper => _requestHelper ?? ( _requestHelper = new WebRequestHelper() );

        public UserOperationsHelper( WebRequestHelper requestHelper )
        {
            _requestHelper = requestHelper;
        }

        public UserOperationsHelper()
        {
        }

        public bool GreetUserBeforeLogin()
        {
            Console.WriteLine( Properties.Resources.AsciiArtDollarBill );
            Console.WriteLine( "\r\n" + Properties.Resources.InitialGreeting );
            Console.WriteLine( Properties.Resources.UserOptionsChoiceSet1 );

            var input = int.TryParse( Console.ReadLine(), out int userSelection );

            while ( !input || ( userSelection != 1 && userSelection != 2 ) )
            {
                Console.WriteLine( Properties.Resources.UserOptionsChoiceSet1 );
                input = int.TryParse( Console.ReadLine(), out userSelection );

                if ( userSelection == (int) UserChoice.SecretTest )
                {
                    Console.WriteLine( TestRequestAsync().Result );
                    break;
                }
            }

            var bankingAction = (UserChoice) userSelection;
            switch ( bankingAction )
            {
                case UserChoice.Login:
                    Login();
                    return true;
                case UserChoice.CreateAccount:
                    CreateUser();
                    return true;
            }

            return false;
        }


        public async void Login()
        {
            Console.WriteLine( "welcome to log in process" );

           
            //Task<SignInStatus> status =



        }

        public void Logout()
        {
        }

        public void CreateUser()
        {
            Console.WriteLine( "Welcome to account creation..." );
        }

        public async Task<string> TestRequestAsync()
        {
            return await RequestHelper.TestRequestAsync();
        }

        public string GreetBankCustomer()
        {
            return null;
        }
    }
}