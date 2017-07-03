using System;
using ConsoleBanking.Properties;

namespace ConsoleBanking.Classes
{
    public class ConsoleDialogHelper
    {


        public string GetUserEmail()
        {

            Console.WriteLine( Resources.NewUserGetEmail );
            var input = Console.ReadLine();

            while ( !ValidateEmail( input ) )
            {
                Console.WriteLine( Resources.InvalidInput );
                input = Console.ReadLine();
            }

            return input;
        }












        private bool ValidateEmail(string email = null)
        {
            return false;
        }
    }
}
