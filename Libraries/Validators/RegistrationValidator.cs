using System.Linq;
using FluentValidation;
using Libraries.Models;
using ValidationMessages = Libraries.Properties.Resources;

namespace Libraries.Validators
{
    public class RegistrationValidator : AbstractValidator<UserViewModel>
    {
        private Dal.Dal _dal;
        public Dal.Dal Dal => _dal ?? ( _dal = new Dal.Dal() );

        public RegistrationValidator()
        {
            // email validation
            RuleFor( model => model.Email )
                .EmailAddress().WithMessage( ValidationMessages.EmailNotValidGenericMessage )
                .Must( NotAlreadyExist ).WithMessage( ValidationMessages.EmailAlreadyExists );

            // password password validation
            RuleFor( model => model.Password )
                .Length( 4, 20 )
                .Must( ContainNumericCharacter ).WithMessage( "Your password must contain one number" )
                .Must( ContainNonNumericCharacter ).WithMessage( "Your password must contain at least one letter" )
                .Must( ContainUpperCaseCharacter ).WithMessage( "Your password must contain at least one uppercase letter" )
                .Must( ContainSpecialCharacter ).WithMessage( "Your passwords must contain one special character" );
        }

        private bool NotAlreadyExist( string email )
        {
            return !Dal.EmailExists( email );
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