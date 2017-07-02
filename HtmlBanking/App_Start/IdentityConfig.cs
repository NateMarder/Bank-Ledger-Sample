using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HtmlBanking.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace HtmlBanking
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync( IdentityMessage message )
        {
            // Plug in your email service here to send an email.
            return Task.FromResult( 0 );
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync( IdentityMessage message )
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult( 0 );
        }
    }

    // Configure the application user manager which is used in this application.
    public class ApplicationUserManager : UserManager<BankUser>
    {
        public ApplicationUserManager( IUserStore<BankUser> store )
            : base( store )
        {
        }

        public static ApplicationUserManager Create( IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context )
        {
            var manager =
                new ApplicationUserManager( new UserStore<BankUser>( context.Get<ApplicationDbContext>() ) );
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<BankUser>( manager )
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes( 5 );
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider( "Phone Code", new PhoneNumberTokenProvider<BankUser>
            {
                MessageFormat = "Your security code is {0}"
            } );
            manager.RegisterTwoFactorProvider( "Email Code", new EmailTokenProvider<BankUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            } );
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if ( dataProtectionProvider != null )
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<BankUser>(
                        dataProtectionProvider.Create( "ASP.NET Identity" ) );
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.  
    public class ApplicationSignInManager : SignInManager<BankUser, string>
    {
        public ApplicationSignInManager( ApplicationUserManager userManager,
            IAuthenticationManager authenticationManager ) :
            base( userManager, authenticationManager )
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync( BankUser user )
        {
            return user.GenerateUserIdentityAsync( (ApplicationUserManager) UserManager );
        }

        public static ApplicationSignInManager Create( IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context )
        {
            return new ApplicationSignInManager( context.GetUserManager<ApplicationUserManager>(),
                context.Authentication );
        }
    }
}