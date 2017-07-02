using System;
using ConsoleBanking.Classes;

namespace ConsoleBanking
{
    public class ConsoleProgram
    {
        private static BankUserOperationsHelper _bankUserOperationsHelper;

        private static TransactionHandler _transactionHandler;

        public static BankUserOperationsHelper BankUserOperationsHelper
            => _bankUserOperationsHelper ?? ( _bankUserOperationsHelper = new BankUserOperationsHelper() );

        public static TransactionHandler TransactionHandler
            => _transactionHandler ?? ( _transactionHandler = new TransactionHandler() );

        public static void Main()
        {
            BankUserOperationsHelper.GreetUserBeforeLogin();
            Console.ReadKey();
        }
    }
}