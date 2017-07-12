namespace HtmlClient.Enums
{
    public enum UserChoice
    {
        Login = 1,
        RegisterNewAccount = 2,
        Logout = 3,
        ViewRecentTransactions = 4,
        DepositMoney = 5,
        WithdrawMoney = 6
    }

    // reflects owin identity sign-in enums
    public enum SignInStatus
    {
        Success = 0,
        LockedOut = 1,
        RequiresVerification = 2,
        Failure = 3
    }

    public enum TransactionType
    {
        GetHistory = 0,
        Deposit = 1,
        Withdraw = 2 
    }

}