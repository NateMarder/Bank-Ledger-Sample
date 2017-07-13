using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.Classes;
using Libraries.Properties;
using Libraries.Enums;

namespace ConsoleApp
{
    class Program
    {
        private static ConsoleDialogHelper _dialogHelper;
        public static ConsoleDialogHelper DialogHelper 
            => _dialogHelper ?? ( _dialogHelper = new ConsoleDialogHelper() );

        private static BankUserOperationsHelper _operationsHelper;
        public static BankUserOperationsHelper OperationsHelper 
            => _operationsHelper ?? ( _operationsHelper = new BankUserOperationsHelper() );

        static void Main( string[] args )
        {
            Console.WriteLine( Resources.InitialGreeting );
            AllocateSessionTokens();
            var userChoice = UserChoice.KeepGoing;
            while ( userChoice == UserChoice.KeepGoing )
            {
                var task = OperationsHelper.PresentInitialOptions();
                userChoice = task.Result 
                    ? DialogHelper.GetUserChoiceForForUserAboutToExit() 
                    : UserChoice.Logout;
            }

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
