

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
        //private DalHandler _dal;
        //public DalHandler DalHandler => _dal ?? ( _dal = new DalHandler() );

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
            try { return password.ToCharArray().Any( c => char.IsNumber( c ) ); }
            catch {  return false; }
        }

        private bool ContainNonNumericCharacter( string password )
        {
            try { return password.ToCharArray().Any( c => char.IsLetter( c ) ); }
            catch {  return false; }
        }

        private bool ContainUpperCaseCharacter( string password )
        {
            try { return password.ToCharArray().Any( c => char.IsUpper( c ) ); }
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