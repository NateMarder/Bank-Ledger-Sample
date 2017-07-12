using System.Linq;
using FluentValidation;
using HtmlClient.Dal;
using HtmlClient.Models;
using ValidationMessages = HtmlClient.Properties.Resources;

namespace HtmlClient.Validators
{
    public class RegistrationValidator : AbstractValidator<UserViewModel>
    {
        private DalHandler _dal;
        public DalHandler DalHandler => _dal ?? ( _dal = new DalHandler() );

        public RegistrationValidator()
        {
            // email validation
            RuleFor( model => model.Email )
                .EmailAddress().WithMessage( ValidationMessages.EmailNotValidGenericMessage )
                .Must( NotAlreadyExist ).WithMessage( ValidationMessages.EmailAlreadyExists );

            // password password validation
            RuleFor( model => model.Password )
                .Length( 4, 20 )
                .Must( ContainNumericCharacter )
                .Must( ContainNonNumericCharacter )
                .Must( ContainUpperCaseCharacter )
                .Must( ContainSpecialCharacter ).WithMessage( ValidationMessages.PasswordMustHaveNecessaryComponents );
        }

        private bool NotAlreadyExist( string email )
        {
            return !DalHandler.EmailExists( email );
        }

        private bool ContainNumericCharacter( string password )
        {
            //Todo: Figure out why fluent validation isn't working intermitently, as this is a terrrible pattern. 
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