using System;
using ConsoleBanking.Classes;

namespace ConsoleBanking
{
    public class ConsoleProgram
    {
        private static UserOperationsHelper _userOperationsHelper;
        public static UserOperationsHelper UserOperationsHelper
            => _userOperationsHelper ?? ( _userOperationsHelper = new UserOperationsHelper() );

        private static TransactionHandler _transactionHandler;
        public static TransactionHandler TransactionHandler
            => _transactionHandler ?? ( _transactionHandler = new TransactionHandler() );

        public static void Main()
        {
            UserOperationsHelper.GreetUserBeforeLogin();
            Console.ReadKey();
        }
    }
}