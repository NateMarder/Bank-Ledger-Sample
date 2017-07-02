using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HtmlBanking.Models
{
    // You can add profile data for the user by adding more properties to your BankUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class BankUser : IdentityUser
    {
        public string Hometown { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<BankUser> manager )
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync( this, DefaultAuthenticationTypes.ApplicationCookie );
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<BankUser>
    {
        public ApplicationDbContext()
            : base( "DefaultConnection", false )
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}