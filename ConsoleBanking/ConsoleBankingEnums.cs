namespace ConsoleBanking
{
    public enum UserChoice
    {
        Login = 1,
        RegisterNewAccount = 2,
        Logout = 3,
        CheckBalance = 4,
        AddFunds = 5,
        RemoveFunds = 6
    }

    // reflects owin identity sign-in enums
    public enum SignInStatus
    {
        Success = 0,
        LockedOut = 1,
        RequiresVerification = 2,
        Failure = 3
    }
}