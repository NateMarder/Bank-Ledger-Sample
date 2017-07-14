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

        private Dal.LocalDal _localDal;
        public Dal.LocalDal LocalDal => _localDal ?? ( _localDal = new Dal.LocalDal() );

        public RegistrationValidator()
        {
            // email validation
            RuleFor( model => model.Email )
                .EmailAddress().WithMessage( Resources.EmailNotValidGenericMessage )
                .Must( NotAlreadyExist ).WithMessage( Resources.EmailAlreadyExists );

            // password password validation
            RuleFor( model => model.Password )
                .Length( 6, 20 ).WithMessage( Resources.PasswordMustBeSixToTwenty )
                .Must( ContainNumericCharacter ).WithMessage( Resources.PasswordMustContainNumber )
                .Must( ContainUpperCaseCharacter ).WithMessage( Resources.PasswordMustContainUpperCaseLetter )
                .Must( ContainSpecialCharacter ).WithMessage( Resources.PasswordMustContainOneSpecialCharacter );
        }

        private bool NotAlreadyExist( string email )
        {
            if ( Settings.Default.UseXmlDataStore )
            {
                return !XmlDal.EmailExists( email );
            }

            //todo: implement this method
            //return !LocalDal.EmailExists( email );  

            return true;
        }

        private bool ContainNumericCharacter( string password )
        {
            return password.ToCharArray().Any( char.IsNumber );
        }

        private bool ContainUpperCaseCharacter( string password )
        {
            return password.ToCharArray().Any( char.IsUpper );
        }

        private bool ContainSpecialCharacter( string password )
        {
            return password.ToCharArray()
                .Any( c => ( !char.IsLetter( c ) && !char.IsNumber( c ) ) );
        }
    }
}