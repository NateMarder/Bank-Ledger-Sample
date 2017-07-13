using System;
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
            AllocateSessionTokens();
            var userChoice = UserChoice.KeepGoing;
            while ( userChoice == UserChoice.KeepGoing )
            {
                var task = BankUserOperationsHelper.PresentInitialOptions();
                userChoice = task.Result 
                    ? DialogHelper.GetUserChoiceForForUserAboutToExit() 
                    : UserChoice.Logout;
            }

            return 0;
        }

        private static void AllocateSessionTokens()
        {
            ConsoleSession.Instance.Data["UserId"] = null;
            ConsoleSession.Instance.Data["Password"] = null;
            ConsoleSession.Instance.Data["SessionID"] = null;
            ConsoleSession.Instance.Data["Balance"] = null;
            ConsoleSession.Instance.Data["ConsoleToken"] = null;
        }
    }
}