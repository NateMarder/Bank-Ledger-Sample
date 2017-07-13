

using System.Linq;
using ConsoleBanking.Properties;
using FluentValidation;

namespace ConsoleBanking.Validators
{

    public class PasswordViewModel
    {
        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<PasswordViewModel>
    {

        public LoginValidator()
        {
            RuleFor( model => model.Password )
                .Length( 4, 20 )
                .Must( ContainNumericCharacter )
                .Must( ContainNonNumericCharacter )
                .Must( ContainUpperCaseCharacter )
                .Must( ContainSpecialCharacter ).WithMessage( Resources.InvalidInput );
        }

        private bool ContainNumericCharacter( string password )
        {
            try { return password.ToCharArray().Any( char.IsNumber ); }
            catch {  return false; }
        }

        private bool ContainNonNumericCharacter( string password )
        {
            try { return password.ToCharArray().Any( char.IsLetter ); }
            catch {  return false; }
        }

        private bool ContainUpperCaseCharacter( string password )
        {
            try { return password.ToCharArray().Any( char.IsUpper ); }
            catch {  return false; }
        }

        private bool ContainSpecialCharacter( string password )
        {
            try { return password.ToCharArray()
                .Any( c => ( !char.IsLetter( c ) && !char.IsNumber( c ) ) ); }
            catch {  return false; }
        }

    }
}