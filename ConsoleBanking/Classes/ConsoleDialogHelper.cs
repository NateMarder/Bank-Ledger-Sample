using System;
using System.Collections.Generic;
using ConsoleBanking.Properties;

namespace ConsoleBanking.Classes
{
    public class ConsoleDialogHelper
    {
        public string GetUserEmailForLogin()
        {
            Console.WriteLine( Resources.NewUserGetEmail );
            var email = Console.ReadLine();

            while ( !ValidateInputForLogin( email ) )
            {
                Console.WriteLine( Resources.InvalidInput );
                email = Console.ReadLine().Trim();
            }

            return email;
        }

        public string GetUserPasswordForLogin()
        {
            Console.WriteLine( Resources.GetUserPassword );
            var password = Console.ReadLine();

            while ( !ValidateInputForLogin( password ) )
            {
                Console.WriteLine( Resources.InvalidInput );
                password = Console.ReadLine().Trim();
            }

            return password;
        }

        private bool ValidateInputForLogin( string input )
        {
            if ( input == null )
            {
                return false;
            }

            if (input.Length < 4 )
            {
                return false;
            }

            if (input.Length > 20 )
            {
                return false;
            }

            return true;
        }

    }
}