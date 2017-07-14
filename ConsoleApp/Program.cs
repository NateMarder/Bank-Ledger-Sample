using System;
using ConsoleApp.Classes;
using Libraries.Properties;
using Libraries.Enums;
using Libraries.SessionManagement;

namespace ConsoleApp
{
    internal class Program
    {
        private static UserInputHelper _dialogHelper;
        public static UserInputHelper DialogHelper 
            => _dialogHelper ?? ( _dialogHelper = new UserInputHelper() );

        private static UserOperationsHelper _operationsHelper;
        public static UserOperationsHelper OperationsHelper 
            => _operationsHelper ?? ( _operationsHelper = new UserOperationsHelper() );

        static void Main()
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
            // todo: just check for nulls before attempting to read values, this is just silly
            ConsoleSession.Instance.Data["UserId"] = null;
            ConsoleSession.Instance.Data["Password"] = null;
            ConsoleSession.Instance.Data["SessionID"] = null;
            ConsoleSession.Instance.Data["Balance"] = null;
            ConsoleSession.Instance.Data["ConsoleToken"] = null;
        }
    }
}
