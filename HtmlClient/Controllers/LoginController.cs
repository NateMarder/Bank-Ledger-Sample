using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HtmlClient.Classes;
using HtmlClient.Filters;
using HtmlClient.Models;
using HtmlClient.Properties;
using HtmlClient.Validators;

namespace HtmlClient.Controllers
{
    public class LoginController : Controller
    {
        private LoginValidator _loginValidator;
        public LoginValidator LoginValidator
            => _loginValidator ?? ( _loginValidator = new LoginValidator() );

        public LoginController( LoginValidator loginValidator ) // for testing
        {
            _loginValidator = loginValidator;
        }

        public LoginController()
        {
        }

        [AllowAnonymous]
        public ActionResult UserSignIn( UserViewModel model )
        {
            return View( "../Login" );
        }

        [AllowAnonymous]
        public ActionResult SignInSuccess( UserViewModel model )
        {
            return View( "../Home/Index" );
        }

        [AllowAnonymous]
        public ActionResult ValidateSignIn( UserViewModel user )
        {
            try
            {
                var result = LoginValidator.Validate( user );
                if ( result.IsValid )
                {
                    Session["UserId"] = user.Email;
                    return RedirectToAction( "SignInSuccess" );
                }

                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                TempData["LoginMessage"] = result.Errors.Select( e => e.ErrorMessage ).ToString();
                return View( "../Login" );
            }
            catch ( Exception ex )
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                TempData["LoginMessage"] = Resources.GenericErrorMessage
                                           + Environment.NewLine + ex.Message;
                return View( "../Login" );
            }
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            return View( "../SignOutComplete" );
        }


        /************************************************************
        *************************************************************
        ** 
        ** Console Endpoints
        **
        *************************************************************
        *************************************************************/
        [AllowAnonymous]
        public ActionResult LoginFromConsole( UserViewModel model )
        {
            try
            {
                var result = LoginValidator.Validate( model );
                if ( result.IsValid )
                {
                    Response.StatusCode = (int) HttpStatusCode.OK;
                    ActiveSessions.MarkSessionAsAuthenticated( Session.SessionID, model.Email );
                    return new ContentResult()
                    {
                        Content = Session.SessionID
                    };
                }

                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return new ContentResult()
                {
                    Content = result.Errors.ToArray().Select( e => e.ErrorMessage ).ToString()
                };
            }
            catch ( Exception ex )
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return new ContentResult()
                {
                    Content = ex.Message
                };
            }
        }


        [HttpPost]
        [AuthorizeConsoleClient]
        public ActionResult ConsoleSignOut()
        {
            try
            {
                ActiveSessions.RemoveSession( Session.SessionID );
                Session.Abandon();
                return new HttpStatusCodeResult( HttpStatusCode.OK );
            }
            catch
            {
                //todo: add logging
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );
            }
        }
    }
}