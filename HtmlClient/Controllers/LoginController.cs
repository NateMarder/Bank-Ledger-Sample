using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HtmlClient.Models;
using HtmlClient.Validators;

namespace HtmlClient.Controllers
{
    public class LoginController : Controller
    {
        private LoginValidator _loginValidator;

        public LoginValidator LoginValidator
            => _loginValidator ?? ( _loginValidator = new LoginValidator() );

        [AllowAnonymous]
        [HttpGet]
        public ActionResult UserSignIn( LoginViewModel model )
        {
            return View( "../Login" );
        }

        [HttpGet]
        public ActionResult RegistrationSuccess( LoginViewModel model )
        {
            // first manually sign the user in, then redirect to the main home page

            return View( "../Home/Index" );
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignInSuccess( LoginViewModel model )
        {
            return View( "../Home/Index" );
        }

        [AllowAnonymous]
        public ActionResult ValidateSignIn( UserViewModel user )
        {
            try
            {
                // 'userIsOkay' look in the local data store to make sure
                // the user now exists there!
                var result = LoginValidator.Validate( user );
                if ( result.IsValid )
                {
                    Session["UserId"] = user.Email;
                    Session["SessionGuid"] = Guid.NewGuid();
                    return RedirectToAction( "SignInSuccess" );
                }

                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                TempData["LoginMessage"] = result.Errors.Select( e => e.ErrorMessage ).ToString();
                return View( "../Login" );
            }
            catch ( Exception ex )
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                TempData["LoginMessage"] = Properties.Resources.GenericErrorMessage
                                           + Environment.NewLine + ex.Message;
                return View( "../Login" );
            }
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            return View( "../SignOutComplete" ); // JsonResult();
        }
    }
}