using FluentValidation;
using Libraries.Models;
using ValidationMessages = Libraries.Properties.Resources;

namespace Libraries.Validators
{
    public class LoginValidator : AbstractValidator<UserViewModel>
    {
        private Dal.XmlDal _xmlDal;
        public Dal.XmlDal XmlDal 
            => _xmlDal ?? ( _xmlDal = new Dal.XmlDal() );

        public LoginValidator()
        {
            // email validation
            RuleFor( model => model.Email )
                .NotEmpty()
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
            return XmlDal.EmailExists( email );
        }

        private bool MatchUsersEmail( string password, string email )
        {
            return XmlDal.VerifyPasswordEmailComboExists(
                new UserViewModel {Email = email, Password = password} );
        }

        // for testing
        public void SetDalHandler( Dal.XmlDal xmlDal )
        {
            _xmlDal = xmlDal;
        }
    }
}