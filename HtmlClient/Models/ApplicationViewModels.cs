using System;
using HtmlClient.Enums;

namespace HtmlClient.Models
{

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConsoleToken { get; set; }
    }

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime ? LastLogin { get; set; }
        public double AccountBalance { get; set; }
    }

    public class XmlLink
    {
        public string Name { get; set; }
        public string LinkValue { get; set; }
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
            var dateString = Date.Split( '(' )[0];
            var type = IsWithdraw ? "withdrawal" : "deposit";
            return $"{dateString} :: {type} of ${Amount}";
        }
    }

    public class TransactionRequestModel
    {
        public string Token {get; set; }
        public string UserId { get; set; }
        public TransactionType Type { get; set; }
        public double? Amount { get; set; }
    }

    public class HomeViewModel{}
}