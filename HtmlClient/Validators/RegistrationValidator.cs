using System.Linq;
using FluentValidation;
using HtmlClient.Models;
using ValidationMessages = HtmlClient.Properties.Resources;

namespace HtmlClient.Validators
{
    public class RegistrationValidator : AbstractValidator<RegisterViewModel>
    {
        public RegistrationValidator()
        {
            // email validation
            RuleFor( model => model.Email )
                .EmailAddress()
                .WithMessage( ValidationMessages.EmailNotValidGenericMessage )
                .Must( NotAlreadyExist )
                .WithMessage( ValidationMessages.EmailAlreadyExists );

            // password password validation
            RuleFor( model => model.Password )
                .NotEmpty()
                .Length( 4, 20 )
                .Must( ContainNumericCharacter )
                .Must( ContainNonNumericCharacter )
                .Must( ContainUpperCaseCharacter )
                .Must( ContainSpecialCharacter )
                .WithMessage( ValidationMessages.PasswordMustHaveNecessaryComponents );
        }

        private bool BeValidEmail( string email )
        {
            try
            {
                var validForMicrosoft = new System.Net.Mail.MailAddress( email );
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool NotAlreadyExist( string email )
        {
            // lookwithin local data store if it's there, then
            // no - can use

            return true;
        }

        private bool ContainNumericCharacter( string password )
        {
            return password.ToCharArray().Any( c => char.IsNumber( c ) );
        }

        private bool ContainNonNumericCharacter( string password )
        {
            return password.ToCharArray().Any( c => char.IsLetter( c ) );
        }

        private bool ContainUpperCaseCharacter( string password )
        {
            return password.ToCharArray().Any( c => char.IsUpper( c ) );
        }

        private bool ContainSpecialCharacter( string password )
        {
            return password.ToCharArray()
                .Any( c => ( !char.IsLetter( c ) && !char.IsNumber( c ) ) );
        }
    }
}