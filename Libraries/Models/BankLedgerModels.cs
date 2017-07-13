using System;
using System.Collections.Generic;
using System.Net;
using Libraries.Enums;

namespace Libraries.Models
{

    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime ? LastLogin { get; set; }
        public double ? AccountBalance { get; set; }
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
        public string SessionToken { get; set; }
        public string UserId { get; set; }
        public TransactionType Type { get; set; }
        public double ? Amount { get; set; }
    }

    public class TransactionModel
    {
        public string Date { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
    }

    public class UserAccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string AccountId { get; set; }
        public double Balance { get; set; }
        public List<TransactionModel> TransactionHistory { get; set; } = new List<TransactionModel>();
    }

    public class SigninStatusModel
    {
        public string Content { get; set; }
        public SignInStatus Status { get; set; }
    }

    public class RegistrationStatusModel
    {
        public string Content { get; set; }
        public RegistrationStatus Status { get; set; }
    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConsoleToken { get; set; }
    }

    public class RegisterNewUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string HomeTown { get; set; }
    }

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class TransactionHistoryModel
    {
        public string[] Transactions { get; set; }
        public string Content { get; set; }
        public HttpStatusCode Status { get; set; }
    }



    public class HomeViewModel
    {
    }
}