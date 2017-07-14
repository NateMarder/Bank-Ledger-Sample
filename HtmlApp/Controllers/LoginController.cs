using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FluentValidation.Results;
using HtmlApp.Classes;
using HtmlApp.Filters;
using Libraries.Validators;
using Libraries.Models;

namespace HtmlApp.Controllers
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
                TempData["LoginMessage"] = GetPrettifiedValidationError( result );
                return View( "../Login" );
            }
            catch ( Exception ex )
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                TempData["LoginMessage"] = ex.Message;
                return View( "../Login" );
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            Session.Abandon();
            return View( "../SignOutComplete" );
        }


        /************************************************************
        *************************************************************
        ** 
        ** ConsoleApp Endpoints and Helper Methods
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
        [AuthorizeConsoleAppUser]
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

        private string GetPrettifiedValidationError( ValidationResult result )
        {
            if ( result.IsValid )
            {
                return null;
            }

            return result.Errors.Aggregate( "", ( current, error ) 
                => current + ( error.ErrorMessage + "\n" ) );
        }
    }
}