using FluentValidation;
using HtmlClient.Dal;
using HtmlClient.Models;
using ValidationMessages = HtmlClient.Properties.Resources;

namespace HtmlClient.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        private DalHandler _dal;
        public DalHandler DalHandler 
            => _dal ?? ( _dal = new DalHandler() );

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
            return DalHandler.EmailExists( email );
        }

        private bool MatchUsersEmail( string password, string email )
        {
            return DalHandler.VerifyPasswordEmailComboExists(
                new UserViewModel {Email = email, Password = password} );
        }

        // for testing
        public void setDalHandler( DalHandler dalHandler )
        {
            _dal = dalHandler;
        }
    }
}