using System.Linq;
using FluentValidation;
using Libraries.Models;
using Resources = Libraries.Properties.Resources;
using Settings = Libraries.Properties.Settings; 

namespace Libraries.Validators
{
    public class RegistrationValidator : AbstractValidator<UserViewModel>
    {
        private Dal.XmlDal _xmlDal;
        public Dal.XmlDal XmlDal => _xmlDal ?? ( _xmlDal = new Dal.XmlDal() );

        public RegistrationValidator()
        {
            // email validation
            RuleFor( model => model.Email )
                .EmailAddress().WithMessage( Resources.EmailNotValidGenericMessage )
                .Must( NotAlreadyExist ).WithMessage( Resources.EmailAlreadyExists );

            // password password validation
            RuleFor( model => model.Password )
                .Length( 6, 20 ).WithMessage( "Your password needs to be between 6 and 20 characters long" )
                .Must( ContainNumericCharacter ).WithMessage( "Your password must contain one number" )
                .Must( ContainUpperCaseCharacter ).WithMessage( "Your password must contain at least one uppercase letter" )
                .Must( ContainSpecialCharacter ).WithMessage( "Your passwords must contain one special character" );
        }

        private bool NotAlreadyExist( string email )
        {
            if ( Settings.Default.UseXmlDataStore )
            {
                return !XmlDal.EmailExists( email );
            }
            else return true;

        }

        private bool ContainNumericCharacter( string password )
        {
            try { return password.ToCharArray().Any( char.IsNumber ); }
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