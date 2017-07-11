﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleBanking.Enums;
using ConsoleBanking.Properties;

namespace ConsoleBanking.Classes
{
    public class ConsoleDialogHelper
    {
        public string GetUserEmailForLogin()
        {
            Console.WriteLine( Resources.NewUserGetEmail );
            var email = Console.ReadLine();

            while ( !ValidateUserInputForLogin( email ) )
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

            while ( !ValidateUserInputForLogin( password ) )
            {
                Console.WriteLine( Resources.InvalidInput );
                password = Console.ReadLine().Trim();
            }

            return password;
        }

        public UserChoice GetUserChoiceForInitialOptions()
        {
            Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );

            var input = Console.ReadLine();
            while ( !ValidateUserOption( input, new[] {'1', '2', '3'} ) )
            {
                Console.WriteLine( Resources.UserOptionsForNonLoggedInUser );
                input = Console.ReadLine();
            }

            return (UserChoice) int.Parse( input );
        }

        public UserChoice GetUserChoiceForLoggedInOptions()
        {
            Console.WriteLine( Resources.UserOptionsForLoggedInUser );

            var input = Console.ReadLine();
            while ( !ValidateUserOption( input, new[] {'3', '4', '5', '6'} ) )
            {
                Console.WriteLine( Resources.UserOptionsForLoggedInUser );
                input = Console.ReadLine();
            }

            return (UserChoice) int.Parse( input );
        }


        public UserChoice GetUserChoiceForForUserAboutToExit()
        {
            Console.WriteLine( Resources.UserOptionsForUnsuccessfulUser );

            var input = Console.ReadLine();
            while ( !ValidateUserOption( input, new[] {'8', '3'} ) )
            {
                Console.WriteLine( Resources.UserOptionsForUnsuccessfulUser );
                input = Console.ReadLine();
            }

            return (UserChoice) int.Parse( input );
        }

        private bool ValidateUserInputForLogin( string input )
        {
            if ( input == null )
            {
                return false;
            }

            if ( input.Length < 4 )
            {
                return false;
            }

            if ( input.Length > 20 )
            {
                return false;
            }

            return true;
        }



        private bool ValidateUserOption( string input, char[] validInputs )
        {
            if ( input == null ) return false;
            if ( input.Length > 1 ) return false;
            if ( input.Length < 1 ) return false;

            var charInput = input.ToCharArray()[0];

            return validInputs.Any( @char => charInput == @char );
        }
    }
}