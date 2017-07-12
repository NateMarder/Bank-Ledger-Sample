using System;
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
            AllocateSessionTokens();
            var choice = UserChoice.KeepGoing;
            while ( choice == UserChoice.KeepGoing )
            {
                var task = HandleAsync( BankUserOperationsHelper.PresentInitialOptions() );
                if ( !task.Result )
                {
                    choice = DialogHelper.GetUserChoiceForForUserAboutToExit();
                }
            }

            return 0;
        }

        private static async Task<bool> HandleAsync( Task<bool> task )
        {
            await task;

            if ( task.IsFaulted )
            {
                return false;
            }

            Console.WriteLine( task.Result
                ? "\nMAIN: Program Ended Successfully"
                : "\nMAIN: Program ended Unsuccessfully" );

            return task.Result;
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