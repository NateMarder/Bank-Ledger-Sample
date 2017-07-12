﻿using System;
using System.Threading.Tasks;
using ConsoleBanking.Classes;
using ConsoleBanking.Enums;
using ConsoleBanking.Properties;

namespace ConsoleBanking
{
    public class ConsoleProgram
    {
        private static BankUserOperationsHelper _bankUserOperationsHelper;
        public static BankUserOperationsHelper BankUserOperationsHelper
            => _bankUserOperationsHelper ?? ( _bankUserOperationsHelper = new BankUserOperationsHelper() );

        private static ConsoleDialogHelper _dialogHelper;

        public static ConsoleDialogHelper DialogHelper =>
            _dialogHelper ?? ( _dialogHelper = new ConsoleDialogHelper() );

        public static int Main()
        {
            Console.WriteLine( Resources.InitialGreeting );
            AllocateKeysForSessionDictionary();
            var choice = UserChoice.KeepGoing;

            Task<bool> result = null;

            while ( choice == UserChoice.KeepGoing )
            {
                result = HandleAsync( BankUserOperationsHelper.PresentInitialOptions() );

                if ( !result.Result )
                {
                    choice = DialogHelper.GetUserChoiceForForUserAboutToExit();
                }
            }

            Console.ReadLine();
            return result != null && result.Result ? 0 : 1;
        }

        private static void AllocateKeysForSessionDictionary()
        {
            ConsoleSession.Instance.Data["UserId"] = null;
            ConsoleSession.Instance.Data["Password"] = null;
            ConsoleSession.Instance.Data["SessionID"] = null;
            ConsoleSession.Instance.Data["Balance"] = null;
        }

        private static async Task<bool> HandleAsync( Task<bool> status )
        {
            await status;

            if ( status.IsFaulted )
            {
                Console.WriteLine( "\n MAIN: Unhandled exception happened" );
                return false;
            }

            var successMessage = status.Result
                ? "MAIN: Program Ended Successfully"
                : "MAIN: Program ended Unsuccessfully";

            Console.WriteLine( "\n MAIN: Program Success:: " + successMessage );
            return status.Result;
        }
    }
}