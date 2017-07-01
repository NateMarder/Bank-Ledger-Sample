using System.Web;
using System.Web.Http;
using HtmlAtmClient.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace HtmlAtmClient.Controllers
{
    [Authorize]
    public class MeController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
            => _userManager ?? (_userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
          
        public MeController()
        {
        }

        public MeController( ApplicationUserManager userManager )
        {
            _userManager = userManager;
        }

        // GET api/Me
        public GetViewModel Get()
        {
            var user = UserManager.FindById( User.Identity.GetUserId() );
            return new GetViewModel {Hometown = user.Hometown};
        }
    }
}