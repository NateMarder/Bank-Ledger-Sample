using System;
using System.Linq;
using Libraries.Properties;
using Libraries.Enums;

namespace ConsoleApp.Classes
{
    public class ConsoleDialogHelper
    {
        public string GetUserEmailForLogin()
        {
            Console.WriteLine( Resources.GetUserEmail );
            // this 'input' pattern gets input while preventing exceptions
            var input = Console.ReadLine()?.Trim() ?? ""; 

            while ( !ValidateEmailForLogin( input ) )
            {
                Console.WriteLine( Resources.InvalidInput );
                input = Console.ReadLine()?.Trim() ?? "";
            }

            return input;
        }

        public string GetUserPasswordForLogin()
        {
            Console.WriteLine( Resources.GetUserPassword );
            var input = Console.ReadLine();
            input = input?.Trim() ?? "";

            while ( !ValidatePasswordForLogin( input ) )
            {
                Console.WriteLine( Resources.InvalidInput );
                input= Console.ReadLine();
                input = input?.Trim() ?? "";
            }

            return input;
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

        public double GetDepositAmount()
        {
            Console.WriteLine( Resources.GetAmountForDeposit );
            return GetValidDoubleFromUser();
        }

        public double GetWithdrawalAmount()
        {
            Console.WriteLine( Resources.GetAmountForWithdrawal );
            return GetValidDoubleFromUser();
        }

        private double GetValidDoubleFromUser()
        {
            var input = Console.ReadLine();
            while ( !ValidateDouble( input ) )
            {
                Console.WriteLine( Resources.InvalidNumericInput );
                input = Console.ReadLine();
            }
            return double.Parse( input );
        }

        private bool ValidatePasswordForLogin( string input )
        {
            return input.Length >= 4 && input.Length <= 20;
        }

        private bool ValidateEmailForLogin( string input )
        {
            return input.Length >= 4 && input.Length <= 20;
        }

        private bool ValidateUserOption( string input, char[] validInputs )
        {
            if ( input == null ) return false;
            if ( input.Length > 1 ) return false;
            if ( input.Length < 1 ) return false;

            var charInput = input.ToCharArray()[0];

            return validInputs.Any( @char => charInput == @char );
        }

        private bool ValidateDouble( string input )
        {
            if( double.TryParse( input, out double testNumber ) )
            {
                return true;
            }
            return false;
        }

    }
}