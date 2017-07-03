using System;
using System.Linq;
using FluentValidation;
using HtmlClient.Models;
using ValidationMessages = HtmlClient.Properties.Resources;

namespace HtmlClient.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            // email validation
            RuleFor( model => model.Email )
                .EmailAddress()
                .Must( Exist )
                .WithMessage( ValidationMessages.EmailDoesntExist );

            // password validation
            RuleFor( model => model.Password )
                .NotEmpty()
                .Must( ( m, email ) => MatchUsersEmail( m.Password, m.Email ) )
                .WithMessage( ValidationMessages.EmailCombinationInvalid );
        }

        private bool Exist( string email )
        {
            return true;
        }

        private bool MatchUsersEmail( string password, string email )
        {
            var passwordShouldBe = "get the passord";

            return password == passwordShouldBe;
        }
    }
}

