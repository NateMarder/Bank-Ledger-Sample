using System;

namespace HtmlClient.Models
{

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
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
    }

    public class HomeViewModel{}
}