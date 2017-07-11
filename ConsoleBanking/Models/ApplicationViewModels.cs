using System.Net;
using ConsoleBanking.Enums;

namespace ConsoleBanking.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterNewUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string HomeTown { get; set; }
    }

    public class SigninStatusModel
    {
        public string Content { get; set; }
        public SignInStatus Status { get; set; }
    }

    public class TransactionRequestModel
    {
        public TransactionType Type { get; set; }
        public double? Amount { get; set; }
    }

    public class TransactionHistoryModel
    {
        public string[] Transactions { get; set; }
        public string Content { get; set; }
        public HttpStatusCode Status { get; set; }
    }

    public class TransactionViewModel
    {
        public string UserEmail { get; set; }
        public string Date { get; set; }
        public bool IsWithdraw { get; set; }
        public bool IsDeposit { get; set; }
        public double Amount { get; set; }

        public override string ToString()
        {
            var type = IsWithdraw ? "Withdraw" : "Deposit";
            return "Date: " + Date + "  Type: " + type + "Amount: " + Amount;;
        }
    }
}