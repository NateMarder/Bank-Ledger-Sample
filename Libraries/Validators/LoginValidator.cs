using FluentValidation;
using Libraries.Models;
using ValidationMessages = Libraries.Properties.Resources;

namespace Libraries.Validators
{
    public class LoginValidator : AbstractValidator<UserViewModel>
    {
        private Dal.Dal _dal;
        public Dal.Dal Dal 
            => _dal ?? ( _dal = new Dal.Dal() );

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
            return Dal.EmailExists( email );
        }

        private bool MatchUsersEmail( string password, string email )
        {
            return Dal.VerifyPasswordEmailComboExists(
                new UserViewModel {Email = email, Password = password} );
        }

        // for testing
        public void SetDalHandler( Dal.Dal dal )
        {
            _dal = dal;
        }
    }
}